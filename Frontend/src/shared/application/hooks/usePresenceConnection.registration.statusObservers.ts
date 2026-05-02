import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { logger } from '@/shared/application/gateways/logger';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

const DIRECTORY_QUERY_KEY = 'readers';
const PROFILE_QUERY_KEY = 'reader-profile';
const OBSERVER_BATCH_SIZE = 200;
const RECONCILIATION_INTERVAL_MS = 30_000;
const SUBSCRIBE_METHOD = 'SubscribeUserStatusObservers';
const UNSUBSCRIBE_METHOD = 'UnsubscribeUserStatusObservers';

interface ReaderSummaryLike {
 userId?: unknown;
}

interface ReaderProfileLike {
 userId?: unknown;
}

export interface PresenceStatusObserverCoordinator {
 dispose: () => void;
 sync: () => void;
}

function normalizeUserId(value: unknown): string | null {
 if (typeof value !== 'string') {
  return null;
 }

 const normalized = value.trim();
 return normalized.length > 0 ? normalized : null;
}

function hasActiveObservers(query: { getObserversCount: () => number }): boolean {
 return query.getObserversCount() > 0;
}

function chunkUserIds(userIds: readonly string[]): string[][] {
 if (userIds.length === 0) {
  return [];
 }

 const chunks: string[][] = [];
 for (let index = 0; index < userIds.length; index += OBSERVER_BATCH_SIZE) {
  chunks.push(userIds.slice(index, index + OBSERVER_BATCH_SIZE));
 }

 return chunks;
}

function collectObservedUserIds(queryClient: QueryClient): string[] {
 const observedUserIds = new Set<string>();
 const queryCache = queryClient.getQueryCache();

 const directoryQueries = queryCache.findAll({ queryKey: [DIRECTORY_QUERY_KEY] });
 for (const query of directoryQueries) {
  if (!hasActiveObservers(query)) {
   continue;
  }

  const data = query.state.data as { readers?: ReaderSummaryLike[] } | undefined;
  const readers = Array.isArray(data?.readers) ? data.readers : [];
  for (const reader of readers) {
   const userId = normalizeUserId(reader.userId);
   if (userId) {
    observedUserIds.add(userId);
   }
  }
 }

 const profileQueries = queryCache.findAll({ queryKey: [PROFILE_QUERY_KEY] });
 for (const query of profileQueries) {
  if (!hasActiveObservers(query)) {
   continue;
  }

  const keyUserId = normalizeUserId((query.queryKey as readonly unknown[])[1]);
  if (keyUserId) {
   observedUserIds.add(keyUserId);
   continue;
  }

  const profile = query.state.data as ReaderProfileLike | undefined;
  const profileUserId = normalizeUserId(profile?.userId);
  if (profileUserId) {
   observedUserIds.add(profileUserId);
  }
 }

 return [...observedUserIds];
}

export function createPresenceStatusObserverCoordinator(
 hubConnection: HubConnection,
 queryClient: QueryClient,
): PresenceStatusObserverCoordinator {
 const queryCache = queryClient.getQueryCache?.();
 if (!queryCache || typeof queryCache.findAll !== 'function' || typeof queryCache.subscribe !== 'function') {
  return {
   dispose: () => undefined,
   sync: () => undefined,
  };
 }

 let disposed = false;
 let observedUserIds = new Set<string>();
 let syncChain = Promise.resolve();
 let reconciliationTimer: ReturnType<typeof setInterval> | null = null;

 const syncNow = async () => {
  if (disposed || hubConnection.state !== HubConnectionState.Connected) {
   return;
  }

  const nextObservedUserIds = new Set(collectObservedUserIds(queryClient));
  const toSubscribe: string[] = [];
  const toUnsubscribe: string[] = [];

  for (const userId of nextObservedUserIds) {
   if (!observedUserIds.has(userId)) {
    toSubscribe.push(userId);
   }
  }

  for (const userId of observedUserIds) {
   if (!nextObservedUserIds.has(userId)) {
    toUnsubscribe.push(userId);
   }
  }

  if (toSubscribe.length > 0) {
   for (const chunk of chunkUserIds(toSubscribe)) {
    await hubConnection.invoke(SUBSCRIBE_METHOD, chunk);
   }
  }

  if (toUnsubscribe.length > 0) {
   for (const chunk of chunkUserIds(toUnsubscribe)) {
    await hubConnection.invoke(UNSUBSCRIBE_METHOD, chunk);
   }
  }

  observedUserIds = nextObservedUserIds;
 };

 const runReconciliation = async () => {
  if (disposed || hubConnection.state !== HubConnectionState.Connected || observedUserIds.size === 0) {
   return;
  }

  await queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.directoryRoot() });
  for (const observedUserId of observedUserIds) {
   await queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.profile(observedUserId) });
  }
 };

 const enqueueSync = () => {
  if (disposed) {
   return;
  }

  syncChain = syncChain
   .then(syncNow)
   .catch((error) => {
    logger.error('[PresenceRealtimeSync] failed to sync user-status observer groups', error);
   });
 };

 const enqueueReconciliation = () => {
  if (disposed) {
   return;
  }

  syncChain = syncChain
   .then(runReconciliation)
   .catch((error) => {
    logger.error('[PresenceRealtimeSync] failed to run presence reconciliation', error);
   });
 };

 const unsubscribeQueryCache = queryCache.subscribe(() => {
  enqueueSync();
 });

 hubConnection.onreconnected(() => {
  observedUserIds.clear();
  enqueueSync();
  enqueueReconciliation();
 });

 reconciliationTimer = setInterval(() => {
  enqueueReconciliation();
 }, RECONCILIATION_INTERVAL_MS);

 return {
  dispose: () => {
   disposed = true;
   unsubscribeQueryCache();
   if (reconciliationTimer) {
    clearInterval(reconciliationTimer);
   }
   observedUserIds.clear();
  },
  sync: enqueueSync,
 };
}
