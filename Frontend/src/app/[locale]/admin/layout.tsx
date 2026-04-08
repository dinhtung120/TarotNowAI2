import type { ReactNode } from 'react';
import { getTranslations } from 'next-intl/server';
import AdminLayoutShell, {
 type AdminLayoutLabels,
} from '@/features/admin/presentation/components/AdminLayoutShell';

interface AdminLayoutProps {
 children: ReactNode;
}

export default async function AdminLayout({ children }: AdminLayoutProps) {
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
  <AdminLayoutShell labels={labels}>{children}</AdminLayoutShell>
 );
}
