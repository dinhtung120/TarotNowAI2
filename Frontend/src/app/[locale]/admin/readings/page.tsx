import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminReadingsPage } from '@/app/_shared/server/prefetch/runners';

const AdminReadingsPage = dynamic(
 () => import('@/features/admin/public').then((m) => m.AdminReadingsPage),
 {
  loading: () => <AdminRouteLoadingFallback />,
 }
);

export default async function AdminReadingsRoutePage() {
 const state = await dehydrateAppQueries(prefetchAdminReadingsPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AdminReadingsPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
