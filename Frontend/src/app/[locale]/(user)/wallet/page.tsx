import { WalletOverviewPage } from '@/features/wallet/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchWalletOverviewPage } from '@/app/_shared/server/prefetch/runners';

export default async function WalletOverviewRoutePage() {
 const state = await dehydrateAppQueries(prefetchWalletOverviewPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <WalletOverviewPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
