import GachaHistoryPageClient from '@/components/ui/gacha/GachaHistoryPageClient';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchGachaHistoryPage } from '@/shared/server/prefetch/runners';

export default async function GachaHistoryRoutePage() {
  const state = await dehydrateAppQueries(prefetchGachaHistoryPage);

  return (
    <AppQueryHydrationBoundary state={state}>
      <GachaHistoryPageClient />
    </AppQueryHydrationBoundary>
  );
}

