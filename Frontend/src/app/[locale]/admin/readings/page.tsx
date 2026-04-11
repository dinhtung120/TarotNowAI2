import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminReadingsPage } from '@/shared/server/prefetch/runners';

const AdminReadingsPage = dynamic(
 () => import('@/features/admin/readings/presentation/AdminReadingsPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
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
