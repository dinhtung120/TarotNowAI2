"use client";

/**
 * Trang Quản lý Người dùng (Admin Users Management) - Astral Premium Redesign
 * 
 * Các cải tiến:
 * 1. Astral Tables: Bảng danh sách mờ kính với hiệu ứng Glassmorphism.
 * 2. Premium Avatars: Hiển thị avatar người dùng với viền Glow.
 * 3. Gradient Chips: Nhãn Role/Level được thiết kế lại rực rỡ hơn.
 * 4. Premium Modals: Giao diện nạp tiền thủ công chuyên nghiệp và sang trọng.
 */

import { useEffect, useState, useMemo } from "react";
import { listUsers, toggleUserLock, addUserBalance, AdminUserItem } from "@/actions/adminActions";
import { 
    Users, 
    Search, 
    ShieldAlert, 
    Lock, 
    Unlock, 
    Plus, 
    Gem, 
    Coins, 
    ChevronLeft, 
    ChevronRight,
    Filter,
    MoreHorizontal,
    Activity,
    Star,
    Mail,
    Calendar,
    Loader2,
    X,
    CheckCircle2
} from "lucide-react";

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
    const [toast, setToast] = useState<{ msg: string; type: 'success' | 'error' } | null>(null);

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
        if (toast) {
            const timer = setTimeout(() => setToast(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [toast]);

    const handleConfirmToggleLock = async () => {
        if (!confirmModal.user) return;
        
        const { id, isLocked } = confirmModal.user;
        setConfirmModal(prev => ({ ...prev, isOpen: false }));
        setActionLoading(true);
        
        try {
            const success = await toggleUserLock(id, !isLocked);
            if (success) {
                setToast({ msg: !isLocked ? "Đã khóa tài khoản thành công." : "Đã mở khóa tài khoản.", type: 'success' });
                await fetchUsers();
            } else {
                setToast({ msg: "Lỗi khi thay đổi trạng thái tài khoản.", type: 'error' });
            }
        } catch (err) {
            setToast({ msg: "Lỗi kết nối.", type: 'error' });
        } finally {
            setActionLoading(false);
        }
    };

    const handleAddBalance = async () => {
        if (!selectedUser) return;
        if (addAmount <= 0) {
            setToast({ msg: "Số tiền phải lớn hơn 0", type: 'error' });
            return;
        }

        setActionLoading(true);
        try {
            const success = await addUserBalance(selectedUser.id, addCurrency, addAmount, addReason || `Admin manual topup ${addCurrency}`);
            if (success) {
                setIsAddBalanceOpen(false);
                setAddReason("");
                setToast({ msg: `Đã cộng ${addAmount} ${addCurrency} thành công!`, type: 'success' });
                await fetchUsers();
            } else {
                setToast({ msg: "Lỗi khi cộng tiền.", type: 'error' });
            }
        } catch (err) {
            setToast({ msg: "Lỗi hệ thống.", type: 'error' });
        } finally {
            setActionLoading(false);
        }
    };

    return (
        <div className="space-y-8 pb-20">
            {/* Toast System */}
            {toast && (
                <div className={`fixed top-10 right-10 z-[200] px-6 py-4 rounded-2xl border backdrop-blur-3xl shadow-2xl animate-in slide-in-from-right-10 duration-500 flex items-center gap-3 ${
                    toast.type === 'success' ? 'bg-emerald-500/20 border-emerald-500/40 text-emerald-400' : 'bg-rose-500/20 border-rose-500/40 text-rose-400'
                }`}>
                    {toast.type === 'success' ? <CheckCircle2 className="w-5 h-5 text-emerald-400" /> : <X className="w-5 h-5 text-rose-400" />}
                    <span className="text-xs font-black uppercase tracking-widest">{toast.msg}</span>
                </div>
            )}

            {/* Custom Confirm Modal (Lock/Unlock) */}
            {confirmModal.isOpen && (
                <div className="fixed inset-0 z-[150] flex items-center justify-center p-6 animate-in fade-in duration-300">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-md" onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))} />
                    <div className="relative z-10 w-full max-w-sm bg-[#0A0A0F] border border-white/10 rounded-[2.5rem] p-10 shadow-2xl animate-in zoom-in-95 duration-300 transform-gpu">
                        <div className={`w-16 h-16 rounded-2xl mx-auto mb-6 flex items-center justify-center border ${
                            confirmModal.user?.isLocked ? 'bg-emerald-500/10 border-emerald-500/20 text-emerald-400' : 'bg-rose-500/10 border-rose-500/20 text-rose-400'
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
                            <button 
                                onClick={() => setConfirmModal(prev => ({...prev, isOpen: false}))}
                                className="flex-1 py-4 rounded-xl bg-white/5 border border-white/5 text-[10px] font-black uppercase tracking-widest text-zinc-600 hover:bg-white/10 transition-all"
                            >
                                Hủy
                            </button>
                            <button 
                                onClick={handleConfirmToggleLock}
                                className={`flex-1 py-4 rounded-xl text-[10px] font-black uppercase tracking-widest text-black shadow-xl transition-all ${
                                    confirmModal.user?.isLocked ? 'bg-emerald-500 hover:bg-emerald-400' : 'bg-rose-500 hover:bg-rose-400'
                                }`}
                            >
                                Thực hiện
                            </button>
                        </div>
                    </div>
                </div>
            )}
            {/* Header Area */}
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 animate-in fade-in slide-in-from-bottom-4 duration-700">
                <div className="space-y-1">
                    <h1 className="text-3xl font-black text-white uppercase italic tracking-tighter flex items-center gap-3">
                        <Users className="w-8 h-8 text-purple-400" />
                        Quản Lý Người Dùng
                    </h1>
                    <p className="text-zinc-500 text-xs font-bold uppercase tracking-widest">
                        Tổng cộng {totalCount} linh hồn đang hiện diện
                    </p>
                </div>

                <div className="flex items-center gap-3">
                    <div className="relative group">
                        <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                            <Search className="w-4 h-4 text-zinc-600 group-focus-within:text-purple-400 transition-colors" />
                        </div>
                        <input
                            type="text"
                            placeholder="TÊN, EMAIL, USERNAME..."
                            value={searchTerm}
                            onChange={(e) => {
                                setSearchTerm(e.target.value);
                                setPage(1);
                            }}
                            className="bg-white/[0.03] border border-white/5 rounded-2xl pl-12 pr-6 py-3 text-xs font-black uppercase tracking-widest text-white focus:outline-none focus:border-purple-500/50 focus:bg-white/[0.05] transition-all w-full md:w-80 shadow-2xl"
                        />
                    </div>
                </div>
            </div>

            {/* Main Table Card - Astral Style */}
            <div className="relative group animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200">
                <div className="absolute inset-0 bg-purple-500/[0.02] blur-3xl rounded-[3rem] pointer-events-none" />
                <div className="relative z-10 bg-white/[0.01] backdrop-blur-3xl rounded-[2.5rem] border border-white/5 overflow-hidden shadow-2xl">
                    <div className="overflow-x-auto custom-scrollbar">
                        <table className="w-full text-left">
                            <thead>
                                <tr className="border-b border-white/5 bg-white/[0.02]">
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Thông tin Tài khoản</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Thứ hạng / XP</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Tài sản (D/G)</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">Vai trò</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-center">Trạng thái</th>
                                    <th className="px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 text-right">Hành động</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-white/5">
                                {loading ? (
                                    <tr>
                                        <td colSpan={6} className="px-8 py-20 text-center">
                                            <Loader2 className="w-8 h-8 animate-spin text-purple-500 mx-auto mb-4" />
                                            <span className="text-[10px] font-black uppercase tracking-widest text-zinc-600">Đang truy vấn dữ liệu...</span>
                                        </td>
                                    </tr>
                                ) : users.length === 0 ? (
                                    <tr>
                                        <td colSpan={6} className="px-8 py-20 text-center text-[10px] font-black uppercase tracking-widest text-zinc-700">
                                            Vũ trụ trống rỗng, không tìm thấy linh hồn phù hợp.
                                        </td>
                                    </tr>
                                ) : (
                                    users.map((u) => (
                                        <tr key={u.id} className="group/row hover:bg-white/[0.02] transition-colors">
                                            <td className="px-8 py-5">
                                                <div className="flex items-center gap-4">
                                                    <div className="w-10 h-10 rounded-xl bg-gradient-to-br from-purple-500/20 to-blue-500/20 border border-white/10 flex items-center justify-center text-white font-black text-sm relative overflow-hidden group-hover/row:scale-110 transition-transform">
                                                        {u.displayName?.charAt(0).toUpperCase() || 'U'}
                                                        <div className="absolute inset-0 bg-white/10 opacity-0 group-hover/row:opacity-100 transition-opacity" />
                                                    </div>
                                                    <div>
                                                        <div className="text-[11px] font-black text-white uppercase tracking-tighter">{u.displayName}</div>
                                                        <div className="flex items-center gap-1.5 text-[9px] font-bold text-zinc-600">
                                                            <Mail className="w-2.5 h-2.5" />
                                                            {u.email}
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="px-8 py-5">
                                                <div className="flex items-center gap-2">
                                                    <div className="px-2 py-0.5 rounded-md bg-amber-500/10 border border-amber-500/20 text-[9px] font-black text-amber-500">
                                                        LV {u.level}
                                                    </div>
                                                    <div className="text-[10px] font-bold text-zinc-500">{u.exp} EXP</div>
                                                </div>
                                            </td>
                                            <td className="px-8 py-5">
                                                <div className="space-y-1">
                                                    <div className="flex items-center gap-2 text-[11px] font-black text-white italic">
                                                        <Gem className="w-3 h-3 text-purple-400" />
                                                        {u.diamondBalance.toLocaleString()}
                                                    </div>
                                                    <div className="flex items-center gap-2 text-[10px] font-bold text-amber-500/80 italic">
                                                        <Coins className="w-3 h-3" />
                                                        {u.goldBalance.toLocaleString()}
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="px-8 py-5">
                                                <div className={`
                                                    inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-[9px] font-black uppercase tracking-widest border
                                                    ${u.role === "admin" 
                                                        ? "bg-rose-500/10 border-rose-500/20 text-rose-400" 
                                                        : u.role === "tarot_reader"
                                                            ? "bg-cyan-500/10 border-cyan-500/20 text-cyan-400"
                                                            : "bg-zinc-500/10 border-white/5 text-zinc-500"
                                                    }
                                                `}>
                                                    <Star className="w-2.5 h-2.5 fill-current" />
                                                    {u.role}
                                                </div>
                                            </td>
                                            <td className="px-8 py-5 text-center">
                                                <div className={`
                                                    inline-flex items-center gap-2 text-[9px] font-black uppercase tracking-widest
                                                    ${u.isLocked ? "text-rose-500" : "text-emerald-500"}
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
                                                        className="p-2 rounded-xl bg-purple-500/10 border border-purple-500/20 text-purple-400 hover:bg-purple-500 hover:text-white transition-all shadow-lg"
                                                        title="Cộng tiền thủ công"
                                                    >
                                                        <Plus className="w-4 h-4" />
                                                    </button>
                                                    <button
                                                        onClick={() => setConfirmModal({ isOpen: true, user: u, isLockAction: true })}
                                                        className={`p-2 rounded-xl border transition-all shadow-lg ${u.isLocked
                                                            ? "bg-emerald-500/10 border-emerald-500/20 text-emerald-400 hover:bg-emerald-500 hover:text-white"
                                                            : "bg-rose-500/10 border-rose-500/20 text-rose-500 hover:bg-rose-500 hover:text-white"
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
                    <div className="px-8 py-6 bg-white/[0.02] flex flex-col md:flex-row md:items-center justify-between gap-4 border-t border-white/5">
                        <div className="text-[10px] font-black uppercase tracking-widest text-zinc-600 text-left">
                            Vũ trụ {page} <span className="mx-2 opacity-30">|</span> Tổng {totalCount} người dùng
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

            {/* Premium Modal — Add Balance */}
            {isAddBalanceOpen && (
                <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 md:p-10 animate-in fade-in duration-300">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-md" onClick={() => setIsAddBalanceOpen(false)} />
                    
                    <div className="relative z-10 w-full max-w-lg bg-[#0A0A0F] border border-white/10 rounded-[3rem] overflow-hidden shadow-[0_0_100px_rgba(168,85,247,0.15)] animate-in zoom-in-95 duration-300">
                        {/* Modal Header */}
                        <div className="p-8 border-b border-white/5 bg-white/[0.02] flex items-center justify-between">
                            <div className="flex items-center gap-4">
                                <div className="w-12 h-12 rounded-2xl bg-purple-500/10 border border-purple-500/20 flex items-center justify-center">
                                    <Gem className="w-6 h-6 text-purple-400" />
                                </div>
                                <div className="text-left">
                                    <h2 className="text-xl font-black text-white uppercase italic tracking-tighter">Cộng tiền Thủ công</h2>
                                    <p className="text-[9px] font-black text-zinc-500 uppercase tracking-widest">Manual Balance Intervention</p>
                                </div>
                            </div>
                            <button 
                                onClick={() => setIsAddBalanceOpen(false)}
                                className="w-10 h-10 rounded-full bg-white/5 flex items-center justify-center text-zinc-500 hover:bg-rose-500 hover:text-white transition-all shadow-xl"
                            >
                                <X className="w-5 h-5" />
                            </button>
                        </div>

                        {/* Modal Body */}
                        <div className="p-10 space-y-8">
                            <div className="space-y-3">
                                <label className="text-[10px] font-black uppercase tracking-widest text-zinc-600 block text-left">Người nhận vận may</label>
                                <div className="flex items-center gap-4 p-5 rounded-2xl bg-white/[0.02] border border-white/5 group transition-colors hover:border-white/10">
                                    <div className="w-10 h-10 rounded-xl bg-purple-500/20 flex items-center justify-center font-black text-xs">
                                        {selectedUser?.displayName?.charAt(0) || 'U'}
                                    </div>
                                    <div className="text-left">
                                        <div className="text-xs font-black text-white uppercase tracking-tighter">{selectedUser?.displayName}</div>
                                        <div className="text-[10px] font-bold text-zinc-600 italic">{selectedUser?.email}</div>
                                    </div>
                                </div>
                            </div>

                            <div className="grid grid-cols-2 gap-4">
                                <button 
                                    onClick={() => setAddCurrency("gold")}
                                    className={`relative p-6 rounded-[2rem] border transition-all duration-500 overflow-hidden text-left ${addCurrency === "gold" ? "bg-amber-500/10 border-amber-500/40" : "bg-white/[0.02] border-white/5 hover:border-white/10"}`}
                                >
                                    <div className="relative z-10 flex flex-col gap-3">
                                        <div className={`p-2.5 rounded-xl border w-fit ${addCurrency === "gold" ? "bg-amber-500/20 border-amber-500/30" : "bg-white/5 border-white/10"}`}>
                                            <Coins className={`w-5 h-5 ${addCurrency === "gold" ? "text-amber-400" : "text-zinc-600"}`} />
                                        </div>
                                        <span className={`text-[11px] font-black uppercase tracking-[0.2em] ${addCurrency === "gold" ? "text-white" : "text-zinc-600"}`}>Gold</span>
                                    </div>
                                    {addCurrency === "gold" && <div className="absolute -bottom-4 -right-4 w-16 h-16 bg-amber-500/10 blur-xl rounded-full" />}
                                </button>

                                <button 
                                    onClick={() => setAddCurrency("diamond")}
                                    className={`relative p-6 rounded-[2rem] border transition-all duration-500 overflow-hidden text-left ${addCurrency === "diamond" ? "bg-purple-500/10 border-purple-500/40" : "bg-white/[0.02] border-white/5 hover:border-white/10"}`}
                                >
                                    <div className="relative z-10 flex flex-col gap-3">
                                        <div className={`p-2.5 rounded-xl border w-fit ${addCurrency === "diamond" ? "bg-purple-500/20 border-purple-500/30" : "bg-white/5 border-white/10"}`}>
                                            <Gem className={`w-5 h-5 ${addCurrency === "diamond" ? "text-purple-400" : "text-zinc-600"}`} />
                                        </div>
                                        <span className={`text-[11px] font-black uppercase tracking-[0.2em] ${addCurrency === "diamond" ? "text-white" : "text-zinc-600"}`}>Diamond</span>
                                    </div>
                                    {addCurrency === "diamond" && <div className="absolute -bottom-4 -right-4 w-16 h-16 bg-purple-500/10 blur-xl rounded-full" />}
                                </button>
                            </div>

                            <div className="space-y-4">
                                <div className="space-y-3">
                                    <label className="text-[10px] font-black uppercase tracking-widest text-zinc-600 block text-left">Số lượng tài sản</label>
                                    <div className="relative group">
                                        <input
                                            type="number"
                                            value={addAmount}
                                            onChange={(e) => setAddAmount(Number(e.target.value))}
                                            className="w-full bg-white/[0.02] border border-white/10 rounded-2xl px-6 py-4 text-2xl font-black text-white italic tracking-tighter focus:outline-none focus:border-purple-500/50 focus:bg-white/[0.05] transition-all"
                                        />
                                        <div className="absolute right-6 top-1/2 -translate-y-1/2 opacity-30">
                                            {addCurrency === "gold" ? <Coins className="w-5 h-5" /> : <Gem className="w-5 h-5" />}
                                        </div>
                                    </div>
                                </div>

                                <div className="space-y-3">
                                    <label className="text-[10px] font-black uppercase tracking-widest text-zinc-600 block text-left">Lý do can thiệp</label>
                                    <textarea
                                        value={addReason}
                                        onChange={(e) => setAddReason(e.target.value)}
                                        placeholder="Ví dụ: Đền bù lỗi hệ thống, Quà tặng sự kiện..."
                                        className="w-full bg-white/[0.02] border border-white/10 rounded-2xl px-6 py-4 text-xs font-bold text-white focus:outline-none focus:border-purple-500/50 focus:bg-white/[0.05] transition-all h-24 resize-none placeholder:text-zinc-800"
                                    />
                                </div>
                            </div>

                            <div className="flex gap-4">
                                <button
                                    onClick={() => setIsAddBalanceOpen(false)}
                                    className="flex-1 py-5 rounded-2xl bg-white/5 border border-white/10 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-500 hover:bg-white/10 hover:text-white transition-all shadow-xl"
                                >
                                    Hủy tác vụ
                                </button>
                                <button
                                    onClick={handleAddBalance}
                                    disabled={actionLoading}
                                    className="flex-1 py-5 rounded-2xl bg-purple-500 text-black text-[10px] font-black uppercase tracking-[0.2em] hover:bg-white transition-all shadow-2xl disabled:opacity-50 disabled:cursor-not-allowed group"
                                >
                                    {actionLoading ? (
                                        <Loader2 className="w-5 h-5 animate-spin mx-auto" />
                                    ) : (
                                        <span className="flex items-center justify-center gap-2">
                                            Kích hoạt <Plus className="w-4 h-4" />
                                        </span>
                                    )}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
