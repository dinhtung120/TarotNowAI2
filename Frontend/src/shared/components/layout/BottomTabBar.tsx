"use client";

/*
 * ===================================================================
 * COMPONENT: BottomTabBar
 * BỐI CẢNH (CONTEXT):
 *   Thanh điều hướng ở mép dưới màn hình, CHỈ DÀNH cho giao diện Mobile (< md).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Thay thế cho UserSidebar trên điện thoại, tiết kiệm diện tích bề ngang.
 *   - Chứa 5 nút điều hướng quan trọng nhất (Home, Tarot, Chat, Wallet, Profile) 
 *     theo chuẩn thiết kế Mobile App (Ví dụ: Instagram, TikTok).
 *   - Xử lý hiệu ứng Active State (Nổi màu tím + icon bự lên) khi User đang ở trang đó.
 *   - Có `safe-area-inset-bottom` để tránh bị cấn thanh Hone Indicator trên iPhone đời mới.
 * ===================================================================
 */

import { Link, usePathname } from "@/i18n/routing";
import { Home, Sparkles, MessageSquare, Wallet, Bell, type LucideIcon } from "lucide-react";
import { useTranslations } from "next-intl";
import { cn } from "@/shared/utils/cn";

interface TabItem {
 labelKey: string;
 href: string;
 icon: LucideIcon;
}

/*
 * Bottom Tab Bar: 5 tabs chính cho mobile.
 * Thay Profile bằng Notifications vì:
 * - Mobile cần truy cập thông báo nhanh (icon chuông quen thuộc).
 * - Profile ít truy cập hơn, user có thể vào từ sidebar hoặc settings.
 * - Instagram, TikTok cũng đặt notification ở bottom bar.
 */
const tabs: TabItem[] = [
 { labelKey: "home", href: "/", icon: Home },
 { labelKey: "readings", href: "/reading", icon: Sparkles },
 { labelKey: "chat", href: "/chat", icon: MessageSquare },
 { labelKey: "wallet", href: "/wallet", icon: Wallet },
 { labelKey: "notifications", href: "/notifications", icon: Bell },
];

export default function BottomTabBar() {
 const pathname = usePathname();
 const t = useTranslations("Navigation");

 const isActive = (href: string) => {
 if (href === "/") return pathname === "/";
 return pathname.startsWith(href);
 };

 return (
 /**
 * md:hidden → Chỉ hiện trên mobile, ẩn trên desktop.
 * fixed bottom-0 → Dính ở dưới cùng màn hình.
 * z-50 → Nổi trên mọi content.
 * safe-area-inset → Hỗ trợ notch/home indicator trên iPhone.
 */
 <nav className="md:hidden fixed bottom-0 left-0 right-0 z-50 bg-[var(--bg-glass)] border-t border-[var(--border-subtle)] pb-[env(safe-area-inset-bottom)] shadow-[0_-10px_26px_var(--c-168-156-255-12)]">
 <div className="flex items-center justify-between px-1 sm:px-2 py-2">
 {tabs.map((tab) => {
 const active = isActive(tab.href);
 const Icon = tab.icon;

 return (
 <Link
 key={tab.href}
 href={tab.href}
 className={cn(
 "flex-1 min-w-0 flex flex-col items-center justify-center gap-1 px-1.5 sm:px-3 py-2 rounded-xl transition-all duration-300 min-h-11",
 active
 ? "text-[var(--purple-muted)]"
 : "text-[var(--text-muted)] active:text-[var(--text-secondary)]",
 )}
 >
 <Icon
 className={cn(
 "w-5 h-5 transition-transform duration-300",
 active ? "scale-110" : "",
 )}
 />
	 <span
 className={cn(
 "text-[9px] uppercase tracking-wider truncate",
 active ? "font-black" : "font-bold",
 )}
	 >
	 {t(tab.labelKey)}
	 </span>

 {/* Active dot indicator */}
 {active && (
 <div className="w-1 h-1 rounded-full bg-[var(--purple-accent)] shadow-[0_0_6px_var(--purple-accent)]" />
 )}
 </Link>
 );
 })}
 </div>
 </nav>
 );
}
