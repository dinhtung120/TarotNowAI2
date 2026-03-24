/*
 * ===================================================================
 * FILE: page.tsx (Admin Dashboard)
 * BỐI CẢNH (CONTEXT):
 *   Trang chủ của khu vực Quản trị viên (Admin). 
 *   Hiển thị tổng quan các thống kê số liệu (Users, Deposits, Promotions, Readings).
 *
 * RENDERING:
 *   Sử dụng 'use client' vì có tương tác state cục bộ và fetch dữ liệu 
 *   trực tiếp bằng Server Actions (React Server Actions) trong useEffect.
 * ===================================================================
 */
'use client';

import React from 'react';
import {
 Users,
 CreditCard,
 Gift,
 History,
 TrendingUp,
 ShieldCheck,
 Activity,
 Zap,
 Gem,
 ChevronRight,
 Loader2
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { SectionHeader, GlassCard } from '@/shared/components/ui';
import { useLocale, useTranslations } from 'next-intl';
import { useAdminDashboard } from '@/features/admin/dashboard/application/useAdminDashboard';

type AdminRoute =
 | '/admin/users'
 | '/admin/deposits'
 | '/admin/promotions'
 | '/admin/readings';

export default function AdminDashboardPage() {
 const router = useRouter();
 const t = useTranslations("Admin");
 const locale = useLocale();
 const { stats, loading } = useAdminDashboard();

 const statCards: Array<{
 name: string;
 value: number;
 icon: typeof Users;
 color: string;
 bg: string;
 hoverRing: string;
 href: AdminRoute;
 }> = [
 {
 name: t("dashboard.stats.users"),
 value: stats.users,
 icon: Users,
 color: "text-[var(--info)]",
 bg: "bg-[var(--info)]/10",
 hoverRing: "hover:ring-[var(--info)]/30",
 href: "/admin/users"
 },
 {
 name: t("dashboard.stats.deposits"),
 value: stats.deposits,
 icon: CreditCard,
 color: "text-[var(--success)]",
 bg: "bg-[var(--success)]/10",
 hoverRing: "hover:ring-[var(--success)]/30",
 href: "/admin/deposits"
 },
 {
 name: t("dashboard.stats.promotions"),
 value: stats.promotions,
 icon: Gift,
 color: "text-[var(--warning)]",
 bg: "bg-[var(--warning)]/10",
 hoverRing: "hover:ring-[var(--warning)]/30",
 href: "/admin/promotions"
 },
 {
 name: t("dashboard.stats.readings"),
 value: stats.readings,
 icon: History,
 color: "text-[var(--purple-accent)]",
 bg: "bg-[var(--purple-accent)]/10",
 hoverRing: "hover:ring-[var(--purple-accent)]/30",
 href: "/admin/readings"
 },
 ];

 if (loading) {
 return (
 <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
 <div className="relative group">
 <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--danger)]/20 blur-[60px] rounded-full animate-pulse" />
 <Loader2 className="w-12 h-12 animate-spin text-[var(--danger)] relative z-10" />
 </div>
 <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">{t("dashboard.loading")}</div>
 </div>
 );
 }

 return (
 <div className="space-y-12 animate-in fade-in slide-in-from-bottom-8 duration-1000 pb-20">
 {/* Header Section */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag={t("dashboard.header.tag")}
 tagIcon={<TrendingUp className="w-3 h-3 text-[var(--success)]" />}
 title={t("dashboard.header.title")}
 subtitle={t("dashboard.header.subtitle")}
 className="mb-0 text-left items-start"
 />

 <div className="flex items-center gap-4 shrink-0">
 <div className="px-6 py-3 rounded-2xl tn-panel ">
 <div className="text-[8px] font-black uppercase tracking-widest tn-text-muted text-left">{t("dashboard.health.title")}</div>
 <div className="flex items-center gap-2 text-xs font-bold text-[var(--success)]">
 <Activity className="w-3 h-3 animate-pulse" />
 {t("dashboard.health.ok")}
 </div>
 </div>
 </div>
 </div>

 {/* Quick Stats Grid */}
 <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
 {statCards.map((stat) => (
 <button
 key={stat.name}
 onClick={() => router.push(stat.href)}
 className={`group relative h-44 tn-surface hover:tn-surface-strong rounded-[2.5rem] border tn-border-soft p-8 text-left transition-all duration-500 shadow-xl overflow-hidden ring-1 ring-transparent ${stat.hoverRing}`}
 >
 <div className="absolute top-0 right-0 p-6 opacity-[0.05] group-hover:opacity-[0.08] transition-all duration-500 group-hover:scale-110 group-hover:rotate-12 translate-x-4 -translate-y-4">
 <stat.icon size={120} />
 </div>

 <div className="relative z-10 h-full flex flex-col justify-between">
 <div className={`w-10 h-10 rounded-xl ${stat.bg} border tn-border-soft flex items-center justify-center transition-transform group-hover:scale-110`}>
 <stat.icon className={`w-5 h-5 ${stat.color}`} />
 </div>

 <div>
 <div className="text-4xl font-black tn-text-primary italic tracking-tighter mb-1 drop-shadow-md">
 {stat.value.toLocaleString(locale)}
 </div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
 {stat.name}
 </div>
 </div>
 </div>
 </button>
 ))}
 </div>

 {/* Mid Section: Insight & Actions */}
 <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
 {/* System Notice - Gradient Card */}
 <GlassCard className="lg:col-span-2 relative group overflow-hidden !rounded-[3rem] border-[var(--purple-accent)]/20 !p-10 !bg-gradient-to-br from-[var(--purple-accent)]/10 to-transparent">
 <div className="absolute top-0 right-0 p-10 opacity-10 pointer-events-none group-hover:scale-110 transition-transform duration-1000">
 <Zap size={200} className="text-[var(--purple-accent)]" />
 </div>

 <div className="relative z-10 space-y-6">
 <div className="flex items-center gap-3">
 <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/20 flex items-center justify-center border border-[var(--purple-accent)]/20 shadow-inner">
 <ShieldCheck className="w-5 h-5 text-[var(--purple-accent)]" />
 </div>
 <h2 className="text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md">{t("dashboard.notice.title")}</h2>
 </div>

 <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
 <div className="p-6 rounded-2xl tn-panel-overlay-soft space-y-2 shadow-inner">
 <div className="text-[10px] font-black text-[var(--success)] uppercase tracking-widest flex items-center gap-2">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--success)] animate-pulse" /> {t("dashboard.notice.reminder_title")}
 </div>
 <p className="text-xs text-[var(--text-secondary)] font-medium leading-relaxed text-left">{t("dashboard.notice.reminder_body")}</p>
 </div>
 <div className="p-6 rounded-2xl tn-panel-overlay-soft space-y-2 shadow-inner">
 <div className="text-[10px] font-black text-[var(--warning)] uppercase tracking-widest flex items-center gap-2">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--warning)] animate-pulse" /> {t("dashboard.notice.announcement_title")}
 </div>
 <p className="text-xs text-[var(--text-secondary)] font-medium leading-relaxed text-left">{t("dashboard.notice.announcement_body")}</p>
 </div>
 </div>
 </div>
 </GlassCard>

 {/* Quick Shortcuts */}
 <div className="space-y-6 text-left">
 <h2 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter flex items-center gap-3 drop-shadow-md">
 <Zap className="w-5 h-5 text-[var(--success)]" />
 {t("dashboard.shortcuts.title")}
 </h2>

 <div className="space-y-4">
 <button
 onClick={() => router.push('/admin/users')}
 className="w-full flex items-center justify-between p-5 rounded-2xl tn-panel-soft hover:tn-surface-strong hover:border-[var(--purple-accent)]/30 transition-all group shadow-sm hover:shadow-md"
 >
 <div className="flex items-center gap-4">
 <Users className="w-5 h-5 text-[var(--text-tertiary)] group-hover:text-[var(--purple-accent)] transition-colors" />
 <span className="text-xs font-black uppercase tracking-widest text-[var(--text-secondary)] group-hover:tn-text-primary transition-colors">{t("dashboard.shortcuts.users")}</span>
 </div>
 <ChevronRight className="w-4 h-4 tn-text-muted group-hover:text-[var(--purple-accent)] transition-colors" />
 </button>

 <button
 onClick={() => router.push('/admin/deposits')}
 className="w-full flex items-center justify-between p-5 rounded-2xl tn-panel-soft hover:tn-surface-strong hover:border-[var(--success)]/30 transition-all group shadow-sm hover:shadow-md"
 >
 <div className="flex items-center gap-4">
 <CreditCard className="w-5 h-5 text-[var(--text-tertiary)] group-hover:text-[var(--success)] transition-colors" />
 <span className="text-xs font-black uppercase tracking-widest text-[var(--text-secondary)] group-hover:tn-text-primary transition-colors">{t("dashboard.shortcuts.deposits")}</span>
 </div>
 <ChevronRight className="w-4 h-4 tn-text-muted group-hover:text-[var(--success)] transition-colors" />
 </button>

 <button
 onClick={() => router.push('/admin/readings')}
 className="w-full flex items-center justify-between p-5 rounded-2xl tn-panel-soft hover:tn-surface-strong hover:border-[var(--warning)]/30 transition-all group shadow-sm hover:shadow-md"
 >
 <div className="flex items-center gap-4">
 <History className="w-5 h-5 text-[var(--text-tertiary)] group-hover:text-[var(--warning)] transition-colors" />
 <span className="text-xs font-black uppercase tracking-widest text-[var(--text-secondary)] group-hover:tn-text-primary transition-colors">{t("dashboard.shortcuts.readings")}</span>
 </div>
 <ChevronRight className="w-4 h-4 tn-text-muted group-hover:text-[var(--warning)] transition-colors" />
 </button>

 <button
 onClick={() => router.push('/admin/reader-requests')}
 className="w-full flex items-center justify-between p-5 rounded-2xl tn-panel-soft hover:tn-surface-strong hover:border-[var(--danger)]/30 transition-all group shadow-sm hover:shadow-md"
 >
 <div className="flex items-center gap-4">
 <ShieldCheck className="w-5 h-5 text-[var(--text-tertiary)] group-hover:text-[var(--danger)] transition-colors" />
 <span className="text-xs font-black uppercase tracking-widest text-[var(--text-secondary)] group-hover:tn-text-primary transition-colors">{t("dashboard.shortcuts.reader_requests")}</span>
 </div>
 <ChevronRight className="w-4 h-4 tn-text-muted group-hover:text-[var(--danger)] transition-colors" />
 </button>
 </div>
 </div>
 </div>

 {/* Revenue & Activity Insight */}
 <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
 <GlassCard className="!p-10 !rounded-[3rem] relative overflow-hidden group">
 <div className="absolute top-0 right-0 p-8 opacity-[0.03] group-hover:scale-110 transition-transform duration-700 blur-sm group-hover:blur-none">
 <Gem size={160} className="text-[var(--purple-accent)]" />
 </div>
 <div className="relative z-10 space-y-6">
 <div className="flex items-center gap-4">
 <div className="w-12 h-12 rounded-2xl bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 flex items-center justify-center shadow-inner">
 <TrendingUp className="w-6 h-6 text-[var(--purple-accent)]" />
 </div>
 <h3 className="text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md">{t("dashboard.insights.revenue_title")}</h3>
 </div>

 <div className="flex items-end gap-4">
 <div className="text-5xl md:text-6xl font-black tn-text-primary italic tracking-tighter drop-shadow-[0_0_15px_var(--c-168-85-247-40)]">
 {(stats.deposits * 50000).toLocaleString(locale)}{" "}
 <span className="text-2xl text-[var(--purple-accent)]">{t("dashboard.insights.revenue_currency")}</span>
 </div>
 </div>
 <p className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)]">
 {t("dashboard.insights.revenue_note", { count: stats.deposits })}
 </p>
 </div>
 </GlassCard>

 <GlassCard className="!p-10 !rounded-[3rem] relative overflow-hidden group">
 <div className="absolute top-0 right-0 p-8 opacity-[0.03] group-hover:scale-110 transition-transform duration-700 blur-sm group-hover:blur-none">
 <Activity size={160} className="text-[var(--success)]" />
 </div>
 <div className="relative z-10 space-y-6">
 <div className="flex items-center gap-4">
 <div className="w-12 h-12 rounded-2xl bg-[var(--success)]/10 border border-[var(--success)]/20 flex items-center justify-center shadow-inner">
 <Zap className="w-6 h-6 text-[var(--success)]" />
 </div>
 <h3 className="text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md">{t("dashboard.insights.activity_title")}</h3>
 </div>

 <div className="flex items-end gap-4">
 <div className="text-5xl md:text-6xl font-black tn-text-primary italic tracking-tighter drop-shadow-[0_0_15px_var(--c-16-185-129-40)]">
 {stats.readings.toLocaleString(locale)} <span className="text-2xl text-[var(--success)]">{t("dashboard.insights.activity_unit")}</span>
 </div>
 </div>
 <p className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)]">
 {t("dashboard.insights.activity_note")}
 </p>
 </div>
 </GlassCard>
 </div>
 </div>
 );
}
