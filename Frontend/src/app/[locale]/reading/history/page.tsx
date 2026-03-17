"use client";

/**
 * Trang Lịch Sử Đọc Bài (Reading History)
 * Redesign: Premium & Compact.
 * - Nền Nebula & Particles huyền bí.
 * - Danh sách Glassmorphism gọn gàng.
 * - Đa ngôn ngữ hoàn toàn (i18n).
 */

import { useState, useEffect } from "react";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { getHistorySessionsAction } from "@/actions/historyActions";
import { Sparkles, Calendar, ArrowRight, BookOpen, Clock, Bot, ChevronLeft, ChevronRight } from "lucide-react";
import { useTranslations } from "next-intl";

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
                setError("Network error. Please try again later.");
            } finally {
                setIsLoading(false);
            }
        };

        fetchHistory();
    }, [isAuthenticated, currentPage, filterType, filterDate, router]);

    const handlePrevPage = () => {
        if (currentPage > 1) setCurrentPage(prev => prev - 1);
    };

    const handleNextPage = () => {
        if (historyData && currentPage < historyData.totalPages) setCurrentPage(prev => prev + 1);
    };

    // Reset page when filters change
    useEffect(() => {
        setCurrentPage(1);
    }, [filterType, filterDate]);

    // Helper map tên trải bài
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

    return (
        <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-hidden font-sans">
            {/* ##### PREMIUM BACKGROUND ##### */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                <div className="absolute top-1/4 -left-1/4 w-[60vw] h-[60vw] bg-purple-900/[0.08] blur-[120px] rounded-full animate-slow-pulse" />
                <div className="absolute bottom-1/4 -right-1/4 w-[50vw] h-[50vw] bg-indigo-900/[0.06] blur-[130px] rounded-full animate-slow-pulse delay-700" />

                {/* Spiritual Particles */}
                <div className="absolute inset-0">
                    {Array.from({ length: 18 }).map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-[1px] h-[1px] bg-white rounded-full animate-float opacity-[0.1]"
                            style={{
                                top: `${Math.random() * 100}%`,
                                left: `${Math.random() * 100}%`,
                                animationDuration: `${18 + Math.random() * 25}s`,
                                animationDelay: `${-Math.random() * 25}s`,
                            }}
                        />
                    ))}
                </div>
            </div>

            <main className="relative z-10 max-w-4xl mx-auto px-6 pt-28 pb-32">
                {/* Header Section - Compact & Filters */}
                <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 mb-14 animate-in fade-in slide-in-from-bottom-4 duration-700">
                    <div className="flex items-center gap-5">
                        <div className="w-12 h-12 bg-white/5 border border-white/10 rounded-2xl flex items-center justify-center shadow-2xl backdrop-blur-md">
                            <BookOpen className="w-6 h-6 text-purple-400" />
                        </div>
                        <div>
                            <h1 className="text-3xl md:text-4xl font-black tracking-tighter text-white">
                                {t('title')}
                            </h1>
                            <p className="text-zinc-500 text-sm font-medium leading-relaxed flex items-center gap-2">
                                <Sparkles className="w-3.5 h-3.5 text-amber-500/60" />
                                {t('subtitle')}
                            </p>
                        </div>
                    </div>

                    {/* Filters Area - Positioned to the right on Desktop */}
                    <div className="flex flex-wrap items-center gap-3">
                        {/* Spread Type Filter */}
                        <div className="relative group/filter">
                            <select
                                value={filterType}
                                onChange={(e) => setFilterType(e.target.value)}
                                className="appearance-none bg-white/[0.03] hover:bg-white/[0.06] border border-white/10 rounded-xl px-4 py-2.5 pr-10 text-[11px] font-black uppercase tracking-widest text-zinc-300 transition-all focus:ring-2 focus:ring-purple-500/40 outline-none backdrop-blur-md cursor-pointer"
                            >
                                <option value="all" className="bg-[#121212]">{t('all_types')}</option>
                                <option value="Daily1Card" className="bg-[#121212]">{t('spread_daily')}</option>
                                <option value="PastPresentFuture" className="bg-[#121212]">{t('spread_3')}</option>
                                <option value="spread_5" className="bg-[#121212]">{t('spread_5')}</option>
                                <option value="spread_10" className="bg-[#121212]">{t('spread_10')}</option>
                            </select>
                            <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none opacity-40 group-hover/filter:opacity-80 transition-opacity">
                                <Sparkles className="w-3.5 h-3.5" />
                            </div>
                        </div>

                        {/* Date Filter */}
                        <div className="relative group/filter">
                            <input
                                type="date"
                                value={filterDate}
                                onChange={(e) => setFilterDate(e.target.value)}
                                className="bg-white/[0.03] hover:bg-white/[0.06] border border-white/10 rounded-xl px-4 py-2 text-[11px] font-black uppercase tracking-widest text-zinc-300 transition-all focus:ring-2 focus:ring-purple-500/40 outline-none backdrop-blur-md cursor-pointer [color-scheme:dark]"
                            />
                        </div>
                    </div>
                </div>

                {error && (
                    <div className="mb-8 p-4 bg-red-500/10 border border-red-500/20 rounded-2xl flex items-center gap-3 text-red-400 text-xs animate-in zoom-in-95">
                        <AlertCircle className="w-4 h-4" />
                        <p className="font-medium">{error}</p>
                    </div>
                )}

                {/* Main List */}
                {isLoading ? (
                    <div className="space-y-3 animate-pulse">
                        {[1, 2, 3, 4].map(i => (
                            <div key={i} className="h-20 bg-white/[0.02] rounded-2xl border border-white/[0.05]"></div>
                        ))}
                    </div>
                ) : historyData?.items.length === 0 ? (
                    <div className="text-center py-24 bg-white/[0.01] border border-white/5 rounded-[3rem] animate-in fade-in duration-1000">
                        <Bot className="w-14 h-14 text-zinc-700 mx-auto mb-6" />
                        <h3 className="text-lg font-bold text-zinc-300 mb-2">{t('empty_title')}</h3>
                        <p className="text-zinc-500 mb-8 text-sm font-medium">{t('empty_desc')}</p>
                        <button
                            onClick={() => router.push("/reading")}
                            className="bg-white text-black px-8 py-3 rounded-full font-black text-xs uppercase tracking-widest transition-all hover:scale-105 active:scale-95 shadow-xl"
                        >
                            {t('cta_draw')}
                        </button>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4 animate-in fade-in slide-in-from-bottom-8 duration-700 delay-200">
                        {historyData?.items.map((session) => (
                            <div
                                key={session.id}
                                onClick={() => router.push(`/reading/history/${session.id}`)}
                                className="group relative bg-white/[0.03] backdrop-blur-md border border-white/5 hover:border-purple-500/30 p-4 rounded-2xl cursor-pointer transition-all duration-300 hover:shadow-[0_10px_40px_rgba(168,85,247,0.08)] flex items-center justify-between"
                            >
                                <div className="flex items-center gap-4">
                                    <div className="w-10 h-10 bg-white/[0.03] rounded-full flex items-center justify-center border border-white/5 group-hover:border-purple-500/40 transition-all duration-500 group-hover:scale-110">
                                        <Sparkles className={`w-4 h-4 ${session.isCompleted ? 'text-amber-400' : 'text-zinc-600'}`} />
                                    </div>
                                    <div>
                                        <h3 className="text-sm font-bold text-zinc-100 group-hover:text-purple-300 transition-colors tracking-tight">
                                            {getSpreadName(session.spreadType)}
                                        </h3>
                                        <div className="flex items-center gap-3 mt-1 opacity-60">
                                            <span className="flex items-center text-[10px] uppercase font-black tracking-widest text-zinc-500">
                                                <Calendar className="w-3 h-3 mr-1" />
                                                {new Date(session.createdAt).toLocaleDateString()}
                                            </span>
                                            <span className="flex items-center text-[10px] uppercase font-black tracking-widest text-zinc-500">
                                                <Clock className="w-3 h-3 mr-1" />
                                                {new Date(session.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div className="flex items-center gap-4">
                                    <span className={`hidden sm:inline-block text-[9px] px-2 py-0.5 rounded-full font-black uppercase tracking-wider border ${session.isCompleted
                                            ? 'bg-amber-500/5 text-amber-500 border-amber-500/10'
                                            : 'bg-red-500/5 text-red-500 border-red-500/10'
                                        }`}>
                                        {session.isCompleted ? t('status_completed') : t('status_interrupted')}
                                    </span>
                                    <div className="w-7 h-7 rounded-full bg-white/5 flex items-center justify-center group-hover:bg-purple-600 group-hover:text-white transition-all duration-300">
                                        <ArrowRight className="w-3.5 h-3.5" />
                                    </div>
                                </div>

                                {/* Active Shine Effect */}
                                <div className="absolute inset-0 bg-gradient-to-r from-purple-500/[0.02] to-transparent opacity-0 group-hover:opacity-100 pointer-events-none rounded-2xl transition-opacity" />
                            </div>
                        ))}

                        {/* Pagination - Sleek & Fixed at Bottom Center */}
                        {historyData && historyData.totalPages > 1 && (
                            <div className="fixed bottom-10 left-1/2 -translate-x-1/2 z-50 flex items-center gap-6 px-6 py-3 bg-black/60 backdrop-blur-xl border border-white/10 rounded-full shadow-[0_20px_50px_rgba(0,0,0,0.5)] animate-in fade-in slide-in-from-bottom-4 duration-1000">
                                <button
                                    onClick={handlePrevPage}
                                    disabled={currentPage === 1}
                                    className="p-2 rounded-full bg-white/5 text-zinc-400 disabled:opacity-20 hover:bg-white/10 hover:text-white transition-all active:scale-90"
                                    title={t('prev_page')}
                                >
                                    <ChevronLeft className="w-4 h-4" />
                                </button>

                                <div className="flex flex-col items-center min-w-[80px]">
                                    <span className="text-[9px] font-black uppercase tracking-[0.2em] text-zinc-500 mb-0.5">
                                        Arcane Index
                                    </span>
                                    <span className="text-[11px] font-bold text-white tracking-widest">
                                        {currentPage} <span className="text-zinc-600 mx-1">/</span> {historyData.totalPages}
                                    </span>
                                </div>

                                <button
                                    onClick={handleNextPage}
                                    disabled={currentPage === historyData.totalPages}
                                    className="p-2 rounded-full bg-white/5 text-zinc-400 disabled:opacity-20 hover:bg-white/10 hover:text-white transition-all active:scale-90"
                                    title={t('next_page')}
                                >
                                    <ChevronRight className="w-4 h-4" />
                                </button>
                            </div>
                        )}
                    </div>
                )}
            </main>

            {/* Custom Animations */}
            <style dangerouslySetInnerHTML={{
                __html: `
                @keyframes float {
                    0% { transform: translateY(0) translateX(0); opacity: 0; }
                    10% { opacity: 0.15; }
                    90% { opacity: 0.15; }
                    100% { transform: translateY(-100vh) translateX(30px); opacity: 0; }
                }
                .animate-float {
                    animation-name: float;
                    animation-timing-function: linear;
                    animation-iteration-count: infinite;
                }
                @keyframes slow-pulse {
                    0%, 100% { opacity: 0.05; transform: scale(1); }
                    50% { opacity: 0.1; transform: scale(1.1); }
                }
                .animate-slow-pulse {
                    animation: slow-pulse 10s ease-in-out infinite;
                }
            `}} />
        </div>
    );
}

// Thêm fallback icon lỗi
function AlertCircle({ className }: { className?: string }) {
    return <AlertCircleIcon className={className} />;
}
import { AlertCircle as AlertCircleIcon } from "lucide-react";
