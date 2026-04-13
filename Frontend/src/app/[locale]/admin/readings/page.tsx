import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminReadingsPage } from '@/shared/server/prefetch/runners';

const AdminReadingsPage = dynamic(
 () => import('@/features/admin/readings/presentation/AdminReadingsPage').then((m) => m.default),
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
