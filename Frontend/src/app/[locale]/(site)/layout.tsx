import type { ReactNode } from "react";
import { cookies } from "next/headers";
import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';
import { AppNavbar } from '@/features/auth/public';
import { WalletStoreBridge } from '@/features/wallet/public';
import AuthBootstrap from "@/shared/components/auth/AuthBootstrap";
import { AUTH_COOKIE } from "@/shared/infrastructure/auth/authConstants";
import { pickClientMessages, SITE_CLIENT_NAMESPACES } from '@/i18n/clientMessages';
import { getCachedServerSessionSnapshot } from '@/shared/server/auth/cachedSessionSnapshot';

interface SiteLayoutProps {
 children: ReactNode;
}

export default async function SiteLayout({ children }: SiteLayoutProps) {
 const [messages, cookieStore] = await Promise.all([getMessages(), cookies()]);
 const siteMessages = pickClientMessages(messages, SITE_CLIENT_NAMESPACES);
 const hasAuthCookie = Boolean(
  cookieStore.get(AUTH_COOKIE.ACCESS)?.value || cookieStore.get(AUTH_COOKIE.REFRESH)?.value,
 );
 const sessionSnapshot = hasAuthCookie
  ? await getCachedServerSessionSnapshot()
  : { authenticated: false, user: null };

 return (
  <NextIntlClientProvider messages={siteMessages}>
   <AuthBootstrap initialUser={sessionSnapshot.user} />
   <WalletStoreBridge />
   <AppNavbar />
   {children}
  </NextIntlClientProvider>
 );
}


export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
