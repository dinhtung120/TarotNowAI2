"use client";

/*
 * ===================================================================
 * COMPONENT: BottomTabBar
 * BỐI CẢNH (CONTEXT):
 *   Thanh điều hướng ở mép dưới màn hình, CHỈ DÀNH cho giao diện Mobile (< md).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Gom nhóm chức năng thành 4 tab lớn (Home, Tarot, Xã hội, Tài khoản) để tránh quá tải UI.
 *   - 3 tab (Tarot, Xã hội, Tài khoản) sẽ hiển thị menu chức năng con theo dạng popup.
 *   - Cập nhật trạng thái active theo cả menu cha và menu con.
 * ===================================================================
 */

import { Link, usePathname } from "@/i18n/routing";
import {
  Home,
  Sparkles,
  MessageSquare,
  Wallet,
  Users,
  User,
  History,
  Bookmark,
  Bell,
  Gamepad2,
  Gift,
  type LucideIcon,
} from "lucide-react";
import { useTranslations } from "next-intl";
import { cn } from "@/shared/utils/cn";
import { useMemo, useState, useRef, useEffect, type ComponentProps } from "react";

interface SubItem {
  labelKey: string;
  href: string;
  icon: LucideIcon;
  matchPrefixes: string[];
}

interface TabGroup {
  id: string;
  labelKey: string;
  icon: LucideIcon;
  href?: string; // Chỉ dành cho nút ko có tính năng mở rộng (như Trang chủ)
  matchPrefixes: string[];
  subItems?: SubItem[];
}

type LinkHref = ComponentProps<typeof Link>["href"];

/*
 * Cấu trúc 4 tab chính, nhóm các module bên trong subItems để tiết kiệm không gian
 */
const tabs: TabGroup[] = [
  {
    id: "home",
    labelKey: "home",
    href: "/",
    icon: Home,
    matchPrefixes: ["/"],
  },
  {
    id: "tarot",
    labelKey: "tarot",
    icon: Sparkles,
    matchPrefixes: ["/reading", "/collection", "/gamification", "/gacha"],
    subItems: [
      { labelKey: "readings", href: "/reading", icon: Sparkles, matchPrefixes: ["/reading"] },
      { labelKey: "collection", href: "/collection", icon: Bookmark, matchPrefixes: ["/collection"] },
      { labelKey: "history", href: "/reading/history", icon: History, matchPrefixes: ["/reading/history"] },
      { labelKey: "gamification", href: "/gamification", icon: Gamepad2, matchPrefixes: ["/gamification"] },
      { labelKey: "gacha", href: "/gacha", icon: Gift, matchPrefixes: ["/gacha"] },
    ],
  },
  {
    id: "social",
    labelKey: "social",
    icon: Users,
    matchPrefixes: ["/chat", "/readers", "/community"],
    subItems: [
      { labelKey: "community", href: "/community", icon: Sparkles, matchPrefixes: ["/community"] },
      { labelKey: "chat", href: "/chat", icon: MessageSquare, matchPrefixes: ["/chat"] },
      { labelKey: "readers", href: "/readers", icon: Users, matchPrefixes: ["/readers"] },
    ],
  },
  {
    id: "account",
    labelKey: "account",
    icon: User,
    matchPrefixes: ["/wallet", "/profile", "/notifications", "/reader"],
    subItems: [
      { labelKey: "wallet", href: "/wallet", icon: Wallet, matchPrefixes: ["/wallet"] },
      { labelKey: "notifications", href: "/notifications", icon: Bell, matchPrefixes: ["/notifications"] },
      { labelKey: "profile", href: "/profile", icon: User, matchPrefixes: ["/profile", "/reader"] },
    ],
  },
];

