import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminReaderRequestsPage } from '@/shared/server/prefetch/runners';

const AdminReaderRequestsPage = dynamic(
 () => import('@/features/admin/reader-requests/presentation/AdminReaderRequestsPage').then((m) => m.default),
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

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
