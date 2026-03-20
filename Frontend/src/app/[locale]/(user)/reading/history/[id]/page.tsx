/*
 * ===================================================================
 * FILE: (user)/reading/history/[id]/page.tsx (Chi tiết Lịch sử Trải bài)
 * BỐI CẢNH (CONTEXT):
 *   Trang hiển thị chi tiết một phiên trải bài Tarot đã hoàn thành hoặc bị gián đoạn.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Lấy thông tin phiên từ API `getHistoryDetailAction`.
 *   - Hiển thị danh sách thẻ bài đã bốc (`cardsDrawn`) với hiệu ứng lật thẻ/ánh sáng.
 *   - Tóm tắt hoạt động AI ở cuối trang (nếu trạng thái isCompleted = true).
 * ===================================================================
 */
"use client";

import { useMemo, useState, useEffect } from "react";
import { useParams } from "next/navigation";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { getHistoryDetailAction } from "@/actions/historyActions";
import { Sparkles, ArrowLeft, Bot, Calendar, Clock, AlertCircle } from "lucide-react";
import { TAROT_DECK } from "@/lib/tarotData";
import { useLocale, useTranslations } from "next-intl";

import { Button } from "@/components/ui";

interface AiRequestDto {
 id: string;
 status: string;
 finishReason: string | null;
 chargeDiamond: number;
 createdAt: string;
 requestType: string;
}

interface ReadingDetailResponse {
 id: string;
 spreadType: string;
 cardsDrawn: string | null;
 isCompleted: boolean;
 createdAt: string;
 completedAt: string | null;
 aiInteractions: AiRequestDto[];
}

