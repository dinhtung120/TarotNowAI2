import GachaPageClient from '@/components/ui/gacha/GachaPageClient';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchGachaPage } from '@/shared/server/prefetch/runners';

export default async function GachaRoutePage() {
 const state = await dehydrateAppQueries(prefetchGachaPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <GachaPageClient />
  </AppQueryHydrationBoundary>
 );
}
