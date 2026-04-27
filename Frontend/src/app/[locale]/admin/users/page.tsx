import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminUsersPage } from '@/shared/server/prefetch/runners';

const AdminUsersPage = dynamic(
 () => import('@/features/admin/users/presentation/AdminUsersPage').then((m) => m.default),
 {
  loading: () => <AdminRouteLoadingFallback />,
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

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
