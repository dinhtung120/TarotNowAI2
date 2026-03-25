import type { ReactNode } from "react";
import AppAuthSessionManager from "@/features/auth/presentation/components/AppAuthSessionManager";
import AppNavbar from "@/features/auth/presentation/components/AppNavbar";
import WalletStoreBridge from "@/features/wallet/presentation/components/WalletStoreBridge";

interface SiteLayoutProps {
 children: ReactNode;
}

export default function SiteLayout({ children }: SiteLayoutProps) {
 return (
  <>
   <AppAuthSessionManager />
   <WalletStoreBridge />
   <AppNavbar />
   {children}
  </>
 );
}
