import InventoryPageClient from '@/components/ui/inventory/InventoryPageClient';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchInventoryPage } from '@/shared/server/prefetch/runners';

export default async function InventoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchInventoryPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <InventoryPageClient />
  </AppQueryHydrationBoundary>
 );
}
