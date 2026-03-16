"use client";

import { useEffect, useState } from "react";
import { getAccessToken } from "@/lib/auth-client";
import { useRouter } from "next/navigation";

interface Promotion {
    id: string;
    minAmountVnd: number;
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
    const [deleteId, setDeleteId] = useState<string | null>(null);

    const fetchPromotions = async () => {
        setLoading(true);
        try {
            const token = getAccessToken();
            const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
            const res = await fetch(`${baseUrl}/admin/promotions`, {
                headers: { 
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                }
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
            const token = getAccessToken();
            const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
            const res = await fetch(`${baseUrl}/admin/promotions`, {
                method: "POST",
                headers: { 
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify({ 
                    minAmountVnd: minAmount, 
                    bonusDiamond,
                    isActive: true 
                })
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

    const handleToggle = async (promotion: Promotion) => {
        try {
            const token = getAccessToken();
            const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
            
            // Backend sử dụng PUT /api/v1/admin/promotions/{id} với UpdatePromotionRequest
            const res = await fetch(`${baseUrl}/admin/promotions/${promotion.id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify({
                    minAmountVnd: promotion.minAmountVnd,
                    bonusDiamond: promotion.bonusDiamond,
                    isActive: !promotion.isActive
                })
            });
            if (res.ok) {
                await fetchPromotions();
            } else {
                alert("Lỗi khi thay đổi trạng thái.");
            }
        } catch {
            alert("Lỗi kết nối.");
        }
    };

    const handleDelete = async () => {
        if (!deleteId) return;
        try {
            const token = getAccessToken();
            const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
            const res = await fetch(`${baseUrl}/admin/promotions/${deleteId}`, {
                method: "DELETE",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            });
            if (res.ok) {
                setDeleteId(null);
                await fetchPromotions();
            } else {
                const errorData = await res.json().catch(() => ({}));
                alert(errorData.message || "Lỗi khi xóa.");
            }
        } catch {
            alert("Lỗi kết nối.");
        }
    };


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
                                        {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(p.minAmountVnd)}
                                    </td>
                                    <td className="px-6 py-4 font-bold text-[#DFF2CB]">+{p.bonusDiamond} Diamond</td>
                                    <td className="px-6 py-4 text-center">
                                        <button
                                            onClick={() => handleToggle(p)}
                                            className={`px-3 py-1 rounded-full text-xs font-semibold hover:opacity-80 transition-opacity ${p.isActive ? "bg-green-500/20 text-green-400" : "bg-gray-700 text-gray-400"
                                                }`}
                                        >
                                            {p.isActive ? "Đang Áp Dụng" : "Tạm Dừng"}
                                        </button>
                                    </td>
                                    <td className="px-6 py-4 text-right">
                                        <button
                                            onClick={() => setDeleteId(p.id)}
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
            {/* Modal Xác nhận Xóa */}
            {deleteId && (
                <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
                    {/* Backdrop */}
                    <div 
                        className="absolute inset-0 bg-black/60 backdrop-blur-sm transition-opacity"
                        onClick={() => setDeleteId(null)}
                    ></div>
                    
                    {/* Modal Content */}
                    <div className="relative bg-[#1A1F2B] border border-[#2D3748] rounded-2xl p-8 shadow-2xl max-w-sm w-full transform transition-all animate-in fade-in zoom-in duration-200">
                        <div className="text-center space-y-4">
                            <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-full bg-red-500/10 text-red-500 mb-4">
                                <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
                                </svg>
                            </div>
                            <h3 className="text-xl font-bold text-white">Xác nhận xóa?</h3>
                            <p className="text-gray-400">Hành động này không thể hoàn tác. Mức khuyến mãi này sẽ bị xóa vĩnh viễn khỏi hệ thống.</p>
                        </div>
                        
                        <div className="flex gap-4 mt-8">
                            <button
                                onClick={() => setDeleteId(null)}
                                className="flex-1 px-4 py-3 rounded-xl border border-[#2D3748] text-gray-300 font-bold hover:bg-[#2D3748] transition-colors"
                            >
                                Hủy
                            </button>
                            <button
                                onClick={handleDelete}
                                className="flex-1 px-4 py-3 rounded-xl bg-red-500 text-white font-bold hover:bg-red-600 shadow-lg shadow-red-500/20 transition-all hover:scale-[1.02] active:scale-[0.98]"
                            >
                                Xóa ngay
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
