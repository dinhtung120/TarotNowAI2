"use client";

/*
 * ===================================================================
 * COMPONENT: UserSidebar
 * BỐI CẢNH (CONTEXT):
 *   Thanh điều hướng dọc (Sidebar) nằm bên trái màn hình, CHỈ DÀNH cho Desktop.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Giúp chuyển trang cực nhanh giữa các phân hệ: Tarot, Xã hội, Tài khoản.
 *   - Tính toán Logic `getMostSpecificActiveHref` để bật trạng thái Active 
 *     chuẩn xác kể cả khi User đang ở các màn hình con (VD: /reading/history/[id]).
 *   - Hỗ trợ hiển thị Badge báo Noti (Ví dụ: 99+ tin nhắn chưa đọc).
 * ===================================================================
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
  Bell,
  Menu,
  X,
  type LucideIcon,
} from "lucide-react";
import { useMemo, useState, useRef, useEffect } from "react";
import { useTranslations } from "next-intl";
import { cn } from "@/shared/utils/cn";

/**
 * Cấu trúc menu item.
 * `badge` dùng cho notification count (tin nhắn chưa đọc, v.v.)
 */
interface MenuItem {
 labelKey: string;
 href: string;
 icon: LucideIcon;
 badge?: number;
}

/**
 * Nhóm items — mỗi nhóm có label và danh sách items.
 * Nhóm tách biệt bằng divider + label.
 */
interface MenuGroup {
 id: string;
 labelKey: string;
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
  id: "main",
  labelKey: "groups.main",
  items: [{ labelKey: "home", href: "/", icon: Home }],
 },
 {
  id: "tarot",
  labelKey: "groups.tarot",
  items: [
   { labelKey: "readings", href: "/reading", icon: Sparkles },
   { labelKey: "collection", href: "/collection", icon: LayoutGrid },
   { labelKey: "history", href: "/reading/history", icon: History },
  ],
 },
 {
  id: "social",
  labelKey: "groups.social",
  items: [
   { labelKey: "chat", href: "/chat", icon: MessageSquare },
   { labelKey: "readers", href: "/readers", icon: Users },
  ],
 },
 {
  id: "account",
  labelKey: "groups.account",
  items: [
   { labelKey: "wallet", href: "/wallet", icon: Wallet },
   /* Thêm link Thông báo vào nhóm Tài khoản — user truy cập nhanh từ sidebar */
   { labelKey: "notifications", href: "/notifications", icon: Bell },
   { labelKey: "profile", href: "/profile", icon: User },
  ],
 },
];

const matchesPath = (pathname: string, href: string) => {
 if (href === "/") return pathname === "/";
 return pathname === href || pathname.startsWith(`${href}/`);
};

const getMostSpecificActiveHref = (pathname: string) => {
 let activeHref: string | null = null;

 for (const group of menuGroups) {
 for (const item of group.items) {
 if (!matchesPath(pathname, item.href)) continue;
 if (!activeHref || item.href.length > activeHref.length) {
 activeHref = item.href;
 }
 }
 }

 return activeHref;
};

export default function UserSidebar() {
  const pathname = usePathname();
  const activeHref = useMemo(() => getMostSpecificActiveHref(pathname), [pathname]);
  const tNav = useTranslations("Navigation");
  const tUserNav = useTranslations("UserNav");

  // State cho việc mở/đóng menu (dropdown)
  const [isOpen, setIsOpen] = useState(false);
  const sidebarRef = useRef<HTMLDivElement>(null);

  // Đóng sidebar khi người dùng click ra vùng khác bên ngoài menu
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (sidebarRef.current && !sidebarRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };
    if (isOpen) {
      document.addEventListener("mousedown", handleClickOutside);
    }
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [isOpen]);

  // Đóng sidebar nếu URL context (pathname) thay đổi -> người dùng đã chuyển trang
  useEffect(() => {
    setIsOpen(false);
  }, [pathname]);

  return (
    <div ref={sidebarRef} className="hidden md:block fixed top-[4.5rem] left-4 z-50">
      {/* Nút Toggle Menu - Hiển thị menu khi click, dùng glassmorphism */}
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="flex items-center justify-center w-10 h-10 rounded-xl bg-[var(--bg-glass)]/60 border border-[var(--border-subtle)] backdrop-blur-md shadow-lg transition-all duration-300 hover:bg-[var(--bg-elevated)] hover:border-[var(--border-hover)] text-[var(--text-secondary)] hover:text-[var(--text-ink)]"
        aria-label="Toggle Sidebar Menu"
      >
        {isOpen ? (
          <X className="w-5 h-5 transition-transform duration-300 rotate-90" />
        ) : (
          <Menu className="w-5 h-5 transition-transform duration-300" />
        )}
      </button>

      {/* Dropdown Menu Panel - Ẩn/hiện dựa trên `isOpen` state */}
      <div
        className={cn(
          "absolute top-12 left-0 w-64 bg-[var(--bg-overlay)]/95 border border-[var(--border-subtle)] rounded-2xl flex-col backdrop-blur-2xl shadow-[var(--glow-purple-sm)] transition-all duration-300 origin-top-left",
          isOpen ? "flex opacity-100 scale-100 visible" : "opacity-0 scale-95 invisible pointer-events-none"
        )}
      >
        <nav className="flex-1 px-3 py-4 space-y-5 overflow-y-auto max-h-[75vh] custom-scrollbar rounded-2xl">
          {menuGroups.map((group) => (
            <div key={group.id}>
              {/* Group Label */}
              <span className="px-4 mb-2 block text-[9px] font-black uppercase tracking-[0.3em] text-[var(--text-muted)]">
                {tUserNav(group.labelKey)}
              </span>

              {/* Group Items */}
              <div className="space-y-1">
                {group.items.map((item) => {
                  const active = item.href === activeHref;
                  const Icon = item.icon;

                  return (
                    <Link
                      key={item.href}
                      href={item.href}
                      className={cn(
                        "group flex items-center justify-between px-4 py-2.5 rounded-xl transition-all duration-300 relative overflow-hidden min-h-10",
                        active
                          ? "bg-[var(--purple-accent)]/15 text-[var(--text-ink)] border border-[var(--purple-accent)]/30 shadow-[var(--glow-purple-sm)]"
                          : "text-[var(--text-secondary)] hover:bg-[var(--bg-surface-hover)] hover:text-[var(--text-ink)] border border-transparent"
                      )}
                    >
                      <div className="flex items-center gap-3 relative z-10">
                        <Icon
                          className={cn(
                            "w-4 h-4 transition-all duration-300",
                            active
                              ? "text-[var(--purple-accent)] scale-110"
                              : "group-hover:text-[var(--text-ink)]"
                          )}
                        />
                        <span
                          className={cn(
                            "text-[11px] uppercase tracking-widest",
                            active ? "font-black text-[var(--purple-accent)]" : "font-bold"
                          )}
                        >
                          {tNav(item.labelKey)}
                        </span>
                      </div>

                      {/* Badge (notification count) */}
                      {item.badge && item.badge > 0 && (
                        <span className="relative z-10 min-w-[20px] h-5 flex items-center justify-center px-1.5 rounded-full bg-[var(--danger)]/20 text-[var(--danger)] text-[9px] font-black">
                          {item.badge > 99 ? "99+" : item.badge}
                        </span>
                      )}

                      {/* Active indicator bar - Một đường viền nhỏ bên trái nếu tab active */}
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
      </div>
    </div>
  );
}
