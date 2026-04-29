import { ReadingHistoryPage } from '@/features/reading/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchReadingHistoryListPage } from '@/shared/server/prefetch/runners';

export default async function ReadingHistoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchReadingHistoryListPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReadingHistoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
