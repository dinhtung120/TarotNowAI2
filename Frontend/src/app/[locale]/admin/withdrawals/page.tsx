import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminWithdrawalsQueuePage } from '@/shared/server/prefetch/runners';

const AdminWithdrawalsPage = dynamic(
 () => import('@/features/admin/withdrawals/presentation/AdminWithdrawalsPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
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
