import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminReadingsPage } from '@/shared/server/prefetch/runners';

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

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
