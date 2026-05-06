"use client";

import { useEffect, useMemo, useRef, useState } from "react";
import { usePathname } from "@/i18n/routing";
import { bottomTabs, matchesPath, type BottomTabGroup } from "./config";

export function useBottomTabBarState() {
 const pathname = usePathname();
 const [activeMenu, setActiveMenu] = useState<string | null>(null);
 const menuRef = useRef<HTMLDivElement>(null);
 const navRef = useRef<HTMLElement>(null);

 useEffect(() => {
  const handleClickOutside = (event: MouseEvent) => {
   const target = event.target as Node;
   if (menuRef.current?.contains(target)) return;
   if (navRef.current?.contains(target) && (target as Element).closest("button, a")) return;
   setActiveMenu(null);
  };
  document.addEventListener("mousedown", handleClickOutside);
  return () => document.removeEventListener("mousedown", handleClickOutside);
 }, []);

 const activeTabId = useMemo(() => {
  for (const tab of bottomTabs) {
   if (tab.id === "home" && matchesPath(pathname, "/")) return tab.id;
   if (tab.subItems?.some((sub) => sub.matchPrefixes.some((prefix) => matchesPath(pathname, prefix)))) return tab.id;
  }
  return null;
 }, [pathname]);

 const activeSubItems = useMemo(() => bottomTabs.find((tab) => tab.id === activeMenu)?.subItems || null, [activeMenu]);

 const handleToggleMenu = (tab: BottomTabGroup) => {
  if (!tab.subItems) {
   setActiveMenu(null);
   return;
  }
  setActiveMenu((current) => (current === tab.id ? null : tab.id));
 };

 return {
  pathname,
  navRef,
  menuRef,
  activeMenu,
  activeTabId,
  activeSubItems,
  tabs: bottomTabs,
  setActiveMenu,
  handleToggleMenu,
 };
}
