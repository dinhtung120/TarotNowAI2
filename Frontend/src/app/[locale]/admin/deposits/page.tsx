"use client";

import { useEffect, useState } from "react";
import { listDeposits, processDeposit, AdminDepositOrder } from "@/actions/adminActions";
import toast from 'react-hot-toast';
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
    ThumbsDown,
    X
} from "lucide-react";
import { SectionHeader, GlassCard, Button } from "@/components/ui";

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

    const handleAction = async () => {
        if (!confirmModal.order) return;
        
        const { id } = confirmModal.order;
        const action = confirmModal.type;
        
        setConfirmModal(prev => ({ ...prev, isOpen: false }));
        setProcessingId(id);
        
        try {
            const success = await processDeposit(id, action);
            if (success) {
                toast.success(action === 'approve' ? "Phê duyệt thành công!" : "Đã từ chối đơn hàng.");
                await fetchOrders();
            } else {
                toast.error("Thất bại. Vui lòng thử lại.");
            }
        } catch (err) {
            toast.error("Lỗi kết nối máy chủ.");
        } finally {
            setProcessingId(null);
        }
    };

    const getStatusIcon = (status: string) => {
        switch (status) {
            case "Success": return <CheckCircle2 className="w-3 h-3 text-[var(--success)]" />;
            case "Failed": return <XCircle className="w-3 h-3 text-[var(--danger)]" />;
            default: return <Clock className="w-3 h-3 text-[var(--warning)]" />;
        }
    };

    const getStatusStyles = (status: string) => {
        switch (status) {
            case "Success": return "bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)] shadow-inner";
            case "Failed": return "bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)] shadow-inner";
            default: return "bg-[var(--warning)]/10 border-[var(--warning)]/20 text-[var(--warning)] shadow-inner";
        }
    };

    return (
        <div className="space-y-8 pb-20 animate-in fade-in duration-700">

            {/* Custom Confirm Modal */}
            {confirmModal.isOpen && (
                <div className="fixed inset-0 z-[150] flex items-center justify-center p-6 animate-in fade-in duration-300">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-md" onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))} />
                    <div className="relative z-10 w-full max-w-sm bg-[#0A0A0F] border border-white/10 rounded-[2.5rem] p-10 shadow-2xl animate-in zoom-in-95 duration-300">
                        <div className={`w-16 h-16 rounded-2xl mx-auto mb-6 flex items-center justify-center border shadow-inner ${
                            confirmModal.type === 'approve' ? 'bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)]' : 'bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]'
                        }`}>
                            {confirmModal.type === 'approve' ? <ThumbsUp className="w-8 h-8" /> : <ThumbsDown className="w-8 h-8" />}
                        </div>
                        <h3 className="text-xl font-black text-white uppercase italic tracking-tighter text-center mb-2">
                            {confirmModal.type === 'approve' ? "Phê duyệt Giao dịch?" : "Từ chối Giao dịch?"}
                        </h3>
                        <p className="text-xs text-[var(--text-secondary)] font-medium text-center leading-relaxed mb-8">
                            Hành động này sẽ thay đổi trạng thái dòng tiền và {confirmModal.type === 'approve' ? 'cộng tài sản cho linh hồn này' : 'hủy bỏ yêu cầu'}.
                        </p>
                        <div className="flex gap-4">
                            <Button 
                                variant="secondary"
                                onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))}
                                className="flex-1"
                            >
                                Quay lại
                            </Button>
                            <Button 
                                variant={confirmModal.type === 'approve' ? 'primary' : 'danger'}
                                onClick={handleAction}
                                className="flex-1"
                            >
                                Xác nhận
                            </Button>
                        </div>
                    </div>
                </div>
            )}

            {/* Header Area */}
            <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
                <SectionHeader
                    tag="Finance"
                    tagIcon={<CreditCard className="w-3 h-3 text-[var(--purple-accent)]" />}
                    title="Dòng tiền Hệ thống"
                    subtitle={`Theo dõi ${totalCount} giao dịch nạp Diamond từ nhân gian`}
                    className="mb-0 text-left items-start"
                />

                <div className="flex items-center gap-4 bg-white/[0.02] border border-white/10 rounded-[2rem] p-3 px-6 shadow-inner backdrop-blur-xl shrink-0">
                    <div className="flex items-center gap-2">
                        <Filter className="w-4 h-4 text-[var(--text-secondary)]" />
                        <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Lọc:</span>
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
                                    px-4 py-2 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all
                                    ${statusFilter === s 
                                        ? "bg-[var(--purple-accent)] text-black shadow-md" 
                                        : "bg-white/5 text-[var(--text-tertiary)] hover:bg-white/10 hover:text-white"
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
            <GlassCard className="!p-0 !rounded-[2.5rem] overflow-hidden text-left">
                <div className="overflow-x-auto custom-scrollbar">
                    <table className="w-full text-left">
                        <thead>
                            <tr className="border-b border-white/5 bg-white/[0.02]">
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Mã Đơn / TXN ID</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Linh hồn Phù hợp</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Giá trị (VND)</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Tài sản Thực nhận</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Thời điểm</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">Hiện trạng</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">Thao tác</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-white/5">
                            {loading ? (
                                <tr>
                                    <td colSpan={7} className="px-8 py-20 text-center">
                                        <div className="flex flex-col items-center justify-center space-y-4">
                                            <Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)]" />
                                            <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Đang truy vấn sổ cái...</span>
                                        </div>
                                    </td>
                                </tr>
                            ) : orders.length === 0 ? (
                                <tr>
                                    <td colSpan={7} className="px-8 py-20 text-center">
                                        <div className="flex flex-col items-center justify-center space-y-4">
                                            <div className="w-16 h-16 rounded-full bg-white/[0.02] border border-white/5 flex items-center justify-center">
                                                <CreditCard className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
                                            </div>
                                            <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">Không có giao dịch nào xuất hiện trong tầm nhìn này.</span>
                                        </div>
                                    </td>
                                </tr>
                            ) : (
                                orders.map((o) => (
                                    <tr key={o.id} className="group/row hover:bg-white/[0.02] transition-colors">
                                        <td className="px-8 py-5">
                                            <div className="space-y-1">
                                                <div className="flex items-center gap-1.5 text-[9px] font-black text-[var(--text-secondary)] uppercase tracking-tighter">
                                                    <Hash className="w-2.5 h-2.5 opacity-50" />
                                                    {o.id.split('-')[0]}...
                                                </div>
                                                {o.transactionId ? (
                                                    <div className="text-[11px] font-black text-[var(--accent)] uppercase tracking-tighter italic drop-shadow-sm">
                                                        {o.transactionId}
                                                    </div>
                                                ) : (
                                                    <div className="text-[10px] font-bold text-zinc-600 uppercase italic">Chưa có mã TXN</div>
                                                )}
                                            </div>
                                        </td>
                                        <td className="px-8 py-5">
                                            <div className="flex items-center gap-3">
                                                <div className="w-8 h-8 rounded-lg bg-black/40 border border-white/10 flex items-center justify-center shadow-inner">
                                                    <User className="w-4 h-4 text-[var(--text-secondary)]" />
                                                </div>
                                                <div>
                                                    <div className="text-[11px] font-black text-white uppercase tracking-tighter drop-shadow-sm">{o.username || "System User"}</div>
                                                    <div className="text-[9px] font-bold text-[var(--text-tertiary)] italic tracking-tighter">
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
                                            <div className="flex items-center gap-2 text-[11px] font-black text-[var(--purple-accent)] italic">
                                                <Gem className="w-3.5 h-3.5" />
                                                +{o.diamondAmount.toLocaleString()}
                                            </div>
                                        </td>
                                        <td className="px-8 py-5">
                                            <div className="flex flex-col text-left">
                                                <div className="text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter">
                                                    {new Date(o.createdAt).toLocaleDateString("vi-VN")}
                                                </div>
                                                <div className="text-[10px] font-bold text-[var(--text-tertiary)] italic">
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
                                                <div className="flex items-center justify-center gap-2 opacity-0 group-hover/row:opacity-100 transition-opacity">
                                                    <button
                                                        onClick={() => {
                                                            setConfirmModal({ isOpen: true, type: 'approve', order: o });
                                                        }}
                                                        disabled={processingId === o.id}
                                                        className="p-2 rounded-xl bg-[var(--success)]/10 text-[var(--success)] border border-[var(--success)]/20 hover:bg-[var(--success)] hover:text-black transition-all disabled:opacity-50 shadow-md group"
                                                        title="Phê duyệt"
                                                    >
                                                        {processingId === o.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <ThumbsUp className="w-4 h-4 group-hover:scale-110 transition-transform" />}
                                                    </button>
                                                    <button
                                                        onClick={() => {
                                                            setConfirmModal({ isOpen: true, type: 'reject', order: o });
                                                        }}
                                                        disabled={processingId === o.id}
                                                        className="p-2 rounded-xl bg-[var(--danger)]/10 text-[var(--danger)] border border-[var(--danger)]/20 hover:bg-[var(--danger)] hover:text-white transition-all disabled:opacity-50 shadow-md group"
                                                        title="Từ chối"
                                                    >
                                                        {processingId === o.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <ThumbsDown className="w-4 h-4 group-hover:scale-110 transition-transform" />}
                                                    </button>
                                                </div>
                                            ) : (
                                                <span className="text-[10px] font-bold text-[var(--text-tertiary)] italic">N/A</span>
                                            )}
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>

                {/* Pagination */}
                <div className="px-8 py-6 bg-white/[0.01] flex flex-col md:flex-row md:items-center justify-between gap-4 border-t border-white/5">
                    <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] text-left">
                        Trang {page} <span className="mx-2 opacity-30">|</span> Tổng {totalCount} giao dịch
                    </div>
                    <div className="flex items-center gap-3">
                        <button
                            onClick={() => setPage(p => Math.max(1, p - 1))}
                            disabled={page === 1}
                            className="p-2.5 rounded-xl bg-white/5 border border-white/10 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md"
                        >
                            <ChevronLeft className="w-4 h-4 text-[var(--text-secondary)]" />
                        </button>
                        <span className="text-xs font-black text-[var(--purple-accent)] italic mx-2">{page}</span>
                        <button
                            onClick={() => setPage(p => p + 1)}
                            disabled={page * 10 >= totalCount}
                            className="p-2.5 rounded-xl bg-white/5 border border-white/10 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md"
                        >
                            <ChevronRight className="w-4 h-4 text-[var(--text-secondary)]" />
                        </button>
                    </div>
                </div>
            </GlassCard>
            
            {/* Quick Summary Section */}
            {!loading && orders.length > 0 && (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 animate-in fade-in slide-in-from-bottom-4 duration-1000">
                    <GlassCard className="!p-8 relative overflow-hidden group hover:border-[var(--purple-accent)]/30 transition-all text-left">
                        <div className="absolute -bottom-4 -right-4 opacity-5 group-hover:scale-110 transition-transform duration-700">
                            <ArrowUpRight size={150} />
                        </div>
                        <div className="relative z-10 space-y-4">
                            <div className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] drop-shadow-sm">Xác nhận Đơn hàng</div>
                            <p className="text-xs text-[var(--text-secondary)] leading-relaxed font-medium">Hệ thống tự động đồng bộ từ cổng thanh toán. Hãy kiểm tra kỹ mã TXN trước khi can thiệp thủ công.</p>
                        </div>
                    </GlassCard>
                    
                    <GlassCard className="!p-8 flex items-center gap-6 group hover:border-white/10 transition-all text-left">
                        <div className="w-14 h-14 rounded-2xl bg-black/40 flex items-center justify-center border border-white/10 group-hover:scale-110 transition-transform shadow-inner">
                            <Gem className="w-7 h-7 text-[var(--purple-accent)]" />
                        </div>
                        <div>
                            <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Thông tin Sổ cái</div>
                            <div className="text-sm font-black text-white uppercase italic tracking-tighter drop-shadow-md">Bản ghi Tài chính v2.4</div>
                        </div>
                    </GlassCard>
                </div>
            )}
        </div>
    );
}
