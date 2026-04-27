import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminWithdrawalsQueuePage } from '@/shared/server/prefetch/runners';

const AdminWithdrawalsPage = dynamic(
 () => import('@/features/admin/withdrawals/presentation/AdminWithdrawalsPage').then((m) => m.default),
 {
  loading: () => <AdminRouteLoadingFallback />,
 }
);

export default async function AdminWithdrawalsRoutePage() {
 const state = await dehydrateAppQueries(prefetchAdminWithdrawalsQueuePage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AdminWithdrawalsPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
