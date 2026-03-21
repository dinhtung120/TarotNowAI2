/*
 * ===================================================================
 * FILE: page.tsx (Admin Deposits)
 * BỐI CẢNH (CONTEXT):
 *   Màn hình kiểm soát các giao dịch Nạp Tiền (Deposit) của User.
 *   Cho phép Admin Xem Lịch sử, Bộ lọc trạng thái, Duyệt (Approve) hoặc Hủy (Reject) đơn.
 *
 * BẢO MẬT:
 *   Admin gọi hành động Phê Duyệt qua Server Actions. Quá trình xử lý phức tạp 
 *   (Giao dịch, nạp kim cương) diễn ra hoàn toàn trên Backend.
 * ===================================================================
 */
"use client";

import { useCallback, useEffect, useState } from "react";
import { listDeposits, processDeposit, AdminDepositOrder } from "@/actions/adminActions";
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from "next-intl";
import { CreditCard, Filter, CheckCircle2, XCircle, Clock, ChevronLeft, ChevronRight,
 ArrowUpRight,
 Gem,
 User,
 Loader2,
 ThumbsUp,
 ThumbsDown
} from "lucide-react";
import { SectionHeader, GlassCard, Button } from "@/components/ui";
import { formatCurrency } from "@/shared/utils/format/formatCurrency";
import { formatDate, formatTime } from "@/shared/utils/format/formatDateTime";

