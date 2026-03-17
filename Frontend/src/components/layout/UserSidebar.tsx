"use client";

/**
 * UserSidebar — Thanh điều hướng bên trái cho khu vực người dùng.
 *
 * === TẠI SAO CẦN SIDEBAR? ===
 * Trước đây: User muốn đi từ Wallet → Chat → Profile → History
 * → Phải click vào Navbar nhỏ ở trên cùng, các link bị ẩn trên mobile.
 * → Trải nghiệm "rời rạc" — mỗi trang là một ốc đảo riêng.
 *
 * === GIẢI PHÁP ===
 * Sidebar cố định bên trái (desktop) với các nhóm navigation rõ ràng:
 * - Nhóm 1 (Tarot): Rút Bài, Miếu Bài, Lịch Sử
 * - Nhóm 2 (Xã Hội): Tin Nhắn, Tìm Reader
 * - Nhóm 3 (Tài Khoản): Ví, Hồ Sơ
 *
 * Trên Mobile (< md): Sidebar ẨN → thay bằng BottomTabBar.
 *
 * === STYLING ===
 * Sidebar surface tối, phù hợp theme huyền bí mới.
 * Active item: Purple highlight + left indicator bar + glow.
 */

import { Link, usePathname } from "@/i18n/routing";
import {
  Sparkles,
  LayoutGrid,
  History,
  MessageSquare,
  Users,
  Wallet,
  User,
  Home,
  type LucideIcon,
} from "lucide-react";

/**
 * Cấu trúc menu item.
 * `badge` dùng cho notification count (tin nhắn chưa đọc, v.v.)
 */
interface MenuItem {
  name: string;
  href: string;
  icon: LucideIcon;
  badge?: number;
}

/**
 * Nhóm items — mỗi nhóm có label và danh sách items.
 * Nhóm tách biệt bằng divider + label.
 */
interface MenuGroup {
  label: string;
  items: MenuItem[];
}

/**
 * Định nghĩa menu groups.
 * Thứ tự: Tổng quan → Tarot → Xã hội → Tài khoản
 *
 * Tại sao Tổng quan (Home) nằm riêng?
 * → Home page khác biệt: public, có Footer, không cần sidebar.
 * → Nhưng user cần link quay về Home từ bất kỳ đâu.
 */
const menuGroups: MenuGroup[] = [
  {
    label: "Chính",
    items: [
      { name: "Trang Chủ", href: "/", icon: Home },
    ],
  },
  {
    label: "Tarot",
    items: [
      { name: "Rút Bài", href: "/reading", icon: Sparkles },
      { name: "Miếu Bài", href: "/collection", icon: LayoutGrid },
      { name: "Lịch Sử", href: "/reading/history", icon: History },
    ],
  },
  {
    label: "Xã Hội",
    items: [
      { name: "Tin Nhắn", href: "/chat", icon: MessageSquare },
      { name: "Tìm Reader", href: "/readers", icon: Users },
    ],
  },
  {
    label: "Tài Khoản",
    items: [
      { name: "Ví", href: "/wallet", icon: Wallet },
      { name: "Hồ Sơ", href: "/profile", icon: User },
    ],
  },
];

export default function UserSidebar() {
  const pathname = usePathname();

  /**
   * Logic xác định item active.
   * → Home ("/") chỉ active khi pathname CHÍNH XÁC là "/".
   * → Các route khác: active nếu pathname BẮT ĐẦU với href.
   *   Ví dụ: `/wallet/deposit` vẫn highlight "Ví" (`/wallet`).
   */
  const isActive = (href: string) => {
    if (href === "/") return pathname === "/";
    return pathname.startsWith(href);
  };

  return (
    <aside className="hidden md:flex relative z-20 w-64 h-full bg-[var(--bg-surface)] border-r border-[var(--border-subtle)] flex-col">
      {/* Navigation Groups */}
      <nav className="flex-1 px-3 py-6 space-y-6 overflow-y-auto">
        {menuGroups.map((group) => (
          <div key={group.label}>
            {/* Group Label */}
            <span className="px-4 mb-2 block text-[9px] font-black uppercase tracking-[0.3em] text-zinc-700">
              {group.label}
            </span>

            {/* Group Items */}
            <div className="space-y-1">
              {group.items.map((item) => {
                const active = isActive(item.href);
                const Icon = item.icon;

                return (
                  <Link
                    key={item.href}
                    href={item.href}
                    className={[
                      "group flex items-center justify-between px-4 py-3 rounded-2xl transition-all duration-300 relative overflow-hidden",
                      active
                        ? "bg-[var(--bg-elevated)] text-[var(--text-primary)] border border-[var(--border-hover)] shadow-[var(--glow-purple-sm)]"
                        : "text-[var(--text-secondary)] hover:bg-[var(--bg-surface-hover)] hover:text-[var(--text-primary)] border border-transparent",
                    ].join(" ")}
                  >
                    <div className="flex items-center gap-3 relative z-10">
                      <Icon
                        className={[
                          "w-4 h-4 transition-all duration-300",
                          active
                            ? "text-[var(--purple-accent)] scale-110"
                            : "group-hover:text-zinc-300",
                        ].join(" ")}
                      />
                      <span
                        className={[
                          "text-[11px] uppercase tracking-widest",
                          active ? "font-black" : "font-bold",
                        ].join(" ")}
                      >
                        {item.name}
                      </span>
                    </div>

                    {/* Badge (notification count) */}
                    {item.badge && item.badge > 0 && (
                      <span className="relative z-10 min-w-[20px] h-5 flex items-center justify-center px-1.5 rounded-full bg-red-500/20 text-red-400 text-[9px] font-black">
                        {item.badge > 99 ? "99+" : item.badge}
                      </span>
                    )}

                    {/* Active indicator bar — thanh tím bên trái */}
                    {active && (
                      <div className="absolute left-0 top-1/4 bottom-1/4 w-1 bg-[var(--purple-accent)] rounded-r-full shadow-[0_0_10px_var(--purple-accent)]" />
                    )}
                  </Link>
                );
              })}
            </div>
          </div>
        ))}
      </nav>
    </aside>
  );
}
