import { InventoryPage } from '@/features/inventory/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchInventoryPage } from '@/shared/server/prefetch/runners';

export default async function InventoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchInventoryPage);

  return (
  <AppQueryHydrationBoundary state={state}>
   <InventoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
