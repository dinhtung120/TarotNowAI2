import type { ReactNode } from "react";
import { cookies } from "next/headers";
import AppNavbar from "@/features/auth/presentation/components/AppNavbar";
import WalletStoreBridge from "@/features/wallet/presentation/components/WalletStoreBridge";
import AuthBootstrap from "@/shared/components/auth/AuthBootstrap";
import { AUTH_COOKIE } from "@/shared/infrastructure/auth/authConstants";
import { getServerSessionSnapshot } from "@/shared/infrastructure/auth/serverAuth";

interface SiteLayoutProps {
 children: ReactNode;
}

export default async function SiteLayout({ children }: SiteLayoutProps) {
 const cookieStore = await cookies();
 const hasAuthCookie = Boolean(
  cookieStore.get(AUTH_COOKIE.ACCESS)?.value || cookieStore.get(AUTH_COOKIE.REFRESH)?.value,
 );
 const sessionSnapshot = hasAuthCookie
  ? await getServerSessionSnapshot({ allowRefresh: true })
  : { authenticated: false, user: null };

 return (
  <>
   <AuthBootstrap initialUser={sessionSnapshot.user} />
   <WalletStoreBridge />
   <AppNavbar />
   {children}
  </>
 );
}
