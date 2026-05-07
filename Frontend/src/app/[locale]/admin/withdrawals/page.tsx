import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminWithdrawalsQueuePage } from '@/app/_shared/server/prefetch/runners';

const AdminWithdrawalsPage = dynamic(
 () => import('@/features/admin/withdrawals/AdminWithdrawalsPage'),
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

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
