
import type { ReactNode } from "react";
import AppAuthSessionManager from "@/features/auth/presentation/components/AppAuthSessionManager";
import AppNavbar from "@/features/auth/presentation/components/AppNavbar";
import WalletStoreBridge from "@/features/wallet/presentation/components/WalletStoreBridge";
import AppQueryProvider from "@/shared/components/common/AppQueryProvider";
import UserLayout from "@/shared/components/layout/UserLayout";
import MetadataInitialLoader from "@/shared/components/common/MetadataInitialLoader";

interface UserSegmentLayoutProps {
  children: ReactNode;
}

export default function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
  return (
    <AppQueryProvider>
      <AppAuthSessionManager />
      {}
      <MetadataInitialLoader />
      <WalletStoreBridge />
      <AppNavbar />
      <UserLayout>{children}</UserLayout>
    </AppQueryProvider>
  );
}
