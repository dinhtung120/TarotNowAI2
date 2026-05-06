import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDepositsPage } from '@/app/_shared/server/prefetch/runners';

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

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
