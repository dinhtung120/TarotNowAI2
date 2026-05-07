import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminReaderRequestsPage } from '@/app/_shared/server/prefetch/runners';

const AdminReaderRequestsPage = dynamic(
 () => import('@/features/admin/reader-requests/AdminReaderRequestsPage'),
 {
  loading: () => <AdminRouteLoadingFallback />,
 }
);

export default async function AdminReaderRequestsRoutePage() {
 const state = await dehydrateAppQueries(prefetchAdminReaderRequestsPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AdminReaderRequestsPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
