import { ReadingHistoryDetailPage } from '@/features/reading/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchReadingHistoryDetailPage } from '@/app/_shared/server/prefetch/runners';

export default async function ReadingHistoryDetailRoutePage({
 params,
}: {
 params: Promise<{ id: string }>;
}) {
 const { id } = await params;
 const state = await dehydrateAppQueries((qc) => prefetchReadingHistoryDetailPage(qc, id));

 return (
  <AppQueryHydrationBoundary state={state}>
   <ReadingHistoryDetailPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
