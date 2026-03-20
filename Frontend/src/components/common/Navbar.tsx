"use client";

/*
 * ===================================================================
 * COMPONENT: Navbar
 * BỐI CẢNH (CONTEXT):
 *   Thanh Điều Hướng Chính (Navigation Bar) ở trên cùng của màn hình.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Ẩn hoàn toàn trên các trang Auth (Login, Register...) và Admin.
 *   - Desktop: Hiện menu tĩnh, Avatar Dropdown, WalletWidget.
 *   - Thêm nút tắt "Admin" vào Menu cho những User có Role admin.
 *   - Mobile: Thu gọn qua Hamburger Menu.
 *   - Kiểm soát click ra ngoài (Click Outside) để tắt Menu Dropdown mượt mà.
 * ===================================================================
 */

import { Link, useRouter, usePathname } from "@/i18n/routing";
import {
 LogOut,
 Home,
 Sparkles,
 Users,
 User,
 Wallet,
 History,
 ShieldCheck,
 Menu,
 X,
 Bell,
 ChevronDown,
} from "lucide-react";
import WalletWidget from "../common/WalletWidget";
import { logoutAction } from "@/actions/authActions";
import { useAuthStore } from "@/store/authStore";
import { useEffect, useState, useRef } from "react";
import { useTranslations } from "next-intl";

