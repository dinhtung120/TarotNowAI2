import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { shouldSkipRealtimeGachaInvalidation } from '@/shared/infrastructure/gacha/gachaRealtimeDedup';
import {
  invalidateUserStateQueries,
  type UserStateInvalidationDomain,
} from '@/shared/infrastructure/query/invalidateUserStateQueries';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

const HEARTBEAT_INTERVAL_MS = 5 * 60 * 1000;
const INVALIDATION_BATCH_DELAY_MS = 320;
const WALLET_REFRESH_MIN_INTERVAL_MS = 2_000;
const MIN_RETRY_INVALIDATION_DELAY_MS = 80;

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

export function registerPresenceConnectionHandlers(hubConnection: HubConnection, queryClient: QueryClient) {
  let inboxInvalidateTimeout: NodeJS.Timeout | null = null;
  let invalidationBatchTimeout: NodeJS.Timeout | null = null;
  let walletRefreshTimeout: NodeJS.Timeout | null = null;
  const pendingInvalidationDomains = new Set<UserStateInvalidationDomain>();
  const domainLastInvalidatedAt = new Map<UserStateInvalidationDomain, number>();
  let lastWalletRefreshAt = 0;

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

  hubConnection.on('UserStatusChanged', (userId: string) => {
    void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.directoryRoot() });
    if (userId) {
      void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.profile(userId) });
    }
  });

  hubConnection.on('notification.new', () => {
    queueInvalidation(['notifications']);
  });

  hubConnection.on('wallet.balance_changed', () => {
    logger.info('[PresenceRealtimeSync]', 'wallet.balance_changed received. Queue wallet sync...');
    queueInvalidation(['wallet']);
    scheduleWalletRefresh();
  });

  hubConnection.on('conversation.updated', () => {
    logger.info('[PresenceRealtimeSync]', 'conversation.updated received. Invalidating inbox queries...');
    invalidateInbox();
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

  hubConnection.on('gacha.result', () => {
    if (shouldSkipRealtimeGachaInvalidation()) {
      return;
    }

    queueInvalidation(['gacha']);
  });

  hubConnection.on('inventory.changed', (payload?: {
    itemCode?: string;
    enhancementType?: string;
  }) => {
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
