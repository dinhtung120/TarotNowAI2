import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDashboardPage } from '@/app/_shared/server/prefetch/runners';

const AdminDashboardPage = dynamic(
 () => import('@/features/admin/public').then((m) => m.AdminDashboardPage),
 {
  loading: () => <AdminRouteLoadingFallback />,
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

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
