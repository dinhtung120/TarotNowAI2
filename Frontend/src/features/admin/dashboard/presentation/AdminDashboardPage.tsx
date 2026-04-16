'use client';

import { CreditCard, Gift, History, Users } from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { useAdminDashboard } from '@/features/admin/dashboard/application/useAdminDashboard';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import {
 AdminDashboardHeader,
 AdminDashboardInsights,
 AdminDashboardLoading,
 AdminDashboardMidSection,
 AdminDashboardStatsGrid,
 type AdminStatCard,
} from '@/features/admin/dashboard/presentation/components';
import { cn } from '@/lib/utils';

export default function AdminDashboardPage() {
 const navigation = useOptimizedNavigation();
 const t = useTranslations('Admin');
 const locale = useLocale();
 const { stats, loading } = useAdminDashboard();

 const statCards: AdminStatCard[] = [
  { name: t('dashboard.stats.users'), value: stats.users, icon: Users, color: 'text-[var(--info)]', bg: 'bg-[var(--info)]/10', hoverRing: 'hover:ring-[var(--info)]/30', href: '/admin/users' },
  { name: t('dashboard.stats.deposits'), value: stats.deposits, icon: CreditCard, color: 'text-[var(--success)]', bg: 'bg-[var(--success)]/10', hoverRing: 'hover:ring-[var(--success)]/30', href: '/admin/deposits' },
  { name: t('dashboard.stats.promotions'), value: stats.promotions, icon: Gift, color: 'text-[var(--warning)]', bg: 'bg-[var(--warning)]/10', hoverRing: 'hover:ring-[var(--warning)]/30', href: '/admin/promotions' },
  { name: t('dashboard.stats.readings'), value: stats.readings, icon: History, color: 'text-[var(--purple-accent)]', bg: 'bg-[var(--purple-accent)]/10', hoverRing: 'hover:ring-[var(--purple-accent)]/30', href: '/admin/readings' },
 ];

 if (loading) {
  return <AdminDashboardLoading />;
 }

 return (
  <div className={cn('space-y-12 animate-in fade-in slide-in-from-bottom-8 duration-1000 pb-20')}>
   <AdminDashboardHeader />
   <AdminDashboardStatsGrid
    cards={statCards}
    locale={locale}
    onNavigate={(href) => navigation.push(href)}
   />
   <AdminDashboardMidSection onNavigate={(href) => navigation.push(href)} />
   <AdminDashboardInsights
    deposits={stats.deposits}
    readings={stats.readings}
    locale={locale}
   />
  </div>
 );
}
