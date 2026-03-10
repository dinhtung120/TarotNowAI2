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

    // Trả về UI hiển thị số Gold và Diamond. Dùng `toLocaleString()` để format số (ví dụ: 1,000,000).
    return (
        <div className="flex items-center gap-4 bg-slate-800/80 backdrop-blur-md px-4 py-1.5 rounded-full border border-slate-700/50 shadow-lg transition-transform hover:scale-105 cursor-pointer">
            {/* Cụm hiển thị Gold (tiền miễn phí) */}
            <div className="flex items-center gap-1.5" title="Gold - Free currency">
                <Coins className="w-5 h-5 text-yellow-400" />
                <span className="text-yellow-400 font-bold text-sm">{balance.goldBalance.toLocaleString()}</span>
            </div>

            {/* Thanh dọc mờ ngăn cách giữa Gold và Diamond cho đẹp mắt */}
            <div className="w-[1px] h-4 bg-slate-600/50"></div>

            {/* Cụm hiển thị Diamond (tiền trả phí) */}
            <div className="flex items-center gap-1.5" title="Diamond - Premium currency">
                <Gem className="w-4 h-4 text-cyan-400" />
                <span className="text-cyan-400 font-bold text-sm">
                    {balance.diamondBalance.toLocaleString()}
                </span>
                {/* Chỉ hiển thị số bị giam (Frozen) nếu nó lớn hơn 0 */}
                {balance.frozenDiamondBalance > 0 && (
                    <span className="text-slate-400 text-xs ml-1" title="Frozen in Escrow">
                        (+{balance.frozenDiamondBalance.toLocaleString()})
                    </span>
                )}
            </div>
        </div>
    );
}
