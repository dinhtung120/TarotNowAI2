/*
 * ===================================================================
 * FILE: page.tsx (Admin Readings)
 * BỐI CẢNH (CONTEXT):
 *   Trang Admin quản lý toàn bộ Lịch sử các phiên trải bài Tarot của người dùng trong hệ thống.
 *
 * RENDERING & FILTERING:
 *   Sử dụng 'use client' để quản lý các state tìm kiếm/lọc phức tạp (theo user, ngày tháng, loại trải bài).
 *   Sử dụng Server Action (getAllHistorySessionsAdminAction) để an toàn lấy dữ liệu tránh rò rỉ JWT tới trình duyệt.
 * ===================================================================
 */
"use client";

/**
 * Trang Quản lý Lịch sử Xem bài (Admin Readings Management) - Astral Premium Redesign
 * * Các cải tiến:
 * 1. Filter Glass: Bộ lọc tìm kiếm được thiết kế lại dạng thanh ngang mờ kính chuyên nghiệp.
 * 2. Session Table: Bảng danh sách phiên xem bài mờ kính với hiệu ứng rực rỡ.
 * 3. Status Badges: Trạng thái (Completed, Processing) với nhãn phát sáng Astral.
 * 4. UX Optimization: Tối ưu hóa hiển thị câu hỏi và thông tin người dùng.
 */

import { useCallback, useEffect, useState } from "react";
import { getAllHistorySessionsAdminAction } from "@/actions/historyActions";
import { History, Search, User, Calendar, BookOpen, CheckCircle2, Clock, ChevronLeft, ChevronRight,
 Loader2,
 Sparkles,
 Eye,
 ArrowUpRight,
 Hash
} from "lucide-react";
import { SectionHeader, GlassCard, Button } from "@/components/ui";
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from "next-intl";

interface AdminReading {
 id: string;
 userId: string;
 username: string;
 spreadType: string;
 question: string | null;
 isCompleted: boolean;
 createdAt: string;
}

interface PaginatedResponse {
 page: number;
 pageSize: number;
 totalPages: number;
 totalCount: number;
 items: AdminReading[];
}

