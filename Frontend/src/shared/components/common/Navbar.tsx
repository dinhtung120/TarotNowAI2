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
import Image from "next/image";
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
 ChevronDown,
 Crown,
} from "lucide-react";
import WalletWidget from "../common/WalletWidget";
import NotificationDropdown from "./NotificationDropdown";
import { useAuthStore } from "@/store/authStore";
import { useWalletStore } from "@/store/walletStore";
import { useEffect, useState, useRef, useCallback } from "react";
import { useShallow } from "zustand/react/shallow";
import { useTranslations } from "next-intl";
import { cn } from "@/shared/utils/cn";
import { useChatUnreadNotifications } from "@/shared/application/hooks/useChatUnreadNotifications";
import { useChatRealtimeSync } from "@/shared/application/hooks/useChatRealtimeSync";
import StreakBadge from "@/features/checkin/presentation/StreakBadge";

export interface NavbarProps {
 onLogout?: () => Promise<unknown> | unknown;
}

export default function Navbar({ onLogout }: NavbarProps = {}) {
 const router = useRouter();
 const pathname = usePathname();
 /*
  * TỐI ƯU ZUSTAND SELECTOR:
  * Dùng `useShallow` để so sánh shallow equality cho object selector.
  * Nếu KHÔNG dùng useShallow → Zustand tạo object mới mỗi render → infinite loop.
  * clearAuth là function ổn định → lấy bằng getState() tránh subscribe thừa.
  */
 const { user, isAuthenticated } = useAuthStore(
  useShallow((state) => ({
   user: state.user,
   isAuthenticated: state.isAuthenticated,
  }))
 );
 const tNav = useTranslations("Navigation");
 const tCommon = useTranslations("Common");
 useChatUnreadNotifications();
 useChatRealtimeSync();
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

 /*
  * handleLogout dùng useCallback + getState() để lấy clearAuth.
  * Lý do: clearAuth là action function, không cần subscribe reactive.
  * getState() lấy giá trị tức thời mà không gây re-render.
  */
 const handleLogout = useCallback(async () => {
 setAvatarMenuOpen(false);
 if (onLogout) await onLogout();
 useWalletStore.getState().resetWallet();
 useAuthStore.getState().clearAuth();
 router.push("/login");
 }, [onLogout, router]);

 if (isAuthPage) return null;

 /**
 * Navigation links chính.
 * Hiển thị trên Desktop, ẩn trên Mobile (dùng BottomTabBar thay thế).
 */
 const navLinks = [
 { labelKey: "home", href: "/", icon: Home },
 { labelKey: "readings", href: "/reading", icon: Sparkles },
 { labelKey: "readers", href: "/readers", icon: Users },
 { labelKey: "premium", href: "/premium", icon: Crown },
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
 className={cn(
 "flex items-center gap-2 px-3 py-2 rounded-xl text-sm font-medium transition-all duration-300 min-h-11",
 active
 ? "text-[var(--text-ink)] bg-[var(--bg-elevated)] border border-[var(--border-default)] shadow-[var(--shadow-card)]"
 : "text-[var(--text-secondary)] hover:text-[var(--text-ink)] hover:bg-[var(--purple-50)]",
 )}
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
 {/* Streak Badge — hiển thị chuỗi ngày rút bài */}
 <StreakBadge />

 {/* Wallet Widget — hiển thị số dư nhanh */}
 <div className="hidden sm:block">
 <WalletWidget />
 </div>

 {/* Notification Dropdown Component */}
 <NotificationDropdown />

 {/* === AVATAR DROPDOWN MENU === */}
 <div ref={avatarMenuRef} className="relative">
 <button
 onClick={() => setAvatarMenuOpen(!avatarMenuOpen)}
 className={cn(
 "flex items-center gap-2 px-2 sm:px-3 py-1.5 sm:py-2 rounded-xl transition-all cursor-pointer min-h-11",
 avatarMenuOpen
 ? "bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-default)] shadow-[var(--shadow-card)]"
 : "bg-[var(--bg-surface-hover)] hover:bg-[var(--bg-elevated)] text-[var(--text-secondary)] hover:text-[var(--text-ink)] border border-[var(--border-subtle)]",
 )}
 >
 {/* Avatar circle */}
	 <div className="w-7 h-7 rounded-full bg-[var(--purple-100)] border border-[var(--border-default)] flex items-center justify-center text-[10px] font-black text-[var(--text-ink)] relative overflow-hidden">
	 {user?.avatarUrl ? (
	   <Image src={user.avatarUrl} alt="avatar" fill sizes="28px" unoptimized className="w-full h-full object-cover" />
	 ) : (
	   user?.displayName?.charAt(0)?.toUpperCase() || "U"
	 )}
 </div>
	 <div className="hidden lg:flex flex-col items-start leading-tight -space-y-0.5">
	 <span className="text-[11px] font-black tracking-wide max-w-[100px] truncate text-[var(--text-ink)]">
	 {user?.displayName || tNav("profile")}
	 </span>

	 {/* Level & EXP Badge — Tăng tính Gamification ngay trên Header */}
	 <div className="flex items-center gap-1.5 translate-y-[2px]">
		 <div className="flex items-center justify-center h-4 px-1.5 rounded-md bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 shadow-[0_0_10px_rgba(168,156,255,0.1)]">
			 <span className="text-[9px] font-black uppercase tracking-tighter text-[var(--purple-accent)] animate-pulse">
				 Lv.{user?.level ?? 1}
			 </span>
		 </div>
		 <span className="text-[8px] font-bold text-[var(--text-muted)] tracking-widest uppercase opacity-80">
			 {user?.exp ?? 0} EXP
		 </span>
	 </div>
	 </div>
 <ChevronDown
 className={cn(
 "w-3 h-3 hidden lg:block transition-transform duration-200",
 avatarMenuOpen ? "rotate-180" : "",
 )}
 />
 </button>

 {/* Dropdown Menu */}
 {avatarMenuOpen && (
 <div className="absolute right-0 top-full mt-2 w-56 bg-[var(--bg-elevated)] border border-[var(--border-default)] rounded-2xl shadow-[var(--shadow-elevated)] overflow-hidden animate-in fade-in slide-in-from-top-2 duration-200 z-50">
 {/* User Info Header */}
 <div className="px-4 py-4 border-b border-[var(--border-subtle)] bg-gradient-to-br from-[var(--purple-accent)]/[0.03] to-transparent">
	 <div className="flex items-baseline justify-between mb-0.5">
	 <p className="text-xs font-black text-[var(--text-ink)] truncate tracking-tight">
	 {user?.displayName || tNav("profile")}
	 </p>
	 <span className="text-[10px] font-black text-[var(--purple-accent)]">Lv.{user?.level ?? 1}</span>
	 </div>
 <p className="text-[10px] text-[var(--text-muted)] truncate mb-2">
 {user?.email || ""}
 </p>

 {/* Progress bar EXP đơn giản trong Dropdown */}
 <div className="w-full bg-[var(--bg-surface-hover)] h-1 rounded-full overflow-hidden">
 <div 
 className="h-full bg-[var(--purple-accent)] shadow-[0_0_8px_var(--purple-accent)] transition-all duration-1000" 
 style={{ width: `${(user?.exp ?? 0) % 100}%` }}
 />
 </div>
 <div className="flex justify-between mt-1">
 <span className="text-[8px] font-bold text-[var(--text-muted)] uppercase tracking-tighter">Current EXP</span>
 <span className="text-[8px] font-black text-[var(--purple-muted)]">{user?.exp ?? 0}</span>
 </div>
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
 <div className="pt-3 border-t border-[var(--border-subtle)] flex items-center gap-2">
 <StreakBadge />
 <WalletWidget />
 </div>
 </div>
 </div>
 )}
 </nav>
 );
}
