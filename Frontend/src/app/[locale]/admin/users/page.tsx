import dynamic from 'next/dynamic';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminUsersPage } from '@/shared/server/prefetch/runners';

const AdminUsersPage = dynamic(
 () => import('@/features/admin/users/presentation/AdminUsersPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
 }
);

export default async function AdminUsersRoutePage() {
 const state = await dehydrateAppQueries(prefetchAdminUsersPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AdminUsersPage />
  </AppQueryHydrationBoundary>
 );
}
