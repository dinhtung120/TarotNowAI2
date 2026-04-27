import DepositHistoryPage from '@/features/wallet/presentation/DepositHistoryPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchDepositHistoryPage } from '@/shared/server/prefetch/runners';

export default async function DepositHistoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchDepositHistoryPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <DepositHistoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
