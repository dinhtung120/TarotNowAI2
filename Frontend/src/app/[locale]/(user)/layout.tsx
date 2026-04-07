
import type { ReactNode } from "react";
import AppAuthSessionManager from "@/features/auth/presentation/components/AppAuthSessionManager";
import AppNavbar from "@/features/auth/presentation/components/AppNavbar";
import WalletStoreBridge from "@/features/wallet/presentation/components/WalletStoreBridge";
import AppQueryProvider from "@/shared/components/common/AppQueryProvider";
import UserLayout from "@/shared/components/layout/UserLayout";
import MetadataInitialLoader from "@/shared/components/common/MetadataInitialLoader";
import { getInitialMetadata } from "@/shared/application/actions/metadata";

interface UserSegmentLayoutProps {
  children: ReactNode;
}

export default async function UserSegmentLayout({ children }: UserSegmentLayoutProps) {
  const metadataResult = await getInitialMetadata();
  const initialMetadata = metadataResult.success ? metadataResult.data : null;

  return (
    <AppQueryProvider>
      <AppAuthSessionManager />
      {}
      <MetadataInitialLoader initialMetadata={initialMetadata} />
      <WalletStoreBridge />
      <AppNavbar />
      <UserLayout>{children}</UserLayout>
    </AppQueryProvider>
  );
}
