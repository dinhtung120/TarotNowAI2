import { ReaderPublicProfilePage } from '@/features/reader/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchReaderPublicProfilePage } from '@/app/_shared/server/prefetch/runners';

export default async function ReaderPublicProfileRoutePage({
 params,
}: {
 params: Promise<{ id: string }>;
}) {
 const { id } = await params;
 const state = await dehydrateAppQueries((qc) => prefetchReaderPublicProfilePage(qc, id));

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReaderPublicProfilePage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
