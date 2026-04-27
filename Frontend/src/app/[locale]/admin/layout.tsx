import type { ReactNode } from 'react';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { NextIntlClientProvider } from 'next-intl';
import { getMessages, getTranslations } from 'next-intl/server';
import AdminLayoutShell, {
 type AdminLayoutLabels,
} from '@/features/admin/presentation/components/AdminLayoutShell';
import AuthBootstrap from '@/shared/components/auth/AuthBootstrap';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';
import { ADMIN_CLIENT_NAMESPACES, pickClientMessages } from '@/i18n/clientMessages';

interface AdminLayoutProps {
 children: ReactNode;
 params: Promise<{ locale: string }>;
}

export default async function AdminLayout({ children, params }: AdminLayoutProps) {
 const { locale } = await params;
 const cookieStore = await cookies();
 const hasAuthCookie = Boolean(
  cookieStore.get(AUTH_COOKIE.ACCESS)?.value || cookieStore.get(AUTH_COOKIE.REFRESH)?.value,
 );
 if (!hasAuthCookie) {
  redirect(`/${locale}/login`);
 }

 const sessionSnapshot = await getServerSessionSnapshot({ allowRefresh: true });
 if (!sessionSnapshot.authenticated || !sessionSnapshot.user) {
  redirect(`/${locale}/login`);
 }
 if (sessionSnapshot.user.role !== 'admin') {
  redirect(`/${locale}`);
 }

 const t = await getTranslations('Admin');
 const messages = await getMessages();
 const adminMessages = pickClientMessages(messages, ADMIN_CLIENT_NAMESPACES);

 const labels: AdminLayoutLabels = {
  title: t('layout.title'),
  subtitle: t('layout.subtitle'),
  sectionMain: t('layout.section_main'),
  exitPortal: t('layout.exit_portal'),
  menu: {
   overview: t('layout.menu.overview'),
   users: t('layout.menu.users'),
   deposits: t('layout.menu.deposits'),
   promotions: t('layout.menu.promotions'),
   readings: t('layout.menu.readings'),
   readerRequests: t('layout.menu.reader_requests'),
   withdrawals: t('layout.menu.withdrawals'),
   disputes: t('layout.menu.disputes'),
   systemConfigs: t('layout.menu.system_configs'),
  },
 };

 return (
  <NextIntlClientProvider messages={adminMessages}>
   <AuthBootstrap initialUser={sessionSnapshot.user} />
   <AdminLayoutShell labels={labels}>{children}</AdminLayoutShell>
  </NextIntlClientProvider>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
