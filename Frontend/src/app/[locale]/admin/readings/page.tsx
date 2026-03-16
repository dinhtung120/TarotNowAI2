"use client";

import { useEffect, useState } from "react";
import { getAllHistorySessionsAdminAction } from "@/actions/historyActions";
import Link from "next/link";

interface AdminReading {
    id: string;
    userId: string;
    username: string; // Thêm trường username
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

    const fetchReadings = async (pageNum: number, currentFilters: { uname: string, type: string, start: string, end: string }) => {
        setLoading(true);
        try {
            console.log("DEBUG: Calling Server Action for Admin Readings", { pageNum, currentFilters });
            
            const result = await getAllHistorySessionsAdminAction({
                page: pageNum,
                pageSize: 10,
                username: currentFilters.uname,
                spreadType: currentFilters.type,
                startDate: currentFilters.start ? new Date(currentFilters.start).toISOString() : undefined,
                endDate: currentFilters.end ? new Date(currentFilters.end).toISOString() : undefined
            });

            if (result.success && result.data) {
                console.log("DEBUG: Server Action Success, items:", result.data.items?.length);
                setData(result.data);
            } else {
                console.error("Failed to fetch readings via Server Action:", result.error);
                // Nếu unauthorized -> có thể redirect hoặc thông báo
                if (result.error === 'unauthorized') {
                    alert("Phiên đăng nhập hết hạn hoặc không có quyền.");
                }
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

    // Handler cho việc tìm kiếm
    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        setPage(1);
        fetchReadings(1, { uname: username, type: spreadType, start: startDate, end: endDate });
    };

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-extrabold text-[#DFF2CB]">Lịch Sử Xem Bài Admin</h1>
                <div className="text-sm text-gray-400">
                    Tổng cộng: <span className="text-[#DFF2CB] font-bold">{data?.totalCount || 0}</span> phiên
                </div>
            </div>

            {/* Filter Bar */}
            <form onSubmit={handleSearch} className="bg-[#1A1F2B] border border-[#2D3748] p-4 rounded-2xl shadow-xl flex flex-wrap gap-4 items-end">
                <div className="flex-1 min-w-[200px]">
                    <label className="block text-xs uppercase text-gray-500 mb-1">Username</label>
                    <input 
                        type="text" 
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        placeholder="Tìm theo username..."
                        className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-3 py-2 text-sm text-white focus:outline-none focus:border-[#DFF2CB]"
                    />
                </div>

                <div className="w-48">
                    <label className="block text-xs uppercase text-gray-500 mb-1">Trải Bài</label>
                    <select 
                        value={spreadType}
                        onChange={(e) => setSpreadType(e.target.value)}
                        className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-3 py-2 text-sm text-white focus:outline-none focus:border-[#DFF2CB]"
                    >
                        <option value="">Tất cả loại bài</option>
                        <option value="daily_1">Daily 1 Card</option>
                        <option value="spread_3">Spread 3 Cards</option>
                        <option value="spread_5">Spread 5 Cards</option>
                        <option value="spread_10">Spread 10 Cards</option>
                    </select>
                </div>

                <div className="w-40">
                    <label className="block text-xs uppercase text-gray-500 mb-1">Từ Ngày</label>
                    <input 
                        type="date" 
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                        className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-3 py-2 text-sm text-white focus:outline-none focus:border-[#DFF2CB]"
                    />
                </div>

                <div className="w-40">
                    <label className="block text-xs uppercase text-gray-500 mb-1">Đến Ngày</label>
                    <input 
                        type="date" 
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                        className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-3 py-2 text-sm text-white focus:outline-none focus:border-[#DFF2CB]"
                    />
                </div>

                <button 
                    type="submit"
                    className="bg-[#DFF2CB] text-[#0F1219] px-6 py-2 rounded-lg font-bold text-sm hover:bg-[#c9dab3] transition-colors"
                >
                    Tìm Kiếm
                </button>
            </form>

            <div className="bg-[#1A1F2B] border border-[#2D3748] rounded-2xl overflow-hidden shadow-2xl">
                <table className="w-full text-left text-sm text-gray-300">
                    <thead className="text-xs uppercase bg-[#0F1219] text-[#DFF2CB] border-b border-[#2D3748]">
                        <tr>
                            <th className="px-6 py-4">Thời Gian</th>
                            <th className="px-6 py-4">Người Dùng</th>
                            <th className="px-6 py-4">Trải Bài</th>
                            <th className="px-6 py-4">Câu Hỏi</th>
                            <th className="px-6 py-4 text-center">Trạng Thái</th>
                        </tr>
                    </thead>
                    <tbody>
                        {loading ? (
                            <tr>
                                <td colSpan={5} className="px-6 py-8 text-center text-gray-500 italic">Đang tải dữ liệu...</td>
                            </tr>
                        ) : !data || data.items.length === 0 ? (
                            <tr>
                                <td colSpan={5} className="px-6 py-8 text-center text-gray-500">Chưa có lịch sử xem bài nào phù hợp.</td>
                            </tr>
                        ) : (
                            data.items.map((r) => (
                                <tr key={r.id} className="border-b border-[#2D3748] hover:bg-[#2D3748]/30 transition-colors">
                                    <td className="px-6 py-4 whitespace-nowrap">
                                        <div className="text-white font-medium">{new Date(r.createdAt).toLocaleDateString('vi-VN')}</div>
                                        <div className="text-xs text-gray-500">{new Date(r.createdAt).toLocaleTimeString('vi-VN')}</div>
                                    </td>
                                    <td className="px-6 py-4">
                                        <div className="text-[#DFF2CB] font-bold">{r.username || "Unknown"}</div>
                                        <div className="text-[10px] text-gray-400 mt-0.5 font-mono">{r.userId}</div>
                                    </td>
                                    <td className="px-6 py-4 font-semibold text-gray-300">
                                        {r.spreadType}
                                    </td>
                                    <td className="px-6 py-4 max-w-xs truncate italic">
                                        {r.question || <span className="text-gray-600">Không có câu hỏi</span>}
                                    </td>
                                    <td className="px-6 py-4 text-center">
                                        <span className={`px-3 py-1 rounded-full text-[10px] font-bold uppercase ${
                                            r.isCompleted ? "bg-green-500/10 text-green-400 border border-green-500/20" : "bg-yellow-500/10 text-yellow-400 border border-yellow-500/20"
                                        }`}>
                                            {r.isCompleted ? "Hoàn Thành" : "Đang Xử Lý"}
                                        </span>
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>
                </table>

                {/* Phân trang */}
                {data && data.totalPages > 1 && (
                    <div className="px-6 py-4 bg-[#0F1219] border-t border-[#2D3748] flex items-center justify-between">
                        <div className="text-xs text-gray-500">
                            Trang {data.page} / {data.totalPages}
                        </div>
                        <div className="flex gap-2">
                            <button
                                onClick={() => setPage(p => Math.max(1, p - 1))}
                                disabled={page === 1 || loading}
                                className="px-3 py-1 rounded bg-[#2D3748] text-white text-xs disabled:opacity-30"
                            >
                                Trước
                            </button>
                            <button
                                onClick={() => setPage(p => Math.min(data.totalPages, p + 1))}
                                disabled={page === data.totalPages || loading}
                                className="px-3 py-1 rounded bg-[#2D3748] text-white text-xs disabled:opacity-30"
                            >
                                Sau
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
