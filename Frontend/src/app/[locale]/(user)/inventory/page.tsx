import { InventoryPage } from '@/features/inventory/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchInventoryPage } from '@/app/_shared/server/prefetch/runners';

export default async function InventoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchInventoryPage);

  return (
  <AppQueryHydrationBoundary state={state}>
   <InventoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
