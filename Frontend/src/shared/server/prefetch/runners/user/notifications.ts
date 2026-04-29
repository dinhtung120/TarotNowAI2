import type { QueryClient } from '@tanstack/react-query';
import { getNotifications } from '@/features/notifications/application/actions';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

export async function prefetchNotificationsPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.notifications.list(1, false),
  queryFn: async () => {
   const result = await getNotifications(1, 20, undefined);
   return result.success ? result.data ?? null : null;
  },
 });
}