export default function AdminReadingsPage() {
 const t = useTranslations("Admin");
 const locale = useLocale();
 const [data, setData] = useState<PaginatedResponse | null>(null);
 const [loading, setLoading] = useState(true);
 const [page, setPage] = useState(1);

 // Filters state
 const [username, setUsername] = useState("");
 const [spreadType, setSpreadType] = useState("");
 const [startDate, setStartDate] = useState("");
 const [endDate, setEndDate] = useState("");

 const fetchReadings = useCallback(async (pageNum: number, currentFilters: { uname: string, type: string, start: string, end: string }) => {
 setLoading(true);
 try {
 const result = await getAllHistorySessionsAdminAction({
 page: pageNum,
 pageSize: 10,
 username: currentFilters.uname,
 spreadType: currentFilters.type,
 startDate: currentFilters.start ? new Date(currentFilters.start).toISOString() : undefined,
 endDate: currentFilters.end ? new Date(currentFilters.end).toISOString() : undefined
 });

 if (result.success && result.data) {
 setData(result.data);
 } else if (result.error === 'unauthorized') {
 toast.error(t("readings.toast.unauthorized"));
 }
 } catch (err) {
 console.error(err);
 } finally {
 setLoading(false);
 }
 }, [t]);

 useEffect(() => {
 void fetchReadings(page, { uname: username, type: spreadType, start: startDate, end: endDate });
 }, [fetchReadings, page, username, spreadType, startDate, endDate]);

 const handleSearch = (e: React.FormEvent) => {
 e.preventDefault();
 setPage(1);
 fetchReadings(1, { uname: username, type: spreadType, start: startDate, end: endDate });
 };

 const getSpreadLabel = (type: string) => {
 switch (type) {
 case "daily_1":
 return t("readings.filters.spread_daily");
 case "spread_3":
 return t("readings.filters.spread_3");
 case "spread_5":
 return t("readings.filters.spread_5");
 case "spread_10":
 return t("readings.filters.spread_10");
 default:
 return type.replace("_", " ");
 }
 };

 return (
 <div className="space-y-8 pb-20 animate-in fade-in duration-700">
 {/* Header Area */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag={t("readings.header.tag")}
 tagIcon={<Eye className="w-3 h-3 text-[var(--purple-accent)]" />}
 title={t("readings.header.title")}
 subtitle={t("readings.header.subtitle", { count: data?.totalCount || 0 })}
 className="mb-0 text-left items-start"
 />
 </div>

 {/* Premium Filter Bar */}
 <form onSubmit={handleSearch} className="p-6 rounded-[2.5rem] bg-[var(--purple-accent)]/5 border tn-border-soft shadow-inner flex flex-wrap items-end gap-6"
 >
 <div className="flex-1 min-w-[240px] space-y-3 text-left">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2">
 <User className="w-3.5 h-3.5" /> {t("readings.filters.username_label")}
 </label>
 <div className="relative">
 <input type="text" value={username}
 onChange={(e) => setUsername(e.target.value)}
 placeholder={t("readings.filters.username_placeholder")}
 className="w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all placeholder:text-[var(--text-tertiary)] shadow-inner"
 />
 </div>
 </div>

 <div className="w-56 space-y-3 text-left">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2">
 <BookOpen className="w-3.5 h-3.5" /> {t("readings.filters.spread_label")}
 </label>
 <select value={spreadType}
 onChange={(e) => setSpreadType(e.target.value)}
 className="w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all appearance-none cursor-pointer shadow-inner"
 >
 <option value="" className="tn-surface">{t("readings.filters.spread_all")}</option>
 <option value="daily_1" className="tn-surface">{t("readings.filters.spread_daily")}</option>
 <option value="spread_3" className="tn-surface">{t("readings.filters.spread_3")}</option>
 <option value="spread_5" className="tn-surface">{t("readings.filters.spread_5")}</option>
 <option value="spread_10" className="tn-surface">{t("readings.filters.spread_10")}</option>
 </select>
 </div>

 <div className="w-44 space-y-3 text-left">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2">
 <Calendar className="w-3.5 h-3.5" /> {t("readings.filters.start_date_label")}
 </label>
 <input type="date" value={startDate}
 onChange={(e) => setStartDate(e.target.value)}
 className="w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner "
 />
 </div>

 <div className="w-44 space-y-3 text-left">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2">
 <Calendar className="w-3.5 h-3.5" /> {t("readings.filters.end_date_label")}
 </label>
 <input type="date" value={endDate}
 onChange={(e) => setEndDate(e.target.value)}
 className="w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner "
 />
 </div>

 <Button type="submit"
 variant="primary"
 className="px-8 py-4 shrink-0 shadow-md flex items-center justify-center min-w-[140px]"
 >
 <Search className="w-4 h-4" />
 {t("readings.filters.submit")}
 </Button>
 </form>

 {/* Main Content Table */}
 <GlassCard className="!p-0 !rounded-[2.5rem] overflow-hidden text-left">
 <div className="overflow-x-auto custom-scrollbar">
 <table className="w-full text-left">
 <thead>
 <tr className="border-b tn-border-soft tn-surface">
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left">{t("readings.table.heading_timeline")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left">{t("readings.table.heading_user")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left">{t("readings.table.heading_spread")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left">{t("readings.table.heading_question")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center text-left">{t("readings.table.heading_status")}</th>
 </tr>
 </thead>
 <tbody className="divide-y divide-white/5">
 {loading ? (
 <tr>
 <td colSpan={5} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)] mx-auto" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("readings.states.loading")}</span>
 </div>
 </td>
 </tr>
 ) : !data || !data.items || data.items.length === 0 ? (
 <tr>
 <td colSpan={5} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <div className="w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center">
 <History className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
 </div>
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("readings.states.empty")}</span>
 </div>
 </td>
 </tr>
 ) : (
 data.items.map((r) => (
 <tr key={r.id} className="group/row hover:tn-surface transition-colors">
 <td className="px-8 py-6 whitespace-nowrap">
 <div className="flex flex-col text-left">
 <div className="text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter italic">
 {new Date(r.createdAt).toLocaleDateString(locale)}
 </div>
 <div className="text-[10px] font-bold text-[var(--text-tertiary)] italic">
 {new Date(r.createdAt).toLocaleTimeString(locale)}
 </div>
 </div>
 </td>
 <td className="px-8 py-6">
 <div className="flex items-center gap-4">
 <div className="w-10 h-10 rounded-[1rem] tn-panel-overlay flex items-center justify-center shadow-inner group-hover/row:scale-110 transition-transform">
 <User className="w-4 h-4 text-[var(--text-secondary)]" />
 </div>
 <div>
 <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">{r.username || t("readings.row.user_unknown")}</div>
 <div className="text-[9px] font-bold text-[var(--text-tertiary)] uppercase tracking-tighter flex items-center gap-1 mt-0.5">
 <Hash className="w-2.5 h-2.5 opacity-50" /> {r.userId.split('-')[0]}...
 </div>
 </div>
 </div>
 </td>
 <td className="px-8 py-6">
 <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-lg tn-panel-soft text-[10px] font-black tn-text-primary uppercase tracking-widest italic text-left shadow-inner">
 <BookOpen className="w-3.5 h-3.5 text-[var(--purple-accent)]" />
 {getSpreadLabel(r.spreadType)}
 </div>
 </td>
 <td className="px-8 py-6 max-w-[200px] truncate text-left">
 <p className="text-[11px] font-bold text-[var(--text-secondary)] italic uppercase leading-relaxed tracking-tight text-left">
 {r.question || t("readings.row.question_empty")}
 </p>
 </td>
 <td className="px-8 py-6 text-center">
 <div className={`
 inline-flex items-center gap-2 px-3 py-1.5 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all shadow-inner
 ${r.isCompleted ? "bg-[var(--success)]/10 border-[var(--success)]/30 text-[var(--success)] shadow-md" : "bg-[var(--accent)]/10 border-[var(--accent)]/30 text-[var(--accent)]"
 }
 `}>
 {r.isCompleted ? <CheckCircle2 className="w-3 h-3" /> : <Clock className="w-3 h-3" />}
 {r.isCompleted ? t("readings.status.completed") : t("readings.status.processing")}
 </div>
 </td>
 </tr>
 ))
 )}
 </tbody>
 </table>
 </div>

 {/* Pagination Expansion */}
 {data && data.totalPages > 1 && (
 <div className="px-8 py-6 tn-surface-soft flex flex-col md:flex-row md:items-center justify-between gap-4 border-t tn-border-soft">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] text-left">
 {t("readings.pagination.summary", { page: data.page, total: data.totalPages })}
 </div>
 <div className="flex items-center gap-3">
 <button
 onClick={() => setPage(p => Math.max(1, p - 1))}
 disabled={page === 1 || loading}
 className="p-3.5 rounded-2xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all shadow-inner"
 >
 <ChevronLeft className="w-4 h-4 text-[var(--text-secondary)]" />
 </button>
 <span className="text-xs font-black tn-text-primary italic mx-4 drop-shadow-sm">{page}</span>
 <button
 onClick={() => setPage(p => Math.min(data.totalPages, p + 1))}
 disabled={page === data.totalPages || loading}
 className="p-3.5 rounded-2xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all shadow-inner"
 >
 <ChevronRight className="w-4 h-4 text-[var(--text-secondary)]" />
 </button>
 </div>
 </div>
 )}
 </GlassCard>

 {/* Quick Insights Bar */}
 {!loading && data && (
 <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
 <GlassCard className="!p-8 group flex flex-col justify-between min-h-[160px] text-left hover:border-[var(--purple-accent)]/30 transition-all">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("readings.insights.total_title")}</div>
 <div className="flex items-end justify-between">
 <div className="text-4xl font-black tn-text-primary italic tracking-tighter drop-shadow-md">{data?.totalCount ?? 0}</div>
 <History className="w-12 h-12 text-[var(--purple-accent)]/20 -mb-2 -mr-2 group-hover:scale-110 group-hover:-rotate-12 transition-transform duration-500" />
 </div>
 </GlassCard>

 <GlassCard className="!p-8 flex items-center gap-6 group hover:border-[var(--success)]/30 transition-all text-left">
 <div className="w-14 h-14 rounded-2xl tn-overlay flex items-center justify-center border tn-border group-hover:scale-110 transition-transform shadow-inner">
 <Sparkles className="w-6 h-6 text-[var(--success)]" />
 </div>
 <div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("readings.insights.update_label")}</div>
 <div className="text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md mt-1">{t("readings.insights.update_value")}</div>
 </div>
 </GlassCard>

 <GlassCard className="!p-8 flex items-center justify-between group overflow-hidden relative text-left hover:border-[var(--accent)]/30 transition-all">
 <div className="relative z-10 text-left">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("readings.insights.monitor_label")}</div>
 <div className="text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md mt-1">{t("readings.insights.monitor_value")}</div>
 </div>
 <ArrowUpRight className="w-12 h-12 text-[var(--text-secondary)] absolute right-8 group-hover:text-[var(--accent)] transition-colors duration-500" />
 <div className="absolute inset-0 bg-gradient-to-tr from-transparent to-[var(--accent)]/5 translate-y-full group-hover:translate-y-0 transition-transform duration-700" />
 </GlassCard>
 </div>
 )}
 </div>
 );
}
