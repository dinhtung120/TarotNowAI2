import type { ReactNode } from "react";
import AppAuthSessionManager from "@/features/auth/presentation/components/AppAuthSessionManager";
import AppNavbar from "@/features/auth/presentation/components/AppNavbar";
import WalletStoreBridge from "@/features/wallet/presentation/components/WalletStoreBridge";
import AppQueryProvider from "@/shared/components/common/AppQueryProvider";

interface SiteLayoutProps {
 children: ReactNode;
}

export default function SiteLayout({ children }: SiteLayoutProps) {
 return (
  <AppQueryProvider>
   <AppAuthSessionManager />
   <WalletStoreBridge />
   <AppNavbar />
   {children}
  </AppQueryProvider>
 );
}
