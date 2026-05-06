"use client";

import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";
import { useChatUnreadNotifications } from "@/features/chat/inbox/hooks/useChatUnreadNotifications";
import BottomTabBarMainTabs from "@/shared/app-shell/navigation/bottom-tab-bar/BottomTabBarMainTabs";
import BottomTabBarMenuPanel from "@/shared/app-shell/navigation/bottom-tab-bar/BottomTabBarMenuPanel";
import { matchesPath } from "@/shared/app-shell/navigation/bottom-tab-bar/config";
import { useBottomTabBarState } from "@/shared/app-shell/navigation/bottom-tab-bar/useBottomTabBarState";

export default function BottomTabBar() {
 const tNav = useTranslations("Navigation");
 const { unreadCount } = useChatUnreadNotifications();
 const { pathname, navRef, menuRef, activeMenu, activeTabId, activeSubItems, tabs, setActiveMenu, handleToggleMenu } = useBottomTabBarState();

 return (
  <nav ref={navRef} className={cn("tn-hide-md fixed bottom-0 left-0 right-0 z-50 tn-bg-glass border-t tn-border-soft tn-bottom-safe-area tn-shadow-top-nav")}>
   <div className={cn("relative w-full")}>
    <BottomTabBarMenuPanel activeMenu={activeMenu} activeSubItems={activeSubItems} pathname={pathname} tNav={tNav} menuRef={menuRef} onSelect={() => setActiveMenu(null)} matchesPath={matchesPath} />
    <BottomTabBarMainTabs tabs={tabs} activeTabId={activeTabId} activeMenu={activeMenu} unreadChatCount={unreadCount} tNav={tNav} onToggleMenu={handleToggleMenu} />
   </div>
  </nav>
 );
}
