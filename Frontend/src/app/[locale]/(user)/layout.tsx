import type { ReactNode } from 'react';
import AppNavbar from '@/features/auth/presentation/components/AppNavbar';
import WalletStoreBridge from '@/features/wallet/presentation/components/WalletStoreBridge';
import UserLayout from '@/shared/components/layout/UserLayout';
import MetadataInitialLoader from '@/shared/components/common/MetadataInitialLoader';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchUserSegmentShell } from '@/shared/server/prefetch/runners';

interface UserSegmentLayoutProps {
 children: ReactNode;
}

export default async function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
 const state = await dehydrateAppQueries(prefetchUserSegmentShell);

 return (
  <AppQueryHydrationBoundary state={state}>
   <MetadataInitialLoader />
   <WalletStoreBridge />
   <AppNavbar />
   <UserLayout>{children}</UserLayout>
  </AppQueryHydrationBoundary>
 );
}
