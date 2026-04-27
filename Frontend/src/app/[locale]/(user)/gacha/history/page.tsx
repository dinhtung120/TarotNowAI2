import GachaHistoryPage from '@/features/gacha/presentation/GachaHistoryPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchGachaHistoryPage } from '@/shared/server/prefetch/runners';

export default async function GachaHistoryRoutePage() {
  const state = await dehydrateAppQueries(prefetchGachaHistoryPage);

  return (
  <AppQueryHydrationBoundary state={state}>
   <GachaHistoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
