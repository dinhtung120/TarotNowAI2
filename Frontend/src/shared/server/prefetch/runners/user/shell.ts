import type { QueryClient } from '@tanstack/react-query';
import { getNavbarSnapshotAction } from '@/shared/application/actions/navbar-snapshot';
import { checkinQueryKeys } from '@/features/checkin/domain/checkinQueryKeys';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { swallowPrefetch } from '@/shared/server/prefetch/runners/user/shared';

/**
 * Một round-trip GET /me/navbar-snapshot → hydrate các query key mà navbar đang dùng.
 */
export async function prefetchUserSegmentShell(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['me', 'navbar-snapshot'],
   queryFn: async () => {
    const result = await getNavbarSnapshotAction();
    if (!result.success || !result.data) {
     throw new Error(result.error || 'navbar-snapshot');
    }
    const d = result.data;
    qc.setQueryData(userStateQueryKeys.notifications.unreadCount(), d.unreadNotificationCount);
    qc.setQueryData(userStateQueryKeys.chat.unreadBadge(), d.unreadChatCount);
    qc.setQueryData(checkinQueryKeys.streakStatus, d.streak);
    qc.setQueryData(userStateQueryKeys.notifications.dropdown(), d.dropdownPreview);
    return d;
   },
   staleTime: 60_000,
  });
 });
}
