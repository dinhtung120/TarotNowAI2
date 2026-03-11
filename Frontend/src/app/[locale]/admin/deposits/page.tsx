"use client";

import { useEffect, useState } from "react";

interface DepositOrder {
    id: string;
    userId: string;
    amountVnd: number;
    diamondAmount: number;
    status: string;
    transactionId: string | null;
    fxSnapshot: string | null;
    createdAt: string;
}

export default function AdminDepositsPage() {
    const [orders, setOrders] = useState<DepositOrder[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [statusFilter, setStatusFilter] = useState("");
    const [loading, setLoading] = useState(true);
    const [errorStatus, setErrorStatus] = useState<number | null>(null);

    const fetchOrders = async () => {
        setLoading(true);
        setErrorStatus(null);
        try {
            const query = new URLSearchParams({
                page: page.toString(),
                pageSize: "10",
            });
            if (statusFilter) query.append("status", statusFilter);

            const res = await fetch(`/api/v1/admin/deposits?${query.toString()}`, {
                headers: { "Content-Type": "application/json" }
            });

            if (!res.ok) {
                if (res.status === 401 || res.status === 403) setErrorStatus(res.status);
                throw new Error("Failed to fetch");
            }

            const data = await res.json();
            setOrders(data.deposits);
            setTotalCount(data.totalCount);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchOrders();
    }, [page, statusFilter]);

    if (errorStatus === 401 || errorStatus === 403) {
        return (
            <div className="flex flex-col items-center justify-center h-full text-red-400 space-y-4">
                <h1 className="text-4xl font-bold">403 Forbidden</h1>
                <p>Bạn không có quyền Admin để xem danh sách giao dịch.</p>
            </div>
        );
    }

    const getStatusColor = (status: string) => {
        switch (status) {
            case "Success": return "bg-green-500/20 text-green-400";
            case "Failed": return "bg-red-500/20 text-red-400";
            default: return "bg-yellow-500/20 text-yellow-500";
        }
    };

    return (
        <div className="space-y-6">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <h1 className="text-3xl font-extrabold text-[#DFF2CB]">Giao Dịch Nạp Diamond</h1>
                <div className="flex items-center gap-4">
                    <label className="text-gray-400 text-sm">Trạng thái:</label>
                    <select
                        value={statusFilter}
                        onChange={(e) => {
                            setStatusFilter(e.target.value);
                            setPage(1); // Reset page on filter change
                        }}
                        className="bg-[#1A1F2B] border border-[#2D3748] rounded-lg px-4 py-2 text-white focus:outline-none focus:border-[#DFF2CB] transition-colors"
                    >
                        <option value="">Tất cả</option>
                        <option value="Pending">Đang xử lý (Pending)</option>
                        <option value="Success">Thành công (Success)</option>
                        <option value="Failed">Thất bại (Failed)</option>
                    </select>
                </div>
            </div>

            <div className="bg-[#1A1F2B] border border-[#2D3748] rounded-2xl overflow-hidden shadow-2xl">
                <div className="overflow-x-auto">
                    <table className="w-full text-left text-sm text-gray-300">
                        <thead className="text-xs uppercase bg-[#0F1219] text-[#DFF2CB] border-b border-[#2D3748]">
                            <tr>
                                <th className="px-6 py-4">Mã Đơn / TXN ID</th>
                                <th className="px-6 py-4">Người Dùng ID</th>
                                <th className="px-6 py-4">Số Tiền (VND)</th>
                                <th className="px-6 py-4">Diamond Nhận</th>
                                <th className="px-6 py-4">Tạo Lúc</th>
                                <th className="px-6 py-4 text-center">Trạng Thái</th>
                            </tr>
                        </thead>
                        <tbody>
                            {loading ? (
                                <tr>
                                    <td colSpan={6} className="px-6 py-8 text-center text-gray-500">Đang tải biểu mẫu...</td>
                                </tr>
                            ) : orders.length === 0 ? (
                                <tr>
                                    <td colSpan={6} className="px-6 py-8 text-center text-gray-500">Chưa có giao dịch nào thỏa điều kiện.</td>
                                </tr>
                            ) : (
                                orders.map((o) => (
                                    <tr key={o.id} className="border-b border-[#2D3748] hover:bg-[#2D3748]/30 transition-colors">
                                        <td className="px-6 py-4">
                                            <div className="font-mono text-xs text-gray-400">{o.id}</div>
                                            {o.transactionId && <div className="text-sm font-semibold text-[#DFF2CB]">TXN: {o.transactionId}</div>}
                                        </td>
                                        <td className="px-6 py-4 font-mono text-xs text-gray-400">{o.userId}</td>
                                        <td className="px-6 py-4 font-semibold text-white">
                                            {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(o.amountVnd)}
                                        </td>
                                        <td className="px-6 py-4 font-semibold text-[#DFF2CB]">+{o.diamondAmount}</td>
                                        <td className="px-6 py-4">{new Date(o.createdAt).toLocaleString("vi-VN")}</td>
                                        <td className="px-6 py-4 text-center">
                                            <span className={`px-2.5 py-1 rounded-full text-xs font-semibold ${getStatusColor(o.status)}`}>
                                                {o.status}
                                            </span>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>

                {/* Pagination */}
                <div className="px-6 py-4 bg-[#0F1219] flex justify-between items-center border-t border-[#2D3748]">
                    <span className="text-sm text-gray-400">
                        Tổng cộng: <strong className="text-white">{totalCount}</strong> giao dịch
                    </span>
                    <div className="flex gap-2">
                        <button
                            onClick={() => setPage(p => Math.max(1, p - 1))}
                            disabled={page === 1}
                            className="px-3 py-1 bg-[#1A1F2B] hover:bg-[#2D3748] rounded border border-[#2D3748] disabled:opacity-50 transition-colors"
                        >
                            Trước
                        </button>
                        <span className="px-3 py-1 text-sm font-semibold">{page}</span>
                        <button
                            onClick={() => setPage(p => p + 1)}
                            disabled={page * 10 >= totalCount}
                            className="px-3 py-1 bg-[#1A1F2B] hover:bg-[#2D3748] rounded border border-[#2D3748] disabled:opacity-50 transition-colors"
                        >
                            Sau
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}
