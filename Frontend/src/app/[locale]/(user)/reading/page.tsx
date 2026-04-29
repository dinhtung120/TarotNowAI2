import { ReadingSetupPage } from '@/features/reading/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchReadingSetupPage } from '@/shared/server/prefetch/runners';

export default async function ReadingSetupRoutePage() {
 const state = await dehydrateAppQueries(prefetchReadingSetupPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReadingSetupPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
