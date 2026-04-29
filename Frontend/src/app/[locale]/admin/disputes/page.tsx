import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchAdminDisputesPage } from '@/shared/server/prefetch/runners';

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

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
