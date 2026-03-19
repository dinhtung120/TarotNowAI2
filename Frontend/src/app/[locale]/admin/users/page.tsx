/*
 * ===================================================================
 * FILE: page.tsx (Admin Users Management)
 * BỐI CẢNH (CONTEXT):
 *   Phân hệ User dành cho Admin.
 *   Bao gồm: Xem danh sách user, Khóa/Mở khóa tài khoản (Ban/Unban) và Nạp Kim Cương thủ công (Top-up).
 * 
 * KIẾN TRÚC UI:
 *   Áp dụng mô hình Glassmorphism tối đa với Confirm Modal. Data mutation (thêm số dư, thay đổi trạng thái user)
 *   phụ thuộc hoàn toàn vào Security Middleware của Backend.
 * ===================================================================
 */
"use client";

import { useCallback, useEffect, useState } from "react";
import { listUsers, toggleUserLock, addUserBalance, AdminUserItem } from "@/actions/adminActions";
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from "next-intl";
import { Users, Search, Lock, Unlock, Plus, Gem, Coins, ChevronLeft, ChevronRight,
 Activity,
 Star,
 Mail,
 Loader2,
 X
} from "lucide-react";
import { SectionHeader, GlassCard, Button, Input } from "@/components/ui";

interface User extends AdminUserItem {
 isLocked: boolean;
}

