import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { useAuthStore } from '@/store/authStore';
import { routing } from '@/i18n/routing';
import { logger } from '@/shared/application/gateways/logger';
import { performClientLogoutCleanup } from '@/shared/application/gateways/clientLogoutCleanup';
import { shouldSkipRealtimeGachaInvalidation } from '@/shared/application/gateways/gachaRealtimeDedup';
import { shouldSkipRealtimeInventoryInvalidation } from '@/shared/application/gateways/inventoryRealtimeDedup';
import {
  invalidateUserStateQueries,
  type UserStateInvalidationDomain,
} from '@/shared/application/gateways/invalidateUserStateQueries';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

const HEARTBEAT_INTERVAL_MS = 5 * 60 * 1000;
const INVALIDATION_BATCH_DELAY_MS = 320;
const WALLET_REFRESH_MIN_INTERVAL_MS = 2_000;
const MIN_RETRY_INVALIDATION_DELAY_MS = 80;
const ROLE_CHANGED_FORCE_LOGOUT_NOTIFICATION_TYPE = 'reader_request_approved';

const DOMAIN_INVALIDATION_COOLDOWN_MS: Record<UserStateInvalidationDomain, number> = {
  wallet: 1_000,
  inventory: 1_800,
  collection: 1_800,
  readingSetup: 1_200,
  readingHistory: 1_000,
  profile: 1_000,
  readerRequest: 1_000,
  notifications: 800,
  chat: 1_000,
  gacha: 1_000,
  gamification: 1_000,
};

interface WalletBalanceChangedPayload {
 currency?: string;
 deltaAmount?: number | string;
}

interface ConversationUpdatedPayload {
 type?: string;
}

interface NotificationNewPayload {
 type?: string;
}

const CHAT_UNREAD_REFRESH_EVENT_TYPES = new Set(['message_created', 'message_read', 'unread_changed']);

function toNumber(value: number | string | undefined): number | null {
 if (typeof value === 'number' && Number.isFinite(value)) {
  return value;
 }

 if (typeof value === 'string') {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : null;
 }

 return null;
}

function applyWalletDelta(payload?: WalletBalanceChangedPayload): boolean {
 const deltaAmount = toNumber(payload?.deltaAmount);
 if (deltaAmount === null || deltaAmount === 0) {
  return false;
 }

 const normalizedCurrency = payload?.currency?.trim().toLowerCase();
 if (!normalizedCurrency) {
  return false;
 }

 const store = useWalletStore.getState();
 if (!store.balance) {
  return false;
 }

 if (normalizedCurrency === 'gold') {
  store.setBalance({
   ...store.balance,
   goldBalance: Math.max(0, store.balance.goldBalance + deltaAmount),
  });
  return true;
 }

 if (normalizedCurrency === 'diamond') {
  store.setBalance({
   ...store.balance,
   diamondBalance: Math.max(0, store.balance.diamondBalance + deltaAmount),
  });
  return true;
 }

 return false;
}

function resolveLoginPathname(currentPathname: string): string {
 const segments = currentPathname.split('/').filter(Boolean);
 const firstSegment = segments[0] ?? '';
 const hasLocalePrefix = routing.locales.some((locale) => locale === firstSegment);
 const locale = hasLocalePrefix ? firstSegment : routing.defaultLocale;
 return `/${locale}/login`;
}

