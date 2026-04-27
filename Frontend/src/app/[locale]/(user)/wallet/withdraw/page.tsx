import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import WithdrawPage from '@/features/wallet/presentation/WithdrawPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';
import { prefetchWithdrawPage } from '@/shared/server/prefetch/runners';

export default async function WithdrawRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 const cookieStore = await cookies();
 const hasAuthCookie = Boolean(
  cookieStore.get(AUTH_COOKIE.ACCESS)?.value || cookieStore.get(AUTH_COOKIE.REFRESH)?.value,
 );
 const sessionSnapshot = hasAuthCookie
  ? await getServerSessionSnapshot({ allowRefresh: true })
  : { authenticated: false, user: null };

 if (!sessionSnapshot.authenticated || !sessionSnapshot.user) {
  redirect(`/${locale}/login`);
 }

 if (sessionSnapshot.user.role !== 'tarot_reader') {
  redirect(`/${locale}/wallet`);
 }

 const state = await dehydrateAppQueries(prefetchWithdrawPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <WithdrawPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
