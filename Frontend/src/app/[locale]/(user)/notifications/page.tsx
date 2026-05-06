import { NotificationsPage } from '@/features/notifications/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchNotificationsPage } from '@/app/_shared/server/prefetch/runners';

export default async function NotificationsRoutePage() {
 const state = await dehydrateAppQueries(prefetchNotificationsPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <NotificationsPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
