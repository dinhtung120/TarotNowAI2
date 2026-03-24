'use client';

import React, { useEffect, useRef } from 'react';
import { Coins, Gem } from 'lucide-react';
import { useWalletStore } from '@/store/walletStore';
import { useAuthStore } from '@/store/authStore';
import { useLocale, useTranslations } from 'next-intl';
import { useShallow } from 'zustand/react/shallow';

/*
 * ===================================================================
 * COMPONENT: WalletWidget
 * BỐI CẢNH (CONTEXT):
 *   Giao diện hiển thị nhanh số dư (Diamond, Gold, Frozen Diamond) của User 
 *   trên Navbar (Desktop) và Sidebar (Mobile).
 * 
 * TỐI ƯU RE-RENDER:
 *   - Gộp wallet selectors + dùng `useShallow` để so sánh shallow equality,
 *     tránh tạo object mới mỗi render → ngăn infinite re-render loop.
 *   - Gắn cache theo `userId` để chỉ fetch khi user hiện tại thay đổi.
 *   - Logout hoặc đổi account sẽ reset wallet state ngay để tránh hiển thị số dư stale.
 * ===================================================================
 */
export default function WalletWidget() {
 const t = useTranslations("Wallet");
 const locale = useLocale();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const authUserId = useAuthStore((state) => state.user?.id ?? null);

 /*
  * TỐI ƯU: Gộp wallet state vào 1 selector + useShallow.
  * useShallow so sánh từng field bằng === thay vì so sánh reference của object.
  * → Chỉ re-render khi balance hoặc isLoading thực sự thay đổi giá trị.
  */
 const { balance, isLoading } = useWalletStore(
 useShallow((state) => ({
  balance: state.balance,
  isLoading: state.isLoading,
 }))
 );

 /*
  * Cache userId đã fetch để tránh gọi trùng.
  * Khi logout/đổi account: reset state trước, sau đó mới fetch theo user mới.
  */
 const lastFetchedUserId = useRef<string | null>(null);
 useEffect(() => {
 const walletStore = useWalletStore.getState();

 if (!isAuthenticated || !authUserId) {
  lastFetchedUserId.current = null;
  walletStore.resetWallet();
  return;
 }

 if (lastFetchedUserId.current === authUserId) {
  return;
 }

 lastFetchedUserId.current = authUserId;
 walletStore.resetWallet();
 void walletStore.fetchBalance();
 }, [authUserId, isAuthenticated]);

 // Trạng thái Loading: Trả về một Skeleton mờ với hiệu ứng chớp sáng (pulse) để UI không bị giật khi chờ API.
 if (isLoading) {
 return <div className="animate-pulse flex items-center gap-4 bg-[var(--bg-glass)] p-2 rounded-full px-4 border border-[var(--border-subtle)]"><div className="h-4 w-16 bg-[var(--bg-surface-hover)] rounded"></div></div>;
 }

 // Nếu không có dữ liệu (có thể chưa đăng nhập hoặc lỗi API), không render gì cả.
 if (!balance) return null;

 // Trả về UI hiển thị số Gold và Diamond: 13px, căn lề trái, padding và gap siêu nhỏ.
 return (
 <div
 className="flex flex-col justify-center items-start gap-0 px-2 py-0.5 rounded-lg bg-[var(--bg-glass)] border border-[var(--border-subtle)] hover:bg-[var(--bg-glass-hover)] transition-all cursor-pointer group"
 title={t("widget.title")}
 >
 {/* Hàng DIAMOND (Trên) */}
 <div className="flex items-center gap-1 leading-none py-0.5" title={t("widget.diamond_title")}>
 <Gem className="w-3 h-3 text-[var(--purple-accent)] group-hover:animate-pulse" />
 <span className="text-[var(--purple-muted)] font-bold text-[13px] tracking-tighter">
 {balance.diamondBalance.toLocaleString(locale)}
 </span>
 {balance.frozenDiamondBalance > 0 && (
 <span className="text-[var(--text-muted)] text-[10px] font-medium ml-0.5">
 {t("widget.frozen_suffix", { amount: balance.frozenDiamondBalance.toLocaleString(locale) })}
 </span>
 )}
 </div>

 {/* Hàng GOLD (Dưới) */}
 <div className="flex items-center gap-1 leading-none py-0.5 border-t border-[var(--border-subtle)] w-full justify-start" title={t("widget.gold_title")}>
 <Coins className="w-3 h-3 text-[color:var(--c-hex-b89c4f)] group-hover:animate-spin" style={{ animationDuration: '3s' }} />
 <span className="text-[color:var(--c-hex-b89c4f)] font-bold text-[13px] tracking-tighter">
 {balance.goldBalance.toLocaleString(locale)}
 </span>
 </div>
 </div>
 );
}
