import type { QueryClient } from '@tanstack/react-query';
import { checkinQueryKeys } from '@/features/checkin/streak/checkinQueryKeys';
import { getInitialMetadata } from '@/shared/application/actions/metadata';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { swallowPrefetch } from '@/shared/server/prefetch/runners/user/shared';

/**
 * Một round-trip GET /user-context/metadata → hydrate shell query keys trước khi client mount.
 */
export async function prefetchUserSegmentShell(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  const result = await getInitialMetadata();
  if (!result.success || !result.data) {
   throw new Error(result.error || 'user-context-metadata');
  }

  const data = result.data;
  qc.setQueryData(userStateQueryKeys.wallet.balance(), data.wallet);
  qc.setQueryData(checkinQueryKeys.streakStatus, data.streak);
  qc.setQueryData(userStateQueryKeys.notifications.unreadCount(), data.unreadNotificationCount);
  qc.setQueryData(userStateQueryKeys.notifications.dropdown(), data.recentNotifications);
  qc.setQueryData(userStateQueryKeys.chat.unreadBadge(), data.unreadChatCount);
  qc.setQueryData(userStateQueryKeys.chat.inboxActive(), data.activeConversations);
  qc.setQueryData(userStateQueryKeys.chat.inboxPreview(), {
   ...data.activeConversations,
   conversations: data.activeConversations.conversations.slice(0, 8),
  });
 });
}
