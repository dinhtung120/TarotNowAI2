"use client";

import { useEffect, useState } from "react";
import { listUsers, toggleUserLock, addUserBalance, AdminUserItem } from "@/actions/adminActions";
import toast from 'react-hot-toast';
import { 
    Users, 
    Search, 
    Lock, 
    Unlock, 
    Plus, 
    Gem, 
    Coins, 
    ChevronLeft, 
    ChevronRight,
    Activity,
    Star,
    Mail,
    Loader2,
    X,
    CheckCircle2
} from "lucide-react";
import { SectionHeader, GlassCard, Button, Input } from "@/components/ui";

interface User extends AdminUserItem {
    isLocked: boolean;
}

export default function AdminUsersPage() {
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
    const [isMounted, setIsMounted] = useState(false);

    // State cho Custom Confirm Modal (Lock/Unlock)
    const [confirmModal, setConfirmModal] = useState<{
        isOpen: boolean;
        user: User | null;
        isLockAction: boolean;
    }>({ isOpen: false, user: null, isLockAction: true });

    const fetchUsers = async () => {
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
    };

    useEffect(() => {
        setIsMounted(true);
        fetchUsers();
    }, [page, searchTerm]);

    useEffect(() => {
        setIsMounted(true);
        fetchUsers();
    }, [page, searchTerm]);

    const handleConfirmToggleLock = async () => {
        if (!confirmModal.user) return;
        
        const { id, isLocked } = confirmModal.user;
        setConfirmModal(prev => ({ ...prev, isOpen: false }));
        setActionLoading(true);
        
        try {
            const success = await toggleUserLock(id, !isLocked);
            if (success) {
                toast.success(!isLocked ? "Đã khóa tài khoản thành công." : "Đã mở khóa tài khoản.");
                await fetchUsers();
            } else {
                toast.error("Lỗi khi thay đổi trạng thái tài khoản.");
            }
        } catch (err) {
            toast.error("Lỗi kết nối.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleAddBalance = async () => {
        if (!selectedUser) return;
        if (addAmount <= 0) {
            toast.error("Số tiền phải lớn hơn 0");
            return;
        }

        setActionLoading(true);
        try {
            const success = await addUserBalance(selectedUser.id, addCurrency, addAmount, addReason || `Admin manual topup ${addCurrency}`);
            if (success) {
                setIsAddBalanceOpen(false);
                setAddReason("");
                toast.success(`Đã cộng ${addAmount} ${addCurrency} thành công!`);
                await fetchUsers();
            } else {
                toast.error("Lỗi khi cộng tiền.");
            }
        } catch (err) {
            toast.error("Lỗi hệ thống.");
        } finally {
            setActionLoading(false);
        }
    };

    return (
        <div className="space-y-8 pb-20 animate-in fade-in duration-700">

            {/* Custom Confirm Modal (Lock/Unlock) */}
            {confirmModal.isOpen && (
                <div className="fixed inset-0 z-[150] flex items-center justify-center p-6 animate-in fade-in duration-300">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-md" onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))} />
                    <div className="relative z-10 w-full max-w-sm bg-[#0A0A0F] border border-white/10 rounded-[2.5rem] p-10 shadow-2xl animate-in zoom-in-95 duration-300 transform-gpu">
                        <div className={`w-16 h-16 rounded-2xl mx-auto mb-6 flex items-center justify-center border ${
                            confirmModal.user?.isLocked ? 'bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)]' : 'bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]'
                        }`}>
                            {confirmModal.user?.isLocked ? <Unlock className="w-8 h-8" /> : <Lock className="w-8 h-8" />}
                        </div>
                        <h3 className="text-xl font-black text-white uppercase italic tracking-tighter text-center mb-2">
                            {confirmModal.user?.isLocked ? "Mở khóa Tài khoản?" : "Khóa Tài khoản này?"}
                        </h3>
                        <p className="text-xs text-zinc-500 font-medium text-center leading-relaxed mb-8">
                            Xác nhận {confirmModal.user?.isLocked ? 'cho phép linh hồn này tiếp tục hành trình' : 'ngăn chặn sự truy cập'} của người dùng: <br/>
                            <span className="text-zinc-300 font-bold">{confirmModal.user?.displayName}</span>
                        </p>
                        <div className="flex gap-4">
                            <Button
                                variant="secondary"
                                onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))}
                                className="flex-1"
                            >
                                Hủy
                            </Button>
                            <Button
                                variant={confirmModal.user?.isLocked ? 'primary' : 'danger'}
                                onClick={handleConfirmToggleLock}
                                className="flex-1"
                            >
                                Thực hiện
                            </Button>
                        </div>
                    </div>
                </div>
            )}
            
            {/* Header Area */}
            <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
                <SectionHeader
                    tag="Database"
                    tagIcon={<Users className="w-3 h-3 text-[var(--purple-accent)]" />}
                    title="Quản Lý Người Dùng"
                    subtitle={`Tổng cộng ${totalCount} linh hồn đang hiện diện trong hệ thống`}
                    className="mb-0 text-left items-start"
                />

                <div className="flex items-center gap-3 shrink-0">
                    <Input
                        leftIcon={<Search className="w-4 h-4" />}
                        placeholder="TÊN, EMAIL, USERNAME..."
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
                            <tr className="border-b border-white/5 bg-white/[0.02]">
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Thông tin Tài khoản</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Thứ hạng / XP</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Tài sản (D/G)</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Vai trò</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center">Trạng thái</th>
                                <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-right">Hành động</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-white/5">
                            {loading ? (
                                <tr>
                                    <td colSpan={6} className="px-8 py-20 text-center">
                                        <div className="flex flex-col items-center justify-center space-y-4">
                                            <Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)]" />
                                            <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Đang truy vấn dữ liệu...</span>
                                        </div>
                                    </td>
                                </tr>
                            ) : users.length === 0 ? (
                                <tr>
                                    <td colSpan={6} className="px-8 py-20 text-center">
                                        <div className="flex flex-col items-center justify-center space-y-4">
                                            <div className="w-16 h-16 rounded-full bg-white/[0.02] border border-white/5 flex items-center justify-center">
                                                <Users className="w-8 h-8 text-[var(--text-tertiary)] opacity-50" />
                                            </div>
                                            <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">Vũ trụ trống rỗng, không tìm thấy linh hồn phù hợp.</span>
                                        </div>
                                    </td>
                                </tr>
                            ) : (
                                users.map((u) => (
                                    <tr key={u.id} className="group/row hover:bg-white/[0.02] transition-colors">
                                        <td className="px-8 py-5">
                                            <div className="flex items-center gap-4">
                                                <div className="w-10 h-10 rounded-xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-blue-500/20 border border-white/10 flex items-center justify-center text-white font-black text-sm relative overflow-hidden group-hover/row:scale-110 transition-transform shadow-inner">
                                                    {u.displayName?.charAt(0).toUpperCase() || 'U'}
                                                    <div className="absolute inset-0 bg-white/10 opacity-0 group-hover/row:opacity-100 transition-opacity" />
                                                </div>
                                                <div>
                                                    <div className="text-[11px] font-black text-white uppercase tracking-tighter drop-shadow-sm">{u.displayName}</div>
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
                                                    LV {u.level}
                                                </div>
                                                <div className="text-[10px] font-bold text-[var(--text-tertiary)]">{u.exp} EXP</div>
                                            </div>
                                        </td>
                                        <td className="px-8 py-5">
                                            <div className="space-y-1">
                                                <div className="flex items-center gap-2 text-[11px] font-black text-white italic">
                                                    <Gem className="w-3 h-3 text-[var(--purple-accent)]" />
                                                    {u.diamondBalance.toLocaleString()}
                                                </div>
                                                <div className="flex items-center gap-2 text-[10px] font-bold text-[var(--warning)] italic">
                                                    <Coins className="w-3 h-3" />
                                                    {u.goldBalance.toLocaleString()}
                                                </div>
                                            </div>
                                        </td>
                                        <td className="px-8 py-5">
                                            <div className={`
                                                inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-[9px] font-black uppercase tracking-widest border shadow-inner
                                                ${u.role === "admin" 
                                                    ? "bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]" 
                                                    : u.role === "tarot_reader"
                                                        ? "bg-cyan-500/10 border-cyan-500/20 text-cyan-400"
                                                        : "bg-white/[0.05] border-white/10 text-[var(--text-secondary)]"
                                                }
                                            `}>
                                                <Star className="w-2.5 h-2.5 fill-current" />
                                                {u.role}
                                            </div>
                                        </td>
                                        <td className="px-8 py-5 text-center">
                                            <div className={`
                                                inline-flex items-center justify-center gap-2 text-[9px] font-black uppercase tracking-widest
                                                ${u.isLocked ? "text-[var(--danger)]" : "text-[var(--success)]"}
                                            `}>
                                                {u.isLocked ? <Lock className="w-3 h-3" /> : <Activity className="w-3 h-3 animate-pulse" />}
                                                {u.isLocked ? "Bị Khóa" : "Hoạt Động"}
                                            </div>
                                        </td>
                                        <td className="px-8 py-5 text-right">
                                            <div className="flex items-center justify-end gap-2 opacity-0 group-hover/row:opacity-100 transition-opacity">
                                                <button
                                                    onClick={() => {
                                                        setSelectedUser(u);
                                                        setIsAddBalanceOpen(true);
                                                    }}
                                                    className="p-2 rounded-xl bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 text-[var(--purple-accent)] hover:bg-[var(--purple-accent)] hover:text-white transition-all shadow-md group border-transparent"
                                                    title="Cộng tiền thủ công"
                                                >
                                                    <Plus className="w-4 h-4" />
                                                </button>
                                                <button
                                                    onClick={() => setConfirmModal({ isOpen: true, user: u, isLockAction: true })}
                                                    className={`p-2 rounded-xl border transition-all shadow-md ${u.isLocked
                                                        ? "bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)] hover:bg-[var(--success)] hover:text-white"
                                                        : "bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)] hover:bg-[var(--danger)] hover:text-white"
                                                    }`}
                                                    title={u.isLocked ? "Mở khóa" : "Khóa tài khoản"}
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
                <div className="px-8 py-6 bg-white/[0.01] flex flex-col md:flex-row md:items-center justify-between gap-4 border-t border-white/5">
                    <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] text-left">
                        Vũ trụ {page} <span className="mx-2 opacity-30">|</span> Tổng {totalCount} người dùng
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

            {/* Premium Modal — Add Balance */}
            {isAddBalanceOpen && (
                <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 md:p-10 animate-in fade-in duration-300">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-md" onClick={() => setIsAddBalanceOpen(false)} />
                    
                    <div className="relative z-10 w-full max-w-lg bg-[#0A0A0F] border border-white/10 rounded-[3rem] overflow-hidden shadow-[0_0_100px_rgba(168,85,247,0.15)] animate-in zoom-in-95 duration-300">
                        {/* Modal Header */}
                        <div className="p-8 border-b border-white/5 bg-white/[0.02] flex items-center justify-between">
                            <div className="flex items-center gap-4">
                                <div className="w-12 h-12 rounded-2xl bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 flex items-center justify-center shadow-inner">
                                    <Gem className="w-6 h-6 text-[var(--purple-accent)]" />
                                </div>
                                <div className="text-left">
                                    <h2 className="text-xl font-black text-white uppercase italic tracking-tighter drop-shadow-md">Cộng tiền Thủ công</h2>
                                    <p className="text-[9px] font-black text-[var(--text-tertiary)] uppercase tracking-widest">Manual Balance Intervention</p>
                                </div>
                            </div>
                            <button 
                                onClick={() => setIsAddBalanceOpen(false)}
                                className="w-10 h-10 rounded-full bg-white/5 flex items-center justify-center text-[var(--text-secondary)] hover:bg-[var(--danger)] hover:text-white transition-all shadow-xl border border-transparent"
                            >
                                <X className="w-5 h-5" />
                            </button>
                        </div>

                        {/* Modal Body */}
                        <div className="p-10 space-y-8">
                            <div className="space-y-3">
                                <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">Người nhận vận may</label>
                                <div className="flex items-center gap-4 p-5 rounded-2xl bg-white/[0.02] border border-white/5 group transition-colors hover:border-white/10 shadow-inner">
                                    <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/20 flex items-center justify-center font-black text-xs text-white">
                                        {selectedUser?.displayName?.charAt(0) || 'U'}
                                    </div>
                                    <div className="text-left">
                                        <div className="text-xs font-black text-white uppercase tracking-tighter drop-shadow-sm">{selectedUser?.displayName}</div>
                                        <div className="text-[10px] font-bold text-[var(--text-tertiary)] italic">{selectedUser?.email}</div>
                                    </div>
                                </div>
                            </div>

                            <div className="grid grid-cols-2 gap-4">
                                <button 
                                    onClick={() => setAddCurrency("gold")}
                                    className={`relative p-6 rounded-[2rem] border transition-all duration-500 overflow-hidden text-left ${addCurrency === "gold" ? "bg-[var(--warning)]/10 border-[var(--warning)]/40 shadow-md" : "bg-white/[0.02] border-white/5 hover:border-white/10 shadow-sm"}`}
                                >
                                    <div className="relative z-10 flex flex-col gap-3">
                                        <div className={`p-2.5 rounded-xl border w-fit shadow-inner ${addCurrency === "gold" ? "bg-[var(--warning)]/20 border-[var(--warning)]/30" : "bg-white/5 border-white/10"}`}>
                                            <Coins className={`w-5 h-5 ${addCurrency === "gold" ? "text-[var(--warning)]" : "text-[var(--text-secondary)]"}`} />
                                        </div>
                                        <span className={`text-[11px] font-black uppercase tracking-[0.2em] ${addCurrency === "gold" ? "text-[var(--warning)]" : "text-[var(--text-secondary)]"}`}>Gold</span>
                                    </div>
                                    {addCurrency === "gold" && <div className="absolute -bottom-4 -right-4 w-16 h-16 bg-[var(--warning)]/20 blur-2xl rounded-full" />}
                                </button>

                                <button 
                                    onClick={() => setAddCurrency("diamond")}
                                    className={`relative p-6 rounded-[2rem] border transition-all duration-500 overflow-hidden text-left ${addCurrency === "diamond" ? "bg-[var(--purple-accent)]/10 border-[var(--purple-accent)]/40 shadow-md" : "bg-white/[0.02] border-white/5 hover:border-white/10 shadow-sm"}`}
                                >
                                    <div className="relative z-10 flex flex-col gap-3">
                                        <div className={`p-2.5 rounded-xl border w-fit shadow-inner ${addCurrency === "diamond" ? "bg-[var(--purple-accent)]/20 border-[var(--purple-accent)]/30" : "bg-white/5 border-white/10"}`}>
                                            <Gem className={`w-5 h-5 ${addCurrency === "diamond" ? "text-[var(--purple-accent)]" : "text-[var(--text-secondary)]"}`} />
                                        </div>
                                        <span className={`text-[11px] font-black uppercase tracking-[0.2em] ${addCurrency === "diamond" ? "text-[var(--purple-accent)]" : "text-[var(--text-secondary)]"}`}>Diamond</span>
                                    </div>
                                    {addCurrency === "diamond" && <div className="absolute -bottom-4 -right-4 w-16 h-16 bg-[var(--purple-accent)]/20 blur-2xl rounded-full" />}
                                </button>
                            </div>

                            <div className="space-y-4">
                                <div className="space-y-3">
                                    <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">Số lượng tài sản</label>
                                    <div className="relative group">
                                        <input
                                            type="number"
                                            value={addAmount}
                                            onChange={(e) => setAddAmount(Number(e.target.value))}
                                            className="w-full bg-white/[0.02] border border-white/10 rounded-2xl px-6 py-4 text-2xl font-black text-white italic tracking-tighter focus:outline-none focus:border-[var(--purple-accent)]/50 focus:bg-white/[0.05] transition-all shadow-inner"
                                        />
                                        <div className="absolute right-6 top-1/2 -translate-y-1/2 opacity-30">
                                            {addCurrency === "gold" ? <Coins className="w-5 h-5 text-[var(--warning)]" /> : <Gem className="w-5 h-5 text-[var(--purple-accent)]" />}
                                        </div>
                                    </div>
                                </div>

                                <div className="space-y-3">
                                    <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left">Lý do can thiệp</label>
                                    <textarea
                                        value={addReason}
                                        onChange={(e) => setAddReason(e.target.value)}
                                        placeholder="Ví dụ: Đền bù lỗi hệ thống, Quà tặng sự kiện..."
                                        className="w-full bg-white/[0.02] border border-white/10 rounded-2xl px-6 py-4 text-xs font-bold text-white focus:outline-none focus:border-[var(--purple-accent)]/50 focus:bg-white/[0.05] transition-all h-24 resize-none placeholder:text-zinc-700 shadow-inner"
                                    />
                                </div>
                            </div>

                            <div className="flex gap-4">
                                <Button
                                    variant="secondary"
                                    onClick={() => setIsAddBalanceOpen(false)}
                                    className="flex-1 py-6"
                                >
                                    Hủy tác vụ
                                </Button>
                                <Button
                                    variant="primary"
                                    onClick={handleAddBalance}
                                    disabled={actionLoading}
                                    className="flex-1 py-6 shadow-[0_0_20px_rgba(168,85,247,0.3)] hover:shadow-[0_0_30px_rgba(168,85,247,0.5)]"
                                >
                                    {actionLoading ? (
                                        <Loader2 className="w-5 h-5 animate-spin mx-auto" />
                                    ) : (
                                        <span className="flex items-center justify-center gap-2">
                                            Kích hoạt <Plus className="w-4 h-4" />
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
