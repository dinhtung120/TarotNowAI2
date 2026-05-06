import { Suspense, type ReactNode } from 'react';
import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';
import { AppNavbar, AuthBootstrap } from '@/features/auth/public';
import { WalletStoreBridge } from '@/features/wallet/public';
import UserLayout from '@/shared/app-shell/layout/UserLayout';
import { UserSegmentMainSkeleton } from '@/shared/app-shell/loading/segment-skeletons';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchUserSegmentShell } from '@/shared/server/prefetch/runners';
import { pickClientMessages, USER_CLIENT_NAMESPACES } from '@/i18n/clientMessages';
import { requireSessionWithHandshake } from '@/shared/server/auth/sessionHandshake';

interface UserSegmentLayoutProps {
 children: ReactNode;
 params: Promise<{ locale: string }>;
}

export default async function UserSegmentLayout({ children, params }: UserSegmentLayoutProps) {
 const { locale } = await params;
 const sessionSnapshot = await requireSessionWithHandshake({
  locale,
  nextPath: `/${locale}`,
 });

 const [state, messages] = await Promise.all([
  dehydrateAppQueries(prefetchUserSegmentShell),
  getMessages(),
 ]);
 const userMessages = pickClientMessages(messages, USER_CLIENT_NAMESPACES);

 return (
  <AppQueryHydrationBoundary state={state}>
   <NextIntlClientProvider messages={userMessages}>
    <AuthBootstrap initialUser={sessionSnapshot.user} />
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
