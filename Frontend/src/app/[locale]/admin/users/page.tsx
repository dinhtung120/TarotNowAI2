import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminUsersPage } from '@/app/_shared/server/prefetch/runners';

const AdminUsersPage = dynamic(
 () => import('@/features/admin/public').then((m) => m.AdminUsersPage),
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

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
