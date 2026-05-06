import { WalletDepositHistoryPage } from '@/features/wallet/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchDepositHistoryPage } from '@/app/_shared/server/prefetch/runners';

export default async function DepositHistoryRoutePage() {
 const state = await dehydrateAppQueries(prefetchDepositHistoryPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <WalletDepositHistoryPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
