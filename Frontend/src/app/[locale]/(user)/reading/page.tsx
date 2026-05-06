import { ReadingSetupPage } from '@/features/reading/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchReadingSetupPage } from '@/app/_shared/server/prefetch/runners';

export default async function ReadingSetupRoutePage() {
 const state = await dehydrateAppQueries(prefetchReadingSetupPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReadingSetupPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