export default function AdminUsersPage() {
 const t = useTranslations("Admin");
 const locale = useLocale();
 const [users, setUsers] = useState<User[]>([]);
 const [totalCount, setTotalCount] = useState(0);
 const [page, setPage] = useState(1);
 const [searchTerm, setSearchTerm] = useState("");
 const [loading, setLoading] = useState(true);

 // State cho Modal nạp tiền
 const [isAddBalanceOpen, setIsAddBalanceOpen] = useState(false);
 const [selectedUser, setSelectedUser] = useState<User | null>(null);
 const [addAmount, setAddAmount] = useState(100);
 const [addCurrency, setAddCurrency] = useState("gold");
 const [addReason, setAddReason] = useState("");
 const [actionLoading, setActionLoading] = useState(false);

 // State cho Custom Confirm Modal (Lock/Unlock)
 const [confirmModal, setConfirmModal] = useState<{
 isOpen: boolean;
 user: User | null;
 isLockAction: boolean;
 }>({ isOpen: false, user: null, isLockAction: true });

 const fetchUsers = useCallback(async () => {
 setLoading(true);
 try {
 const data = await listUsers(page, 10, searchTerm);
 if (data) {
 const mappedUsers: User[] = data.users.map(u => ({
 ...u,
 isLocked: u.status === "Locked"
 }));
 setUsers(mappedUsers);
 setTotalCount(data.totalCount);
 }
 } catch (err) {
 console.error(err);
 } finally {
 setLoading(false);
 }
 }, [page, searchTerm]);

 useEffect(() => {
 const initialFetchTimer = window.setTimeout(() => {
 void fetchUsers();
 }, 0);

 return () => {
 window.clearTimeout(initialFetchTimer);
 };
 }, [fetchUsers]);

 const handleConfirmToggleLock = async () => {
 if (!confirmModal.user) return;
 const { id, isLocked } = confirmModal.user;
 setConfirmModal(prev => ({ ...prev, isOpen: false }));
 setActionLoading(true);
 try {
 const success = await toggleUserLock(id, !isLocked);
 if (success) {
 toast.success(!isLocked ? t("users.toast.lock_success") : t("users.toast.unlock_success"));
 await fetchUsers();
 } else {
 toast.error(t("users.toast.toggle_failed"));
 }
 } catch {
 toast.error(t("users.toast.network_error"));
 } finally {
 setActionLoading(false);
 }
 };

 const handleAddBalance = async () => {
 if (!selectedUser) return;
 if (addAmount <= 0) {
 toast.error(t("users.toast.amount_invalid"));
 return;
 }

 setActionLoading(true);
 try {
 const currencyLabel = addCurrency === "gold" ? t("users.add_balance.currency_gold") : t("users.add_balance.currency_diamond");
 const success = await addUserBalance(selectedUser.id, addCurrency, addAmount, addReason || `Admin manual topup ${addCurrency}`);
 if (success) {
 setIsAddBalanceOpen(false);
 setAddReason("");
 toast.success(t("users.toast.add_success", { amount: addAmount, currency: currencyLabel }));
 await fetchUsers();
 } else {
 toast.error(t("users.toast.add_failed"));
 }
 } catch {
 toast.error(t("users.toast.system_error"));
 } finally {
 setActionLoading(false);
 }
 };

 return (
 <div className="space-y-8 pb-20 animate-in fade-in duration-700">

 {/* Custom Confirm Modal (Lock/Unlock) */}
 {confirmModal.isOpen && (
 <div className="fixed inset-0 z-[150] flex items-center justify-center p-6 animate-in fade-in duration-300">
 <div className="absolute inset-0 tn-overlay-strong " onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))} />
 <div className="relative z-10 w-full max-w-sm tn-panel rounded-[2.5rem] p-10 shadow-2xl animate-in zoom-in-95 duration-300 transform-gpu">
 <div className={`w-16 h-16 rounded-2xl mx-auto mb-6 flex items-center justify-center border ${
 confirmModal.user?.isLocked ? 'bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)]' : 'bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]'
 }`}>
 {confirmModal.user?.isLocked ? <Unlock className="w-8 h-8" /> : <Lock className="w-8 h-8" />}
 </div>
 <h3 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter text-center mb-2">
 {confirmModal.user?.isLocked ? t("users.lock_modal.title_unlock") : t("users.lock_modal.title_lock")}
 </h3>
 <p className="text-xs tn-text-muted font-medium text-center leading-relaxed mb-8">
 {confirmModal.user?.isLocked
 ? t("users.lock_modal.desc_unlock", { name: confirmModal.user?.displayName || "" })
 : t("users.lock_modal.desc_lock", { name: confirmModal.user?.displayName || "" })}
 </p>
 <div className="flex gap-4">
 <Button
 variant="secondary"
 onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))}
 className="flex-1"
 >
 {t("users.lock_modal.cancel")}
 </Button>
 <Button
 variant={confirmModal.user?.isLocked ? 'primary' : 'danger'}
 onClick={handleConfirmToggleLock}
 className="flex-1"
 >
 {t("users.lock_modal.confirm")}
 </Button>
 </div>
 </div>
 </div>
 )}
 {/* Header Area */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag={t("users.header.tag")}
 tagIcon={<Users className="w-3 h-3 text-[var(--purple-accent)]" />}
 title={t("users.header.title")}
 subtitle={t("users.header.subtitle", { count: totalCount })}
 className="mb-0 text-left items-start"
 />

 <div className="flex items-center gap-3 shrink-0">
 <Input
 leftIcon={<Search className="w-4 h-4" />}
 placeholder={t("users.search.placeholder")}
 value={searchTerm}
 onChange={(e) => {
 setSearchTerm(e.target.value);
 setPage(1);
 }}
 className="w-full md:w-80"
 />
 </div>
 </div>

 {/* Main Table Card */}
 <GlassCard className="!p-0 !rounded-[2.5rem] overflow-hidden">
 <div className="overflow-x-auto custom-scrollbar">
 <table className="w-full text-left">
 <thead>
 <tr className="border-b tn-border-soft tn-surface">
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_account")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_rank")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_assets")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">{t("users.table.heading_role")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">{t("users.table.heading_status")}</th>
 <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-right">{t("users.table.heading_actions")}</th>
 </tr>
 </thead>
 <tbody className="divide-y divide-white/5">
 {loading ? (
 <tr>
 <td colSpan={6} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("users.states.loading")}</span>
 </div>
 </td>
 </tr>
 ) : users.length === 0 ? (
 <tr>
 <td colSpan={6} className="px-8 py-20 text-center">
 <div className="flex flex-col items-center justify-center space-y-4">
 <div className="w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center">
 <Users className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
 </div>
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("users.states.empty")}</span>
 </div>
 </td>
 </tr>
 ) : (
 users.map((u) => (
 <tr key={u.id} className="group/row hover:tn-surface transition-colors">
 <td className="px-8 py-5">
 <div className="flex items-center gap-4">
 <div className="w-10 h-10 rounded-xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--info)]/20 border tn-border flex items-center justify-center tn-text-primary font-black text-sm relative overflow-hidden group-hover/row:scale-110 transition-transform shadow-inner">
 {u.displayName?.charAt(0).toUpperCase() || 'U'}
 <div className="absolute inset-0 tn-surface-strong opacity-0 group-hover/row:opacity-100 transition-opacity" />
 </div>
 <div>
 <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">{u.displayName}</div>
 <div className="flex items-center gap-1.5 text-[9px] font-bold text-[var(--text-tertiary)]">
 <Mail className="w-2.5 h-2.5" />
 {u.email}
 </div>
 </div>
 </div>
 </td>
 <td className="px-8 py-5">
 <div className="flex items-center gap-2">
 <div className="px-2 py-0.5 rounded-md bg-[var(--warning)]/10 border border-[var(--warning)]/20 text-[9px] font-black text-[var(--warning)] shadow-inner">
 {t("users.row.level", { level: u.level })}
 </div>
 <div className="text-[10px] font-bold text-[var(--text-tertiary)]">{t("users.row.exp", { exp: u.exp })}</div>
 </div>
 </td>
 <td className="px-8 py-5">
 <div className="space-y-1">
 <div className="flex items-center gap-2 text-[11px] font-black tn-text-primary italic">
 <Gem className="w-3 h-3 text-[var(--purple-accent)]" />
 {u.diamondBalance.toLocaleString(locale)}
 </div>
 <div className="flex items-center gap-2 text-[10px] font-bold text-[var(--warning)] italic">
 <Coins className="w-3 h-3" />
 {u.goldBalance.toLocaleString(locale)}
 </div>
 </div>
 </td>
 <td className="px-8 py-5">
 <div className={`
 inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-[9px] font-black uppercase tracking-widest border shadow-inner
 ${u.role === "admin" ? "bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]" : u.role === "tarot_reader"
 ? "bg-[var(--info)]/10 border-[var(--info)]/20 text-[var(--info)]"
 : "tn-surface-strong tn-border text-[var(--text-secondary)]"
 }
 `}>
 <Star className="w-2.5 h-2.5 fill-current" />
 {u.role === "admin" ? t("users.roles.admin") : u.role === "tarot_reader" ? t("users.roles.tarot_reader") : u.role === "user" ? t("users.roles.user") : u.role}
 </div>
 </td>
 <td className="px-8 py-5 text-center">
 <div className={`
 inline-flex items-center justify-center gap-2 text-[9px] font-black uppercase tracking-widest
 ${u.isLocked ? "text-[var(--danger)]" : "text-[var(--success)]"}
 `}>
 {u.isLocked ? <Lock className="w-3 h-3" /> : <Activity className="w-3 h-3 animate-pulse" />}
 {u.isLocked ? t("users.status.locked") : t("users.status.active")}
 </div>
 </td>
 <td className="px-8 py-5 text-right">
 <div className="flex items-center justify-end gap-2 opacity-0 group-hover/row:opacity-100 transition-opacity">
 <button
 onClick={() => {
 setSelectedUser(u);
 setIsAddBalanceOpen(true);
 }}
 className="p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 text-[var(--purple-accent)] hover:bg-[var(--purple-accent)] hover:tn-text-primary transition-all shadow-md group border-transparent"
 title={t("users.actions.add_balance_title")}
 >
 <Plus className="w-4 h-4" />
 </button>
 <button
 onClick={() => setConfirmModal({ isOpen: true, user: u, isLockAction: true })}
 className={`p-2.5 min-h-11 min-w-11 rounded-xl border transition-all shadow-md ${u.isLocked
 ? "bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)] hover:bg-[var(--success)] hover:tn-text-primary"
 : "bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)] hover:bg-[var(--danger)] hover:tn-text-primary"
 }`}
 title={u.isLocked ? t("users.actions.unlock_title") : t("users.actions.lock_title")}
 >
 {u.isLocked ? <Unlock className="w-4 h-4" /> : <Lock className="w-4 h-4" />}
 </button>
 </div>
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
 {t("users.pagination.summary", { page, total: totalCount })}
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

 {/* Premium Modal — Add Balance */}
 {isAddBalanceOpen && (
 <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 md:p-10 animate-in fade-in duration-300">
 <div className="absolute inset-0 tn-overlay-strong " onClick={() => setIsAddBalanceOpen(false)} />
 <div className="relative z-10 w-full max-w-lg tn-panel rounded-[3rem] overflow-hidden shadow-[0_0_100px_var(--c-168-85-247-15)] animate-in zoom-in-95 duration-300">
 {/* Modal Header */}
 <div className="p-8 border-b tn-border-soft tn-surface flex items-center justify-between">
 <div className="flex items-center gap-4">
 <div className="w-12 h-12 rounded-2xl bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 flex items-center justify-center shadow-inner">
 <Gem className="w-6 h-6 text-[var(--purple-accent)]" />
 </div>
 <div className="text-left">
 <h2 className="text-xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md">{t("users.add_balance.title")}</h2>
 <p className="text-[9px] font-black text-[var(--text-tertiary)] uppercase tracking-widest">{t("users.add_balance.subtitle")}</p>
 </div>
 </div>
 <button onClick={() => setIsAddBalanceOpen(false)}
 className="w-10 h-10 rounded-full tn-surface flex items-center justify-center text-[var(--text-secondary)] hover:bg-[var(--danger)] hover:tn-text-primary transition-all shadow-xl border border-transparent"
 >
 <X className="w-5 h-5" />
 </button>
 </div>

 {/* Modal Body */}
 <div className="p-10 space-y-8">
 <div className="space-y-3">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">{t("users.add_balance.recipient_label")}</label>
 <div className="flex items-center gap-4 p-5 rounded-2xl tn-panel-soft group transition-colors hover:tn-border shadow-inner">
 <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/20 flex items-center justify-center font-black text-xs tn-text-primary">
 {selectedUser?.displayName?.charAt(0) || 'U'}
 </div>
 <div className="text-left">
 <div className="text-xs font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">{selectedUser?.displayName}</div>
 <div className="text-[10px] font-bold text-[var(--text-tertiary)] italic">{selectedUser?.email}</div>
 </div>
 </div>
 </div>

 <div className="grid grid-cols-2 gap-4">
 <button onClick={() => setAddCurrency("gold")}
 className={`relative p-6 rounded-[2rem] border transition-all duration-500 overflow-hidden text-left ${addCurrency === "gold" ? "bg-[var(--warning)]/10 border-[var(--warning)]/40 shadow-md" : "tn-panel-soft hover:tn-border shadow-sm"}`}
 >
 <div className="relative z-10 flex flex-col gap-3">
 <div className={`p-2.5 rounded-xl border w-fit shadow-inner ${addCurrency === "gold" ? "bg-[var(--warning)]/20 border-[var(--warning)]/30" : "tn-panel"}`}>
 <Coins className={`w-5 h-5 ${addCurrency === "gold" ? "text-[var(--warning)]" : "text-[var(--text-secondary)]"}`} />
 </div>
 <span className={`text-[11px] font-black uppercase tracking-[0.2em] ${addCurrency === "gold" ? "text-[var(--warning)]" : "text-[var(--text-secondary)]"}`}>{t("users.add_balance.currency_gold")}</span>
 </div>
 {addCurrency === "gold" && <div className="absolute -bottom-4 -right-4 w-16 h-16 bg-[var(--warning)]/20 blur-2xl rounded-full" />}
 </button>

 <button onClick={() => setAddCurrency("diamond")}
 className={`relative p-6 rounded-[2rem] border transition-all duration-500 overflow-hidden text-left ${addCurrency === "diamond" ? "bg-[var(--purple-accent)]/10 border-[var(--purple-accent)]/40 shadow-md" : "tn-panel-soft hover:tn-border shadow-sm"}`}
 >
 <div className="relative z-10 flex flex-col gap-3">
 <div className={`p-2.5 rounded-xl border w-fit shadow-inner ${addCurrency === "diamond" ? "bg-[var(--purple-accent)]/20 border-[var(--purple-accent)]/30" : "tn-panel"}`}>
 <Gem className={`w-5 h-5 ${addCurrency === "diamond" ? "text-[var(--purple-accent)]" : "text-[var(--text-secondary)]"}`} />
 </div>
 <span className={`text-[11px] font-black uppercase tracking-[0.2em] ${addCurrency === "diamond" ? "text-[var(--purple-accent)]" : "text-[var(--text-secondary)]"}`}>{t("users.add_balance.currency_diamond")}</span>
 </div>
 {addCurrency === "diamond" && <div className="absolute -bottom-4 -right-4 w-16 h-16 bg-[var(--purple-accent)]/20 blur-2xl rounded-full" />}
 </button>
 </div>

 <div className="space-y-4">
 <div className="space-y-3">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">{t("users.add_balance.amount_label")}</label>
 <div className="relative group">
 <input
 type="number"
 value={addAmount}
 onChange={(e) => setAddAmount(Number(e.target.value))}
 className="w-full tn-field rounded-2xl px-6 py-4 text-2xl font-black tn-text-primary italic tracking-tighter tn-field-accent transition-all shadow-inner"
 />
 <div className="absolute right-6 top-1/2 -translate-y-1/2 opacity-30">
 {addCurrency === "gold" ? <Coins className="w-5 h-5 text-[var(--warning)]" /> : <Gem className="w-5 h-5 text-[var(--purple-accent)]" />}
 </div>
 </div>
 </div>

 <div className="space-y-3">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">{t("users.add_balance.reason_label")}</label>
 <textarea
 value={addReason}
 onChange={(e) => setAddReason(e.target.value)}
 placeholder={t("users.add_balance.reason_placeholder")}
 className="w-full tn-field rounded-2xl px-6 py-4 text-xs font-bold tn-text-primary tn-field-accent transition-all h-24 resize-none placeholder:tn-text-muted shadow-inner"
 />
 </div>
 </div>

 <div className="flex gap-4">
 <Button
 variant="secondary"
 onClick={() => setIsAddBalanceOpen(false)}
 className="flex-1 py-6"
 >
 {t("users.add_balance.cancel")}
 </Button>
 <Button
 variant="primary"
 onClick={handleAddBalance}
 disabled={actionLoading}
 className="flex-1 py-6 shadow-[0_0_20px_var(--c-168-85-247-30)] hover:shadow-[0_0_30px_var(--c-168-85-247-50)]"
 >
 {actionLoading ? (
 <Loader2 className="w-5 h-5 animate-spin mx-auto" />
 ) : (
 <span className="flex items-center justify-center gap-2">
 {t("users.add_balance.submit")} <Plus className="w-4 h-4" />
 </span>
 )}
 </Button>
 </div>
 </div>
 </div>
 </div>
 )}
 </div>
 );
}
