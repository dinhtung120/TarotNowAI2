
import type { ReactNode } from "react";
import AppNavbar from "@/features/auth/presentation/components/AppNavbar";
import WalletStoreBridge from "@/features/wallet/presentation/components/WalletStoreBridge";
import UserLayout from "@/shared/components/layout/UserLayout";
import MetadataInitialLoader from "@/shared/components/common/MetadataInitialLoader";

interface UserSegmentLayoutProps {
  children: ReactNode;
}

export default function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
  return (
    <>
      {}
      <MetadataInitialLoader />
      <WalletStoreBridge />
      <AppNavbar />
      <UserLayout>{children}</UserLayout>
    </>
  );
}
