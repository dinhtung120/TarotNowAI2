import HistoryPage from '@/features/reading/history/presentation/HistoryPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchReadingHistoryListPage } from '@/shared/server/prefetch/runners';

export default async function ReadingHistoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchReadingHistoryListPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <HistoryPage />
  </AppQueryHydrationBoundary>
 );
}
