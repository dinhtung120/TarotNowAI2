import { ReaderApplyPage } from '@/features/reader/public';
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

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
