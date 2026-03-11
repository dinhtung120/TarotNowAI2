"use client";

import { useState, useEffect } from "react";
import { useParams } from "next/navigation";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { Sparkles, ArrowLeft, Bot } from "lucide-react";
import { TAROT_DECK } from "@/lib/tarotData";

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
    const accessToken = useAuthStore(state => state.accessToken);

    const [detail, setDetail] = useState<ReadingDetailResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!accessToken) {
            router.push("/auth/login");
            return;
        }

        const fetchDetail = async () => {
            setIsLoading(true);
            setError(null);
            try {
                const res = await fetch(`http://localhost:5000/api/v1/history/sessions/${sessionId}`, {
                    headers: {
                        "Authorization": `Bearer ${accessToken}`
                    }
                });

                if (!res.ok) {
                    if (res.status === 401) {
                        router.push("/auth/login");
                        return;
                    }
                    if (res.status === 404) {
                        throw new Error("Không tìm thấy phiên đọc bài này");
                    }
                    throw new Error("Không thể tải chi tiết phiên đọc bài");
                }

                const data: ReadingDetailResponse = await res.json();
                setDetail(data);
            } catch (err: unknown) {
                if (err instanceof Error) {
                    setError(err.message || "Đã xảy ra lỗi khi kết nối với máy chủ.");
                } else {
                    setError("Đã xảy ra lỗi không xác định.");
                }
            } finally {
                setIsLoading(false);
            }
        };

        fetchDetail();
    }, [sessionId, accessToken, router]);

    // Parse cards
    const parsedCards: number[] = detail?.cardsDrawn ? JSON.parse(detail.cardsDrawn) : [];

    return (
        <div className="min-h-screen bg-black text-white p-6 pt-24 overflow-hidden relative">
            <div className="max-w-6xl mx-auto relative z-10">

                {/* Header */}
                <div className="flex items-center justify-between mb-8 pb-4 border-b border-zinc-800">
                    <button
                        onClick={() => router.push("/reading/history")}
                        className="flex items-center text-zinc-400 hover:text-white transition"
                    >
                        <ArrowLeft className="w-5 h-5 mr-2" />
                        Trở lại Giao Thức
                    </button>
                    <div className="text-right">
                        <h1 className="text-2xl font-bold bg-gradient-to-r from-purple-400 to-amber-400 text-transparent bg-clip-text">
                            Hồ Sơ Tinh Tú
                        </h1>
                        <p className="text-xs text-zinc-500 font-mono mt-1">ID: {sessionId.split('-')[0]}...</p>
                    </div>
                </div>

                {isLoading ? (
                    <div className="flex flex-col items-center justify-center py-20 animate-pulse text-purple-400/50">
                        <div className="w-16 h-16 bg-purple-900/40 rounded-full mb-4"></div>
                        <div className="h-6 w-48 bg-zinc-900/60 rounded"></div>
                    </div>
                ) : error ? (
                    <div className="bg-red-900/20 border border-red-500/30 p-8 rounded-2xl text-center">
                        <p className="text-red-400 mb-4">{error}</p>
                        <button
                            onClick={() => router.push("/reading/history")}
                            className="bg-zinc-800 hover:bg-zinc-700 px-6 py-2 rounded-lg transition"
                        >
                            Quay Lại
                        </button>
                    </div>
                ) : (
                    detail && (
                        <div className="space-y-16 animate-in fade-in duration-1000">
                            {/* Session Meta */}
                            <div className="text-center mb-8">
                                <span className={`text-xs px-3 py-1 rounded-full font-medium ${detail.isCompleted ? 'bg-amber-500/10 text-amber-500 border border-amber-500/20' : 'bg-red-500/10 text-red-500 border border-red-500/20'}`}>
                                    {detail.isCompleted ? 'Trải Bài Hoàn Tất' : 'Trải Bài Gián Đoạn'}
                                </span>
                                <p className="text-zinc-400 mt-4">
                                    Mở vào ngày {new Date(detail.createdAt).toLocaleDateString('vi-VN')} lúc {new Date(detail.createdAt).toLocaleTimeString('vi-VN')}
                                </p>
                            </div>

                            {/* Cards Display Area */}
                            {parsedCards.length > 0 ? (
                                <div className="flex flex-wrap justify-center gap-8 perspective-1000">
                                    {parsedCards.map((cardId, index) => {
                                        const cardData = TAROT_DECK.find(c => c.id === cardId) || TAROT_DECK[0];

                                        return (
                                            <div key={index} className="flex flex-col items-center gap-6">
                                                {/* Card Front */}
                                                <div className="relative w-56 h-80 bg-gradient-to-b from-amber-100 to-amber-50 rounded-2xl border-2 border-amber-300/50 shadow-[0_0_30px_rgba(251,191,36,0.2)] flex flex-col items-center p-4">
                                                    <div className="w-full text-center border-b border-amber-300 pb-2 mb-4">
                                                        <span className="text-xs font-bold text-amber-600 uppercase tracking-widest">{cardData.suit}</span>
                                                    </div>
                                                    {/* Card Art Placeholder */}
                                                    <div className="flex-1 w-full bg-zinc-800 rounded-lg mb-4 flex items-center justify-center shadow-inner overflow-hidden relative">
                                                        <div className="absolute inset-0 bg-gradient-to-tr from-purple-900/50 to-transparent"></div>
                                                        <span className="text-6xl font-serif text-amber-500/80 drop-shadow-lg">{index + 1}</span>
                                                    </div>
                                                    <h3 className="text-lg font-bold text-zinc-900 font-serif leading-tight text-center">{cardData.name}</h3>
                                                </div>

                                                {/* Interpretation Text */}
                                                <div className="w-56 text-center">
                                                    <p className="text-sm font-medium text-purple-300 mb-1">Ý Nghĩa Cốt Lõi</p>
                                                    <p className="text-xs text-zinc-400 leading-relaxed">{cardData.meaning}</p>
                                                </div>
                                            </div>
                                        );
                                    })}
                                </div>
                            ) : (
                                <div className="text-center text-zinc-500">
                                    Chưa mở lá bài nào trong phiên này.
                                </div>
                            )}

                            {/* AI Chat History */}
                            {detail.isCompleted && parsedCards.length > 0 && (
                                <div className="mt-16">
                                    <h3 className="text-xl font-serif text-center mb-8 flex items-center justify-center gap-2">
                                        <Bot className="w-6 h-6 text-purple-400" />
                                        <span>Ghi Chép Lời Sấm Truyền</span>
                                    </h3>

                                    {/* 
                                        Since we use SSE Stream for chat, History should theoretically load past chat from DB.
                                        However, in this MVP, AiInterpretationStream only works via SSE.
                                        For true history, we need a DB field storing the full Chat Array or use AiInteractions to render static text.
                                        To simplify the MVP, we re-mount AiInterpretationStream (which will fail if the session is old and AI doesn't re-stream)
                                        OR better: display a placeholder saying history chat is archived.
                                    */}
                                    <div className="bg-zinc-900/50 border border-purple-500/20 p-8 rounded-3xl max-w-4xl mx-auto text-center">
                                        <Sparkles className="w-10 h-10 text-amber-500/50 mx-auto mb-4" />
                                        <p className="text-amber-100 font-medium mb-2">Tính Năng Đang Phát Triển</p>
                                        <p className="text-sm text-zinc-400">
                                            Vũ trụ đã lưu trữ {detail.aiInteractions.length} tương tác của bạn trong phiên này. Nội dung hội thoại chi tiết sẽ sớm được hiển thị trong vòng xoáy thời gian tiếp theo.
                                        </p>
                                    </div>
                                </div>
                            )}
                        </div>
                    )
                )}
            </div>

            <div className="fixed top-1/4 left-10 w-96 h-96 bg-purple-900/20 rounded-full blur-[100px] pointer-events-none"></div>
            <div className="fixed bottom-1/4 right-10 w-96 h-96 bg-amber-900/10 rounded-full blur-[100px] pointer-events-none"></div>
        </div>
    );
}
