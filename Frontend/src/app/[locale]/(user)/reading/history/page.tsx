"use client";

import { useState, useEffect } from "react";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { getHistorySessionsAction } from "@/actions/historyActions";
import { Sparkles, Calendar, ArrowRight, Clock, Bot, ChevronLeft, ChevronRight, History } from "lucide-react";
import { useLocale, useTranslations } from "next-intl";

import { GlassCard, SectionHeader, Button } from "@/components/ui";

interface ReadingSessionDto {
 id: string;
 spreadType: string;
 isCompleted: boolean;
 createdAt: string;
}

interface HistoryResponse {
 page: number;
 pageSize: number;
 totalPages: number;
 totalCount: number;
 items: ReadingSessionDto[];
}

export default function HistoryPage() {
 const router = useRouter();
 const t = useTranslations("History");
 const tApi = useTranslations("ApiErrors");
 const locale = useLocale();
 const { isAuthenticated } = useAuthStore();
 const [historyData, setHistoryData] = useState<HistoryResponse | null>(null);
 const [isLoading, setIsLoading] = useState(true);
 const [error, setError] = useState<string | null>(null);
 const [currentPage, setCurrentPage] = useState(1);
 const [filterType, setFilterType] = useState<string>("all");
 const [filterDate, setFilterDate] = useState<string>("");
 const pageSize = 10;

 useEffect(() => {
 if (!isAuthenticated) {
 router.push("/login");
 }
 }, [isAuthenticated, router]);

 useEffect(() => {
 if (!isAuthenticated) return;

 const fetchHistory = async () => {
 setIsLoading(true);
 setError(null);
 try {
 const result = await getHistorySessionsAction(
 currentPage,
 pageSize,
 filterType,
 filterDate
 );

 if (result.error) {
 if (result.error === 'unauthorized') {
 router.push("/login");
 return;
 }
 setError(result.error);
 return;
 }

 if (result.success && result.data) {
 setHistoryData(result.data as HistoryResponse);
 }
 } catch {
 setError(tApi("network_error"));
 } finally {
 setIsLoading(false);
 }
 };

 fetchHistory();
 }, [isAuthenticated, currentPage, filterType, filterDate, router, tApi]);

 const handlePrevPage = () => {
 if (currentPage > 1) setCurrentPage(prev => prev - 1);
 };

 const handleNextPage = () => {
 if (historyData && currentPage < historyData.totalPages) setCurrentPage(prev => prev + 1);
 };

 useEffect(() => {
 setCurrentPage(1);
 }, [filterType, filterDate]);

 const getSpreadName = (type: string) => {
 const mapping: Record<string, string> = {
 'Daily1Card': t('spread_daily'),
 'daily_1': t('spread_daily'),
 'PastPresentFuture': t('spread_3'),
 'spread_3': t('spread_3'),
 'spread_5': t('spread_5'),
 'spread_10': t('spread_10'),
 };
 return mapping[type] || type;
 };

 const FiltersContent = (
 <div className="flex flex-wrap items-center gap-3">
 <div className="relative group/filter">
 <select
 value={filterType}
 onChange={(e) => setFilterType(e.target.value)}
 className="appearance-none tn-field tn-field-accent rounded-xl px-4 py-2.5 pr-10 text-[11px] font-black uppercase tracking-widest text-[var(--text-secondary)] cursor-pointer min-h-11"
 >
 <option value="all" className="tn-surface">{t('all_types')}</option>
 <option value="Daily1Card" className="tn-surface">{t('spread_daily')}</option>
 <option value="PastPresentFuture" className="tn-surface">{t('spread_3')}</option>
 <option value="spread_5" className="tn-surface">{t('spread_5')}</option>
 <option value="spread_10" className="tn-surface">{t('spread_10')}</option>
 </select>
 <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none opacity-40 group-hover/filter:opacity-80 transition-opacity">
 <Sparkles className="w-3.5 h-3.5" />
 </div>
 </div>

 <div className="relative group/filter">
 <input
 type="date"
 value={filterDate}
 onChange={(e) => setFilterDate(e.target.value)}
 className="tn-field tn-field-accent rounded-xl px-4 py-2.5 text-[11px] font-black uppercase tracking-widest text-[var(--text-secondary)] cursor-pointer min-h-11"
 />
 </div>
 </div>
 );

 return (
 <div className="max-w-5xl mx-auto px-4 sm:px-6 pt-8 pb-32 font-sans">
 <SectionHeader title={t('title')}
 subtitle={t('subtitle')}
 tag={t("tag")}
 tagIcon={<History className="w-3 h-3" />}
 action={FiltersContent}
 className="mb-12"
 />

 {error && (
 <div className="mb-8 p-4 bg-[var(--danger)]/10 border border-[var(--danger)]/20 rounded-xl flex items-center gap-3 text-[var(--danger)] text-sm animate-in zoom-in-95">
 <Clock className="w-5 h-5 flex-shrink-0" />
 <p>{error}</p>
 </div>
 )}

 {isLoading ? (
 <div className="space-y-4 animate-pulse">
 {[1, 2, 3, 4].map(i => (
 <div key={i} className="h-24 tn-surface rounded-2xl border tn-border-soft"></div>
 ))}
 </div>
 ) : historyData?.items.length === 0 ? (
 <GlassCard className="text-center py-24 animate-in fade-in duration-1000">
 <Bot className="w-16 h-16 text-[var(--text-tertiary)] mx-auto mb-6" />
 <h3 className="text-xl font-bold text-[var(--text-primary)] mb-2 tracking-tight">{t('empty_title')}</h3>
 <p className="text-[var(--text-secondary)] mb-8 max-w-sm mx-auto">{t('empty_desc')}</p>
 <Button
 variant="brand"
 onClick={() => router.push("/reading")}
 >
 {t('cta_draw')}
 </Button>
 </GlassCard>
 ) : (
 <div className="grid grid-cols-1 md:grid-cols-2 gap-4 animate-in fade-in slide-in-from-bottom-8 duration-700 delay-200">
 {historyData?.items.map((session) => (
 <GlassCard
 key={session.id}
 variant="interactive"
 onClick={() => router.push(`/reading/history/${session.id}`)}
 className="group relative flex items-center justify-between !py-4"
 >
 <div className="flex items-center gap-4">
 <div className={`w-12 h-12 rounded-2xl flex items-center justify-center border transition-all duration-500 group-hover:scale-110 ${
 session.isCompleted ? "bg-[var(--warning-bg)] border-[var(--warning)]" : "bg-[var(--bg-card)] tn-border group-hover:border-[var(--purple-accent)]/50"
 }`}>
 <Sparkles className={`w-5 h-5 ${session.isCompleted ? 'text-[var(--warning)]' : 'tn-text-muted'}`} />
 </div>
 <div>
 <h3 className="text-[15px] font-bold tn-text-primary group-hover:text-[var(--purple-accent)] transition-colors tracking-tight">
 {getSpreadName(session.spreadType)}
 </h3>
 <div className="flex items-center gap-3 mt-1.5 opacity-60">
 <span className="flex items-center text-[10px] uppercase font-black tracking-widest text-[var(--text-tertiary)]">
 <Calendar className="w-3 h-3 mr-1" />
 {new Date(session.createdAt).toLocaleDateString(locale)}
 </span>
 <span className="flex items-center text-[10px] uppercase font-black tracking-widest text-[var(--text-tertiary)]">
 <Clock className="w-3 h-3 mr-1" />
 {new Date(session.createdAt).toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit' })}
 </span>
 </div>
 </div>
 </div>

 <div className="flex items-center gap-4">
 <span className={`hidden sm:inline-flex text-[9px] px-2.5 py-1 rounded-full font-black uppercase tracking-widest border ${
 session.isCompleted
 ? 'bg-[var(--success-bg)] text-[var(--success)] border-[var(--success)]'
 : 'bg-[var(--danger-bg)] text-[var(--danger)] border-[var(--danger)]'
 }`}>
 {session.isCompleted ? t('status_completed') : t('status_interrupted')}
 </span>
 <div className="w-8 h-8 rounded-full tn-surface flex items-center justify-center group-hover:bg-[var(--purple-accent)] group-hover:tn-text-ink transition-all duration-300">
 <ArrowRight className="w-4 h-4" />
 </div>
 </div>
 </GlassCard>
 ))}
 </div>
 )}

 {/* Pagination */}
 {historyData && historyData.totalPages > 1 && (
 <div className="fixed bottom-[calc(6.5rem+env(safe-area-inset-bottom))] md:bottom-10 left-1/2 -translate-x-1/2 z-50 flex items-center gap-3 sm:gap-6 px-4 sm:px-6 py-2.5 sm:py-3 bg-[var(--bg-glass)]/90 border tn-border rounded-full shadow-[0_20px_50px_var(--c-0-0-0-80)] animate-in fade-in slide-in-from-bottom-4 duration-1000 max-w-[calc(100vw-1rem)]">
 <button
 onClick={handlePrevPage}
 disabled={currentPage === 1}
 className="p-2.5 min-h-11 min-w-11 rounded-full tn-surface tn-text-secondary disabled:opacity-20 hover:tn-surface-strong hover:tn-text-primary transition-all active:scale-90"
 title={t('prev_page')}
 >
 <ChevronLeft className="w-4 h-4" />
 </button>

 <div className="flex flex-col items-center min-w-[72px]">
 <span className="text-[9px] font-black uppercase tracking-[0.15em] sm:tracking-[0.2em] text-[var(--text-tertiary)] text-center">
 {t("page_info", { current: currentPage, total: historyData.totalPages })}
 </span>
 </div>

 <button
 onClick={handleNextPage}
 disabled={currentPage === historyData.totalPages}
 className="p-2.5 min-h-11 min-w-11 rounded-full tn-surface tn-text-secondary disabled:opacity-20 hover:tn-surface-strong hover:tn-text-primary transition-all active:scale-90"
 title={t('next_page')}
 >
 <ChevronRight className="w-4 h-4" />
 </button>
 </div>
 )}
 </div>
 );
}
