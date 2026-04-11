import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDepositsPage } from '@/shared/server/prefetch/runners';

const AdminDepositsPage = dynamic(
 () => import('@/features/admin/deposits/presentation/AdminDepositsPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
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
