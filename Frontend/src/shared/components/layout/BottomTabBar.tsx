"use client";

import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";
import BottomTabBarMainTabs from "./bottom-tab-bar/BottomTabBarMainTabs";
import BottomTabBarMenuPanel from "./bottom-tab-bar/BottomTabBarMenuPanel";
import { matchesPath } from "./bottom-tab-bar/config";
import { useBottomTabBarState } from "./bottom-tab-bar/useBottomTabBarState";

export default function BottomTabBar() {
 const tNav = useTranslations("Navigation");
 const { pathname, navRef, menuRef, activeMenu, activeTabId, activeSubItems, tabs, setActiveMenu, handleToggleMenu } = useBottomTabBarState();

 return (
  <nav ref={navRef} className={cn("md:hidden fixed bottom-0 left-0 right-0 z-50 bg-[var(--bg-glass)] border-t border-[var(--border-subtle)] pb-[env(safe-area-inset-bottom)] shadow-[0_-10px_26px_var(--c-168-156-255-12)]")}>
   <div className={cn("relative w-full")}>
    <BottomTabBarMenuPanel activeMenu={activeMenu} activeSubItems={activeSubItems} pathname={pathname} tNav={tNav} menuRef={menuRef} onSelect={() => setActiveMenu(null)} matchesPath={matchesPath} />
    <BottomTabBarMainTabs tabs={tabs} activeTabId={activeTabId} activeMenu={activeMenu} tNav={tNav} onToggleMenu={handleToggleMenu} />
   </div>
  </nav>
 );
}
