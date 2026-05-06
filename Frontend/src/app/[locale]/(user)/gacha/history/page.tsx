import { GachaHistoryPage } from '@/features/gacha/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchGachaHistoryPage } from '@/app/_shared/server/prefetch/runners';

export default async function GachaHistoryRoutePage() {
  const state = await dehydrateAppQueries(prefetchGachaHistoryPage);

  return (
  <AppQueryHydrationBoundary state={state}>
   <GachaHistoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
