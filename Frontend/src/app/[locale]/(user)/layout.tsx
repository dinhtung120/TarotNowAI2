import { Suspense, type ReactNode } from 'react';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';
import AppNavbar from '@/features/auth/presentation/components/AppNavbar';
import WalletStoreBridge from '@/features/wallet/presentation/components/WalletStoreBridge';
import AuthBootstrap from '@/shared/components/auth/AuthBootstrap';
import UserLayout from '@/shared/components/layout/UserLayout';
import MetadataInitialLoader from '@/shared/components/common/MetadataInitialLoader';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';
import { UserSegmentMainSkeleton } from '@/shared/components/loading/segment-skeletons';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchUserSegmentShell } from '@/shared/server/prefetch/runners';
import { pickClientMessages, USER_CLIENT_NAMESPACES } from '@/i18n/clientMessages';

interface UserSegmentLayoutProps {
 children: ReactNode;
 params: Promise<{ locale: string }>;
}

export default async function UserSegmentLayout({ children, params }: UserSegmentLayoutProps) {
 const { locale } = await params;
 const cookieStore = await cookies();
 const hasAuthCookie = Boolean(
  cookieStore.get(AUTH_COOKIE.ACCESS)?.value || cookieStore.get(AUTH_COOKIE.REFRESH)?.value,
 );
 if (!hasAuthCookie) {
  redirect(`/${locale}/login`);
 }

 const sessionSnapshot = await getServerSessionSnapshot({ allowRefresh: true });
 if (!sessionSnapshot.authenticated || !sessionSnapshot.user) {
  redirect(`/${locale}/login`);
 }

 const state = await dehydrateAppQueries(prefetchUserSegmentShell);
 const messages = await getMessages();
 const userMessages = pickClientMessages(messages, USER_CLIENT_NAMESPACES);

 return (
  <AppQueryHydrationBoundary state={state}>
   <NextIntlClientProvider messages={userMessages}>
    <AuthBootstrap initialUser={sessionSnapshot.user} />
    <MetadataInitialLoader />
    <WalletStoreBridge />
    <AppNavbar />
    <UserLayout>
     <Suspense fallback={<UserSegmentMainSkeleton />}>{children}</Suspense>
    </UserLayout>
   </NextIntlClientProvider>
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
