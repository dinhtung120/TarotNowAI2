"use client";

/**
 * Trang Quản lý Giao dịch (Admin Deposits Management) - Astral Premium Redesign
 * 
 * Các cải tiến:
 * 1. Transaction Table: Bảng danh sách mờ kính chuyên sâu cho dữ liệu tài chính.
 * 2. Status Glow: Nhãn trạng thái (Success, Pending, Failed) với hiệu ứng phát sáng.
 * 3. Filter Hub: Bộ lọc trạng thái được thiết kế lại dạng Glassmorphism.
 * 4. Data Visualization: Hiển thị mã giao dịch và mã đơn hàng rõ ràng, dễ sao chép.
 */

import { useEffect, useState } from "react";
import { listDeposits, processDeposit, AdminDepositOrder } from "@/actions/adminActions";
import { 
    CreditCard, 
    Filter, 
    CheckCircle2, 
    XCircle, 
    Clock, 
    Search, 
    ChevronLeft, 
    ChevronRight,
    ArrowUpRight,
    Gem,
    Hash,
    User,
    Calendar,
    Loader2,
    ThumbsUp,
    ThumbsDown
} from "lucide-react";

export default function AdminDepositsPage() {
    const [orders, setOrders] = useState<AdminDepositOrder[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [statusFilter, setStatusFilter] = useState("");
    const [loading, setLoading] = useState(true);
    const [processingId, setProcessingId] = useState<string | null>(null);
    
    // State cho Custom Confirm Modal
    const [confirmModal, setConfirmModal] = useState<{
        isOpen: boolean;
        type: 'approve' | 'reject';
        order: AdminDepositOrder | null;
    }>({ isOpen: false, type: 'approve', order: null });

    const [toast, setToast] = useState<{ msg: string; type: 'success' | 'error' } | null>(null);

    const fetchOrders = async () => {
        setLoading(true);
        try {
            const data = await listDeposits(page, 10, statusFilter);
            if (data) {
                setOrders(data.deposits);
                setTotalCount(data.totalCount);
            }
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchOrders();
    }, [page, statusFilter]);

    useEffect(() => {
        if (toast) {
            const timer = setTimeout(() => setToast(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [toast]);

    const handleAction = async () => {
        if (!confirmModal.order) return;
        
        const { id } = confirmModal.order;
        const action = confirmModal.type;
        
        setConfirmModal(prev => ({ ...prev, isOpen: false }));
        setProcessingId(id);
        
        try {
            const success = await processDeposit(id, action);
            if (success) {
                setToast({ msg: action === 'approve' ? "Phê duyệt thành công!" : "Đã từ chối đơn hàng.", type: 'success' });
                await fetchOrders();
            } else {
                setToast({ msg: "Thất bại. Vui lòng thử lại.", type: 'error' });
            }
        } catch (err) {
            setToast({ msg: "Lỗi kết nối máy chủ.", type: 'error' });
        } finally {
            setProcessingId(null);
        }
    };

    const getStatusIcon = (status: string) => {
        switch (status) {
            case "Success": return <CheckCircle2 className="w-3 h-3 text-emerald-400" />;
            case "Failed": return <XCircle className="w-3 h-3 text-rose-400" />;
            default: return <Clock className="w-3 h-3 text-amber-400" />;
        }
    };

    const getStatusStyles = (status: string) => {
        switch (status) {
            case "Success": return "bg-emerald-500/10 border-emerald-500/20 text-emerald-400 shadow-[0_0_15px_rgba(16,185,129,0.1)]";
            case "Failed": return "bg-rose-500/10 border-rose-500/20 text-rose-400 shadow-[0_0_15px_rgba(244,63,94,0.1)]";
            default: return "bg-amber-500/10 border-amber-500/20 text-amber-500 shadow-[0_0_15px_rgba(245,158,11,0.1)]";
        }
    };

    return (
        <div className="space-y-8 pb-20">
            {/* Toast System */}
            {toast && (
                <div className={`fixed top-10 right-10 z-[200] px-6 py-4 rounded-2xl border backdrop-blur-3xl shadow-2xl animate-in slide-in-from-right-10 duration-500 flex items-center gap-3 ${
                    toast.type === 'success' ? 'bg-emerald-500/20 border-emerald-500/40 text-emerald-400' : 'bg-rose-500/20 border-rose-500/40 text-rose-400'
                }`}>
                    {toast.type === 'success' ? <CheckCircle2 className="w-5 h-5" /> : <XCircle className="w-5 h-5" />}
                    <span className="text-xs font-black uppercase tracking-widest">{toast.msg}</span>
                </div>
            )}

            {/* Custom Confirm Modal */}
            {confirmModal.isOpen && (
                <div className="fixed inset-0 z-[150] flex items-center justify-center p-6 animate-in fade-in duration-300">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-md" onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))} />
                    <div className="relative z-10 w-full max-w-sm bg-[#0A0A0F] border border-white/10 rounded-[2.5rem] p-10 shadow-2xl animate-in zoom-in-95 duration-300">
                        <div className={`w-16 h-16 rounded-2xl mx-auto mb-6 flex items-center justify-center border ${
                            confirmModal.type === 'approve' ? 'bg-emerald-500/10 border-emerald-500/20 text-emerald-400' : 'bg-rose-500/10 border-rose-500/20 text-rose-400'
                        }`}>
                            {confirmModal.type === 'approve' ? <ThumbsUp className="w-8 h-8" /> : <ThumbsDown className="w-8 h-8" />}
                        </div>
                        <h3 className="text-xl font-black text-white uppercase italic tracking-tighter text-center mb-2">
                            {confirmModal.type === 'approve' ? "Phê duyệt Giao dịch?" : "Từ chối Giao dịch?"}
                        </h3>
                        <p className="text-xs text-zinc-500 font-medium text-center leading-relaxed mb-8">
                            Hành động này sẽ thay đổi trạng thái dòng tiền và {confirmModal.type === 'approve' ? 'cộng tài sản cho linh hồn này' : 'hủy bỏ yêu cầu'}.
                        </p>
                        <div className="flex gap-4">
                            <button 
                                onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))}
                                className="flex-1 py-4 rounded-xl bg-white/5 border border-white/5 text-[10px] font-black uppercase tracking-widest text-zinc-600 hover:bg-white/10 transition-all"
                            >
                                Quay lại
                            </button>
                            <button 
                                onClick={handleAction}
                                className={`flex-1 py-4 rounded-xl text-[10px] font-black uppercase tracking-widest text-black shadow-xl transition-all ${
                                    confirmModal.type === 'approve' ? 'bg-emerald-500 hover:bg-emerald-400' : 'bg-rose-500 hover:bg-rose-400'
                                }`}
                            >
                                Xác nhận
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {/* Header Area */}
            <div className="flex flex-col md:flex-row md:items-end justify-between gap-6 animate-in fade-in slide-in-from-bottom-4 duration-700">
                <div className="space-y-1 text-left">
                    <h1 className="text-3xl font-black text-white uppercase italic tracking-tighter flex items-center gap-3">
                        <CreditCard className="w-8 h-8 text-emerald-400" />
                        Dòng tiền Hệ thống
                    </h1>
                    <p className="text-zinc-500 text-xs font-bold uppercase tracking-widest">
                        Theo dõi {totalCount} giao dịch nạp Diamond từ nhân gian
                    </p>
                </div>

                <div className="flex items-center gap-4 bg-white/[0.03] border border-white/5 rounded-2xl p-2 px-4 shadow-2xl backdrop-blur-xl">
                    <div className="flex items-center gap-2">
                        <Filter className="w-3.5 h-3.5 text-zinc-600" />
                        <span className="text-[10px] font-black uppercase tracking-widest text-zinc-600">Lọc trạng thái:</span>
                    </div>
                    <div className="flex gap-2">
                        {["", "Pending", "Success", "Failed"].map((s) => (
                            <button
                                key={s}
                                onClick={() => {
                                    setStatusFilter(s);
                                    setPage(1);
                                }}
                                className={`
                                    px-3 py-1.5 rounded-lg text-[10px] font-black uppercase tracking-widest transition-all
                                    ${statusFilter === s 
                                        ? "bg-emerald-500 text-black shadow-lg" 
                                        : "bg-white/5 text-zinc-500 hover:bg-white/10"
                                    }
                                `}
                            >
                                {s === "" ? "Tất cả" : s}
                            </button>
                        ))}
                    </div>
                </div>
            </div>

            {/* Main Table Card */}
            <div className="relative group animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200 text-left">
                <div className="absolute inset-0 bg-emerald-500/[0.02] blur-3xl rounded-[3rem] pointer-events-none" />
                <div className="relative z-10 bg-white/[0.01] backdrop-blur-3xl rounded-[2.5rem] border border-white/5 overflow-hidden shadow-2xl">
                    <div className="overflow-x-auto custom-scrollbar">
                        <table className="w-full text-left">
                            <thead>
                                <tr className="border-b border-white/5 bg-white/[0.02]">
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Mã Đơn / TXN ID</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Linh hồn Phù hợp</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Giá trị (VND)</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Tài sản Thực nhận</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Thời điểm</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-center">Hiện trạng</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-center">Thao tác</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-white/5">
                                {loading ? (
                                    <tr>
                                        <td colSpan={7} className="px-8 py-20 text-center">
                                            <Loader2 className="w-8 h-8 animate-spin text-emerald-500 mx-auto mb-4" />
                                            <span className="text-[10px] font-black uppercase tracking-widest text-zinc-600">Đang truy vấn sổ cái...</span>
                                        </td>
                                    </tr>
                                ) : orders.length === 0 ? (
                                    <tr>
                                        <td colSpan={7} className="px-8 py-20 text-center text-[10px] font-black uppercase tracking-widest text-zinc-700">
                                            Không có giao dịch nào xuất hiện trong tầm nhìn này.
                                        </td>
                                    </tr>
                                ) : (
                                    orders.map((o) => (
                                        <tr key={o.id} className="group/row hover:bg-white/[0.02] transition-colors">
                                            <td className="px-8 py-5">
                                                <div className="space-y-1">
                                                    <div className="flex items-center gap-1.5 text-[9px] font-black text-zinc-600 uppercase tracking-tighter">
                                                        <Hash className="w-2.5 h-2.5 opacity-50" />
                                                        {o.id.split('-')[0]}...
                                                    </div>
                                                    {o.transactionId ? (
                                                        <div className="text-[11px] font-black text-emerald-400 uppercase tracking-tighter italic">
                                                            {o.transactionId}
                                                        </div>
                                                    ) : (
                                                        <div className="text-[10px] font-bold text-zinc-800 uppercase italic">Chưa có mã TXN</div>
                                                    )}
                                                </div>
                                            </td>
                                            <td className="px-8 py-5">
                                                <div className="flex items-center gap-3">
                                                    <div className="w-8 h-8 rounded-lg bg-zinc-900 border border-white/5 flex items-center justify-center">
                                                        <User className="w-4 h-4 text-zinc-700" />
                                                    </div>
                                                    <div>
                                                        <div className="text-[11px] font-black text-white uppercase tracking-tighter">{o.username || "System User"}</div>
                                                        <div className="text-[9px] font-bold text-zinc-600 italic tracking-tighter opacity-50">
                                                            ID: {o.userId.split('-')[0]}...
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="px-8 py-5">
                                                <div className="text-[11px] font-black text-white uppercase tracking-tighter">
                                                    {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(o.amountVnd)}
                                                </div>
                                            </td>
                                            <td className="px-8 py-5">
                                                <div className="flex items-center gap-2 text-[11px] font-black text-emerald-400 italic">
                                                    <Gem className="w-3.5 h-3.5" />
                                                    +{o.diamondAmount.toLocaleString()}
                                                </div>
                                            </td>
                                            <td className="px-8 py-5">
                                                <div className="flex flex-col text-left">
                                                    <div className="text-[10px] font-black text-zinc-500 uppercase tracking-tighter">
                                                        {new Date(o.createdAt).toLocaleDateString("vi-VN")}
                                                    </div>
                                                    <div className="text-[10px] font-bold text-zinc-700 italic">
                                                        {new Date(o.createdAt).toLocaleTimeString("vi-VN")}
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="px-8 py-5 text-center">
                                                <div className={`
                                                    inline-flex items-center gap-2 px-3 py-1.5 rounded-xl text-[9px] font-black uppercase tracking-widest border
                                                    ${getStatusStyles(o.status)}
                                                `}>
                                                    {getStatusIcon(o.status)}
                                                    {o.status === "Success" ? "Thành công" : o.status === "Failed" ? "Thất bại" : "Đang xử lý"}
                                                </div>
                                            </td>
                                            <td className="px-8 py-5 text-center">
                                                {o.status === "Pending" ? (
                                                    <div className="flex items-center justify-center gap-2">
                                                        <button
                                                            onClick={() => {
                                                                setConfirmModal({ isOpen: true, type: 'approve', order: o });
                                                            }}
                                                            disabled={processingId === o.id}
                                                            className="p-2 rounded-lg bg-emerald-500/10 text-emerald-500 hover:bg-emerald-500 hover:text-black transition-all disabled:opacity-50"
                                                            title="Phê duyệt"
                                                        >
                                                            {processingId === o.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <ThumbsUp className="w-4 h-4" />}
                                                        </button>
                                                        <button
                                                            onClick={() => {
                                                                setConfirmModal({ isOpen: true, type: 'reject', order: o });
                                                            }}
                                                            disabled={processingId === o.id}
                                                            className="p-2 rounded-lg bg-rose-500/10 text-rose-500 hover:bg-rose-500 hover:text-black transition-all disabled:opacity-50"
                                                            title="Từ chối"
                                                        >
                                                            {processingId === o.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <ThumbsDown className="w-4 h-4" />}
                                                        </button>
                                                    </div>
                                                ) : (
                                                    <span className="text-[10px] font-bold text-zinc-700 italic">N/A</span>
                                                )}
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>

                    {/* Pagination */}
                    <div className="px-8 py-6 bg-white/[0.02] flex flex-col md:flex-row md:items-center justify-between gap-4 border-t border-white/5">
                        <div className="text-[10px] font-black uppercase tracking-widest text-zinc-600 text-left">
                            Trang {page} <span className="mx-2 opacity-30">|</span> Tổng {totalCount} giao dịch
                        </div>
                        <div className="flex items-center gap-3">
                            <button
                                onClick={() => setPage(p => Math.max(1, p - 1))}
                                disabled={page === 1}
                                className="p-2.5 rounded-xl bg-white/5 border border-white/10 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
                            >
                                <ChevronLeft className="w-4 h-4" />
                            </button>
                            <span className="text-xs font-black text-white italic mx-2">{page}</span>
                            <button
                                onClick={() => setPage(p => p + 1)}
                                disabled={page * 10 >= totalCount}
                                className="p-2.5 rounded-xl bg-white/5 border border-white/10 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
                            >
                                <ChevronRight className="w-4 h-4" />
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            
            {/* Quick Summary Section */}
            {!loading && orders.length > 0 && (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 animate-in fade-in slide-in-from-bottom-4 duration-1000">
                    <div className="p-8 rounded-[2.5rem] bg-gradient-to-br from-emerald-500/10 to-transparent border border-emerald-500/20 backdrop-blur-3xl shadow-2xl relative overflow-hidden group">
                        <div className="absolute -bottom-4 -right-4 opacity-5 group-hover:scale-110 transition-transform duration-700">
                            <ArrowUpRight size={150} />
                        </div>
                        <div className="relative z-10 space-y-4 text-left">
                            <div className="text-[10px] font-black uppercase tracking-widest text-emerald-500/70">Xác nhận Đơn hàng</div>
                            <p className="text-xs text-zinc-400 leading-relaxed font-medium">Hệ thống tự động đồng bộ từ cổng thanh toán. Hãy kiểm tra kỹ mã TXN trước khi can thiệp thủ công.</p>
                        </div>
                    </div>
                    
                    <div className="p-8 rounded-[2.5rem] bg-white/[0.02] border border-white/5 backdrop-blur-3xl shadow-2xl flex items-center gap-6 group hover:border-white/10 transition-all text-left">
                        <div className="w-14 h-14 rounded-2xl bg-zinc-900 flex items-center justify-center border border-white/10 group-hover:scale-110 transition-transform">
                            <Gem className="w-7 h-7 text-emerald-400" />
                        </div>
                        <div>
                            <div className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Thông tin Sổ cái</div>
                            <div className="text-sm font-black text-white uppercase italic tracking-tighter">Bản ghi Tài chính v2.4</div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