export default function BottomTabBar() {
  const pathname = usePathname();
  const t = useTranslations("Navigation");
  // Lưu state để biết popup menu của tab nào đang mở
  const [activeMenu, setActiveMenu] = useState<string | null>(null);
  const menuRef = useRef<HTMLDivElement>(null);
  const navRef = useRef<HTMLElement>(null);

  /** Đóng menu nếu click ra ngoài khu vực menu hoặc bấm vào khoảng trống trên navbar */
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      const target = e.target as Node;
      
      // Bỏ qua nếu click bên trong menu popup đang mở
      if (menuRef.current && menuRef.current.contains(target)) {
        return;
      }

      // Kiểm tra nếu click vào khu vực thanh Navbar
      if (navRef.current && navRef.current.contains(target)) {
        // Nếu click trúng vào 1 thẻ button hay Link, thì bỏ qua để cho sự kiện onClick của nút đó tự Toggle (mở/đóng)
        if ((target as Element).closest("button, a")) {
          return;
        }
      }

      // Nếu click ra ngoài hoàn toàn, hoặc click vào khoảng trống của Navbar -> Đóng Menu
      setActiveMenu(null);
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const matchesPath = (candidatePath: string, prefix: string) => {
    if (prefix === "/") return candidatePath === "/";
    return candidatePath === prefix || candidatePath.startsWith(`${prefix}/`);
  };

  /** Xác định tab chính nào đang active dựa trên đường dẫn hiện tại */
  const activeTabId = useMemo(() => {
    for (const tab of tabs) {
      if (tab.id === "home" && matchesPath(pathname, "/")) return tab.id;

      if (tab.subItems) {
        const isSubMatch = tab.subItems.some((sub) =>
          sub.matchPrefixes.some((p) => matchesPath(pathname, p))
        );
        if (isSubMatch) return tab.id;
      }
    }
    return null;
  }, [pathname]);

  const activeSubItems = tabs.find((t) => t.id === activeMenu)?.subItems || null;

  return (
    <nav ref={navRef} className="md:hidden fixed bottom-0 left-0 right-0 z-50 bg-[var(--bg-glass)] border-t border-[var(--border-subtle)] pb-[env(safe-area-inset-bottom)] shadow-[0_-10px_26px_var(--c-168-156-255-12)]">
      <div className="relative w-full">
        {/* Overylay Panel chứa các thẻ con */}
        {activeSubItems && (
          <div
            ref={menuRef}
            className={cn(
              "absolute bottom-full mb-3 p-3 bg-[var(--bg-glass)] backdrop-blur-2xl border border-[var(--border-subtle)] rounded-3xl shadow-[0_12px_40px_rgba(0,0,0,0.18)] animate-in slide-in-from-bottom-2 duration-200 z-[60] min-w-[150px]",
              activeMenu === "tarot" && "left-[37.5%] -translate-x-1/2",
              activeMenu === "social" && "left-[62.5%] -translate-x-1/2",
              activeMenu === "account" && "right-3"
            )}
            style={{ WebkitBackdropFilter: "blur(20px)" }}
          >
            {/* Menu con hiển thị 1 hàng dọc, ngay trên vị trí nút */}
            <div className="flex flex-col gap-1.5">
              {activeSubItems.map((sub) => {
                const SubIcon = sub.icon;
                const isSubActive = sub.matchPrefixes.some((p) =>
                  matchesPath(pathname, p)
                );

                return (
                  <Link
                    key={sub.href}
                    href={sub.href as LinkHref}
                    onClick={() => setActiveMenu(null)}
                    className={cn(
                      "flex items-center gap-3 p-3 rounded-2xl transition-all",
                      isSubActive
                        ? "bg-[var(--purple-50)] text-[var(--purple-accent)] border border-[var(--purple-100)]"
                        : "bg-transparent hover:bg-[var(--bg-surface-hover)] text-[var(--text-secondary)] border border-transparent"
                    )}
                  >
                    <SubIcon
                      className={cn(
                        "w-5 h-5",
                        isSubActive
                          ? "text-[var(--purple-accent)]"
                          : "text-[var(--text-muted)]"
                      )}
                    />
                    <span
                      className={cn(
                        "text-[13px] tracking-wide",
                        isSubActive ? "font-black" : "font-semibold"
                      )}
                    >
                      {t(sub.labelKey)}
                    </span>
                  </Link>
                );
              })}
            </div>
          </div>
      )}

      {/* 4 Nút chức năng chính trên thanh menu Bottom */}
      <div className="flex items-stretch justify-around gap-1 px-2 py-2">
        {tabs.map((tab) => {
          const isMainActive = activeTabId === tab.id;
          const isOpen = activeMenu === tab.id;
          const Icon = tab.icon;

          const handleClick = (e: React.MouseEvent) => {
            if (tab.subItems) {
              e.preventDefault();
              // Nếu click lại đúng tab đang mở thì đóng nó đi, ngược lại mở nó ra
              setActiveMenu(isOpen ? null : tab.id);
              return;
            }

            setActiveMenu(null);
          };

          const content = (
            <>
              <Icon
                className={cn(
                  "w-[22px] h-[22px] transition-transform duration-300",
                  isMainActive || isOpen ? "scale-110" : "scale-100"
                )}
              />
              <span
                className={cn(
                  "text-[9px] uppercase tracking-wider truncate mt-1",
                  isMainActive || isOpen ? "font-black" : "font-bold"
                )}
              >
                {t(tab.labelKey)}
              </span>

              {/* Chấm tròn biểu thị menu đang active */}
              {isMainActive && (
                <div className="absolute top-1.5 right-3 w-1.5 h-1.5 rounded-full bg-[var(--purple-accent)] shadow-[0_0_6px_var(--purple-accent)]" />
              )}
            </>
          );

          const classNameProps = cn(
            "relative flex-1 min-w-0 flex flex-col items-center justify-center py-2.5 rounded-2xl transition-all duration-300 min-h-[52px]",
            isMainActive || isOpen
              ? "text-[var(--purple-muted)] bg-[var(--purple-50)] border border-[var(--purple-100)] shadow-sm"
              : "text-[var(--text-muted)] active:text-[var(--text-secondary)] border border-transparent"
          );

          // Nếu tab có href, ta dùng thẻ Link (Ví dụ: tab Trang chủ)
          if (tab.href) {
            return (
              <Link
                key={tab.id}
                href={tab.href as LinkHref}
                onClick={handleClick}
                className={classNameProps}
              >
                {content}
              </Link>
            );
          }

          // Ngược lại nếu có subItems, dùng thẻ button để mở menu popup
          return (
            <button
              key={tab.id}
              onClick={handleClick}
              className={classNameProps}
            >
              {content}
            </button>
          );
        })}
      </div>
      </div>
    </nav>
  );
}
