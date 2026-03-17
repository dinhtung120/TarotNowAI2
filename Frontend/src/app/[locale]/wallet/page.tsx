'use client';

/**
 * Trang Ví (Wallet Page) - Astral Premium Redesign
 * 
 * Các cải tiến:
 * 1. Hệ thống Nền Astral: Nebula + Spiritual Particles.
 * 2. 3D Balance Cards: Thẻ Diamond và Gold với hiệu ứng Glow và Glassmorphism.
 * 3. Compact Ledger: Bảng lịch sử giao dịch tinh gọn, dễ theo dõi.
 * 4. Navbar Fix: Sử dụng pt-28 để tránh bị che bởi Navbar cố định.
 */

import React, { useEffect, useState } from 'react';
import { getLedger } from '@/actions/walletActions';
import { PaginatedList, WalletTransaction } from '@/types/wallet';
import { useWalletStore } from '@/store/walletStore';
import { 
    Coins, 
    Gem, 
    ArrowUpRight, 
    ArrowDownLeft, 
    Clock, 
    Search, 
    ChevronLeft, 
    ChevronRight, 
    Activity,
    CreditCard,
    Plus,
    Sparkles,
    Stars,
    Loader2
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';

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
        <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-x-hidden font-sans pb-20">
            {/* ##### PREMIUM BACKGROUND SYSTEM ##### */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                {/* Nebula Glows */}
                <div className="absolute top-0 -left-1/4 w-[70vw] h-[70vw] bg-purple-900/[0.1] blur-[120px] rounded-full animate-slow-pulse" />
                <div className="absolute bottom-0 -right-1/4 w-[60vw] h-[60vw] bg-amber-900/[0.05] blur-[130px] rounded-full animate-slow-pulse delay-700" />
                
                {/* Spiritual Particles */}
                <div className="absolute inset-0">
                    {Array.from({ length: 20 }).map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-[1px] h-[1px] bg-white rounded-full animate-float opacity-[0.1]"
                            style={{
                                top: `${Math.random() * 100}%`,
                                left: `${Math.random() * 100}%`,
                                animationDuration: `${25 + Math.random() * 35}s`,
                                animationDelay: `${-Math.random() * 20}s`,
                            }}
                        />
                    ))}
                </div>
            </div>

            <main className="relative z-10 max-w-5xl mx-auto px-6 pt-28">
                {/* Header Section */}
                <div className="flex flex-col md:flex-row md:items-end justify-between mb-12 gap-6 animate-in fade-in slide-in-from-bottom-4 duration-1000">
                    <div className="space-y-4">
                        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md">
                            <Sparkles className="w-3 h-3 text-amber-500" />
                            Financial Protocol
                        </div>
                        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
                            Kho Báu Tâm Linh
                        </h1>
                        <p className="text-zinc-500 font-medium max-w-lg text-sm leading-relaxed">
                            Quản lý vận mệnh tài chính và dòng chảy Diamond/Gold của bạn trong hệ sinh thái TarotNow AI.
                        </p>
                    </div>
                    
                    <button 
                        onClick={() => router.push('/wallet/deposit')}
                        className="group flex items-center gap-3 bg-white text-black px-6 py-3 rounded-2xl font-black text-[10px] uppercase tracking-widest transition-all hover:scale-[1.03] active:scale-95 shadow-2xl"
                    >
                        <Plus className="w-4 h-4" />
                        Nạp Diamond
                    </button>
                </div>

                {/* Balance Cards - 3D Glassmorphism */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-16 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-300">
                    {/* Diamond Card */}
                    <div className="relative group perspective-1000">
                        <div className="absolute inset-0 bg-gradient-to-br from-purple-500/20 to-indigo-500/10 blur-2xl opacity-0 group-hover:opacity-100 transition-opacity duration-700" />
                        <div className="relative z-10 h-64 bg-zinc-900/40 backdrop-blur-3xl rounded-[3rem] border border-white/10 p-10 flex flex-col justify-between overflow-hidden group-hover:border-purple-500/40 transition-all duration-700 shadow-2xl">
                            <div className="absolute top-0 right-0 p-8 opacity-10 transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12">
                                <Gem className="w-32 h-32 text-purple-400" />
                            </div>
                            
                            <div className="flex items-center gap-4">
                                <div className="w-12 h-12 rounded-2xl bg-purple-500/10 flex items-center justify-center border border-purple-500/20 shadow-xl">
                                    <Gem className="w-6 h-6 text-purple-400" />
                                </div>
                                <div>
                                    <div className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Diamond (Premium)</div>
                                    <div className="text-xs font-bold text-zinc-300">Tiền nạp chính thức</div>
                                </div>
                            </div>

                            <div className="space-y-1">
                                <div className="text-5xl md:text-6xl font-black text-white italic tracking-tighter transition-transform duration-700 group-hover:scale-105 group-hover:translate-x-2 original-origin-left">
                                    {balance?.diamondBalance.toLocaleString() ?? '...'}
                                </div>
                                <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-zinc-600">
                                    <Activity className="w-3 h-3" />
                                    Đang đóng băng: {balance?.frozenDiamondBalance.toLocaleString() ?? 0}
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Gold Card */}
                    <div className="relative group perspective-1000">
                        <div className="absolute inset-0 bg-gradient-to-br from-amber-500/10 to-transparent blur-2xl opacity-0 group-hover:opacity-100 transition-opacity duration-700" />
                        <div className="relative z-10 h-64 bg-zinc-900/40 backdrop-blur-3xl rounded-[3rem] border border-white/10 p-10 flex flex-col justify-between overflow-hidden group-hover:border-amber-500/40 transition-all duration-700 shadow-2xl">
                            <div className="absolute top-0 right-0 p-8 opacity-10 transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12">
                                <Coins className="w-32 h-32 text-amber-500" />
                            </div>

                            <div className="flex items-center gap-4">
                                <div className="w-12 h-12 rounded-2xl bg-amber-500/10 flex items-center justify-center border border-amber-500/20 shadow-xl">
                                    <Coins className="w-6 h-6 text-amber-500" />
                                </div>
                                <div>
                                    <div className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Gold (Reward)</div>
                                    <div className="text-xs font-bold text-zinc-300">Điểm thưởng vận may</div>
                                </div>
                            </div>

                            <div className="space-y-1">
                                <div className="text-5xl md:text-6xl font-black text-white italic tracking-tighter transition-transform duration-700 group-hover:scale-105 group-hover:translate-x-2 original-origin-left">
                                    {balance?.goldBalance.toLocaleString() ?? '...'}
                                </div>
                                <div className="text-[10px] font-black uppercase tracking-widest text-zinc-600">
                                    Nhận từ điểm danh & hoạt động
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Ledger Section - Compact Glass */}
                <div className="animate-in fade-in slide-in-from-bottom-12 duration-1000 delay-500">
                    <div className="mb-8 flex items-center justify-between">
                        <h2 className="text-2xl font-black text-white uppercase italic tracking-tighter flex items-center gap-3">
                            <Clock className="w-5 h-5 text-purple-400" />
                            Hành Trình Giao Dịch
                        </h2>
                        <div className="hidden md:flex items-center gap-2 px-4 py-2 rounded-2xl bg-white/[0.03] border border-white/10 focus-within:border-purple-500/50 transition-all duration-300">
                            <Search className="w-3.5 h-3.5 text-zinc-600" />
                            <input 
                                type="text" 
                                placeholder="Tìm giao thức..." 
                                className="bg-transparent border-none text-[10px] font-black uppercase tracking-widest text-white focus:outline-none w-48 placeholder:text-zinc-700" 
                            />
                        </div>
                    </div>

                    <div className="relative">
                        <div className="absolute inset-0 bg-white/[0.01] backdrop-blur-3xl rounded-[2.5rem] border border-white/5 shadow-2xl" />
                        <div className="relative z-10 overflow-hidden">
                            <table className="w-full text-left">
                                <thead>
                                    <tr className="border-b border-white/5">
                                        <th className="px-8 py-5 text-[9px] font-black uppercase tracking-[0.2em] text-zinc-600">Thời gian</th>
                                        <th className="px-8 py-5 text-[9px] font-black uppercase tracking-[0.2em] text-zinc-600">Tài sản</th>
                                        <th className="px-8 py-5 text-[9px] font-black uppercase tracking-[0.2em] text-zinc-600">Hoạt động</th>
                                        <th className="px-8 py-5 text-[9px] font-black uppercase tracking-[0.2em] text-zinc-600 text-right">Lượng</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-white/5">
                                    {isLoadingLedger ? (
                                        <tr>
                                            <td colSpan={4} className="py-20 text-center">
                                                <Loader2 className="w-6 h-6 animate-spin text-purple-500 mx-auto mb-4" />
                                                <span className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Đang truy vấn lịch sử...</span>
                                            </td>
                                        </tr>
                                    ) : ledger?.items.length === 0 ? (
                                        <tr>
                                            <td colSpan={4} className="py-20 text-center text-[10px] font-black uppercase tracking-widest text-zinc-600">
                                                Dòng chảy tài chính chưa có biến động.
                                            </td>
                                        </tr>
                                    ) : (
                                        ledger?.items.map((tx) => {
                                            const isPositive = tx.amount > 0;
                                            return (
                                                <tr key={tx.id} className="hover:bg-white/[0.02] transition-colors group">
                                                    <td className="px-8 py-5">
                                                        <div className="text-[10px] font-bold text-zinc-400">
                                                            {new Date(tx.createdAt).toLocaleDateString()}
                                                        </div>
                                                        <div className="text-[9px] font-medium text-zinc-600">
                                                            {new Date(tx.createdAt).toLocaleTimeString()}
                                                        </div>
                                                    </td>
                                                    <td className="px-8 py-5">
                                                        <div className="flex items-center gap-2">
                                                            {tx.currency.toLowerCase() === 'diamond' ? (
                                                                <Gem className="w-3.5 h-3.5 text-purple-400" />
                                                            ) : (
                                                                <Coins className="w-3.5 h-3.5 text-amber-500" />
                                                            )}
                                                            <span className="text-[10px] font-black uppercase tracking-widest text-zinc-200">{tx.currency}</span>
                                                        </div>
                                                    </td>
                                                    <td className="px-8 py-5">
                                                        <div className="text-[10px] font-black text-zinc-200 uppercase tracking-tighter">{formatType(tx.type)}</div>
                                                        {tx.description && <div className="text-[9px] text-zinc-600 font-medium truncate max-w-[150px]">{tx.description}</div>}
                                                    </td>
                                                    <td className="px-8 py-5 text-right">
                                                        <div className={`flex items-center justify-end gap-1 font-black text-sm italic ${isPositive ? 'text-emerald-400' : 'text-rose-400'}`}>
                                                            {isPositive ? '+' : ''}{tx.amount.toLocaleString()}
                                                            {isPositive ? <ArrowUpRight className="w-3.5 h-3.5" /> : <ArrowDownLeft className="w-3.5 h-3.5" />}
                                                        </div>
                                                        <div className="text-[8px] font-black uppercase tracking-widest text-zinc-700 opacity-0 group-hover:opacity-100 transition-opacity">
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
                            <div className="relative z-10 px-8 py-6 border-t border-white/5 flex items-center justify-between">
                                <span className="text-[9px] font-black uppercase tracking-widest text-zinc-600">
                                    Vũ trụ <strong className="text-zinc-300">{ledger.pageIndex}</strong> / {ledger.totalPages}
                                </span>
                                <div className="flex items-center gap-3">
                                    <button
                                        onClick={() => setPage(p => Math.max(1, p - 1))}
                                        disabled={!ledger.hasPreviousPage}
                                        className="p-2 rounded-xl bg-white/5 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all border border-white/5"
                                    >
                                        <ChevronLeft className="w-4 h-4 text-zinc-300" />
                                    </button>
                                    <button
                                        onClick={() => setPage(p => Math.min(ledger.totalPages, p + 1))}
                                        disabled={!ledger.hasNextPage}
                                        className="p-2 rounded-xl bg-white/5 hover:bg-white/10 disabled:opacity-30 disabled:cursor-not-allowed transition-all border border-white/5"
                                    >
                                        <ChevronRight className="w-4 h-4 text-zinc-300" />
                                    </button>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </main>

            <style dangerouslySetInnerHTML={{
                __html: `
                @keyframes float {
                    0% { transform: translateY(0) rotate(0deg); opacity: 0; }
                    15% { opacity: 0.15; }
                    85% { opacity: 0.15; }
                    100% { transform: translateY(-100vh) rotate(360deg); opacity: 0; }
                }
                .animate-float {
                    animation-name: float;
                    animation-timing-function: linear;
                    animation-iteration-count: infinite;
                }
                @keyframes slow-pulse {
                    0%, 100% { opacity: 0.05; transform: scale(1); }
                    50% { opacity: 0.12; transform: scale(1.1); }
                }
                .animate-slow-pulse {
                    animation: slow-pulse 15s ease-in-out infinite;
                }
                .perspective-1000 {
                    perspective: 1000px;
                }
            `}} />
        </div>
    );
}
