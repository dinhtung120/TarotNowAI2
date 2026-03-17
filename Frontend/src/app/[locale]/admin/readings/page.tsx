"use client";

/**
 * Trang Quản lý Lịch sử Xem bài (Admin Readings Management) - Astral Premium Redesign
 * 
 * Các cải tiến:
 * 1. Filter Glass: Bộ lọc tìm kiếm được thiết kế lại dạng thanh ngang mờ kính chuyên nghiệp.
 * 2. Session Table: Bảng danh sách phiên xem bài mờ kính với hiệu ứng rực rỡ.
 * 3. Status Badges: Trạng thái (Completed, Processing) với nhãn phát sáng Astral.
 * 4. UX Optimization: Tối ưu hóa hiển thị câu hỏi và thông tin người dùng.
 */

import { useEffect, useState } from "react";
import { getAllHistorySessionsAdminAction } from "@/actions/historyActions";
import { 
    History, 
    Search, 
    Filter, 
    User, 
    Calendar, 
    BookOpen, 
    CheckCircle2, 
    Clock, 
    ChevronLeft, 
    ChevronRight,
    Loader2,
    Sparkles,
    Eye,
    ArrowUpRight,
    Hash,
    XCircle
} from "lucide-react";

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
    const [data, setData] = useState<PaginatedResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState(1);

    // Filters state
    const [username, setUsername] = useState("");
    const [spreadType, setSpreadType] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [toast, setToast] = useState<{ msg: string; type: 'success' | 'error' } | null>(null);

    const fetchReadings = async (pageNum: number, currentFilters: { uname: string, type: string, start: string, end: string }) => {
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
                setToast({ msg: "Phiên đăng nhập hết hạn hoặc không có quyền.", type: 'error' });
            }
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchReadings(page, { uname: username, type: spreadType, start: startDate, end: endDate });
    }, [page]);

    useEffect(() => {
        if (toast) {
            const timer = setTimeout(() => setToast(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [toast]);

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        setPage(1);
        fetchReadings(1, { uname: username, type: spreadType, start: startDate, end: endDate });
    };

    return (
        <div className="space-y-8 pb-20">
            {/* Toast System */}
            {toast && (
                <div className={`fixed top-10 right-10 z-[200] px-6 py-4 rounded-2xl border backdrop-blur-3xl shadow-2xl animate-in slide-in-from-right-10 duration-500 flex items-center gap-3 bg-rose-500/20 border-rose-500/40 text-rose-400`}>
                    <XCircle className="w-5 h-5 text-rose-400" />
                    <span className="text-xs font-black uppercase tracking-widest">{toast.msg}</span>
                </div>
            )}
            
            {/* Header Area */}
            <div className="flex flex-col md:flex-row md:items-end justify-between gap-6 animate-in fade-in slide-in-from-bottom-4 duration-700">
                <div className="space-y-1 text-left">
                    <h1 className="text-3xl font-black text-white uppercase italic tracking-tighter flex items-center gap-3">
                        <History className="w-8 h-8 text-indigo-400" />
                        Nhật ký Vận mệnh
                    </h1>
                    <p className="text-zinc-500 text-xs font-bold uppercase tracking-widest">
                        Khám phá {data?.totalCount || 0} hành trình tâm linh qua các lá bài Tarot
                    </p>
                </div>
            </div>

            {/* Premium Filter Bar */}
            <div className="animate-in fade-in slide-in-from-bottom-4 duration-1000 delay-100">
                <form 
                    onSubmit={handleSearch} 
                    className="p-6 rounded-[2rem] bg-indigo-500/[0.02] border border-white/5 backdrop-blur-3xl shadow-2xl flex flex-wrap items-end gap-6"
                >
                    <div className="flex-1 min-w-[240px] space-y-2 text-left">
                        <label className="text-[10px] font-black uppercase tracking-widest text-zinc-600 flex items-center gap-2">
                            <User className="w-3 h-3" /> Linh hồn
                        </label>
                        <div className="relative group/input">
                            <input 
                                type="text" 
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                                placeholder="Tìm theo tên người dùng..."
                                className="w-full bg-zinc-900 border border-white/5 rounded-2xl px-5 py-3.5 text-xs font-black text-white focus:outline-none focus:border-indigo-500/50 transition-all placeholder:text-zinc-700"
                            />
                        </div>
                    </div>

                    <div className="w-56 space-y-2 text-left">
                        <label className="text-[10px] font-black uppercase tracking-widest text-zinc-600 flex items-center gap-2">
                            <BookOpen className="w-3 h-3" /> Trải bài
                        </label>
                        <select 
                            value={spreadType}
                            onChange={(e) => setSpreadType(e.target.value)}
                            className="w-full bg-zinc-900 border border-white/5 rounded-2xl px-5 py-3.5 text-xs font-black text-white focus:outline-none focus:border-indigo-500/50 transition-all appearance-none cursor-pointer"
                        >
                            <option value="">Tất cả loại bài</option>
                            <option value="daily_1">Daily 1 Card</option>
                            <option value="spread_3">Spread 3 Cards</option>
                            <option value="spread_5">Spread 5 Cards</option>
                            <option value="spread_10">Spread 10 Cards</option>
                        </select>
                    </div>

                    <div className="w-44 space-y-2 text-left">
                        <label className="text-[10px] font-black uppercase tracking-widest text-zinc-600 flex items-center gap-2">
                            <Calendar className="w-3 h-3" /> Từ ngày
                        </label>
                        <input 
                            type="date" 
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            className="w-full bg-zinc-900 border border-white/5 rounded-2xl px-5 py-3.5 text-xs font-black text-white focus:outline-none focus:border-indigo-500/50 transition-all"
                        />
                    </div>

                    <div className="w-44 space-y-2 text-left">
                        <label className="text-[10px] font-black uppercase tracking-widest text-zinc-600 flex items-center gap-2">
                            <Calendar className="w-3 h-3" /> Đến ngày
                        </label>
                        <input 
                            type="date" 
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            className="w-full bg-zinc-900 border border-white/5 rounded-2xl px-5 py-3.5 text-xs font-black text-white focus:outline-none focus:border-indigo-500/50 transition-all"
                        />
                    </div>

                    <button 
                        type="submit"
                        className="relative group px-8 py-3.5 rounded-2xl overflow-hidden transition-all active:scale-95"
                    >
                        <div className="absolute inset-0 bg-indigo-500 group-hover:bg-indigo-400 transition-colors" />
                        <div className="relative flex items-center gap-2 text-black font-black text-[10px] uppercase tracking-widest">
                            <Search className="w-3.5 h-3.5" />
                            Tìm kiếm
                        </div>
                    </button>
                </form>
            </div>

            {/* Main Content Table */}
            <div className="relative group animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200">
                <div className="absolute inset-0 bg-indigo-500/[0.01] blur-3xl rounded-[3rem] pointer-events-none" />
                <div className="relative z-10 bg-white/[0.01] backdrop-blur-3xl rounded-[2.5rem] border border-white/5 overflow-hidden shadow-2xl">
                    <div className="overflow-x-auto custom-scrollbar">
                        <table className="w-full text-left">
                            <thead>
                                <tr className="border-b border-white/5 bg-white/[0.02]">
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-left">Dòng thời gian</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-left">Linh hồn</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-left">Loại trải bài</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-left">Câu hỏi Truy vấn</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-center text-left">Tình trạng</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-white/5">
                                {loading ? (
                                    <tr>
                                        <td colSpan={5} className="px-8 py-20 text-center">
                                            <Loader2 className="w-8 h-8 animate-spin text-indigo-500 mx-auto mb-4" />
                                            <span className="text-[10px] font-black uppercase tracking-widest text-zinc-600">Đang lật mở các chương vận mệnh...</span>
                                        </td>
                                    </tr>
                                ) : !data || !data.items || data.items.length === 0 ? (
                                    <tr>
                                        <td colSpan={5} className="px-8 py-20 text-center text-[10px] font-black uppercase tracking-widest text-zinc-700">
                                            Không có lời tiên tri nào được tìm thấy trong vùng không gian này.
                                        </td>
                                    </tr>
                                ) : (
                                    data.items.map((r) => (
                                        <tr key={r.id} className="group/row hover:bg-white/[0.02] transition-colors">
                                            <td className="px-8 py-6 whitespace-nowrap">
                                                <div className="flex flex-col text-left">
                                                    <div className="text-[10px] font-black text-white uppercase tracking-tighter italic">
                                                        {new Date(r.createdAt).toLocaleDateString("vi-VN")}
                                                    </div>
                                                    <div className="text-[10px] font-bold text-zinc-600 italic">
                                                        {new Date(r.createdAt).toLocaleTimeString("vi-VN")}
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="px-8 py-6">
                                                <div className="flex items-center gap-3">
                                                    <div className="w-10 h-10 rounded-xl bg-zinc-900 border border-white/5 flex items-center justify-center">
                                                        <User className="w-5 h-5 text-indigo-500/50" />
                                                    </div>
                                                    <div>
                                                        <div className="text-[11px] font-black text-indigo-400 uppercase tracking-tighter italic">{r.username || "Chưa định danh"}</div>
                                                        <div className="text-[9px] font-bold text-zinc-700 uppercase tracking-tighter opacity-50 flex items-center gap-1">
                                                            <Hash className="w-2 h-2" /> {r.userId.split('-')[0]}...
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="px-8 py-6">
                                                <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-lg bg-white/5 border border-white/5 text-[10px] font-black text-white uppercase tracking-widest italic text-left">
                                                    <BookOpen className="w-3 h-3 text-indigo-400" />
                                                    {r.spreadType.replace('_', ' ')}
                                                </div>
                                            </td>
                                            <td className="px-8 py-6 max-w-xs truncate text-left">
                                                <p className="text-[10px] font-bold text-zinc-500 italic uppercase leading-relaxed tracking-tight text-left">
                                                    {r.question || "Linh hồn không đặt câu hỏi..."}
                                                </p>
                                            </td>
                                            <td className="px-8 py-6 text-center">
                                                <div className={`
                                                    inline-flex items-center gap-2 px-3 py-1.5 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all
                                                    ${r.isCompleted 
                                                        ? "bg-emerald-500/10 border-emerald-500/20 text-emerald-400 shadow-[0_0_15px_rgba(16,185,129,0.1)]" 
                                                        : "bg-indigo-500/10 border-indigo-500/20 text-indigo-400 shadow-[0_0_15px_rgba(99,102,241,0.1)]"
                                                    }
                                                `}>
                                                    {r.isCompleted ? <CheckCircle2 className="w-3 h-3" /> : <Clock className="w-3 h-3" />}
                                                    {r.isCompleted ? "Hoàn tất" : "Đang mở bài"}
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
                        <div className="px-8 py-6 bg-white/[0.02] flex flex-col md:flex-row md:items-center justify-between gap-4 border-t border-white/5">
                            <div className="text-[10px] font-black uppercase tracking-widest text-zinc-600 text-left">
                                Chương {data.page} <span className="mx-2 opacity-30">/</span> {data.totalPages} đại lục vận mệnh
                            </div>
                            <div className="flex items-center gap-3">
                                <button
                                    onClick={() => setPage(p => Math.max(1, p - 1))}
                                    disabled={page === 1 || loading}
                                    className="p-3 rounded-2xl bg-white/5 border border-white/10 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
                                >
                                    <ChevronLeft className="w-4 h-4" />
                                </button>
                                <span className="text-xs font-black text-white italic mx-4">{page}</span>
                                <button
                                    onClick={() => setPage(p => Math.min(data.totalPages, p + 1))}
                                    disabled={page === data.totalPages || loading}
                                    className="p-3 rounded-2xl bg-white/5 border border-white/10 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
                                >
                                    <ChevronRight className="w-4 h-4" />
                                </button>
                            </div>
                        </div>
                    )}
                </div>
            </div>

            {/* Quick Insights Bar */}
            {!loading && data && (
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6 animate-in fade-in slide-in-from-bottom-4 duration-1000">
                    <div className="p-8 rounded-[2.5rem] bg-indigo-500/10 border border-indigo-500/20 backdrop-blur-3xl shadow-2xl group flex flex-col justify-between min-h-[160px] text-left">
                        <div className="text-[10px] font-black uppercase tracking-[0.2em] text-indigo-300 opacity-70">Tổng hành trình</div>
                        <div className="flex items-end justify-between">
                            <div className="text-3xl font-black text-white italic tracking-tighter">{data?.totalCount ?? 0}</div>
                            <History className="w-10 h-10 text-indigo-500/20 -mb-2 -mr-2 group-hover:scale-110 transition-transform" />
                        </div>
                    </div>

                    <div className="p-8 rounded-[2.5rem] bg-white/[0.02] border border-white/5 shadow-2xl flex items-center gap-6 group hover:border-white/10 transition-all text-left">
                        <div className="w-14 h-14 rounded-2xl bg-zinc-900 flex items-center justify-center border border-white/10 group-hover:scale-110 transition-transform">
                            <Sparkles className="w-7 h-7 text-indigo-400" />
                        </div>
                        <div>
                            <div className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Chu kỳ cập nhật</div>
                            <div className="text-sm font-black text-white uppercase italic tracking-tighter">Thời gian thực</div>
                        </div>
                    </div>

                    <div className="p-8 rounded-[2.5rem] bg-white/[0.02] border border-white/5 shadow-2xl flex items-center justify-between group overflow-hidden relative text-left">
                        <div className="relative z-10 text-left">
                            <div className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Giám sát</div>
                            <div className="text-sm font-black text-white uppercase italic tracking-tighter">Sổ lệnh S.Admin</div>
                        </div>
                        <ArrowUpRight className="w-12 h-12 text-zinc-800 relative z-10 group-hover:text-indigo-400/50 transition-colors" />
                        <div className="absolute inset-0 bg-indigo-500/5 translate-y-full group-hover:translate-y-0 transition-transform duration-500" />
                    </div>
                </div>
            )}
        </div>
    );
}
