import { cn } from "@/lib/utils";
import type { RefObject } from "react";
import type { BottomTabSubItem } from "./config";
import { BottomTabBarMenuItem } from './BottomTabBarMenuItem';
import { getActiveBottomTabSubHref } from './getActiveBottomTabSubHref';

interface BottomTabBarMenuPanelProps {
  activeMenu: string | null;
  activeSubItems: BottomTabSubItem[] | null;
  pathname: string;
  tNav: (key: string) => string;
  menuRef: RefObject<HTMLDivElement | null>;
  onSelect: () => void;
  matchesPath: (candidatePath: string, prefix: string) => boolean;
}

export default function BottomTabBarMenuPanel({
  activeMenu,
  activeSubItems,
  pathname,
  tNav,
  menuRef,
  onSelect,
  matchesPath,
}: BottomTabBarMenuPanelProps) {
  if (!activeSubItems) return null;

  const activeSubHref = getActiveBottomTabSubHref(activeSubItems, pathname, matchesPath);

  return (
    <div
      ref={menuRef}
      className={cn(
        "animate-in slide-in-from-bottom-2 absolute bottom-full z-[60] mb-3 min-w-[150px] rounded-3xl border border-[var(--border-subtle)] bg-[var(--bg-glass)] p-3 shadow-[0_12px_40px_rgba(0,0,0,0.18)] backdrop-blur-2xl duration-200",
        activeMenu === "tarot" && "left-[30%] -translate-x-1/2",
        activeMenu === "social" && "left-1/2 -translate-x-1/2",
        activeMenu === "game" && "left-[70%] -translate-x-1/2",
        activeMenu === "account" && "right-3",
      )}
    >
      <div className={cn("flex flex-col gap-1.5")}>
        {activeSubItems.map((sub) => {
          const isSubActive = sub.href === activeSubHref;
          return <BottomTabBarMenuItem key={sub.href} isActive={isSubActive} onSelect={onSelect} sub={sub} tNav={tNav} />;
        })}
      </div>
    </div>
  );
}
