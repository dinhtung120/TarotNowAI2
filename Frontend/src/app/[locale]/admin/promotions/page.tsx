"use client";

/**
 * Trang Quản lý Khuyến mãi (Admin Promotions) - Astral Premium Redesign
 * 
 * Các cải tiến:
 * 1. Astral Form: Form thêm khuyến mãi được tích hợp mờ kính và hiệu ứng phát sáng.
 * 2. Status Toggles: Nút gạt trạng thái (Active/Inactive) chuyển sang ngôn ngữ thiết kế Astral.
 * 3. Reward Alignment: Cập nhật hiển thị Quà tặng là Gold thay vì Diamond (theo yêu cầu User).
 * 4. Micro-interactions: Hiệu ứng hover cho bảng và các nút hành động.
 */

import { useEffect, useState } from "react";
import { getAccessToken } from "@/lib/auth-client";
import { 
    Ticket, 
    Plus, 
    X, 
    Trash2, 
    CheckCircle2, 
    Power, 
    AlertTriangle,
    Gem,
    Coins,
    Sparkles,
    Loader2,
    PlusCircle
} from "lucide-react";

interface Promotion {
    id: string;
    minAmountVnd: number;
    bonusDiamond: number; // Backend property name (representing Gold as per new rules)
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
    const [bonusGold, setBonusGold] = useState<number>(0);
    const [deleteId, setDeleteId] = useState<string | null>(null);
    const [submitting, setSubmitting] = useState(false);
    const [toast, setToast] = useState<{ msg: string; type: 'success' | 'error' } | null>(null);

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

