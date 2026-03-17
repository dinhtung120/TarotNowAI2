'use client';

/**
 * Trang Ví (Wallet Page)
 * Đã Refactor với UserLayout, SectionHeader và GlassCard.
 */

import React, { useEffect, useState } from 'react';
import { getLedger } from '@/actions/walletActions';
import { PaginatedList, WalletTransaction } from '@/types/wallet';
import { useWalletStore } from '@/store/walletStore';
import { Coins, Gem, ArrowUpRight, ArrowDownLeft, Clock, Search, ChevronLeft, ChevronRight, Activity,
 Plus,
 Sparkles,
 Loader2,
 Wallet
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';

import UserLayout from '@/components/layout/UserLayout';
import { SectionHeader, Button, GlassCard } from '@/components/ui';

export default function WalletPage() {
 const router = useRouter();
 const { balance, fetchBalance } = useWalletStore();
 const [ledger, setLedger] = useState<PaginatedList<WalletTransaction> | null>(null);
 const [isLoadingLedger, setIsLoadingLedger] = useState(true);
 const [page, setPage] = useState(1);

 useEffect(() => {
 fetchBalance();
 const fetchLedger = async () => {
 setIsLoadingLedger(true);
 const data = await getLedger(page, 10);
 setLedger(data);
 setIsLoadingLedger(false);
 };
 fetchLedger();
 }, [page, fetchBalance]);

 const formatType = (typeStr: string) => {
 return typeStr.replace(/([A-Z])/g, ' $1').trim();
 };

 return (
 <UserLayout>
 <div className="max-w-5xl mx-auto px-6 pt-8 pb-32 font-sans relative">
 {/* Header Section */}
 <SectionHeader
 tag="Financial Protocol"
 tagIcon={<Sparkles className="w-3 h-3" />}
 title="Kho Báu Tâm Linh"
 subtitle="Quản lý vận mệnh tài chính và dòng chảy Diamond/Gold của bạn trong hệ sinh thái TarotNow AI."
 action={
 <Button variant="primary" onClick={() => router.push('/wallet/deposit')}
 className="shadow-2xl md:ml-4"
 >
 <Plus className="w-4 h-4 mr-2" />
 Nạp Diamond
 </Button>
 }
 className="mb-12 animate-in fade-in slide-in-from-bottom-4 duration-1000"
 />

 {/* Balance Cards - Glassmorphism */}
 <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-16 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-300">
 {/* Diamond Card */}
 <GlassCard className="relative group overflow-hidden !p-10 flex flex-col justify-between min-h-[16rem]">
 <div className="absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)]/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-700 pointer-events-none" />
 <div className="absolute top-0 right-0 p-8 opacity-5 transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12 pointer-events-none">
 <Gem className="w-40 h-40 text-[var(--purple-accent)]" />
 </div>
 <div className="flex items-center gap-4 relative z-10">
 <div className="w-14 h-14 rounded-2xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 shadow-xl">
 <Gem className="w-7 h-7 text-[var(--purple-accent)]" />
 </div>
 <div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Diamond (Premium)</div>
 <div className="text-xs font-bold tn-text-primary">Tiền nạp chính thức</div>
 </div>
 </div>

 <div className="space-y-1 relative z-10 mt-8">
 <div className="text-5xl md:text-6xl font-black tn-text-primary italic tracking-tighter transition-transform duration-700 group-hover:translate-x-2">
 {balance?.diamondBalance.toLocaleString() ?? '...'}
 </div>
 <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] group-hover:text-[var(--text-secondary)] transition-colors">
 <Activity className="w-3.5 h-3.5" />
 Đang đóng băng: {balance?.frozenDiamondBalance.toLocaleString() ?? 0}
 </div>
 </div>
 </GlassCard>

 {/* Gold Card */}
 <GlassCard className="relative group overflow-hidden !p-10 flex flex-col justify-between min-h-[16rem]">
 <div className="absolute inset-0 bg-gradient-to-br from-[var(--warning)]/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-700 pointer-events-none" />
 <div className="absolute top-0 right-0 p-8 opacity-5 transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12 pointer-events-none">
 <Coins className="w-40 h-40 text-[var(--warning)]" />
 </div>

 <div className="flex items-center gap-4 relative z-10">
 <div className="w-14 h-14 rounded-2xl bg-[var(--warning)]/10 flex items-center justify-center border border-[var(--warning)]/20 shadow-xl">
 <Coins className="w-7 h-7 text-[var(--warning)]" />
 </div>
 <div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Gold (Reward)</div>
 <div className="text-xs font-bold tn-text-primary">Điểm thưởng vận may</div>
 </div>
 </div>

 <div className="space-y-1 relative z-10 mt-8">
 <div className="text-5xl md:text-6xl font-black tn-text-primary italic tracking-tighter transition-transform duration-700 group-hover:translate-x-2">
 {balance?.goldBalance.toLocaleString() ?? '...'}
 </div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] group-hover:text-[var(--text-secondary)] transition-colors">
 Nhận từ điểm danh & hoạt động
 </div>
 </div>
 </GlassCard>
 </div>

 {/* Ledger Section */}
 <div className="animate-in fade-in slide-in-from-bottom-12 duration-1000 delay-500">
 <div className="mb-6 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
 <h2 className="text-xl md:text-2xl font-black tn-text-primary uppercase italic tracking-tighter flex items-center gap-3">
 <Clock className="w-5 h-5 text-[var(--purple-accent)]" />
 Hành Trình Giao Dịch
 </h2>
 <div className="flex items-center gap-2 px-4 py-2 border tn-border focus-within:border-[var(--purple-accent)]/50 transition-all duration-300 rounded-xl tn-overlay ">
 <Search className="w-4 h-4 tn-text-muted" />
 <input type="text" placeholder="TÌM GIAO THỨC..." className="bg-transparent border-none text-[10px] font-black uppercase tracking-widest tn-text-primary w-full sm:w-48 placeholder:tn-text-muted" />
 </div>
 </div>

 <GlassCard className="!p-0 overflow-hidden">
 <div className="overflow-x-auto custom-scrollbar">
 <table className="w-full text-left min-w-[600px]">
 <thead>
 <tr className="border-b tn-border-soft tn-overlay-soft">
 <th className="px-6 py-5 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Thời gian</th>
 <th className="px-6 py-5 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Tài sản</th>
 <th className="px-6 py-5 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)]">Hoạt động</th>
 <th className="px-6 py-5 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-right">Lượng</th>
 </tr>
 </thead>
 <tbody className="divide-y divide-white/5">
 {isLoadingLedger ? (
 <tr>
 <td colSpan={4} className="py-24 text-center">
 <Loader2 className="w-8 h-8 animate-spin text-[var(--purple-accent)] mx-auto mb-4" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Đang truy vấn lịch sử...</span>
 </td>
 </tr>
 ) : ledger?.items.length === 0 ? (
 <tr>
 <td colSpan={4} className="py-24 text-center">
 <Wallet className="w-12 h-12 tn-text-muted mx-auto mb-4" />
 <span className="text-xs font-bold text-[var(--text-secondary)]">Dòng chảy tài chính chưa có biến động.</span>
 </td>
 </tr>
 ) : (
 ledger?.items.map((tx) => {
 const isPositive = tx.amount > 0;
 return (
 <tr key={tx.id} className="hover:tn-surface transition-colors group">
 <td className="px-6 py-5">
 <div className="text-[11px] font-bold text-[var(--text-primary)]">
 {new Date(tx.createdAt).toLocaleDateString()}
 </div>
 <div className="text-[10px] font-medium text-[var(--text-secondary)] mt-0.5">
 {new Date(tx.createdAt).toLocaleTimeString()}
 </div>
 </td>
 <td className="px-6 py-5">
 <div className="flex items-center gap-2">
 {tx.currency.toLowerCase() === 'diamond' ? (
 <Gem className="w-4 h-4 text-[var(--purple-accent)]" />
 ) : (
 <Coins className="w-4 h-4 text-[var(--warning)]" />
 )}
 <span className="text-[10px] font-black uppercase tracking-widest tn-text-primary">{tx.currency}</span>
 </div>
 </td>
 <td className="px-6 py-5 max-w-[200px]">
 <div className="text-[11px] font-black tn-text-primary uppercase tracking-tighter truncate">{formatType(tx.type)}</div>
 {tx.description && <div className="text-[10px] text-[var(--text-tertiary)] font-medium truncate mt-0.5" title={tx.description}>{tx.description}</div>}
 </td>
 <td className="px-6 py-5 text-right">
 <div className={`flex items-center justify-end gap-1 font-black text-sm md:text-base italic ${isPositive ? 'text-[var(--success)]' : 'tn-text-secondary'}`}>
 {isPositive ? '+' : ''}{tx.amount.toLocaleString()}
 {isPositive ? <ArrowUpRight className="w-4 h-4" /> : <ArrowDownLeft className="w-4 h-4 opacity-50 tn-text-muted" />}
 </div>
 <div className="text-[9px] font-black uppercase tracking-widest text-[var(--text-secondary)] mt-1 opacity-0 group-hover:opacity-100 transition-opacity">
 Dư: {tx.balanceAfter.toLocaleString()}
 </div>
 </td>
 </tr>
 );
 })
 )}
 </tbody>
 </table>
 </div>

 {/* Pagination - Compact */}
 {ledger && ledger.totalPages > 1 && (
 <div className="px-6 py-5 border-t tn-border-soft flex items-center justify-between tn-overlay-soft">
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">
 Trang <strong className="tn-text-primary mx-1">{ledger.pageIndex}</strong> / {ledger.totalPages}
 </span>
 <div className="flex items-center gap-2">
 <button
 onClick={() => setPage(p => Math.max(1, p - 1))}
 disabled={!ledger.hasPreviousPage}
 className="p-2 rounded-xl tn-surface hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all border tn-border-soft"
 >
 <ChevronLeft className="w-4 h-4 tn-text-primary" />
 </button>
 <button
 onClick={() => setPage(p => Math.min(ledger.totalPages, p + 1))}
 disabled={!ledger.hasNextPage}
 className="p-2 rounded-xl tn-surface hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all border tn-border-soft"
 >
 <ChevronRight className="w-4 h-4 tn-text-primary" />
 </button>
 </div>
 </div>
 )}
 </GlassCard>
 </div>
 </div>

 </UserLayout>
 );
}