export function registerPresenceConnectionHandlers(hubConnection: HubConnection, queryClient: QueryClient) {
  let inboxInvalidateTimeout: NodeJS.Timeout | null = null;
  let unreadInvalidateTimeout: NodeJS.Timeout | null = null;
  let invalidationBatchTimeout: NodeJS.Timeout | null = null;
  let walletRefreshTimeout: NodeJS.Timeout | null = null;
  const pendingInvalidationDomains = new Set<UserStateInvalidationDomain>();
  const domainLastInvalidatedAt = new Map<UserStateInvalidationDomain, number>();
  let lastWalletRefreshAt = 0;
  let roleChangeLogoutTriggered = false;

  const scheduleInvalidationFlush = (delayMs = INVALIDATION_BATCH_DELAY_MS) => {
    if (invalidationBatchTimeout) {
      return;
    }

    invalidationBatchTimeout = setTimeout(flushInvalidationBatch, delayMs);
  };

  const flushInvalidationBatch = () => {
    invalidationBatchTimeout = null;
    if (pendingInvalidationDomains.size === 0) {
      return;
    }

    const now = Date.now();
    const runnableDomains: UserStateInvalidationDomain[] = [];
    let earliestRetryDelayMs: number | null = null;

    for (const domain of [...pendingInvalidationDomains]) {
      const lastInvalidatedAt = domainLastInvalidatedAt.get(domain) ?? 0;
      const cooldownMs = DOMAIN_INVALIDATION_COOLDOWN_MS[domain];
      const elapsedMs = now - lastInvalidatedAt;
      if (elapsedMs >= cooldownMs) {
        pendingInvalidationDomains.delete(domain);
        domainLastInvalidatedAt.set(domain, now);
        runnableDomains.push(domain);
        continue;
      }

      const retryDelayMs = Math.max(MIN_RETRY_INVALIDATION_DELAY_MS, cooldownMs - elapsedMs);
      earliestRetryDelayMs = earliestRetryDelayMs === null
        ? retryDelayMs
        : Math.min(earliestRetryDelayMs, retryDelayMs);
    }

    if (runnableDomains.length > 0) {
      void invalidateUserStateQueries(queryClient, runnableDomains);
    }

    if (pendingInvalidationDomains.size > 0) {
      scheduleInvalidationFlush(earliestRetryDelayMs ?? INVALIDATION_BATCH_DELAY_MS);
    }
  };

  const queueInvalidation = (domains: readonly UserStateInvalidationDomain[]) => {
    domains.forEach((domain) => pendingInvalidationDomains.add(domain));
    scheduleInvalidationFlush();
  };

  const forceLogoutAfterRoleChange = async () => {
    if (roleChangeLogoutTriggered) {
      return;
    }

    roleChangeLogoutTriggered = true;
    try {
      await fetch('/api/auth/logout', {
        method: 'POST',
        credentials: 'include',
        cache: 'no-store',
      });
    } catch {
      // No-op: vẫn cưỡng bức clear session cục bộ để chặn tiếp tục dùng role cũ.
    } finally {
      performClientLogoutCleanup(queryClient);
    }

    if (typeof window === 'undefined') {
      return;
    }

    const loginPath = resolveLoginPathname(window.location.pathname);
    if (window.location.pathname !== loginPath) {
      window.location.assign(loginPath);
    }
  };

  const scheduleWalletRefresh = () => {
    const elapsedMs = Date.now() - lastWalletRefreshAt;
    const remainingMs = Math.max(0, WALLET_REFRESH_MIN_INTERVAL_MS - elapsedMs);
    if (walletRefreshTimeout) {
      return;
    }

    walletRefreshTimeout = setTimeout(() => {
      walletRefreshTimeout = null;
      lastWalletRefreshAt = Date.now();
      void useWalletStore.getState().fetchBalance();
    }, remainingMs);
  };

  const invalidateInbox = () => {
    if (inboxInvalidateTimeout) {
      clearTimeout(inboxInvalidateTimeout);
    }

    inboxInvalidateTimeout = setTimeout(() => {
      void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.inboxRoot() });
    }, 1000);
  };

  const invalidateUnreadBadge = () => {
    if (unreadInvalidateTimeout) {
      clearTimeout(unreadInvalidateTimeout);
    }

    unreadInvalidateTimeout = setTimeout(() => {
      void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() });
    }, 1000);
  };

  hubConnection.on('UserStatusChanged', (userId: string, status?: string) => {
    logger.info('[PresenceRealtimeSync]', 'UserStatusChanged received', { userId, status });
    void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.directoryRoot() });
    if (userId) {
      void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.profile(userId) });
    }
  });

  hubConnection.on('notification.new', (payload?: NotificationNewPayload) => {
    queueInvalidation(['notifications']);
    const notificationType = payload?.type?.trim().toLowerCase();
    if (notificationType === ROLE_CHANGED_FORCE_LOGOUT_NOTIFICATION_TYPE && useAuthStore.getState().isAuthenticated) {
      void forceLogoutAfterRoleChange();
    }
  });

  hubConnection.on('wallet.balance_changed', (payload?: WalletBalanceChangedPayload) => {
    logger.info('[PresenceRealtimeSync]', 'wallet.balance_changed received. Queue wallet sync...');
    queueInvalidation(['wallet']);
    if (!applyWalletDelta(payload)) {
      scheduleWalletRefresh();
    }
  });

  hubConnection.on('conversation.updated', (payload?: ConversationUpdatedPayload) => {
    logger.info('[PresenceRealtimeSync]', 'conversation.updated received. Invalidating inbox queries...');
    invalidateInbox();
    const eventType = payload?.type?.trim().toLowerCase();
    if (!eventType || CHAT_UNREAD_REFRESH_EVENT_TYPES.has(eventType)) {
      invalidateUnreadBadge();
    }
  });

  hubConnection.on('gamification.quest_completed', () => {
    queueInvalidation(['gamification']);
  });

  hubConnection.on('gamification.achievement_unlocked', () => {
    queueInvalidation(['gamification', 'profile']);
  });

  hubConnection.on('gamification.card_level_up', () => {
    queueInvalidation(['collection']);
  });

  hubConnection.on('gacha.result', (payload?: {
    operationId?: string;
    idempotencyKey?: string;
  }) => {
    const correlationKey = payload?.idempotencyKey?.trim() || payload?.operationId?.trim();
    if (shouldSkipRealtimeGachaInvalidation(correlationKey)) {
      return;
    }

    queueInvalidation(['gacha']);
  });

  hubConnection.on('inventory.changed', (payload?: {
    itemCode?: string;
    enhancementType?: string;
    operationId?: string;
    idempotencyKey?: string;
  }) => {
    const correlationKey = payload?.idempotencyKey?.trim() || payload?.operationId?.trim();
    if (shouldSkipRealtimeInventoryInvalidation(correlationKey)) {
      return;
    }

    const domains: UserStateInvalidationDomain[] = ['inventory'];
    if (payload?.enhancementType) {
      domains.push('collection');
    }

    const normalizedItemCode = payload?.itemCode?.trim().toLowerCase() ?? '';
    if (normalizedItemCode.startsWith('free_draw_ticket_')) {
      domains.push('readingSetup');
    }

    queueInvalidation(domains);
  });

  hubConnection.on('reading.quota_changed', () => {
    queueInvalidation(['readingSetup']);
  });

  hubConnection.on('profile.changed', () => {
    queueInvalidation(['profile']);
  });

  hubConnection.on('title.changed', () => {
    queueInvalidation(['profile', 'gamification']);
  });

  hubConnection.on('Error', (error: string) => {
    logger.error('[PresenceRealtimeSync] hub error', error);
  });

  hubConnection.onreconnecting((error) => {
    logger.warn('[PresenceRealtimeSync] reconnecting due to error', error?.message || 'Unknown error');
  });

  hubConnection.onreconnected((connectionId) => {
    logger.info('[PresenceRealtimeSync] reconnected. new id:', connectionId || 'unknown');
    queueInvalidation([
      'wallet',
      'inventory',
      'collection',
      'readingSetup',
      'readingHistory',
      'profile',
      'readerRequest',
      'notifications',
      'chat',
      'gacha',
      'gamification',
    ]);
    invalidateInbox();
    invalidateUnreadBadge();
    scheduleWalletRefresh();
  });

  hubConnection.onclose((error) => {
    if (error) {
      logger.error('[PresenceRealtimeSync] connection closed with error', error.message);
    }
  });

  return {
    dispose: () => {
      if (inboxInvalidateTimeout) {
        clearTimeout(inboxInvalidateTimeout);
      }
      if (unreadInvalidateTimeout) {
        clearTimeout(unreadInvalidateTimeout);
      }
      if (invalidationBatchTimeout) {
        clearTimeout(invalidationBatchTimeout);
      }
      if (walletRefreshTimeout) {
        clearTimeout(walletRefreshTimeout);
      }
    },
    startHeartbeat: () => {
      return setInterval(() => {
        if (hubConnection.state === HubConnectionState.Connected) {
          hubConnection.invoke('Heartbeat').catch((err) => {
            logger.error('[PresenceRealtimeSync] heartbeat error', err);
          });
        }
      }, HEARTBEAT_INTERVAL_MS);
    },
  };
}
