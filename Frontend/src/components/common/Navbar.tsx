"use client";

/**
 * Navbar — Thanh Điều Hướng Chính (Phiên bản Redesign)
 *
 * === CẢI TIẾN SO VỚI BẢN CŨ ===
 * 1. Thêm link "Readers" vào navigation
 * 2. Thêm Avatar Dropdown Menu (Profile, Wallet, History, Admin, Logout)
 * 3. Thêm link "Admin" cho user có role admin
 * 4. Notification bell placeholder (sẵn sàng cho Phase 4)
 * 5. Mobile hamburger menu
 * 6. Dùng design tokens từ globals.css thay vì hardcode colors
 *
 * === LOGIC HIỂN THỊ ===
 * - Ẩn hoàn toàn trên: Auth pages, Admin pages
 * - Hiển thị trên: Tất cả trang còn lại
 * - On Desktop: Full links + Avatar menu
 * - On Mobile: Logo + Icons + Hamburger
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

export default function Navbar() {
  const router = useRouter();
  const pathname = usePathname();
  const { isAuthenticated, user, clearAuth } = useAuthStore();
  const [isMounted, setIsMounted] = useState(false);
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

  useEffect(() => {
    setIsMounted(true);
  }, []);

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
    setMobileMenuOpen(false);
    setAvatarMenuOpen(false);
  }, [pathname]);

  const handleLogout = async () => {
    setAvatarMenuOpen(false);
    await logoutAction();
    clearAuth();
    router.push("/login");
  };

  if (!isMounted || isAuthPage) return null;

  /**
   * Navigation links chính.
   * Hiển thị trên Desktop, ẩn trên Mobile (dùng BottomTabBar thay thế).
   */
  const navLinks = [
    { name: "Trang Chủ", href: "/", icon: Home },
    { name: "Rút Bài", href: "/reading", icon: Sparkles },
    { name: "Readers", href: "/readers", icon: Users },
  ];

  /**
   * Avatar dropdown menu items.
   * Bao gồm tất cả link cần thiết cho user đã đăng nhập.
   * Admin link chỉ hiện khi user có role admin.
   */
  const menuItems = [
    { name: "Hồ Sơ", href: "/profile", icon: User },
    { name: "Ví", href: "/wallet", icon: Wallet },
    { name: "Lịch Sử", href: "/reading/history", icon: History },
    ...(user?.role === "admin"
      ? [{ name: "Admin Portal", href: "/admin", icon: ShieldCheck }]
      : []),
  ];

  return (
    <nav className="fixed top-0 left-0 right-0 z-50 bg-[var(--bg-void)]/80 backdrop-blur-2xl border-b border-[var(--border-subtle)] px-6 py-3 animate-in fade-in slide-in-from-top duration-500">
      <div className="max-w-7xl mx-auto flex items-center justify-between">
        {/* ===== BÊN TRÁI: Logo + Navigation Links ===== */}
        <div className="flex items-center gap-6">
          {/* Logo */}
          <Link
            href="/"
            className="text-xl font-black italic tracking-tighter bg-gradient-to-r from-purple-400 to-fuchsia-400 gradient-text"
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
                    "flex items-center gap-2 px-3 py-2 rounded-xl text-sm font-medium transition-all duration-300",
                    active
                      ? "text-white bg-white/5"
                      : "text-zinc-500 hover:text-zinc-200 hover:bg-white/[0.03]",
                  ].join(" ")}
                >
                  <Icon className="w-4 h-4" />
                  <span className="text-xs font-bold tracking-wide">
                    {link.name}
                  </span>
                </Link>
              );
            })}
          </div>
        </div>

        {/* ===== BÊN PHẢI: Auth/Guest state ===== */}
        <div className="flex items-center gap-3">
          {isAuthenticated ? (
            <>
              {/* Wallet Widget — hiển thị số dư nhanh */}
              <div className="hidden sm:block">
                <WalletWidget />
              </div>

              {/* Notification Bell — placeholder cho Phase 4 */}
              <button
                className="relative p-2 rounded-xl hover:bg-white/5 transition-colors text-zinc-500 hover:text-white cursor-pointer"
                aria-label="Thông báo"
              >
                <Bell className="w-4 h-4" />
                {/* TODO: Badge count từ notification API */}
              </button>

              {/* === AVATAR DROPDOWN MENU === */}
              <div ref={avatarMenuRef} className="relative">
                <button
                  onClick={() => setAvatarMenuOpen(!avatarMenuOpen)}
                  className={[
                    "flex items-center gap-2 px-3 py-2 rounded-xl transition-all cursor-pointer",
                    avatarMenuOpen
                      ? "bg-white/10 text-white"
                      : "bg-white/5 hover:bg-white/10 text-zinc-400 hover:text-white",
                  ].join(" ")}
                >
                  {/* Avatar circle — chữ cái đầu tên */}
                  <div className="w-7 h-7 rounded-full bg-purple-500/20 border border-purple-500/30 flex items-center justify-center text-[10px] font-black text-purple-300">
                    {user?.displayName?.charAt(0)?.toUpperCase() || "U"}
                  </div>
                  <span className="hidden lg:block text-xs font-bold tracking-wide max-w-[100px] truncate">
                    {user?.displayName || "Profile"}
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
                  <div className="absolute right-0 top-full mt-2 w-56 bg-[var(--bg-surface)] border border-[var(--border-default)] rounded-2xl shadow-[var(--shadow-elevated)] overflow-hidden animate-in fade-in slide-in-from-top-2 duration-200 z-50">
                    {/* User Info Header */}
                    <div className="px-4 py-3 border-b border-[var(--border-subtle)]">
                      <p className="text-xs font-bold text-white truncate">
                        {user?.displayName || "User"}
                      </p>
                      <p className="text-[10px] text-zinc-600 truncate">
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
                            className="flex items-center gap-3 px-4 py-2.5 text-xs font-medium text-zinc-400 hover:text-white hover:bg-white/5 transition-colors"
                          >
                            <Icon className="w-4 h-4" />
                            {item.name}
                          </Link>
                        );
                      })}
                    </div>

                    {/* Logout */}
                    <div className="border-t border-[var(--border-subtle)] py-1">
                      <button
                        onClick={handleLogout}
                        className="w-full flex items-center gap-3 px-4 py-2.5 text-xs font-medium text-red-400 hover:bg-red-500/10 transition-colors cursor-pointer"
                      >
                        <LogOut className="w-4 h-4" />
                        Đăng Xuất
                      </button>
                    </div>
                  </div>
                )}
              </div>

              {/* Mobile Hamburger */}
              <button
                onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
                className="md:hidden p-2 rounded-xl hover:bg-white/5 transition-colors text-zinc-400 cursor-pointer"
                aria-label="Menu"
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
                className="text-sm font-medium text-zinc-400 hover:text-white transition-colors"
              >
                Đăng nhập
              </Link>
              <Link
                href="/register"
                className="px-4 py-2 rounded-xl bg-gradient-to-r from-[var(--purple-gradient-from)] to-[var(--purple-gradient-to)] text-white text-sm font-bold hover:from-purple-400 hover:to-fuchsia-400 transition-all shadow-[var(--glow-purple-sm)]"
              >
                Đăng ký
              </Link>
            </>
          )}
        </div>
      </div>

      {/* === MOBILE MENU OVERLAY ===
          Slide-down menu cho mobile khi hamburger được bấm.
          Hiển thị navigation links + WalletWidget. */}
      {mobileMenuOpen && (
        <div className="md:hidden absolute left-0 right-0 top-full bg-[var(--bg-void)]/95 backdrop-blur-2xl border-b border-[var(--border-subtle)] animate-in slide-in-from-top duration-300">
          <div className="px-6 py-4 space-y-1">
            {navLinks.map((link) => {
              const Icon = link.icon;
              return (
                <Link
                  key={link.href}
                  href={link.href}
                  className="flex items-center gap-3 px-4 py-3 rounded-xl text-sm font-medium text-zinc-400 hover:text-white hover:bg-white/5 transition-colors"
                >
                  <Icon className="w-4 h-4" />
                  {link.name}
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