export default function HistoryDetailPage() {
 const params = useParams();
 const router = useRouter();
 const sessionId = params.id as string;
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const t = useTranslations("History");
 const tApi = useTranslations("ApiErrors");
 const tTarot = useTranslations("Tarot");
 const locale = useLocale();

 const [detail, setDetail] = useState<ReadingDetailResponse | null>(null);
 const [isLoading, setIsLoading] = useState(true);
 const [error, setError] = useState<string | null>(null);

 useEffect(() => {
 if (!isAuthenticated) {
 router.push("/login");
 }
 }, [isAuthenticated, router]);

 useEffect(() => {
 if (!isAuthenticated) return;

 const fetchDetail = async () => {
 setIsLoading(true);
 setError(null);
 try {
 const result = await getHistoryDetailAction(sessionId);

 if (result.error) {
 if (result.error === 'unauthorized') {
 router.push("/login");
 return;
 }
 setError(result.error);
 return;
 }

 if (result.success && result.data) {
 setDetail(result.data as ReadingDetailResponse);
 }
 } catch {
 setError(tApi("network_error"));
 } finally {
 setIsLoading(false);
 }
 };

 fetchDetail();
 }, [sessionId, isAuthenticated, router, tApi]);

 const parsedCards = useMemo<number[]>(() => {
  if (!detail?.cardsDrawn) return [];
  try {
   const parsed = JSON.parse(detail.cardsDrawn) as unknown;
   if (!Array.isArray(parsed)) return [];
   return parsed.filter((item): item is number => typeof item === "number");
  } catch {
   return [];
  }
 }, [detail?.cardsDrawn]);

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
 <div className="max-w-[100rem] mx-auto px-4 sm:px-6 pt-8 pb-32 font-sans relative">
 {/* Header Section */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-8 mb-12 sm:mb-16 animate-in fade-in slide-in-from-bottom-4 duration-700">
 <div className="flex flex-col gap-6">
 <button
 onClick={() => router.push("/reading/history")}
 className="group flex items-center gap-2 text-[var(--text-secondary)] hover:tn-text-primary transition-all w-fit min-h-11 px-2 rounded-xl hover:tn-surface-soft"
 >
 <div className="w-8 h-8 rounded-full tn-surface flex items-center justify-center group-hover:bg-[var(--purple-accent)] group-hover:tn-text-ink transition-all">
 <ArrowLeft className="w-4 h-4" />
 </div>
 <span className="text-[10px] uppercase font-black tracking-widest">{t('prev_page')}</span>
 </button>
 <div className="flex items-center gap-6">
 <div className="w-16 h-16 bg-[var(--bg-glass)] border tn-border rounded-[1.5rem] flex items-center justify-center shadow-2xl ">
 <Sparkles className="w-8 h-8 text-[var(--warning)]" />
 </div>
 <div>
 <h1 className="text-3xl sm:text-4xl md:text-5xl font-black tracking-tighter tn-text-primary uppercase italic">
 {detail ? getSpreadName(detail.spreadType) : "..."}
 </h1>
 <div className="flex flex-wrap items-center gap-3 mt-3 text-[var(--text-secondary)] font-medium text-xs">
 <span className="flex items-center gap-1.5 px-3 py-1.5 tn-surface rounded-full">
 <Calendar className="w-3.5 h-3.5" />
 {detail ? new Date(detail.createdAt).toLocaleDateString(locale) : "..."}
 </span>
 <span className="flex items-center gap-1.5 px-3 py-1.5 tn-surface rounded-full">
 <Clock className="w-3.5 h-3.5" />
 {detail ? new Date(detail.createdAt).toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit' }) : "..."}
 </span>
 </div>
 </div>
 </div>
 </div>

 <div className="flex flex-col items-start md:items-end gap-2">
 <span className={`text-[10px] px-4 py-1.5 rounded-full font-black uppercase tracking-[0.2em] border ${detail?.isCompleted ? 'bg-[var(--success-bg)] text-[var(--success)] border-[var(--success)]' : 'bg-[var(--danger-bg)] text-[var(--danger)] border-[var(--danger)]'}`}>
 {detail?.isCompleted ? t('status_completed') : t('status_interrupted')}
 </span>
 <p className="text-[10px] text-[var(--text-tertiary)] font-black tracking-widest uppercase mt-1">
 {t("detail_fragment", { id: sessionId.split("-")[0] })}
 </p>
 </div>
 </div>

 {isLoading ? (
 <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6 animate-pulse">
 {[1, 2, 3, 4, 5].map(i => (
 <div key={i} className="aspect-[3/4] tn-surface rounded-[2.5rem] border tn-border-soft"></div>
 ))}
 </div>
 ) : error ? (
 <div className="bg-[var(--danger-bg)] border border-[var(--danger)] p-12 rounded-[3rem] text-center max-w-2xl mx-auto">
 <AlertCircle className="w-12 h-12 text-[var(--danger)] mx-auto mb-4 opacity-80" />
 <p className="text-[var(--danger)] font-bold mb-8 italic">{error}</p>
 <Button
 variant="primary"
 onClick={() => router.push("/reading/history")}
 >
 {t('prev_page')}
 </Button>
 </div>
 ) : (
 detail && (
 <div className="space-y-24 animate-in fade-in slide-in-from-bottom-8 duration-1000">
 {/* Cards Grid */}
 <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-8 lg:gap-10">
 {parsedCards.map((cardId, index) => {
 const cardMeta = TAROT_DECK.find(c => c.id === cardId) || TAROT_DECK[0];

 return (
 <div key={index} className="group flex flex-col items-center gap-6">
 {/* Tarot Card - Compact Vertical */}
 <div className="relative aspect-[2/3.2] w-full bg-[var(--bg-glass)] rounded-[2.5rem] border tn-border p-5 cursor-default transition-all duration-700 hover:border-[var(--warning)] hover:shadow-[0_20px_80px_var(--c-251-191-36-10)] hover:-translate-y-2">
 {/* Card Background Pattern */}
 <div className="absolute inset-4 border border-[var(--warning)]/10 rounded-[1.8rem] pointer-events-none" />
 <div className="h-full flex flex-col relative z-10">
 {/* Card Art Area */}
 <div className="flex-1 rounded-2xl tn-surface overflow-hidden relative shadow-inner group-hover/art transition-all duration-700 mb-4 border tn-border-soft">
 <div className="absolute inset-0 tn-card-art-fade z-10" />
 <div className="absolute inset-0 flex items-center justify-center opacity-20 group-hover:opacity-40 transition-opacity">
 <Sparkles className="w-10 h-10 text-[var(--warning)]" />
 </div>
 {/* Card Name Overlay */}
 <div className="absolute bottom-4 left-0 right-0 z-20 px-3 text-center">
 <span className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--warning)]/60 block mb-1">
 {tTarot(`suits.${cardMeta.suit}.full`)}
 </span>
 <h3 className="text-xs sm:text-sm font-black tn-text-primary italic tracking-tighter leading-tight drop-shadow-lg">
 {tTarot(`cards.c${cardId}.name`)}
 </h3>
 </div>
 </div>

 {/* Index Ribbon */}
 <div className="absolute top-0 right-8 w-6 h-10 bg-[var(--warning)]/20 rounded-b-lg flex items-center justify-center border border-t-0 border-[var(--warning)]/30">
 <span className="text-[10px] font-black italic text-[var(--warning)]">{index + 1}</span>
 </div>
 </div>

 {/* Hover Shine */}
 <div className="absolute inset-0 tn-card-shine-overlay opacity-0 group-hover:opacity-100 transition-opacity rounded-[2.5rem] pointer-events-none" />
 </div>

 {/* Micro-Meaning - Simple & Elegant */}
 <div className="text-center px-2 transition-all duration-500 group-hover:scale-105">
 <p className="text-[9px] font-black uppercase tracking-[0.25em] text-[var(--purple-accent)] mb-2">{t("essence_label")}</p>
 <p className="text-[11px] font-medium text-[var(--text-secondary)] leading-relaxed italic line-clamp-2 hover:line-clamp-none transition-all">
 {tTarot(`cards.c${cardId}.meaning`)}
 </p>
 </div>
 </div>
 );
 })}
 </div>

 {/* AI Reflection Section */}
 {detail.isCompleted && (
 <div className="mt-32 max-w-4xl mx-auto">
 <div className="flex flex-col items-center gap-8 text-center bg-[var(--bg-glass)] border tn-border-soft p-12 sm:p-16 rounded-[4rem] relative overflow-hidden group">
 <div className="absolute -top-24 -left-24 w-64 h-64 bg-[var(--purple-accent)]/10 blur-[100px] rounded-full group-hover:bg-[var(--purple-accent)]/20 transition-all" />
 <div className="absolute -bottom-24 -right-24 w-64 h-64 bg-[var(--warning)]/10 blur-[100px] rounded-full group-hover:bg-[var(--warning)]/20 transition-all" />
 <div className="w-16 h-16 tn-panel rounded-full flex items-center justify-center animate-bounce-slow relative z-10 shadow-xl">
 <Bot className="w-8 h-8 text-[var(--purple-accent)]" />
 </div>
 <div className="relative z-10">
 <h3 className="text-2xl font-black italic tracking-tight tn-text-primary mb-4 uppercase">
 {t("ai_title")}
 </h3>
 <p className="text-[var(--text-secondary)] font-medium leading-[1.8] max-w-2xl mx-auto">
 {t("ai_desc", { count: detail.aiInteractions.length })}
 </p>
 </div>

 <div className="h-px w-32 bg-gradient-to-r from-transparent via-[var(--purple-accent)] to-transparent opacity-50 relative z-10" />
 <div className="flex items-center gap-2 opacity-40 relative z-10">
 <div className="w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-pulse" />
 <div className="w-1.5 h-1.5 bg-[var(--warning)] rounded-full animate-pulse delay-75" />
                                                <div className="w-1.5 h-1.5 bg-[var(--text-inverse)] rounded-full animate-pulse delay-150" />
 </div>
 </div>
 </div>
 )}
 </div>
 )
 )}
 </div>

 );
}
