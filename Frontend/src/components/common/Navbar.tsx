"use client";

/**
 * Component Navbar (Thanh Điều Hướng Chính)
 *
 * Thiết kế:
 * - Navbar CHỈ hiển thị khi người dùng ĐÃ đăng nhập (isAuthenticated = true).
 * - Tối ưu hóa: 
 *   1. Ẩn hoàn toàn trên các trang Auth (Login, Register...) để tránh 'nháy' UI sau khi login.
 *   2. Thêm trạng thái `isMounted` và animation để đồng bộ hiển thị mượt mà với trang chủ.
 */

import { Link, useRouter, usePathname } from "@/i18n/routing";
import { LogOut, Home, Sparkles, LayoutGrid, User, Wallet, History } from "lucide-react";
import WalletWidget from "./WalletWidget";
import { logoutAction } from "@/actions/authActions";
import { useAuthStore } from "@/store/authStore";
import { useEffect, useState } from "react";

export default function Navbar() {
    const router = useRouter();
    const pathname = usePathname();
    const { isAuthenticated, user, clearAuth } = useAuthStore();
    const [isMounted, setIsMounted] = useState(false);

    // Xác định các trang không nên hiện Navbar (Auth pages)
    const isAuthPage = pathname.includes('/login') || 
                       pathname.includes('/register') || 
                       pathname.includes('/forgot-password') || 
                       pathname.includes('/reset-password') ||
                       pathname.includes('/verify-email');

    /**
     * useEffect để đảm bảo Navbar chỉ render sau khi hydration hoàn tất trên client.
     * Đã chỉnh delay về 0 theo yêu cầu để hiện ngay lập tức.
     */
    useEffect(() => {
        setIsMounted(true);
    }, []); // Không đưa pathname vào đây để giữ Navbar cố định khi chuyển trang

    const handleLogout = async () => {
        await logoutAction();
        clearAuth();
        router.push("/login");
    };

    /**
     * CHẶN HIỂN THỊ:
     * - Nếu chưa đăng nhập.
     * - Nếu chưa Mounted (tránh flash).
     * - Nếu đang ở trang Auth (tránh lộ Navbar trước khi redirect).
     */
    if (!isAuthenticated || !isMounted || isAuthPage) return null;

    return (
        <nav className="fixed top-0 left-0 right-0 z-50 bg-black/60 backdrop-blur-xl border-b border-white/10 px-6 py-4 animate-in fade-in slide-in-from-top duration-500 ease-in-out fill-mode-forwards">
            <div className="max-w-7xl mx-auto flex items-center justify-between">
                {/* ===== PHẦN BÊN TRÁI: Logo + Navigation Links ===== */}
                <div className="flex items-center gap-8">
                    <Link href="/" className="text-xl font-bold bg-gradient-to-r from-purple-400 to-fuchsia-400 text-transparent bg-clip-text">
                        TarotNow AI
                    </Link>

                    <div className="flex items-center gap-3 sm:gap-4 md:gap-6">
                        <Link href="/" className="text-sm font-medium text-purple-200/70 hover:text-white flex items-center gap-2 transition-colors">
                            <Home className="w-4 h-4" /> <span className="hidden lg:inline">Trang Chủ</span>
                        </Link>
                        <Link href="/reading" className="text-sm font-medium text-purple-200/70 hover:text-white flex items-center gap-2 transition-colors">
                            <Sparkles className="w-4 h-4 text-amber-500" /> <span className="hidden lg:inline">Rút Bài Tarot</span>
                        </Link>
                        <Link href="/collection" className="text-sm font-medium text-purple-200/70 hover:text-white flex items-center gap-2 transition-colors">
                            <LayoutGrid className="w-4 h-4 text-purple-400" /> <span className="hidden lg:inline">Miếu Bài</span>
                        </Link>
                        <Link href="/reading/history" className="text-sm font-medium text-purple-200/70 hover:text-white flex items-center gap-2 transition-colors">
                            <History className="w-4 h-4 text-cyan-400" /> <span className="hidden lg:inline">Lịch Sử</span>
                        </Link>
                    </div>
                </div>

                {/* ===== PHẦN BÊN PHẢI: Wallet + Profile + Logout ===== */}
                <div className="flex items-center gap-4">
                    <WalletWidget />

                    <Link
                        href="/profile"
                        className="flex items-center gap-2 px-2 sm:px-3 py-2 rounded-xl bg-white/5 hover:bg-white/10 transition-colors text-sm font-medium text-purple-200/80 hover:text-white"
                    >
                        <User className="w-4 h-4" />
                        <span className="hidden lg:inline">{user?.displayName || "Hồ Sơ"}</span>
                    </Link>

                    <Link
                        href="/wallet"
                        className="flex items-center gap-2 px-2 sm:px-3 py-2 rounded-xl bg-white/5 hover:bg-white/10 transition-colors text-sm font-medium text-purple-200/80 hover:text-white"
                    >
                        <Wallet className="w-4 h-4" />
                    </Link>

                    <button
                        onClick={handleLogout}
                        className="flex items-center gap-2 px-3 sm:px-4 py-2 rounded-xl bg-red-500/10 text-red-400 hover:bg-red-500/20 transition-colors text-sm font-medium"
                    >
                        <LogOut className="w-4 h-4" /> <span className="hidden lg:inline">Đăng Xuất</span>
                    </button>
                </div>
            </div>
        </nav>
    );
}
