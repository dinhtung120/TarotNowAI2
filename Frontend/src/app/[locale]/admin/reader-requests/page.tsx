import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminReaderRequestsPage } from '@/shared/server/prefetch/runners';

const AdminReaderRequestsPage = dynamic(
 () => import('@/features/admin/reader-requests/presentation/AdminReaderRequestsPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
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
