import type { ReactNode } from 'react';
import { redirect } from 'next/navigation';
import { NextIntlClientProvider } from 'next-intl';
import { getMessages, getTranslations } from 'next-intl/server';
import {
 AdminLayoutShell,
 type AdminLayoutLabels,
} from '@/features/admin/public';
import AuthBootstrap from '@/shared/components/auth/AuthBootstrap';
import { ADMIN_CLIENT_NAMESPACES, pickClientMessages } from '@/i18n/clientMessages';
import { requireSessionWithHandshake } from '@/shared/server/auth/sessionHandshake';

interface AdminLayoutProps {
 children: ReactNode;
 params: Promise<{ locale: string }>;
}

export default async function AdminLayout({ children, params }: AdminLayoutProps) {
 const { locale } = await params;
 const sessionSnapshot = await requireSessionWithHandshake({
  locale,
  nextPath: `/${locale}/admin`,
 });
 if (sessionSnapshot.user.role !== 'admin') {
  redirect(`/${locale}`);
 }

 const [t, messages] = await Promise.all([
  getTranslations('Admin'),
  getMessages(),
 ]);
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
