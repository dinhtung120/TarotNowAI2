import WalletOverviewPage from '@/features/wallet/presentation/WalletOverviewPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchWalletOverviewPage } from '@/shared/server/prefetch/runners';

export default async function WalletOverviewRoutePage() {
 const state = await dehydrateAppQueries(prefetchWalletOverviewPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <WalletOverviewPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
