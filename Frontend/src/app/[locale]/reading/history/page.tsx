"use client";

import { useState, useEffect } from "react";
import { useRouter } from "@/i18n/routing";
import { useAuthStore } from "@/store/authStore";
import { Sparkles, Calendar, ArrowRight, BookOpen, Clock, Bot } from "lucide-react";

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
    const accessToken = useAuthStore(state => state.accessToken);
    const [historyData, setHistoryData] = useState<HistoryResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const pageSize = 10;

    useEffect(() => {
        if (!accessToken) {
            router.push("/auth/login");
            return;
        }

        const fetchHistory = async () => {
            setIsLoading(true);
            setError(null);
            try {
                const res = await fetch(`http://localhost:5000/api/v1/history/sessions?page=${currentPage}&pageSize=${pageSize}`, {
                    headers: {
                        "Authorization": `Bearer ${accessToken}`
                    }
                });

                if (!res.ok) {
                    if (res.status === 401) {
                        router.push("/auth/login");
                        return;
                    }
                    throw new Error("Không thể tải lịch sử đọc bài");
                }

                const data: HistoryResponse = await res.json();
                setHistoryData(data);
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

        fetchHistory();
    }, [accessToken, currentPage, router]);

    const handlePrevPage = () => {
        if (currentPage > 1) setCurrentPage(prev => prev - 1);
    };

    const handleNextPage = () => {
        if (historyData && currentPage < historyData.totalPages) setCurrentPage(prev => prev + 1);
    };

    return (
        <div className="min-h-screen bg-black text-white p-6 pt-32 relative overflow-hidden">
            {/* Background elements */}
            <div className="absolute top-1/4 -left-20 w-96 h-96 bg-purple-900/20 rounded-full blur-[100px] pointer-events-none"></div>
            <div className="absolute bottom-1/4 -right-20 w-96 h-96 bg-amber-900/10 rounded-full blur-[100px] pointer-events-none"></div>

            <div className="max-w-4xl mx-auto relative z-10">
                <div className="flex items-center gap-4 mb-8">
                    <div className="w-14 h-14 bg-purple-900/40 border border-purple-500/30 rounded-2xl flex items-center justify-center">
                        <BookOpen className="w-7 h-7 text-purple-400" />
                    </div>
                    <div>
                        <h1 className="text-3xl font-serif font-bold bg-gradient-to-r from-purple-400 to-amber-400 text-transparent bg-clip-text">
                            Hành Trình Giao Thức
                        </h1>
                        <p className="text-zinc-400 mt-1 flex items-center">
                            <Sparkles className="w-4 h-4 mr-2 text-amber-500/70" />
                            Ghi chép lại những thông điệp từ Các Tinh Tú
                        </p>
                    </div>
                </div>

                {error && (
                    <div className="bg-red-900/20 border border-red-500/30 p-4 rounded-xl text-red-400 mb-8">
                        {error}
                    </div>
                )}

                {isLoading ? (
                    <div className="space-y-4 animate-pulse">
                        {[1, 2, 3].map(i => (
                            <div key={i} className="h-24 bg-zinc-900/50 rounded-2xl border border-zinc-800/50"></div>
                        ))}
                    </div>
                ) : historyData?.items.length === 0 ? (
                    <div className="text-center py-20 bg-zinc-900/30 border border-zinc-800 rounded-3xl">
                        <Bot className="w-16 h-16 text-zinc-600 mx-auto mb-4" />
                        <h3 className="text-xl font-serif text-white mb-2">Chưa có dấu ấn nào</h3>
                        <p className="text-zinc-500 mb-8">Vũ trụ đang chờ đón bạn mở lời cho điểm khởi đầu mới.</p>
                        <button
                            onClick={() => router.push("/reading")}
                            className="bg-purple-600 hover:bg-purple-500 px-6 py-2.5 rounded-xl font-medium transition"
                        >
                            Rút Bài Ngay
                        </button>
                    </div>
                ) : (
                    <div className="space-y-4">
                        {historyData?.items.map((session) => (
                            <div
                                key={session.id}
                                onClick={() => router.push(`/reading/history/${session.id}`)}
                                className="group bg-zinc-900/60 backdrop-blur-sm border border-purple-500/10 hover:border-purple-500/40 p-5 rounded-2xl cursor-pointer transition-all hover:-translate-y-1 hover:shadow-[0_10px_30px_rgba(168,85,247,0.1)] flex items-center justify-between"
                            >
                                <div className="flex items-center gap-5">
                                    <div className="w-12 h-12 bg-black/50 rounded-full flex items-center justify-center border border-zinc-800 group-hover:border-purple-500/50 transition-colors">
                                        <Sparkles className={`w-5 h-5 ${session.isCompleted ? 'text-amber-400' : 'text-zinc-500'}`} />
                                    </div>
                                    <div>
                                        <h3 className="text-lg font-serif font-medium text-purple-100 group-hover:text-purple-300 transition-colors">
                                            {session.spreadType === 'Daily1Card' ? 'Lá Bài Trong Ngày (1 Lá)' :
                                                session.spreadType === 'PastPresentFuture' ? 'Quá Khứ - Hiện Tại - Tương Lai (3 Lá)' : session.spreadType}
                                        </h3>
                                        <div className="flex items-center gap-4 mt-1.5 flex-wrap">
                                            <span className="flex items-center text-xs text-zinc-500">
                                                <Calendar className="w-3.5 h-3.5 mr-1" />
                                                {new Date(session.createdAt).toLocaleDateString('vi-VN')}
                                            </span>
                                            <span className="flex items-center text-xs text-zinc-500">
                                                <Clock className="w-3.5 h-3.5 mr-1" />
                                                {new Date(session.createdAt).toLocaleTimeString('vi-VN')}
                                            </span>
                                            <span className={`text-[10px] px-2 py-0.5 rounded-full font-medium ${session.isCompleted ? 'bg-amber-500/10 text-amber-500 border border-amber-500/20' : 'bg-red-500/10 text-red-500 border border-red-500/20'}`}>
                                                {session.isCompleted ? 'Hoàn Tất' : 'Bị Gián Đoạn'}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="w-8 h-8 rounded-full bg-zinc-800 flex items-center justify-center group-hover:bg-purple-600 transition-colors shrink-0">
                                    <ArrowRight className="w-4 h-4 text-zinc-400 group-hover:text-white" />
                                </div>
                            </div>
                        ))}

                        {/* Pagination Controls */}
                        {historyData && historyData.totalPages > 1 && (
                            <div className="flex items-center justify-center gap-4 mt-8 pt-8 border-t border-zinc-800/50">
                                <button
                                    onClick={handlePrevPage}
                                    disabled={currentPage === 1}
                                    className="px-4 py-2 rounded-lg bg-zinc-900 border border-zinc-700 disabled:opacity-50 hover:bg-zinc-800 transition"
                                >
                                    Trang trước
                                </button>
                                <span className="text-zinc-400 text-sm">
                                    Trang {currentPage} / {historyData.totalPages}
                                </span>
                                <button
                                    onClick={handleNextPage}
                                    disabled={currentPage === historyData.totalPages}
                                    className="px-4 py-2 rounded-lg bg-zinc-900 border border-zinc-700 disabled:opacity-50 hover:bg-zinc-800 transition"
                                >
                                    Trang sau
                                </button>
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}
