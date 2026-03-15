'use client';

import React, { useEffect, useState } from 'react';
import { getLedger } from '@/actions/walletActions';
import { PaginatedList, WalletTransaction } from '@/types/wallet';
import { useWalletStore } from '@/store/walletStore';
import { Coins, Gem, ArrowUpRight, ArrowDownLeft, Clock, Search, ChevronLeft, ChevronRight, Activity } from 'lucide-react';
import Link from 'next/link';

/**
 * Trang Wallet (Ví) & Lịch Sử Giao Dịch
 * Tại sao cần thiết kế đặc biệt?
 * - Trải nghiệm Premium App: Giao diện nền tối mượt mà (glassmorphism), highlight số dư như các app Fintech hiện đại.
 * - Component bao gồm 3 phần chính: Thẻ Tổng số dư (Balance Cards), Điều hướng Tabs, và Bảng Lịch sử Ledger.
 * - Dùng Server Actions gọi API lấy dữ liệu phân trang. Trạng thái `page` chuyển đổi qua lại được xử lý bằng state nội bộ ở Client Component.
 */
export default function WalletPage() {
    const { balance, fetchBalance } = useWalletStore();
    const [ledger, setLedger] = useState<PaginatedList<WalletTransaction> | null>(null);
    const [isLoadingLedger, setIsLoadingLedger] = useState(true);
    const [page, setPage] = useState(1);

    // Kéo dữ liệu qua API mỗi khi page thay đổi hoặc lần đầu render.
    useEffect(() => {
        fetchBalance(); // Kéo số dư mới nhất

        const fetchLedger = async () => {
            setIsLoadingLedger(true);
            const data = await getLedger(page, 10); // Lấy 10 giao dịch / trang
            setLedger(data);
            setIsLoadingLedger(false);
        };
        fetchLedger();
    }, [page, fetchBalance]);

    // Hàm chuyển đổi Type (Enums từ Backend) sang tiếng Anh hiển thị
    const formatType = (typeStr: string) => {
        return typeStr.replace(/([A-Z])/g, ' $1').trim(); // Ví dụ: DepositBonus -> Deposit Bonus
    };

    return (
        <div className="min-h-screen bg-slate-950 text-slate-100 p-6 md:p-12 font-sans selection:bg-cyan-500/30">
            <div className="max-w-6xl mx-auto space-y-8">

                {/* Header Tiêu đề */}
                <div className="space-y-2">
                    <h1 className="text-4xl md:text-5xl font-extrabold tracking-tight bg-gradient-to-r from-cyan-400 via-indigo-400 to-purple-400 text-transparent bg-clip-text">
                        Tài Sản & Ví
                    </h1>
                    <p className="text-slate-400 text-lg">Quản lý số dư, nạp thẻ và lịch sử giao dịch trong Hệ Sinh Thái TarotNow.</p>
                </div>

                {/* Các Thẻ Số Dư (Balance Cards) */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {/* Thẻ Diamond (Tiền nạp) */}
                    <div className="relative overflow-hidden rounded-3xl bg-gradient-to-br from-indigo-900/60 to-purple-900/40 border border-indigo-500/20 p-8 shadow-2xl backdrop-blur-md group">
                        <div className="absolute top-0 right-0 -mr-8 -mt-8 w-40 h-40 bg-indigo-500/20 rounded-full blur-3xl group-hover:bg-indigo-400/30 transition-all duration-700"></div>
                        <div className="relative z-10 flex items-start justify-between">
                            <div className="space-y-4">
                                <div className="flex items-center gap-3">
                                    <div className="p-3 bg-indigo-500/20 rounded-2xl ring-1 ring-indigo-500/30">
                                        <Gem className="w-8 h-8 text-cyan-300" />
                                    </div>
                                    <h2 className="text-xl font-semibold text-slate-200">Diamond</h2>
                                </div>
                                <div>
                                    <p className="text-5xl font-black text-white tracking-tight">
                                        {balance?.diamondBalance.toLocaleString() ?? '...'}
                                    </p>
                                    <div className="flex items-center gap-2 mt-2 text-indigo-300 text-sm font-medium">
                                        <Activity className="w-4 h-4" />
                                        <span>Đang đóng băng (Escrow): {balance?.frozenDiamondBalance.toLocaleString() ?? 0}</span>
                                    </div>
                                </div>
                            </div>
                            <Link 
                                href="/wallet/deposit"
                                className="bg-indigo-500 hover:bg-indigo-400 text-white font-bold py-2.5 px-6 rounded-xl shadow-lg transition-colors ring-1 ring-white/20 text-sm"
                            >
                                Nạp Diamond
                            </Link>
                        </div>
                    </div>

                    {/* Thẻ Gold (Tiền miễn phí) */}
                    <div className="relative overflow-hidden rounded-3xl bg-gradient-to-br from-slate-800/80 to-slate-900/80 border border-slate-700/50 p-8 shadow-2xl backdrop-blur-md group">
                        <div className="absolute bottom-0 right-0 -mr-8 -mb-8 w-40 h-40 bg-yellow-500/10 rounded-full blur-3xl group-hover:bg-yellow-500/20 transition-all duration-700"></div>
                        <div className="relative z-10 flex items-start justify-between h-full flex-col">
                            <div className="space-y-4 w-full">
                                <div className="flex items-center gap-3">
                                    <div className="p-3 bg-yellow-500/10 rounded-2xl ring-1 ring-yellow-500/20">
                                        <Coins className="w-8 h-8 text-yellow-400" />
                                    </div>
                                    <h2 className="text-xl font-semibold text-slate-200">Gold</h2>
                                </div>
                                <div>
                                    <p className="text-5xl font-black text-white tracking-tight">
                                        {balance?.goldBalance.toLocaleString() ?? '...'}
                                    </p>
                                    <p className="mt-2 text-slate-400 text-sm font-medium">Nhận qua điểm danh & hoạt động.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Bảng Ledger (Lịch Sử Giao Dịch) */}
                <div className="bg-slate-900/50 border border-slate-800 rounded-3xl overflow-hidden backdrop-blur-sm shadow-xl">
                    <div className="p-6 border-b border-slate-800/80 flex items-center justify-between">
                        <h3 className="text-2xl font-bold flex items-center gap-3">
                            <Clock className="w-6 h-6 text-cyan-400" /> Lịch sử Biến động Số dư
                        </h3>
                        {/* Fake Filter input để trông giống App xịn */}
                        <div className="relative hidden md:block">
                            <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-slate-500" />
                            <input type="text" placeholder="Tìm kiếm giao dịch..." className="bg-slate-950 border border-slate-800 rounded-lg pl-9 pr-4 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-cyan-500/50 text-slate-200 w-64 transition-all" />
                        </div>
                    </div>

                    <div className="overflow-x-auto">
                        <table className="w-full text-left border-collapse">
                            <thead>
                                <tr className="bg-slate-950/50 text-slate-400 text-sm font-semibold tracking-wide border-b border-slate-800/80">
                                    <th className="p-4 pl-6 font-medium">THỜI GIAN</th>
                                    <th className="p-4 font-medium">LOẠI TIỀN</th>
                                    <th className="p-4 font-medium">GIAO DỊCH</th>
                                    <th className="p-4 font-medium text-right pr-6">SỐ LƯỢNG</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-slate-800/50">
                                {isLoadingLedger ? (
                                    <tr>
                                        <td colSpan={4} className="p-12 text-center text-slate-500">
                                            <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-cyan-500 mx-auto mb-4"></div>
                                            Đang tải dữ liệu lịch sử...
                                        </td>
                                    </tr>
                                ) : ledger?.items.length === 0 ? (
                                    <tr>
                                        <td colSpan={4} className="p-12 text-center text-slate-400">Không có giao dịch nào.</td>
                                    </tr>
                                ) : (
                                    ledger?.items.map((tx) => {
                                        // Phân tích logic tăng giảm: số amount truyền từ DB dương là Credit (cộng tiền), âm là Debit (trừ tiền)
                                        const isPositive = tx.amount > 0;
                                        return (
                                            <tr key={tx.id} className="hover:bg-slate-800/30 transition-colors group">
                                                <td className="p-4 pl-6 text-sm text-slate-400">
                                                    {new Date(tx.createdAt).toLocaleString()}
                                                </td>
                                                <td className="p-4">
                                                    <div className="flex items-center gap-2">
                                                        {tx.currency === 'diamond' || tx.currency === 'Diamond' ? (
                                                            <Gem className="w-4 h-4 text-cyan-400" />
                                                        ) : (
                                                            <Coins className="w-4 h-4 text-yellow-400" />
                                                        )}
                                                        <span className="capitalize font-medium text-slate-300">{tx.currency}</span>
                                                    </div>
                                                </td>
                                                <td className="p-4">
                                                    <div className="font-semibold text-slate-200 capitalize">{formatType(tx.type)}</div>
                                                    {tx.description && <div className="text-xs text-slate-500 mt-1 max-w-xs truncate" title={tx.description}>{tx.description}</div>}
                                                </td>
                                                <td className="p-4 pr-6 text-right">
                                                    <div className={`inline-flex items-center gap-1 font-bold text-lg ${isPositive ? 'text-emerald-400' : 'text-rose-400'}`}>
                                                        {isPositive ? <ArrowUpRight className="w-5 h-5" /> : <ArrowDownLeft className="w-5 h-5" />}
                                                        {isPositive ? '+' : ''}{tx.amount.toLocaleString()}
                                                    </div>
                                                    <div className="text-xs text-slate-500 mt-1 opacity-0 group-hover:opacity-100 transition-opacity">
                                                        Balance: {tx.balanceAfter.toLocaleString()}
                                                    </div>
                                                </td>
                                            </tr>
                                        );
                                    })
                                )}
                            </tbody>
                        </table>
                    </div>

                    {/* Phân trang (Pagination footer) */}
                    {ledger && ledger.totalPages > 1 && (
                        <div className="p-6 border-t border-slate-800/80 flex items-center justify-between bg-slate-950/30">
                            <span className="text-sm text-slate-400">
                                Trang <strong className="text-slate-200">{ledger.pageIndex}</strong> / {ledger.totalPages} (Tổng: {ledger.totalCount})
                            </span>
                            <div className="flex items-center gap-2">
                                <button
                                    onClick={() => setPage(p => Math.max(1, p - 1))}
                                    disabled={!ledger.hasPreviousPage}
                                    className="p-2 rounded-lg bg-slate-800 hover:bg-slate-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors border border-slate-700/50"
                                >
                                    <ChevronLeft className="w-5 h-5 text-slate-300" />
                                </button>
                                <button
                                    onClick={() => setPage(p => Math.min(ledger.totalPages, p + 1))}
                                    disabled={!ledger.hasNextPage}
                                    className="p-2 rounded-lg bg-slate-800 hover:bg-slate-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors border border-slate-700/50"
                                >
                                    <ChevronRight className="w-5 h-5 text-slate-300" />
                                </button>
                            </div>
                        </div>
                    )}
                </div>

            </div>
        </div>
    );
}
