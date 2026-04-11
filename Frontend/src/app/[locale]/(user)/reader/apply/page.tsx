import ReaderApplyPage from '@/features/reader/presentation/ReaderApplyPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchReaderApplyPage } from '@/shared/server/prefetch/runners';

export default async function ReaderApplyRoutePage() {
 const state = await dehydrateAppQueries(prefetchReaderApplyPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReaderApplyPage />
  </AppQueryHydrationBoundary>
 );
}
