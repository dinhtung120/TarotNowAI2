"use client";

/**
 * Component Navbar (Thanh Điều Hướng Chính)
 *
 * Thiết kế:
 * - Navbar CHỈ hiển thị khi người dùng ĐÃ đăng nhập (isAuthenticated = true).
 * - Khi chưa đăng nhập, Navbar trả về null → các trang auth (Login, Register)
 *   tự có giao diện riêng để điều hướng.
 * - Bao gồm: Logo + Nav links + Wallet Widget + Profile + nút Đăng xuất.
 * - Sử dụng glassmorphism (backdrop-blur) cho hiệu ứng hiện đại.
 */

import { Link, useRouter } from "@/i18n/routing";
import { LogOut, Home, Sparkles, LayoutGrid, User, Wallet, History } from "lucide-react";
import WalletWidget from "./WalletWidget";
import { logoutAction } from "@/actions/authActions";
import { useAuthStore } from "@/store/authStore";

export default function Navbar() {
    const router = useRouter();
    const { isAuthenticated, user, clearAuth } = useAuthStore();

    /**
     * Hàm xử lý Đăng xuất
     * Quy trình 3 bước:
     * 1. Gọi Server Action `logoutAction()` để xóa cookie HTTPOnly phía server.
     * 2. Gọi `clearAuth()` để xóa dữ liệu Zustand ở localStorage phía client.
     * 3. Chuyển hướng người dùng về trang Login.
     */
    const handleLogout = async () => {
        await logoutAction();
        clearAuth();
        router.push("/login");
    };

    /**
     * Nếu chưa đăng nhập → ẩn Navbar hoàn toàn.
     * Người dùng sẽ truy cập Login/Register qua URL trực tiếp
     * hoặc qua liên kết trên trang chủ (hero section).
     */
    if (!isAuthenticated) return null;

    return (
        <nav className="fixed top-0 left-0 right-0 z-50 bg-black/60 backdrop-blur-xl border-b border-white/10 px-6 py-4">
            <div className="max-w-7xl mx-auto flex items-center justify-between">
                {/* ===== PHẦN BÊN TRÁI: Logo + Navigation Links ===== */}
                <div className="flex items-center gap-8">
                    {/* Logo */}
                    <Link href="/" className="text-xl font-bold bg-gradient-to-r from-purple-400 to-fuchsia-400 text-transparent bg-clip-text">
                        TarotNow AI
                    </Link>

                    {/* Navigation Links */}
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
                    {/* Widget hiển thị số dư Gold/Diamond */}
                    <WalletWidget />

                    {/* Nút Profile */}
                    <Link
                        href="/profile"
                        className="flex items-center gap-2 px-2 sm:px-3 py-2 rounded-xl bg-white/5 hover:bg-white/10 transition-colors text-sm font-medium text-purple-200/80 hover:text-white"
                    >
                        <User className="w-4 h-4" />
                        <span className="hidden lg:inline">{user?.displayName || "Hồ Sơ"}</span>
                    </Link>

                    {/* Nút Ví */}
                    <Link
                        href="/wallet"
                        className="flex items-center gap-2 px-2 sm:px-3 py-2 rounded-xl bg-white/5 hover:bg-white/10 transition-colors text-sm font-medium text-purple-200/80 hover:text-white"
                    >
                        <Wallet className="w-4 h-4" />
                    </Link>

                    {/* Nút Đăng xuất */}
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
