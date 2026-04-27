import type { QueryClient } from '@tanstack/react-query';
import { getNotifications } from '@/features/notifications/application/actions';

export async function prefetchNotificationsPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: ['notifications', 1, false],
  queryFn: async () => {
   const result = await getNotifications(1, 20, undefined);
   return result.success ? result.data ?? null : null;
  },
 });
}
