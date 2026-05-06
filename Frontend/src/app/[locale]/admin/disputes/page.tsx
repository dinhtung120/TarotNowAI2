import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDisputesPage } from '@/app/_shared/server/prefetch/runners';

const AdminDisputesPage = dynamic(
 () => import('@/features/admin/public').then((m) => m.AdminDisputesPage),
 {
  loading: () => <AdminRouteLoadingFallback />,
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

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
