"use client";

/**
 * Trang Quản lý Khuyến mãi (Admin Promotions) - Astral Premium Redesign
 * * Các cải tiến:
 * 1. Astral Form: Form thêm khuyến mãi được tích hợp mờ kính và hiệu ứng phát sáng.
 * 2. Status Toggles: Nút gạt trạng thái (Active/Inactive) chuyển sang ngôn ngữ thiết kế Astral.
 * 3. Reward Alignment: Cập nhật hiển thị Quà tặng là Gold thay vì Diamond (theo yêu cầu User).
 * 4. Micro-interactions: Hiệu ứng hover cho bảng và các nút hành động.
 */

import { useEffect, useState } from "react";
import { getAccessToken } from "@/lib/auth-client";
import { Ticket, X, Trash2, CheckCircle2, Power, AlertTriangle,
 Coins,
 Sparkles,
 Loader2,
 PlusCircle
} from "lucide-react";
import { SectionHeader, GlassCard, Button } from "@/components/ui";
import toast from 'react-hot-toast';

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

 // Form states
 const [isCreating, setIsCreating] = useState(false);
 const [minAmount, setMinAmount] = useState<number>(0);
 const [bonusGold, setBonusGold] = useState<number>(0);
 const [deleteId, setDeleteId] = useState<string | null>(null);
 const [submitting, setSubmitting] = useState(false);

 const fetchPromotions = async () => {
 setLoading(true);
 try {
 const token = getAccessToken();
 const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
 const res = await fetch(`${baseUrl}/admin/promotions`, {
 headers: { "Content-Type": "application/json",
 "Authorization": `Bearer ${token}`
 }
 });

 if (!res.ok) {
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
 setSubmitting(true);
 try {
 const token = getAccessToken();
 const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";
 const res = await fetch(`${baseUrl}/admin/promotions`, {
 method: "POST",
 headers: { "Content-Type": "application/json",
 "Authorization": `Bearer ${token}`
 },
 body: JSON.stringify({ minAmountVnd: minAmount, bonusDiamond: bonusGold, // Mapping UI Gold to backend field
 isActive: true })
 });
 if (res.ok) {
 setIsCreating(false);
 setMinAmount(0);
 setBonusGold(0);
 toast.success("Đã thêm ưu đãi thành công!");
 await fetchPromotions();
 } else {
 toast.error("Lỗi khi thêm khuyến mãi.");
 }
 } catch {
 toast.error("Lỗi kết nối.");
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
 toast.success("Đã cập nhật trạng thái.");
 await fetchPromotions();
 } else {
 toast.error("Lỗi khi thay đổi trạng thái.");
 }
 } catch {
 toast.error("Lỗi kết nối.");
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
 toast.success("Đã xóa ưu đãi vĩnh viễn.");
 await fetchPromotions();
 } else {
 const errorData = await res.json().catch(() => ({}));
 toast.error(errorData.message || "Lỗi khi xóa.");
 }
 } catch {
 toast.error("Lỗi kết nối.");
 }
 };

 return (
 <div className="space-y-8 pb-20 animate-in fade-in duration-700">
 {/* Header Area */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag="Marketing"
 tagIcon={<Ticket className="w-3 h-3 text-[var(--warning)]" />}
 title="Chương trình Ưu đãi"
 subtitle="Thiết lập các mốc nạp tệ để ban thưởng Vàng cho linh hồn"
 className="mb-0 text-left items-start"
 />

 <Button
 variant={isCreating ? "secondary" : "primary"}
 onClick={() => setIsCreating(!isCreating)}
 className={`shrink-0 ${!isCreating && 'bg-[var(--warning)] tn-text-ink hover:bg-[var(--warning)] hover:brightness-110 shadow-[0_0_20px_var(--c-245-158-11-20)]'}`}
 >
 {isCreating ? <X className="w-4 h-4" /> : <PlusCircle className="w-4 h-4" />}
 {isCreating ? "Hủy bỏ" : "Thêm Ưu đãi mới"}
 </Button>
 </div>

 {/* Create form - Inline Expansion */}
 {isCreating && (
 <div className="animate-in fade-in slide-in-from-top-4 duration-500">
 <GlassCard className="relative overflow-hidden group hover:border-[var(--warning)]/30 transition-all !p-8">
 <div className="absolute top-0 right-0 p-16 bg-[var(--warning)]/10 blur-[100px] rounded-full" />
 <form onSubmit={handleCreate} className="relative z-10 space-y-8">
 <div className="flex items-center gap-3 border-b tn-border-soft pb-4">
 <Sparkles className="w-5 h-5 text-[var(--warning)]" />
 <h2 className="text-sm font-black tn-text-primary uppercase tracking-widest drop-shadow-sm">Khởi tạo Mốc thưởng mới</h2>
 </div>

 <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
 <div className="space-y-3 text-left">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Mức nạp tối thiểu (VND)</label>
 <div className="relative">
 <input
 type="number"
 min="0"
 required
 value={minAmount}
 onChange={(e) => setMinAmount(Number(e.target.value))}
 placeholder="200,000"
 className="w-full tn-field rounded-2xl pl-12 pr-4 py-4 text-xs font-black tn-text-primary tn-field-warning transition-all placeholder:text-[var(--text-tertiary)] shadow-inner"
 />
 <div className="absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none">
 <span className="text-sm font-black text-[var(--text-secondary)]">₫</span>
 </div>
 </div>
 </div>

 <div className="space-y-3 text-left">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Lượng Vàng thưởng thêm</label>
 <div className="relative">
 <input
 type="number"
 min="0"
 required
 value={bonusGold}
 onChange={(e) => setBonusGold(Number(e.target.value))}
 placeholder="500"
 className="w-full tn-field rounded-2xl pl-12 pr-4 py-4 text-xs font-black tn-text-primary tn-field-warning transition-all placeholder:text-[var(--text-tertiary)] shadow-inner"
 />
 <div className="absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none">
 <Coins className="w-4 h-4 text-[var(--warning)]" />
 </div>
 </div>
 </div>
 </div>

 <div className="flex flex-col sm:flex-row justify-end items-center gap-4 pt-2 border-t tn-border-soft">
 <p className="text-[9px] font-bold text-[var(--text-tertiary)] uppercase tracking-tighter text-center sm:text-right leading-relaxed flex-1">
 Mốc ưu đãi sẽ được kích hoạt ngay sau khi lưu vào Sổ cái.
 </p>
 <Button type="submit" disabled={submitting}
 className="w-full sm:w-auto px-8 py-4 bg-[var(--warning)] tn-text-ink hover:bg-[var(--warning)] hover:brightness-110 shadow-[0_0_20px_var(--c-245-158-11-20)]"
 >
 {submitting ? <Loader2 className="w-4 h-4 animate-spin mr-2" /> : <CheckCircle2 className="w-4 h-4 mr-2" />}
 {submitting ? "Đang ghi sổ..." : "Lưu vào Hệ thống"}
 </Button>
 </div>
 </form>
 </GlassCard>
 </div>
 )}

 {/* List Section */}
 <GlassCard className="!p-0 !rounded-[2.5rem] overflow-hidden text-left">
 <div className="overflow-x-auto custom-scrollbar">
 <table className="w-full text-left">
 <thead>
 <tr className="border-b tn-border-soft tn-surface">
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Điều kiện Kích hoạt</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Phần thưởng (Gold)</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">Tình trạng</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-right">Pháp lệnh</th>
 </tr>
 </thead>
 <tbody className="divide-y divide-white/5">
 {loading ? (
 <tr>
 <td colSpan={4} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <Loader2 className="w-8 h-8 animate-spin text-[var(--warning)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Đang đọc Sổ lệnh...</span>
 </div>
 </td>
 </tr>
 ) : promotions.length === 0 ? (
 <tr>
 <td colSpan={4} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <div className="w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center">
 <Ticket className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
 </div>
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">Vùng không gian ưu đãi đang trống vắng.</span>
 </div>
 </td>
 </tr>
 ) : (
 promotions.map((p) => (
 <tr key={p.id} className="group/row hover:tn-surface transition-colors">
 <td className="px-8 py-6">
 <div className="flex items-center gap-4">
 <div className="w-10 h-10 rounded-xl tn-panel-overlay flex items-center justify-center shadow-inner group-hover/row:scale-110 transition-transform">
 <span className="text-[12px] font-black text-[var(--text-secondary)]">₫</span>
 </div>
 <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">
 Từ {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(p.minAmountVnd)}
 </div>
 </div>
 </td>
 <td className="px-8 py-6">
 <div className="flex items-center gap-2 text-sm font-black text-[var(--warning)] italic drop-shadow-sm">
 <Coins className="w-4 h-4" />
 +{p.bonusDiamond.toLocaleString()}
 </div>
 </td>
 <td className="px-8 py-6 text-center">
 <button
 onClick={() => handleToggle(p)}
 className={`
 relative inline-flex items-center gap-2 px-4 py-2 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all shadow-inner
 ${p.isActive ? "bg-[var(--warning)]/10 border-[var(--warning)]/30 text-[var(--warning)] shadow-md hover:bg-[var(--warning)]/20" : "tn-panel text-[var(--text-secondary)] hover:tn-surface hover:tn-text-primary"
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
 className="p-3 rounded-xl text-[var(--text-secondary)] tn-panel-soft hover:tn-text-primary hover:bg-[var(--danger)] hover:border-transparent transition-all shadow-sm group"
 >
 <Trash2 className="w-4 h-4 group-hover:scale-110 transition-transform" />
 </button>
 </td>
 </tr>
 ))
 )}
 </tbody>
 </table>
 </div>
 </GlassCard>

 {/* Delete Confirmation Modal */}
 {deleteId && (
 <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
 <div className="absolute inset-0 tn-overlay-strong transition-opacity"
 onClick={() => setDeleteId(null)}
 ></div>
 <div className="relative tn-panel rounded-[2.5rem] p-10 shadow-2xl max-w-sm w-full transform transition-all animate-in fade-in zoom-in duration-300">
 <div className="text-center space-y-6">
 <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-2xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] shadow-inner">
 <AlertTriangle className="h-8 w-8" />
 </div>
 <div className="space-y-4 text-left">
 <h3 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter text-center mb-2">Xóa Khuyến mãi?</h3>
 <p className="text-[11px] font-bold text-[var(--text-secondary)] uppercase leading-relaxed tracking-wide text-center">
 Hành động này sẽ xóa vĩnh viễn mức ưu đãi này khỏi Sổ cái Hệ thống. Các giao dịch hiện tại sẽ không bị ảnh hưởng.
 </p>
 </div>
 </div>
 <div className="flex gap-4 mt-8">
 <Button
 variant="secondary"
 onClick={() => setDeleteId(null)}
 className="flex-1"
 >
 Giữ lại
 </Button>
 <Button
 variant="danger"
 onClick={handleDelete}
 className="flex-1 shadow-[0_0_20px_var(--c-244-63-94-30)] hover:shadow-[0_0_30px_var(--c-244-63-94-50)]"
 >
 Xóa bỏ
 </Button>
 </div>
 </div>
 </div>
 )}
 </div>
 );
}
