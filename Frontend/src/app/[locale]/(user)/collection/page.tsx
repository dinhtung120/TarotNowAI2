import { CollectionPage } from '@/features/collection/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchCollectionPage } from '@/shared/server/prefetch/runners';

export default async function CollectionRoutePage() {
 const state = await dehydrateAppQueries(prefetchCollectionPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <CollectionPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
