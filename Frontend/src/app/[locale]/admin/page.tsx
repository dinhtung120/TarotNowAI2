import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDashboardPage } from '@/shared/server/prefetch/runners';

const AdminDashboardPage = dynamic(
 () => import('@/features/admin/dashboard/presentation/AdminDashboardPage').then((m) => m.default),
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

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
