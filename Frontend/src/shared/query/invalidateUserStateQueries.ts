import type { QueryClient } from '@tanstack/react-query';
import { gamificationKeys } from '@/features/gamification/shared/gamificationQueryKeys';
import { gachaQueryKeys } from '@/features/gacha/shared/gachaConstants';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';

const GACHA_HISTORY_QUERY_KEY = [...gachaQueryKeys.all, 'history'] as const;

export type UserStateInvalidationDomain =
 | 'wallet'
 | 'inventory'
 | 'collection'
 | 'readingSetup'
 | 'readingHistory'
 | 'profile'
 | 'readerRequest'
 | 'notifications'
 | 'chat'
 | 'gacha'
 | 'gamification';

/**
 * Central invalidation by user-state domain to prevent stale data after mutations/realtime.
 */
export async function invalidateUserStateQueries(
 queryClient: QueryClient,
 domains: readonly UserStateInvalidationDomain[],
): Promise<void> {
 const tasks: Array<Promise<void>> = [];
 const uniqueDomains = new Set(domains);

 if (uniqueDomains.has('wallet')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.wallet.all }).then(() => undefined));
 }

 if (uniqueDomains.has('inventory')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.inventory.all }).then(() => undefined));
 }

 if (uniqueDomains.has('collection')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.collection.all }).then(() => undefined));
 }

 if (uniqueDomains.has('readingSetup')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reading.setupSnapshot() }).then(() => undefined));
 }

 if (uniqueDomains.has('readingHistory')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reading.historyRoot() }).then(() => undefined));
 }

 if (uniqueDomains.has('profile')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.profile.detail() }).then(() => undefined));
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.profile.mfaStatus() }).then(() => undefined));
 }

 if (uniqueDomains.has('readerRequest')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.myRequest() }).then(() => undefined));
 }

 if (uniqueDomains.has('notifications')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.notifications.all }).then(() => undefined));
 }

 if (uniqueDomains.has('chat')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.inboxRoot() }).then(() => undefined));
  tasks.push(queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() }).then(() => undefined));
 }

 if (uniqueDomains.has('gacha')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: gachaQueryKeys.pools() }).then(() => undefined));
  tasks.push(queryClient.invalidateQueries({ queryKey: GACHA_HISTORY_QUERY_KEY }).then(() => undefined));
 }

 if (uniqueDomains.has('gamification')) {
  tasks.push(queryClient.invalidateQueries({ queryKey: gamificationKeys.all }).then(() => undefined));
 }

 await Promise.all(tasks);
}
