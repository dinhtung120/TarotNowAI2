"use client";

import { useEffect, useState } from "react";

interface Promotion {
    id: string;
    minAmount: number;
    bonusDiamond: number;
    isActive: boolean;
    createdAt: string;
}

export default function AdminPromotionsPage() {
    const [promotions, setPromotions] = useState<Promotion[]>([]);
    const [loading, setLoading] = useState(true);
    const [errorStatus, setErrorStatus] = useState<number | null>(null);

    // Form states
    const [isCreating, setIsCreating] = useState(false);
    const [minAmount, setMinAmount] = useState<number>(0);
    const [bonusDiamond, setBonusDiamond] = useState<number>(0);

    const fetchPromotions = async () => {
        setLoading(true);
        try {
            const res = await fetch(`/api/v1/admin/promotions`, {
                headers: { "Content-Type": "application/json" }
            });

            if (!res.ok) {
                if (res.status === 401 || res.status === 403) setErrorStatus(res.status);
                throw new Error("Failed to fetch");
            }

            const data = await res.json();
            setPromotions(data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPromotions();
    }, []);

    const handleCreate = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const res = await fetch("/api/v1/admin/promotions", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ minAmount, bonusDiamond })
            });
            if (res.ok) {
                setIsCreating(false);
                setMinAmount(0);
                setBonusDiamond(0);
                await fetchPromotions();
            } else {
                alert("Lỗi khi thêm khuyến mãi.");
            }
        } catch {
            alert("Lỗi kết nối.");
        }
    };

    const handleToggle = async (id: string, currentStatus: boolean) => {
        try {
            const action = currentStatus ? "deactivate" : "activate";
            const res = await fetch(`/api/v1/admin/promotions/${id}/${action}`, {
                method: "POST"
            });
            if (res.ok) {
                await fetchPromotions();
            }
        } catch {
            alert("Lỗi thay đổi trạng thái.");
        }
    };

    const handleDelete = async (id: string) => {
        if (!confirm("Xóa vĩnh viễn mức khuyến mãi này?")) return;
        try {
            const res = await fetch(`/api/v1/admin/promotions/${id}`, {
                method: "DELETE"
            });
            if (res.ok) {
                await fetchPromotions();
            }
        } catch {
            alert("Lỗi khi xóa.");
        }
    };

    if (errorStatus === 401 || errorStatus === 403) {
        return (
            <div className="flex flex-col items-center justify-center h-full text-red-400 space-y-4">
                <h1 className="text-4xl font-bold">403 Forbidden</h1>
                <p>Bạn không có quyền Admin để cấu hình khuyến mãi.</p>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-extrabold text-[#DFF2CB]">Khuyến Mãi Nạp (Promotions)</h1>
                <button
                    onClick={() => setIsCreating(!isCreating)}
                    className="bg-[#DFF2CB] text-[#1A1F2B] px-4 py-2 rounded-lg font-bold hover:bg-[#B6D996] transition-colors"
                >
                    {isCreating ? "Hủy" : "+ Thêm Mức Khuyến Mãi"}
                </button>
            </div>

            {isCreating && (
                <form onSubmit={handleCreate} className="bg-[#1A1F2B] border border-[#2D3748] rounded-xl p-6 shadow-xl space-y-4">
                    <h2 className="text-xl font-bold text-white mb-4">Thêm mới Khuyến mãi</h2>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div>
                            <label className="block text-sm font-medium text-gray-300 mb-2">Số tiền nạp tối thiểu (VND)</label>
                            <input
                                type="number"
                                min="0"
                                required
                                value={minAmount}
                                onChange={(e) => setMinAmount(Number(e.target.value))}
                                className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-2 text-white focus:outline-none focus:border-[#DFF2CB]"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-300 mb-2">Bonus Diamond nhận thêm</label>
                            <input
                                type="number"
                                min="0"
                                required
                                value={bonusDiamond}
                                onChange={(e) => setBonusDiamond(Number(e.target.value))}
                                className="w-full bg-[#0F1219] border border-[#2D3748] rounded-lg px-4 py-2 text-white focus:outline-none focus:border-[#DFF2CB]"
                            />
                        </div>
                    </div>
                    <div className="flex justify-end pt-2">
                        <button type="submit" className="bg-green-500 hover:bg-green-600 text-white font-bold py-2 px-6 rounded-lg transition-colors">
                            Lưu Khuyến Mãi
                        </button>
                    </div>
                </form>
            )}

            <div className="bg-[#1A1F2B] border border-[#2D3748] rounded-2xl overflow-hidden shadow-2xl">
                <table className="w-full text-left text-sm text-gray-300">
                    <thead className="text-xs uppercase bg-[#0F1219] text-[#DFF2CB] border-b border-[#2D3748]">
                        <tr>
                            <th className="px-6 py-4">Mức Nạp Tối Thiểu</th>
                            <th className="px-6 py-4">Diamond Thưởng</th>
                            <th className="px-6 py-4 text-center">Trạng Thái</th>
                            <th className="px-6 py-4 text-right">Hành Động</th>
                        </tr>
                    </thead>
                    <tbody>
                        {loading ? (
                            <tr>
                                <td colSpan={4} className="px-6 py-8 text-center text-gray-500">Đang tải...</td>
                            </tr>
                        ) : promotions.length === 0 ? (
                            <tr>
                                <td colSpan={4} className="px-6 py-8 text-center text-gray-500">Chưa có khuyến mãi nào.</td>
                            </tr>
                        ) : (
                            promotions.map((p) => (
                                <tr key={p.id} className="border-b border-[#2D3748] hover:bg-[#2D3748]/30 transition-colors">
                                    <td className="px-6 py-4 font-bold text-white">
                                        {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(p.minAmount)}
                                    </td>
                                    <td className="px-6 py-4 font-bold text-[#DFF2CB]">+{p.bonusDiamond} Diamond</td>
                                    <td className="px-6 py-4 text-center">
                                        <button
                                            onClick={() => handleToggle(p.id, p.isActive)}
                                            className={`px-3 py-1 rounded-full text-xs font-semibold hover:opacity-80 transition-opacity ${p.isActive ? "bg-green-500/20 text-green-400" : "bg-gray-700 text-gray-400"
                                                }`}
                                        >
                                            {p.isActive ? "Đang Áp Dụng" : "Tạm Dừng"}
                                        </button>
                                    </td>
                                    <td className="px-6 py-4 text-right">
                                        <button
                                            onClick={() => handleDelete(p.id)}
                                            className="text-red-400 hover:text-red-300 px-3 py-1 text-xs font-medium bg-red-500/10 rounded transition-colors"
                                        >
                                            Xóa
                                        </button>
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