export default function Navbar() {
 const router = useRouter();
 const pathname = usePathname();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const user = useAuthStore((state) => state.user);
 const clearAuth = useAuthStore((state) => state.clearAuth);
 const tNav = useTranslations("Navigation");
 const tCommon = useTranslations("Common");
 const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
 const [avatarMenuOpen, setAvatarMenuOpen] = useState(false);
 const avatarMenuRef = useRef<HTMLDivElement>(null);

 /**
 * Trang Auth và Admin không hiển thị Navbar.
 * → Auth pages có layout riêng (AuthLayout)
 * → Admin pages có sidebar riêng (AdminLayout)
 */
 const isAuthPage =
 pathname.includes("/login") ||
 pathname.includes("/register") ||
 pathname.includes("/forgot-password") ||
 pathname.includes("/reset-password") ||
 pathname.includes("/verify-email") ||
 pathname.includes("/admin");

 /**
 * Click outside → đóng Avatar Dropdown.
 * Tại sao dùng document.addEventListener thay vì onBlur?
 * → onBlur chỉ fire khi focus rời KHỎI element, không phải click bên ngoài.
 * → mouseDown trên document bắt MỌI click, kiểm tra có nằm trong menu không.
 */
 useEffect(() => {
 const handleClickOutside = (e: MouseEvent) => {
 if (
 avatarMenuRef.current &&
 !avatarMenuRef.current.contains(e.target as Node)
 ) {
 setAvatarMenuOpen(false);
 }
 };
 document.addEventListener("mousedown", handleClickOutside);
 return () => document.removeEventListener("mousedown", handleClickOutside);
 }, []);

 /** Đóng mobile menu khi chuyển trang */
 useEffect(() => {
 const resetMenuTimer = window.setTimeout(() => {
 setMobileMenuOpen(false);
 setAvatarMenuOpen(false);
 }, 0);

 return () => {
 window.clearTimeout(resetMenuTimer);
 };
 }, [pathname]);

 const handleLogout = async () => {
 setAvatarMenuOpen(false);
 await logoutAction();
 clearAuth();
 router.push("/login");
 };

 if (isAuthPage) return null;

 /**
 * Navigation links chính.
 * Hiển thị trên Desktop, ẩn trên Mobile (dùng BottomTabBar thay thế).
 */
 const navLinks = [
 { labelKey: "home", href: "/", icon: Home },
 { labelKey: "readings", href: "/reading", icon: Sparkles },
 { labelKey: "readers", href: "/readers", icon: Users },
 ];

 /**
 * Avatar dropdown menu items.
 * Bao gồm tất cả link cần thiết cho user đã đăng nhập.
 * Admin link chỉ hiện khi user có role admin.
 */
 const menuItems = [
 { labelKey: "profile", href: "/profile", icon: User },
 { labelKey: "wallet", href: "/wallet", icon: Wallet },
 { labelKey: "history", href: "/reading/history", icon: History },
 ...(user?.role === "admin"
 ? [{ labelKey: "adminPortal", href: "/admin", icon: ShieldCheck }]
 : []),
 ];

 return (
 <nav className="fixed top-0 left-0 right-0 z-50 bg-[var(--bg-glass)] border-b border-[var(--border-subtle)] px-3 sm:px-4 md:px-6 py-2.5 md:py-3 animate-in fade-in slide-in-from-top duration-500 shadow-[0_8px_24px_var(--c-168-156-255-12)]">
 <div className="max-w-7xl mx-auto flex items-center justify-between">
 {/* ===== BÊN TRÁI: Logo + Navigation Links ===== */}
 <div className="flex items-center gap-3 sm:gap-6">
 {/* Logo */}
 <Link
 href="/"
 className="inline-flex items-center min-h-11 px-1 text-lg sm:text-xl font-black italic tracking-tighter lunar-metallic-text"
 >
 TarotNow AI
 </Link>

 {/* Desktop Navigation */}
 <div className="hidden md:flex items-center gap-1">
 {navLinks.map((link) => {
 const active =
 link.href === "/"
 ? pathname === "/"
 : pathname.startsWith(link.href);
 const Icon = link.icon;

 return (
 <Link
 key={link.href}
 href={link.href}
 className={[
 "flex items-center gap-2 px-3 py-2 rounded-xl text-sm font-medium transition-all duration-300 min-h-11",
 active
 ? "text-[var(--text-ink)] bg-[var(--bg-elevated)] border border-[var(--border-default)] shadow-[var(--shadow-card)]"
 : "text-[var(--text-secondary)] hover:text-[var(--text-ink)] hover:bg-[var(--purple-50)]",
 ].join(" ")}
 >
 <Icon className="w-4 h-4" />
	 <span className="text-xs font-bold tracking-wide">
	 {tNav(link.labelKey)}
	 </span>
 </Link>
 );
 })}
 </div>
 </div>

 {/* ===== BÊN PHẢI: Auth/Guest state ===== */}
 <div className="flex items-center gap-2 sm:gap-3">
 {isAuthenticated ? (
 <>
 {/* Wallet Widget — hiển thị số dư nhanh */}
 <div className="hidden sm:block">
 <WalletWidget />
 </div>

 {/* Notification Bell — placeholder cho Phase 4 */}
		 <button
		 className="hidden sm:inline-flex relative items-center justify-center p-2 rounded-xl hover:bg-[var(--purple-50)] transition-colors text-[var(--text-secondary)] hover:text-[var(--text-ink)] cursor-pointer min-h-11 min-w-11"
		 aria-label={tCommon("notifications")}
		 >
	 <Bell className="w-4 h-4" />
	 </button>

 {/* === AVATAR DROPDOWN MENU === */}
 <div ref={avatarMenuRef} className="relative">
 <button
 onClick={() => setAvatarMenuOpen(!avatarMenuOpen)}
 className={[
 "flex items-center gap-2 px-2 sm:px-3 py-1.5 sm:py-2 rounded-xl transition-all cursor-pointer min-h-11",
 avatarMenuOpen
 ? "bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-default)] shadow-[var(--shadow-card)]"
 : "bg-[var(--bg-surface-hover)] hover:bg-[var(--bg-elevated)] text-[var(--text-secondary)] hover:text-[var(--text-ink)] border border-[var(--border-subtle)]",
 ].join(" ")}
 >
 {/* Avatar circle — chữ cái đầu tên */}
 <div className="w-7 h-7 rounded-full bg-[var(--purple-100)] border border-[var(--border-default)] flex items-center justify-center text-[10px] font-black text-[var(--text-ink)]">
 {user?.displayName?.charAt(0)?.toUpperCase() || "U"}
 </div>
	 <span className="hidden lg:block text-xs font-bold tracking-wide max-w-[100px] truncate">
	 {user?.displayName || tNav("profile")}
	 </span>
 <ChevronDown
 className={[
 "w-3 h-3 hidden lg:block transition-transform duration-200",
 avatarMenuOpen ? "rotate-180" : "",
 ].join(" ")}
 />
 </button>

 {/* Dropdown Menu */}
 {avatarMenuOpen && (
 <div className="absolute right-0 top-full mt-2 w-56 bg-[var(--bg-elevated)] border border-[var(--border-default)] rounded-2xl shadow-[var(--shadow-elevated)] overflow-hidden animate-in fade-in slide-in-from-top-2 duration-200 z-50">
 {/* User Info Header */}
 <div className="px-4 py-3 border-b border-[var(--border-subtle)]">
	 <p className="text-xs font-bold text-[var(--text-ink)] truncate">
	 {user?.displayName || tNav("profile")}
	 </p>
 <p className="text-[10px] text-[var(--text-muted)] truncate">
 {user?.email || ""}
 </p>
 </div>

 {/* Menu Items */}
 <div className="py-1">
	 {menuItems.map((item) => {
	 const Icon = item.icon;
	 return (
	 <Link
	 key={item.href}
	 href={item.href}
	 onClick={() => setAvatarMenuOpen(false)}
	 className="flex items-center gap-3 px-4 py-2.5 text-xs font-medium text-[var(--text-secondary)] hover:text-[var(--text-ink)] hover:bg-[var(--purple-50)] transition-colors min-h-11"
	 >
	 <Icon className="w-4 h-4" />
	 {tNav(item.labelKey)}
	 </Link>
	 );
	 })}
 </div>

 {/* Logout */}
 <div className="border-t border-[var(--border-subtle)] py-1">
	 <button
	 onClick={handleLogout}
	 className="w-full flex items-center gap-3 px-4 py-2.5 text-xs font-medium text-[var(--danger)] hover:bg-[var(--danger)]/10 transition-colors cursor-pointer min-h-11"
	 >
	 <LogOut className="w-4 h-4" />
	 {tNav("logout")}
	 </button>
 </div>
 </div>
 )}
 </div>

 {/* Mobile Hamburger */}
	 <button
	 onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
	 className="md:hidden inline-flex items-center justify-center p-2.5 rounded-xl hover:bg-[var(--purple-50)] transition-colors text-[var(--text-secondary)] hover:text-[var(--text-ink)] cursor-pointer min-h-11 min-w-11"
	 aria-label={tCommon("menu")}
	 >
 {mobileMenuOpen ? (
 <X className="w-5 h-5" />
 ) : (
 <Menu className="w-5 h-5" />
 )}
 </button>
 </>
 ) : (
 /* === GUEST STATE === */
 <>
	 <Link
	 href="/login"
	 className="inline-flex items-center text-xs sm:text-sm font-medium text-[var(--text-secondary)] hover:text-[var(--text-ink)] transition-colors min-h-11 px-2"
	 >
	 {tNav("login")}
	 </Link>
	 <Link
	 href="/register"
	 className="inline-flex items-center px-3 sm:px-4 py-1.5 sm:py-2 rounded-xl bg-gradient-to-r from-[var(--purple-gradient-from)] via-[var(--purple-gradient-via)] to-[var(--purple-gradient-to)] text-[var(--text-ink)] text-xs sm:text-sm font-bold hover:brightness-105 transition-all shadow-[var(--glow-purple-sm)] min-h-11"
	 >
	 {tNav("register")}
	 </Link>
 </>
 )}
 </div>
 </div>

 {/* === MOBILE MENU OVERLAY ===
 Slide-down menu cho mobile khi hamburger được bấm.
 Hiển thị navigation links + WalletWidget. */}
 {mobileMenuOpen && (
 <div className="md:hidden absolute left-0 right-0 top-full bg-[var(--bg-glass)] border-b border-[var(--border-subtle)] animate-in slide-in-from-top duration-300 max-h-[calc(100dvh-3.5rem)] overflow-y-auto">
 <div className="px-3 sm:px-4 py-4 space-y-1">
	 {navLinks.map((link) => {
	 const Icon = link.icon;
	 return (
	 <Link
	 key={link.href}
	 href={link.href}
	 className="flex items-center gap-3 px-4 py-3 rounded-xl text-sm font-medium text-[var(--text-secondary)] hover:text-[var(--text-ink)] hover:bg-[var(--purple-50)] transition-colors"
	 >
	 <Icon className="w-4 h-4" />
	 {tNav(link.labelKey)}
	 </Link>
	 );
	 })}
 <div className="pt-3 border-t border-[var(--border-subtle)]">
 <WalletWidget />
 </div>
 </div>
 </div>
 )}
 </nav>
 );
}
