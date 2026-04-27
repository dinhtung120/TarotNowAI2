import GachaPage from '@/features/gacha/presentation/GachaPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchGachaPage } from '@/shared/server/prefetch/runners';

export default async function GachaRoutePage() {
 const state = await dehydrateAppQueries(prefetchGachaPage);

  return (
  <AppQueryHydrationBoundary state={state}>
   <GachaPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
