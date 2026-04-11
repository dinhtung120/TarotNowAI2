import WithdrawPage from '@/features/wallet/presentation/WithdrawPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchWithdrawPage } from '@/shared/server/prefetch/runners';

export default async function WithdrawRoutePage() {
 const state = await dehydrateAppQueries(prefetchWithdrawPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <WithdrawPage />
  </AppQueryHydrationBoundary>
 );
}
