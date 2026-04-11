import ReadingSetupPage from '@/features/reading/presentation/ReadingSetupPage';
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
