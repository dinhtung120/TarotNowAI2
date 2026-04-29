import { ReadersDirectoryPage } from '@/features/reader/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchReadersDirectoryPage } from '@/shared/server/prefetch/runners';

export default async function ReadersDirectoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchReadersDirectoryPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReadersDirectoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
