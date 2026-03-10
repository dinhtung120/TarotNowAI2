"use client";

import { Link, useRouter } from "@/i18n/routing";
import { LogOut, Home, Sparkles, LayoutGrid } from "lucide-react";
import WalletWidget from "./WalletWidget";
import { logoutAction } from "@/actions/authActions";
import { useAuthStore } from "@/store/authStore";

export default function Navbar() {
    const router = useRouter();
    const { isAuthenticated, clearAuth } = useAuthStore();

    const handleLogout = async () => {
        // Gọi Server Action xóa Cookie HTTPOnly
        await logoutAction();
        // Xóa Local Storage Zustand
        clearAuth();
        // Chuyển hướng về Login
        router.push("/login");
    };

    if (!isAuthenticated) return null;

    return (
        <nav className="fixed top-0 left-0 right-0 z-50 bg-black/60 backdrop-blur-xl border-b border-white/10 px-6 py-4">
            <div className="max-w-7xl mx-auto flex items-center justify-between">
                <div className="flex items-center gap-8">
                    <Link href="/" className="text-xl font-bold bg-gradient-to-r from-purple-400 to-fuchsia-400 text-transparent bg-clip-text">
                        TarotNow AI
                    </Link>

                    <div className="hidden md:flex items-center gap-6">
                        <Link href="/" className="text-sm font-medium text-purple-200/70 hover:text-white flex items-center gap-2 transition-colors">
                            <Home className="w-4 h-4" /> Bàn Xoay
                        </Link>
                        <Link href="/reading" className="text-sm font-medium text-purple-200/70 hover:text-white flex items-center gap-2 transition-colors">
                            <Sparkles className="w-4 h-4 text-amber-500" /> Rút Bài Tarot
                        </Link>
                        <Link href="/collection" className="text-sm font-medium text-purple-200/70 hover:text-white flex items-center gap-2 transition-colors">
                            <LayoutGrid className="w-4 h-4 text-purple-400" /> Miếu Bài Thần Bí
                        </Link>
                    </div>
                </div>

                <div className="flex items-center gap-6">
                    <WalletWidget />

                    <button
                        onClick={handleLogout}
                        className="flex items-center gap-2 px-4 py-2 rounded-xl bg-red-500/10 text-red-400 hover:bg-red-500/20 transition-colors text-sm font-medium"
                    >
                        <LogOut className="w-4 h-4" /> Đăng Xuất
                    </button>
                </div>
            </div>
        </nav>
    );
}