    useEffect(() => {
        if (toast) {
            const timer = setTimeout(() => setToast(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [toast]);

    const handleCreate = async (e: React.FormEvent) => {
        e.preventDefault();
        setSubmitting(true);
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
                    bonusDiamond: bonusGold, // Mapping UI Gold to backend field
                    isActive: true 
                })
            });
            if (res.ok) {
                setIsCreating(false);
                setMinAmount(0);
                setBonusGold(0);
                setToast({ msg: "Đã thêm ưu đãi thành công!", type: 'success' });
                await fetchPromotions();
            } else {
                setToast({ msg: "Lỗi khi thêm khuyến mãi.", type: 'error' });
            }
        } catch {
            setToast({ msg: "Lỗi kết nối.", type: 'error' });
        } finally {
            setSubmitting(false);
        }
    };

    const handleToggle = async (promotion: Promotion) => {
        try {
            const token = getAccessToken();
            const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
            
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
                setToast({ msg: "Đã cập nhật trạng thái.", type: 'success' });
                await fetchPromotions();
            } else {
                setToast({ msg: "Lỗi khi thay đổi trạng thái.", type: 'error' });
            }
        } catch {
            setToast({ msg: "Lỗi kết nối.", type: 'error' });
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
                setToast({ msg: "Đã xóa ưu đãi vĩnh viễn.", type: 'success' });
                await fetchPromotions();
            } else {
                const errorData = await res.json().catch(() => ({}));
                setToast({ msg: errorData.message || "Lỗi khi xóa.", type: 'error' });
            }
        } catch {
            setToast({ msg: "Lỗi kết nối.", type: 'error' });
        }
    };

    return (
        <div className="space-y-8 pb-20">
            {/* Toast System */}
            {toast && (
                <div className={`fixed top-10 right-10 z-[200] px-6 py-4 rounded-2xl border backdrop-blur-3xl shadow-2xl animate-in slide-in-from-right-10 duration-500 flex items-center gap-3 ${
                    toast.type === 'success' ? 'bg-amber-500/20 border-amber-500/40 text-amber-500' : 'bg-rose-500/20 border-rose-500/40 text-rose-400'
                }`}>
                    {toast.type === 'success' ? <CheckCircle2 className="w-5 h-5" /> : <X className="w-5 h-5" />}
                    <span className="text-xs font-black uppercase tracking-widest">{toast.msg}</span>
                </div>
            )}
            {/* Header Area */}
            <div className="flex flex-col md:flex-row md:items-end justify-between gap-6 animate-in fade-in slide-in-from-bottom-4 duration-700">
                <div className="space-y-1">
                    <h1 className="text-3xl font-black text-white uppercase italic tracking-tighter flex items-center gap-3 text-left">
                        <Ticket className="w-8 h-8 text-amber-400" />
                        Chương trình Ưu đãi
                    </h1>
                    <p className="text-zinc-500 text-xs font-bold uppercase tracking-widest text-left">
                        Thiết lập các mốc nạp tệ để ban thưởng Vàng cho linh hồn
                    </p>
                </div>

                <button
                    onClick={() => setIsCreating(!isCreating)}
                    className="group relative px-6 py-3 rounded-2xl overflow-hidden transition-all active:scale-95"
                >
                    <div className="absolute inset-0 bg-gradient-to-r from-amber-500 to-yellow-400 blur-xl opacity-20 group-hover:opacity-40 transition-opacity" />
                    <div className="relative flex items-center gap-2 bg-amber-500 text-black px-4 py-1.5 rounded-xl font-black text-[10px] uppercase tracking-widest shadow-2xl group-hover:bg-yellow-400 transition-colors">
                        {isCreating ? <X className="w-3.5 h-3.5" /> : <PlusCircle className="w-3.5 h-3.5" />}
                        {isCreating ? "Hủy bỏ" : "Thêm Ưu đãi mới"}
                    </div>
                </button>
            </div>

            {/* Create form - Inline Expansion */}
            {isCreating && (
                <div className="animate-in fade-in slide-in-from-top-4 duration-500">
                    <div className="bg-white/[0.02] border border-amber-500/20 backdrop-blur-3xl rounded-[2rem] p-8 shadow-2xl relative overflow-hidden">
                        <div className="absolute top-0 right-0 p-12 bg-amber-500/5 blur-[100px] rounded-full" />
                        
                        <form onSubmit={handleCreate} className="relative z-10 space-y-6">
                            <div className="flex items-center gap-3 border-b border-white/5 pb-4">
                                <Sparkles className="w-5 h-5 text-amber-400" />
                                <h2 className="text-sm font-black text-white uppercase tracking-widest">Khởi tạo Mốc thưởng mới</h2>
                            </div>

                            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                                <div className="space-y-2 text-left">
                                    <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Mức nạp tối thiểu (VND)</label>
                                    <div className="relative group/input">
                                        <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                                            <span className="text-[10px] font-black text-zinc-600">₫</span>
                                        </div>
                                        <input
                                            type="number"
                                            min="0"
                                            required
                                            value={minAmount}
                                            onChange={(e) => setMinAmount(Number(e.target.value))}
                                            placeholder="200,000"
                                            className="w-full bg-zinc-900/50 border border-white/5 rounded-2xl pl-10 pr-4 py-4 text-xs font-black text-white focus:outline-none focus:border-amber-500/50 transition-all placeholder:text-zinc-700"
                                        />
                                    </div>
                                </div>

                                <div className="space-y-2 text-left">
                                    <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Lượng Vàng thưởng thêm</label>
                                    <div className="relative group/input">
                                        <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                                            <Coins className="w-3.5 h-3.5 text-amber-500/50" />
                                        </div>
                                        <input
                                            type="number"
                                            min="0"
                                            required
                                            value={bonusGold}
                                            onChange={(e) => setBonusGold(Number(e.target.value))}
                                            placeholder="500"
                                            className="w-full bg-zinc-900/50 border border-white/5 rounded-2xl pl-10 pr-4 py-4 text-xs font-black text-white focus:outline-none focus:border-amber-500/50 transition-all placeholder:text-zinc-700"
                                        />
                                    </div>
                                </div>
                            </div>

                            <div className="flex justify-end items-center gap-4">
                                <p className="text-[9px] font-bold text-zinc-600 uppercase tracking-tighter max-w-[200px] text-right leading-tight">
                                    Mốc ưu đãi sẽ được kích hoạt ngay sau khi lưu vào Sổ cái.
                                </p>
                                <button 
                                    type="submit" 
                                    disabled={submitting}
                                    className="bg-white text-black px-8 py-3 rounded-xl font-black text-[10px] uppercase tracking-widest hover:bg-amber-400 transition-all shadow-xl disabled:opacity-50"
                                >
                                    {submitting ? "Đang ghi sổ..." : "Lưu vào Hệ thống"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {/* List Section */}
            <div className="relative group animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200">
                <div className="absolute inset-0 bg-amber-500/[0.01] blur-3xl rounded-[3rem] pointer-events-none" />
                <div className="relative z-10 bg-white/[0.01] backdrop-blur-3xl rounded-[2.5rem] border border-white/5 overflow-hidden shadow-2xl">
                    <table className="w-full text-left">
                        <thead>
                            <tr className="border-b border-white/5 bg-white/[0.02]">
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-left">Điều kiện Kích hoạt</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-left">Phần thưởng (Gold)</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-center text-left">Tình trạng</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-right text-left">Pháp lệnh</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-white/5">
                            {loading ? (
                                <tr>
                                    <td colSpan={4} className="px-8 py-20 text-center">
                                        <Loader2 className="w-8 h-8 animate-spin text-amber-500 mx-auto mb-4" />
                                        <span className="text-[10px] font-black uppercase tracking-widest text-zinc-600">Đang đọc Sổ lệnh...</span>
                                    </td>
                                </tr>
                            ) : promotions.length === 0 ? (
                                <tr>
                                    <td colSpan={4} className="px-8 py-20 text-center text-[10px] font-black uppercase tracking-widest text-zinc-700">
                                        Vùng không gian ưu đãi đang trống vắng.
                                    </td>
                                </tr>
                            ) : (
                                promotions.map((p) => (
                                    <tr key={p.id} className="group/row hover:bg-white/[0.02] transition-colors">
                                        <td className="px-8 py-6">
                                            <div className="flex items-center gap-3">
                                                <div className="w-10 h-10 rounded-xl bg-zinc-900 border border-white/5 flex items-center justify-center">
                                                    <span className="text-[10px] font-black text-amber-500/50">₫</span>
                                                </div>
                                                <div className="text-[11px] font-black text-white uppercase tracking-tighter">
                                                    Từ {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(p.minAmountVnd)}
                                                </div>
                                            </div>
                                        </td>
                                        <td className="px-8 py-6">
                                            <div className="flex items-center gap-2 text-[11px] font-black text-amber-400 italic">
                                                <Coins className="w-4 h-4" />
                                                +{p.bonusDiamond.toLocaleString()}
                                            </div>
                                        </td>
                                        <td className="px-8 py-6 text-center">
                                            <button
                                                onClick={() => handleToggle(p)}
                                                className={`
                                                    relative inline-flex items-center gap-2 px-4 py-2 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all
                                                    ${p.isActive 
                                                        ? "bg-amber-500/10 border-amber-500/20 text-amber-400 shadow-[0_0_15px_rgba(245,158,11,0.1)] hover:bg-amber-500/20" 
                                                        : "bg-zinc-900 border-white/5 text-zinc-600 hover:text-zinc-400"
                                                    }
                                                `}
                                            >
                                                <Power className="w-3 h-3" />
                                                {p.isActive ? "Đang áp dụng" : "Vô hiệu hóa"}
                                            </button>
                                        </td>
                                        <td className="px-8 py-6 text-right">
                                            <button
                                                onClick={() => setDeleteId(p.id)}
                                                className="p-2 rounded-lg text-zinc-700 hover:text-rose-500 hover:bg-rose-500/10 transition-all active:scale-90"
                                            >
                                                <Trash2 className="w-4 h-4" />
                                            </button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* Delete Confirmation Modal */}
            {deleteId && (
                <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
                    <div 
                        className="absolute inset-0 bg-black/80 backdrop-blur-md transition-opacity"
                        onClick={() => setDeleteId(null)}
                    ></div>
                    
                    <div className="relative bg-[#0F0F0F] border border-white/10 rounded-[2.5rem] p-10 shadow-3xl max-w-sm w-full transform transition-all animate-in fade-in zoom-in duration-300">
                        <div className="text-center space-y-6">
                            <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-2xl bg-rose-500/10 border border-rose-500/20 text-rose-500">
                                <AlertTriangle className="h-8 w-8" />
                            </div>
                            <div className="space-y-2 text-left">
                                <h3 className="text-xl font-black text-white uppercase italic tracking-tighter">Xóa Khuyến mãi?</h3>
                                <p className="text-[11px] font-bold text-zinc-500 uppercase leading-relaxed tracking-wide">
                                    Hành động này sẽ xóa vĩnh viễn mức ưu đãi này khỏi Sổ cái Hệ thống. Các giao dịch hiện tại sẽ không bị ảnh hưởng.
                                </p>
                            </div>
                        </div>
                        
                        <div className="flex gap-4 mt-10">
                            <button
                                onClick={() => setDeleteId(null)}
                                className="flex-1 px-6 py-4 rounded-2xl border border-white/5 text-[10px] font-black uppercase tracking-widest text-zinc-500 hover:bg-white/5 transition-all text-left"
                            >
                                Giữ lại
                            </button>
                            <button
                                onClick={handleDelete}
                                className="flex-1 px-6 py-4 rounded-2xl bg-rose-500 text-black text-[10px] font-black uppercase tracking-widest hover:bg-rose-600 shadow-2xl shadow-rose-500/20 transition-all text-left"
                            >
                                Xóa bỏ
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
