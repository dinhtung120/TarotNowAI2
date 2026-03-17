"use client";

/**
 * Trang Chi Tiết Phiên Đọc Bài (History Detail)
 *
 * Phiên bản sửa lỗi:
 * - [TRƯỚC] Gọi fetch() trực tiếp với URL hardcode `localhost:5000` (sai port)
 * - [SAU] Gọi qua Server Action `getHistoryDetailAction()` → URL đúng (localhost:5037)
 * - [TRƯỚC] Auth guard redirect sai `/auth/login`
 * - [SAU] Redirect đúng `/login`
 */

import { useState, useEffect } from "react";
import { useParams } from "next/navigation";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { getHistoryDetailAction } from "@/actions/historyActions";
import { Sparkles, ArrowLeft, Bot } from "lucide-react";
import { TAROT_DECK } from "@/lib/tarotData";
import { useTranslations } from "next-intl";

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
    const { isAuthenticated } = useAuthStore();
    const t = useTranslations("History");

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
                setError("Đã xảy ra lỗi khi kết nối với máy chủ.");
            } finally {
                setIsLoading(false);
            }
        };

        fetchDetail();
    }, [sessionId, isAuthenticated, router]);

    // Parse cards
    const parsedCards: number[] = detail?.cardsDrawn ? JSON.parse(detail.cardsDrawn) : [];

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
        <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-x-hidden font-sans">
            {/* ##### PREMIUM BACKGROUND ##### */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                <div className="absolute top-1/4 -left-1/4 w-[60vw] h-[60vw] bg-purple-900/[0.08] blur-[120px] rounded-full animate-slow-pulse" />
                <div className="absolute bottom-1/4 -right-1/4 w-[50vw] h-[50vw] bg-indigo-900/[0.06] blur-[130px] rounded-full animate-slow-pulse delay-700" />
                
                {/* Spiritual Particles */}
                <div className="absolute inset-0">
                    {Array.from({ length: 25 }).map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-[1.5px] h-[1.5px] bg-white rounded-full animate-float opacity-[0.15]"
                            style={{
                                top: `${Math.random() * 100}%`,
                                left: `${Math.random() * 100}%`,
                                animationDuration: `${20 + Math.random() * 30}s`,
                                animationDelay: `${-Math.random() * 30}s`,
                            }}
                        />
                    ))}
                </div>
            </div>

            <main className="relative z-10 max-w-[100rem] mx-auto px-6 pt-28 pb-32">
                {/* Header Section */}
                <div className="flex flex-col md:flex-row md:items-end justify-between gap-8 mb-16 animate-in fade-in slide-in-from-bottom-4 duration-700">
                    <div className="flex flex-col gap-6">
                        <button
                            onClick={() => router.push("/reading/history")}
                            className="group flex items-center gap-2 text-zinc-500 hover:text-white transition-all w-fit"
                        >
                            <div className="w-8 h-8 rounded-full bg-white/5 flex items-center justify-center group-hover:bg-purple-500 group-hover:text-white transition-all">
                                <ArrowLeft className="w-4 h-4" />
                            </div>
                            <span className="text-[10px] uppercase font-black tracking-widest">{t('prev_page')}</span>
                        </button>
                        
                        <div className="flex items-center gap-6">
                            <div className="w-16 h-16 bg-white/5 border border-white/10 rounded-3xl flex items-center justify-center shadow-2xl backdrop-blur-md">
                                <Sparkles className="w-8 h-8 text-amber-400" />
                            </div>
                            <div>
                                <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
                                    {detail ? getSpreadName(detail.spreadType) : "..."}
                                </h1>
                                <div className="flex items-center gap-4 mt-2 text-zinc-500 font-medium text-xs">
                                    <span className="flex items-center gap-1.5 p-1 px-2.5 bg-white/5 rounded-full">
                                        <Calendar className="w-3.5 h-3.5" />
                                        {detail ? new Date(detail.createdAt).toLocaleDateString() : "..."}
                                    </span>
                                    <span className="flex items-center gap-1.5 p-1 px-2.5 bg-white/5 rounded-full">
                                        <Clock className="w-3.5 h-3.5" />
                                        {detail ? new Date(detail.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : "..."}
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="flex flex-col items-end gap-2 text-right">
                        <span className={`text-[10px] px-4 py-1.5 rounded-full font-black uppercase tracking-[0.2em] border ${detail?.isCompleted ? 'bg-amber-500/10 text-amber-500 border-amber-500/20 shadow-[0_0_20px_rgba(245,158,11,0.1)]' : 'bg-red-500/10 text-red-500 border-red-500/20'}`}>
                            {detail?.isCompleted ? t('status_completed') : t('status_interrupted')}
                        </span>
                        <p className="text-[10px] text-zinc-600 font-black tracking-widest uppercase mt-1">Fragment Code: {sessionId.split('-')[0]}...</p>
                    </div>
                </div>

                {isLoading ? (
                    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6 animate-pulse">
                        {[1, 2, 3, 4, 5].map(i => (
                            <div key={i} className="aspect-[3/4] bg-white/[0.02] rounded-[2.5rem] border border-white/[0.05]"></div>
                        ))}
                    </div>
                ) : error ? (
                    <div className="bg-red-500/5 border border-red-500/10 p-12 rounded-[3rem] text-center max-w-2xl mx-auto">
                        <p className="text-red-400 font-bold mb-6 italic">{error}</p>
                        <button
                            onClick={() => router.push("/reading/history")}
                            className="bg-white text-black px-10 py-3 rounded-full font-black text-xs uppercase tracking-widest transition-all hover:scale-105"
                        >
                            Quay Lại
                        </button>
                    </div>
                ) : (
                    detail && (
                        <div className="space-y-24 animate-in fade-in slide-in-from-bottom-8 duration-1000">
                            {/* Cards Grid - 5 Columns */}
                            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-8 lg:gap-10">
                                {parsedCards.map((cardId, index) => {
                                    const cardData = TAROT_DECK.find(c => c.id === cardId) || TAROT_DECK[0];

                                    return (
                                        <div key={index} className="group flex flex-col items-center gap-6">
                                            {/* Tarot Card - Compact Vertical */}
                                            <div className="relative aspect-[2/3.2] w-full bg-gradient-to-br from-amber-50/10 to-transparent backdrop-blur-3xl rounded-[2.5rem] border border-white/10 p-5 cursor-default transition-all duration-700 hover:border-amber-500/40 hover:shadow-[0_20px_80px_rgba(251,191,36,0.15)] hover:-translate-y-2">
                                                {/* Card Background Pattern */}
                                                <div className="absolute inset-4 border border-amber-500/10 rounded-[1.8rem] pointer-events-none" />
                                                
                                                <div className="h-full flex flex-col">
                                                    {/* Card Art Area */}
                                                    <div className="flex-1 rounded-2xl bg-zinc-950 overflow-hidden relative shadow-inner group/art transition-all duration-700 mb-4 border border-white/5">
                                                        <div className="absolute inset-0 bg-gradient-to-t from-black via-transparent to-transparent z-10" />
                                                        <div className="absolute inset-0 flex items-center justify-center opacity-20 group-hover:opacity-40 transition-opacity">
                                                            <Sparkles className="w-16 h-16 text-amber-500" />
                                                        </div>
                                                        
                                                        {/* Card Name Overlay */}
                                                        <div className="absolute bottom-4 left-0 right-0 z-20 px-3 text-center">
                                                            <span className="text-[10px] font-black uppercase tracking-[0.2em] text-amber-500/60 block mb-1">
                                                                {cardData.suit}
                                                            </span>
                                                            <h3 className="text-sm font-black text-white italic tracking-tighter leading-tight drop-shadow-lg">
                                                                {cardData.name}
                                                            </h3>
                                                        </div>
                                                    </div>

                                                    {/* Index Ribbon */}
                                                    <div className="absolute top-0 right-8 w-6 h-10 bg-amber-500/20 backdrop-blur-md rounded-b-lg flex items-center justify-center border border-t-0 border-amber-500/30">
                                                        <span className="text-[10px] font-black italic text-amber-400">{index + 1}</span>
                                                    </div>
                                                </div>

                                                {/* Hover Shine */}
                                                <div className="absolute inset-0 bg-gradient-to-tr from-white/[0.05] to-transparent opacity-0 group-hover:opacity-100 transition-opacity rounded-[2.5rem] pointer-events-none" />
                                            </div>

                                            {/* Micro-Meaning - Simple & Elegant */}
                                            <div className="text-center px-4 transition-all duration-500 group-hover:scale-105">
                                                <p className="text-[9px] font-black uppercase tracking-[0.25em] text-purple-400 mb-2">Essence</p>
                                                <p className="text-[11px] font-medium text-zinc-400 leading-relaxed italic line-clamp-2 hover:line-clamp-none transition-all">
                                                    {cardData.meaning}
                                                </p>
                                            </div>
                                        </div>
                                    );
                                })}
                            </div>

                            {/* AI Reflection Section */}
                            {detail.isCompleted && (
                                <div className="mt-32 max-w-4xl mx-auto">
                                    <div className="flex flex-col items-center gap-8 text-center bg-white/[0.01] backdrop-blur-3xl border border-white/5 p-16 rounded-[4rem] relative overflow-hidden group">
                                        <div className="absolute -top-24 -left-24 w-64 h-64 bg-purple-500/[0.03] blur-[100px] rounded-full group-hover:bg-purple-500/[0.06] transition-all" />
                                        <div className="absolute -bottom-24 -right-24 w-64 h-64 bg-amber-500/[0.02] blur-[100px] rounded-full group-hover:bg-amber-500/[0.04] transition-all" />
                                        
                                        <div className="w-16 h-16 bg-white/5 border border-white/10 rounded-full flex items-center justify-center animate-bounce-slow">
                                            <Bot className="w-8 h-8 text-purple-400" />
                                        </div>
                                        
                                        <div>
                                            <h3 className="text-2xl font-black italic tracking-tight text-white mb-4 uppercase">
                                                Lời Sấm Bản Nguyên
                                            </h3>
                                            <p className="text-zinc-500 font-medium leading-[1.8] max-w-2xl">
                                                Vũ trụ đã lưu trữ {detail.aiInteractions.length} dấu ấn trí tuệ trong phiên này. Nội dung hội thoại đang được hội tụ từ thực tại ảo và sẽ hiện diện đầy đủ trong các lần cập nhật Giao Thức tiếp theo.
                                            </p>
                                        </div>

                                        <div className="h-px w-32 bg-gradient-to-r from-transparent via-purple-500/20 to-transparent" />
                                        
                                        <div className="flex items-center gap-2 opacity-40">
                                            <div className="w-1 h-1 bg-white rounded-full animate-pulse" />
                                            <div className="w-1 h-1 bg-white rounded-full animate-pulse delay-75" />
                                            <div className="w-1 h-1 bg-white rounded-full animate-pulse delay-150" />
                                        </div>
                                    </div>
                                </div>
                            )}
                        </div>
                    )
                )}
            </main>

            {/* Custom Animations */}
            <style dangerouslySetInnerHTML={{
                __html: `
                @keyframes float {
                    0% { transform: translateY(0) rotate(0deg); opacity: 0; }
                    15% { opacity: 0.2; }
                    85% { opacity: 0.2; }
                    100% { transform: translateY(-100vh) rotate(360deg); opacity: 0; }
                }
                .animate-float {
                    animation-name: float;
                    animation-timing-function: linear;
                    animation-iteration-count: infinite;
                }
                @keyframes slow-pulse {
                    0%, 100% { opacity: 0.05; transform: scale(1); }
                    50% { opacity: 0.12; transform: scale(1.15); }
                }
                .animate-slow-pulse {
                    animation: slow-pulse 15s ease-in-out infinite;
                }
                @keyframes bounce-slow {
                    0%, 100% { transform: translateY(0); }
                    50% { transform: translateY(-10px); }
                }
                .animate-bounce-slow {
                    animation: bounce-slow 4s ease-in-out infinite;
                }
            `}} />
        </div>
    );
}

// Thêm icons (AlertCircle, Calendar, Clock)
import { AlertCircle as AlertCircleIcon, Calendar, Clock } from "lucide-react";
function AlertCircle({ className }: { className?: string }) {
    return <AlertCircleIcon className={className} />;
}
