import { FeedPage } from '@/features/community/public';
import { cn } from '@/lib/utils';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchCommunityFeedsPage } from '@/shared/server/prefetch/runners';

export default async function CommunityIndexPage() {
 const state = await dehydrateAppQueries(prefetchCommunityFeedsPage);

 return (
  <main className={cn('min-h-screen bg-zinc-950 text-white')}>
   <AppQueryHydrationBoundary state={state}>
    <FeedPage />
   </AppQueryHydrationBoundary>
  </main>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
