import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDepositsPage } from '@/shared/server/prefetch/runners';

const AdminDepositsPage = dynamic(
 () => import('@/features/admin/deposits/presentation/AdminDepositsPage').then((m) => m.default),
 {
  loading: () => <AdminRouteLoadingFallback />,
 }
);

export default async function AdminDepositsRoutePage() {
 const state = await dehydrateAppQueries(prefetchAdminDepositsPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AdminDepositsPage />
  </AppQueryHydrationBoundary>
 );
}
