import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDepositsPage } from '@/shared/server/prefetch/runners';

const AdminDepositsPage = dynamic(
 () => import('@/features/admin/public').then((m) => m.AdminDepositsPage),
 {
  loading: () => <AdminRouteLoadingFallback />,
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

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
