import type { HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { logger } from '@/shared/application/gateways/logger';
import { shouldSkipRealtimeGachaInvalidation } from '@/shared/application/gateways/gachaRealtimeDedup';
import { shouldSkipRealtimeInventoryInvalidation } from '@/shared/application/gateways/inventoryRealtimeDedup';
import type { UserStateInvalidationDomain } from '@/shared/application/gateways/invalidateUserStateQueries';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

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

type UserStatusChangedEventAt = string | number | Date | null | undefined;

const CHAT_UNREAD_REFRESH_EVENT_TYPES = new Set(['message_created', 'message_read', 'unread_changed']);
const MAX_PRESENCE_EVENT_TRACKER_SIZE = 5_000;

function toEventUnixMs(value: UserStatusChangedEventAt): number | null {
 if (value instanceof Date) {
  const timestamp = value.getTime();
  return Number.isFinite(timestamp) ? timestamp : null;
 }

 if (typeof value === 'number') {
  if (!Number.isFinite(value)) {
   return null;
  }

  return value > 1e12 ? Math.trunc(value) : Math.trunc(value * 1000);
 }

 if (typeof value !== 'string') {
  return null;
 }

 const normalized = value.trim();
 if (normalized.length === 0) {
  return null;
 }

 const asNumber = Number(normalized);
 if (Number.isFinite(asNumber)) {
  return asNumber > 1e12 ? Math.trunc(asNumber) : Math.trunc(asNumber * 1000);
 }

 const asDate = Date.parse(normalized);
 return Number.isFinite(asDate) ? asDate : null;
}

interface RegisterPresenceDomainEventHandlersParams {
 forceLogoutAfterRoleChange: (notificationType?: string) => Promise<void>;
 hubConnection: HubConnection;
 invalidateInbox: () => void;
 invalidateUnreadBadge: () => void;
 queryClient: QueryClient;
 queueInvalidation: (domains: readonly UserStateInvalidationDomain[]) => void;
 scheduleWalletRefresh: () => void;
 tryApplyWalletDelta: (payload?: WalletBalanceChangedPayload) => boolean;
}

export function registerPresenceDomainEventHandlers({
 forceLogoutAfterRoleChange,
 hubConnection,
 invalidateInbox,
 invalidateUnreadBadge,
 queryClient,
 queueInvalidation,
 scheduleWalletRefresh,
 tryApplyWalletDelta,
}: RegisterPresenceDomainEventHandlersParams) {
 const lastSeenStatusEventByUserId = new Map<string, number>();

 const handleUserStatusChanged = (userId: string, status?: string, at?: UserStatusChangedEventAt) => {
  const eventUnixMs = toEventUnixMs(at) ?? Date.now();
  const previousEventUnixMs = userId ? lastSeenStatusEventByUserId.get(userId) : undefined;
  if (typeof previousEventUnixMs === 'number' && eventUnixMs < previousEventUnixMs) {
   logger.warn('[PresenceRealtimeSync]', 'Skipped stale UserStatusChanged event', {
    at,
    previousAtMs: previousEventUnixMs,
    status,
    userId,
   });
   return;
  }

  if (userId) {
   lastSeenStatusEventByUserId.set(userId, eventUnixMs);
   if (lastSeenStatusEventByUserId.size > MAX_PRESENCE_EVENT_TRACKER_SIZE) {
    lastSeenStatusEventByUserId.clear();
   }
  }

  logger.info('[PresenceRealtimeSync]', 'UserStatusChanged received', { at, userId, status });
  void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.directoryRoot() });
  if (userId) {
   void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.profile(userId) });
  }
 };

 hubConnection.on('UserStatusChanged', handleUserStatusChanged);
 hubConnection.on('user.status_changed', handleUserStatusChanged);

 hubConnection.on('notification.new', (payload?: NotificationNewPayload) => {
  queueInvalidation(['notifications']);
  const notificationType = payload?.type?.trim().toLowerCase();
  void forceLogoutAfterRoleChange(notificationType);
 });

 hubConnection.on('wallet.balance_changed', (payload?: WalletBalanceChangedPayload) => {
  logger.info('[PresenceRealtimeSync]', 'wallet.balance_changed received. Queue wallet sync...');
  queueInvalidation(['wallet']);
  if (!tryApplyWalletDelta(payload)) {
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

 hubConnection.on('conversation.updated.delta', () => {
  invalidateInbox();
 });

 hubConnection.on('chat.unread.delta', () => {
  invalidateInbox();
  invalidateUnreadBadge();
 });

 hubConnection.on('message.created.fast', () => {
  invalidateInbox();
  invalidateUnreadBadge();
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
   'notifications',
   'chat',
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
}
