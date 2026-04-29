import { redirect } from 'next/navigation';
import { WalletWithdrawPage } from '@/features/wallet/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchWithdrawPage } from '@/shared/server/prefetch/runners';
import { requireSessionWithHandshake } from '@/shared/server/auth/sessionHandshake';

export default async function WithdrawRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 const sessionSnapshot = await requireSessionWithHandshake({
  locale,
  nextPath: `/${locale}/wallet/withdraw`,
 });

 if (sessionSnapshot.user.role !== 'tarot_reader') {
  redirect(`/${locale}/wallet`);
 }

 const state = await dehydrateAppQueries(prefetchWithdrawPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <WalletWithdrawPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
