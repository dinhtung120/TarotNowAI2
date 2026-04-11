import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDisputesPage } from '@/shared/server/prefetch/runners';

const AdminDisputesPage = dynamic(
 () => import('@/features/admin/disputes/presentation/AdminDisputesPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
 }
);

export default async function AdminDisputesRoutePage() {
 const state = await dehydrateAppQueries(prefetchAdminDisputesPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AdminDisputesPage />
  </AppQueryHydrationBoundary>
 );
}
