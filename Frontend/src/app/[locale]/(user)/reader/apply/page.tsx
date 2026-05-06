import { ReaderApplyPage } from '@/features/reader/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchReaderApplyPage } from '@/app/_shared/server/prefetch/runners';

export default async function ReaderApplyRoutePage() {
 const state = await dehydrateAppQueries(prefetchReaderApplyPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReaderApplyPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