export default function AdminDepositsPage() {
 const t = useTranslations("Admin");
 const locale = useLocale();
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

 const fetchOrders = useCallback(async () => {
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
 }, [page, statusFilter]);

 useEffect(() => {
 const initialFetchTimer = window.setTimeout(() => {
 void fetchOrders();
 }, 0);

 return () => {
 window.clearTimeout(initialFetchTimer);
 };
 }, [fetchOrders]);

 const handleAction = async () => {
 if (!confirmModal.order) return;
 const { id } = confirmModal.order;
 const action = confirmModal.type;
 setConfirmModal(prev => ({ ...prev, isOpen: false }));
 setProcessingId(id);
 try {
 const success = await processDeposit(id, action);
 if (success) {
 toast.success(action === 'approve' ? t("deposits.toast.approve_success") : t("deposits.toast.reject_success"));
 await fetchOrders();
 } else {
 toast.error(t("deposits.toast.action_failed"));
 }
 } catch {
 toast.error(t("deposits.toast.network_error"));
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
 <div className="absolute inset-0 tn-overlay-strong " onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))} />
 <div className="relative z-10 w-full max-w-sm tn-panel rounded-[2.5rem] p-10 shadow-2xl animate-in zoom-in-95 duration-300">
 <div className={`w-16 h-16 rounded-2xl mx-auto mb-6 flex items-center justify-center border shadow-inner ${
 confirmModal.type === 'approve' ? 'bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)]' : 'bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]'
 }`}>
 {confirmModal.type === 'approve' ? <ThumbsUp className="w-8 h-8" /> : <ThumbsDown className="w-8 h-8" />}
 </div>
 <h3 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter text-center mb-2">
 {confirmModal.type === 'approve' ? t("deposits.modal.title_approve") : t("deposits.modal.title_reject")}
 </h3>
 <p className="text-xs text-[var(--text-secondary)] font-medium text-center leading-relaxed mb-8">
 {confirmModal.type === 'approve' ? t("deposits.modal.desc_approve") : t("deposits.modal.desc_reject")}
 </p>
 <div className="flex gap-4">
 <Button variant="secondary"
 onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))}
 className="flex-1"
 >
 {t("deposits.modal.back")}
 </Button>
 <Button variant={confirmModal.type === 'approve' ? 'primary' : 'danger'}
 onClick={handleAction}
 className="flex-1"
 >
 {t("deposits.modal.confirm")}
 </Button>
 </div>
 </div>
 </div>
 )}

 {/* Header Area */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag={t("deposits.header.tag")}
 tagIcon={<CreditCard className="w-3 h-3 text-[var(--purple-accent)]" />}
 title={t("deposits.header.title")}
 subtitle={t("deposits.header.subtitle", { count: totalCount })}
 className="mb-0 text-left items-start"
 />

 <div className="flex items-center gap-4 tn-panel rounded-[2rem] p-3 px-6 shadow-inner shrink-0">
 <div className="flex items-center gap-2">
 <Filter className="w-4 h-4 text-[var(--text-secondary)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("deposits.filters.label")}:</span>
 </div>
 <div className="flex gap-2">
 {[
 { value: "", label: t("deposits.filters.all") },
 { value: "Pending", label: t("deposits.filters.pending") },
 { value: "Success", label: t("deposits.filters.success") },
 { value: "Failed", label: t("deposits.filters.failed") },
 ].map(({ value, label }) => (
 <button
 key={value}
 onClick={() => {
 setStatusFilter(value);
 setPage(1);
 }}
 className={`
 px-4 py-2.5 min-h-11 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all
 ${statusFilter === value ? "bg-[var(--purple-accent)] tn-text-ink shadow-md" : "tn-surface text-[var(--text-tertiary)] hover:tn-surface-strong hover:tn-text-primary"
 }
 `}
 >
 {label}
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
 <tr className="border-b tn-border-soft tn-surface">
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("deposits.table.heading_user")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("deposits.table.heading_amount")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("deposits.table.heading_assets")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("deposits.table.heading_time")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">{t("deposits.table.heading_status")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">{t("deposits.table.heading_actions")}</th>
 </tr>
 </thead>
 <tbody className="divide-y divide-white/5">
 {loading ? (
 <tr>
 <td colSpan={6} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("deposits.states.loading")}</span>
 </div>
 </td>
 </tr>
 ) : orders.length === 0 ? (
 <tr>
 <td colSpan={6} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <div className="w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center">
 <CreditCard className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
 </div>
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("deposits.states.empty")}</span>
 </div>
 </td>
 </tr>
 ) : (
 orders.map((o) => (
 <tr key={o.id} className="group/row hover:tn-surface transition-colors">
 <td className="px-8 py-5">
 <div className="flex items-center gap-3">
 <div className="w-8 h-8 rounded-lg tn-panel-overlay flex items-center justify-center shadow-inner">
 <User className="w-4 h-4 text-[var(--text-secondary)]" />
 </div>
 <div>
 <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">{o.username || t("deposits.row.system_user")}</div>
 <div className="text-[9px] font-bold text-[var(--text-tertiary)] italic tracking-tighter">
 {t("deposits.row.user_id_prefix", { id: o.userId.split('-')[0] })}
 </div>
 </div>
 </div>
 </td>
 <td className="px-8 py-5">
 <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter">
 {formatCurrency(o.amountVnd, locale)}
 </div>
 </td>
 <td className="px-8 py-5">
 <div className="flex items-center gap-2 text-[11px] font-black text-[var(--purple-accent)] italic">
 <Gem className="w-3.5 h-3.5" />
 +{o.diamondAmount.toLocaleString(locale)}
 </div>
 </td>
 <td className="px-8 py-5">
 <div className="flex flex-col text-left">
 <div className="text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter">
 {formatDate(o.createdAt, locale)}
 </div>
 <div className="text-[10px] font-bold text-[var(--text-tertiary)] italic">
 {formatTime(o.createdAt, locale)}
 </div>
 </div>
 </td>
 <td className="px-8 py-5 text-center">
 <div className={`
 inline-flex items-center gap-2 px-3 py-1.5 rounded-xl text-[9px] font-black uppercase tracking-widest border
 ${getStatusStyles(o.status)}
 `}>
 {getStatusIcon(o.status)}
 {o.status === "Success" ? t("deposits.status.success") : o.status === "Failed" ? t("deposits.status.failed") : t("deposits.status.pending")}
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
 className="p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--success)]/10 text-[var(--success)] border border-[var(--success)]/20 hover:bg-[var(--success)] hover:tn-text-ink transition-all disabled:opacity-50 shadow-md group"
 title={t("deposits.actions.approve_title")}
 >
 {processingId === o.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <ThumbsUp className="w-4 h-4 group-hover:scale-110 transition-transform" />}
 </button>
 <button
 onClick={() => {
 setConfirmModal({ isOpen: true, type: 'reject', order: o });
 }}
 disabled={processingId === o.id}
 className="p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--danger)]/10 text-[var(--danger)] border border-[var(--danger)]/20 hover:bg-[var(--danger)] hover:tn-text-primary transition-all disabled:opacity-50 shadow-md group"
 title={t("deposits.actions.reject_title")}
 >
 {processingId === o.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <ThumbsDown className="w-4 h-4 group-hover:scale-110 transition-transform" />}
 </button>
 </div>
 ) : (
 <span className="text-[10px] font-bold text-[var(--text-tertiary)] italic">{t("deposits.actions.na")}</span>
 )}
 </td>
 </tr>
 ))
 )}
 </tbody>
 </table>
 </div>

 {/* Pagination */}
 <div className="px-8 py-6 tn-surface-soft flex flex-col md:flex-row md:items-center justify-between gap-4 border-t tn-border-soft">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] text-left">
 {t("deposits.pagination.summary", { page, total: totalCount })}
 </div>
 <div className="flex items-center gap-3">
 <button
 onClick={() => setPage(p => Math.max(1, p - 1))}
 disabled={page === 1}
 className="p-2.5 min-h-11 min-w-11 rounded-xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md"
 >
 <ChevronLeft className="w-4 h-4 text-[var(--text-secondary)]" />
 </button>
 <span className="text-xs font-black text-[var(--purple-accent)] italic mx-2">{page}</span>
 <button
 onClick={() => setPage(p => p + 1)}
 disabled={page * 10 >= totalCount}
 className="p-2.5 min-h-11 min-w-11 rounded-xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md"
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
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] drop-shadow-sm">{t("deposits.summary.confirm_title")}</div>
 <p className="text-xs text-[var(--text-secondary)] leading-relaxed font-medium">{t("deposits.summary.confirm_desc")}</p>
 </div>
 </GlassCard>
 <GlassCard className="!p-8 flex items-center gap-6 group hover:tn-border transition-all text-left">
 <div className="w-14 h-14 rounded-2xl tn-overlay flex items-center justify-center border tn-border group-hover:scale-110 transition-transform shadow-inner">
 <Gem className="w-7 h-7 text-[var(--purple-accent)]" />
 </div>
 <div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("deposits.summary.ledger_title")}</div>
 <div className="text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md">{t("deposits.summary.ledger_subtitle")}</div>
 </div>
 </GlassCard>
 </div>
 )}
 </div>
 );
}
