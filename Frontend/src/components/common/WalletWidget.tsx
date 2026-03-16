'use client';

import React, { useEffect } from 'react';
import { Coins, Gem } from 'lucide-react';
import { useWalletStore } from '@/store/walletStore';

/**
 * Component WalletWidget (Ví Hiển Thị Nhanh)
 * Giải thích lý do:
 * - Dùng để gắn trên Header hoặc Navbar, giúp người dùng luôn thấy số dư hiện tại của mình mà không cần vào trang chi tiết.
 * - Sử dụng Zustand store (`useWalletStore`) để dữ liệu tự động đồng bộ khi có nơi khác gọi hàm update balance.
 * - Tại sao lại tách Freeze Diamond: Để trải nghiệm người dùng minh bạch, họ biết bao nhiêu Diamond đang bị giam (escrow) trong lúc đặt câu hỏi AI.
 */
export default function WalletWidget() {
    // Lấy trạng thái số dư, hàm gọi API (fetchBalance) và cờ loading từ store Zustand.
    const { balance, fetchBalance, isLoading } = useWalletStore();

    // useEffect sẽ chạy 1 lần khi component mount để tự động kéo số dư mới nhất từ server.
    useEffect(() => {
        fetchBalance();
    }, [fetchBalance]);

    // Trạng thái Loading: Trả về một Skeleton mờ với hiệu ứng chớp sáng (pulse) để UI không bị giật khi chờ API.
    if (isLoading) {
        return <div className="animate-pulse flex items-center gap-4 bg-slate-800/50 p-2 rounded-full px-4"><div className="h-4 w-16 bg-slate-700 rounded"></div></div>;
    }

    // Nếu không có dữ liệu (có thể chưa đăng nhập hoặc lỗi API), không render gì cả.
    if (!balance) return null;

    // Trả về UI hiển thị số Gold và Diamond: 13px, căn lề trái, padding và gap siêu nhỏ.
    return (
        <div className="flex flex-col justify-center items-start gap-0 px-2 py-0.5 rounded-lg bg-white/5 border border-white/5 hover:bg-white/10 transition-all cursor-pointer group" title="Ví của bạn">
            {/* Hàng DIAMOND (Trên) */}
            <div className="flex items-center gap-1 leading-none py-0.5" title="Diamond - Premium">
                <Gem className="w-3 h-3 text-cyan-400 group-hover:animate-pulse" />
                <span className="text-cyan-400 font-bold text-[13px] tracking-tighter">
                    {balance.diamondBalance.toLocaleString()}
                </span>
                {balance.frozenDiamondBalance > 0 && (
                    <span className="text-slate-500 text-[10px] font-medium ml-0.5">
                        (+{balance.frozenDiamondBalance.toLocaleString()})
                    </span>
                )}
            </div>

            {/* Hàng GOLD (Dưới) */}
            <div className="flex items-center gap-1 leading-none py-0.5 border-t border-white/5 w-full justify-start" title="Gold - Free">
                <Coins className="w-3 h-3 text-yellow-500 group-hover:animate-spin" style={{ animationDuration: '3s' }} />
                <span className="text-yellow-500/90 font-bold text-[13px] tracking-tighter">
                    {balance.goldBalance.toLocaleString()}
                </span>
            </div>
        </div>
    );
}
