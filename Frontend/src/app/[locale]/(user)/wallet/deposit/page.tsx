import { WalletDepositPage } from '@/features/wallet/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchDepositPage } from '@/shared/server/prefetch/runners';

export default async function DepositRoutePage() {
 const state = await dehydrateAppQueries(prefetchDepositPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <WalletDepositPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
