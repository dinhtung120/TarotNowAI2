import type { ReactNode } from 'react';
import { cookies } from 'next/headers';
import { getTranslations } from 'next-intl/server';
import AdminLayoutShell, {
 type AdminLayoutLabels,
} from '@/features/admin/presentation/components/AdminLayoutShell';
import AuthBootstrap from '@/shared/components/auth/AuthBootstrap';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';

interface AdminLayoutProps {
 children: ReactNode;
}

export default async function AdminLayout({ children }: AdminLayoutProps) {
 const cookieStore = await cookies();
 const hasAccessToken = Boolean(cookieStore.get(AUTH_COOKIE.ACCESS)?.value);
 const sessionSnapshot = hasAccessToken
  ? await getServerSessionSnapshot({ allowRefresh: false })
  : { authenticated: false, user: null };
 const t = await getTranslations('Admin');

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
  },
 };

 return (
  <>
   <AuthBootstrap initialUser={sessionSnapshot.user} />
   <AdminLayoutShell labels={labels}>{children}</AdminLayoutShell>
  </>
 );
}
