import { ReadingHistoryPage } from '@/features/reading/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchReadingHistoryListPage } from '@/app/_shared/server/prefetch/runners';

export default async function ReadingHistoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchReadingHistoryListPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReadingHistoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
