import { Suspense, type ReactNode } from 'react';
import { cookies } from 'next/headers';
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

interface UserSegmentLayoutProps {
 children: ReactNode;
}

export default async function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
 const cookieStore = await cookies();
 const hasAccessToken = Boolean(cookieStore.get(AUTH_COOKIE.ACCESS)?.value);
 const sessionSnapshot = hasAccessToken
  ? await getServerSessionSnapshot({ allowRefresh: false })
  : { authenticated: false, user: null };
 const state = await dehydrateAppQueries(prefetchUserSegmentShell);

 return (
  <AppQueryHydrationBoundary state={state}>
   <AuthBootstrap initialUser={sessionSnapshot.user} />
   <MetadataInitialLoader />
   <WalletStoreBridge />
   <AppNavbar />
   <UserLayout>
    <Suspense fallback={<UserSegmentMainSkeleton />}>{children}</Suspense>
   </UserLayout>
  </AppQueryHydrationBoundary>
 );
}
