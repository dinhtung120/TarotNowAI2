import { CollectionPage } from '@/features/collection/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchCollectionPage } from '@/app/_shared/server/prefetch/runners';

export default async function CollectionRoutePage() {
 const state = await dehydrateAppQueries(prefetchCollectionPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <CollectionPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
