import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDashboardPage } from '@/shared/server/prefetch/runners';

const AdminDashboardPage = dynamic(
 () => import('@/features/admin/dashboard/presentation/AdminDashboardPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
 }
);

export default async function AdminDashboardRoutePage() {
 const state = await dehydrateAppQueries(prefetchAdminDashboardPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AdminDashboardPage />
  </AppQueryHydrationBoundary>
 );
}
