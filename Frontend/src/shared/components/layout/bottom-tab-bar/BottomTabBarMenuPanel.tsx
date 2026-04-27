import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from "@/lib/utils";
import type { RefObject } from "react";
import type { BottomTabLinkHref, BottomTabSubItem } from "./config";

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

  const activeSubHref = activeSubItems.reduce<string | null>(
    (currentActiveHref, sub) => {
      let bestMatchLength = -1;
      for (const prefix of sub.matchPrefixes) {
        if (!matchesPath(pathname, prefix)) continue;
        if (prefix.length > bestMatchLength) bestMatchLength = prefix.length;
      }
      if (bestMatchLength < 0) return currentActiveHref;

      if (!currentActiveHref) return sub.href;

      const current = activeSubItems.find(
        (item) => item.href === currentActiveHref,
      );
      if (!current) return sub.href;

      let currentBestMatchLength = -1;
      for (const prefix of current.matchPrefixes) {
        if (!matchesPath(pathname, prefix)) continue;
        if (prefix.length > currentBestMatchLength)
          currentBestMatchLength = prefix.length;
      }

      if (bestMatchLength > currentBestMatchLength) return sub.href;
      if (
        bestMatchLength === currentBestMatchLength &&
        sub.href.length > current.href.length
      )
        return sub.href;
      return currentActiveHref;
    },
    null,
  );

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
          const SubIcon = sub.icon;
          const isSubActive = sub.href === activeSubHref;
          return (
            <Link
              key={sub.href}
              href={sub.href as BottomTabLinkHref}
              onClick={onSelect}
              className={cn(
                "flex items-center gap-3 rounded-2xl p-3 transition-all",
                isSubActive
                  ? "border border-[var(--purple-100)] bg-[var(--purple-50)] text-[var(--purple-accent)]"
                  : "border border-transparent bg-transparent text-[var(--text-secondary)] hover:bg-[var(--bg-surface-hover)]",
              )}
            >
              <SubIcon
                className={cn(
                  "h-5 w-5",
                  isSubActive
                    ? "text-[var(--purple-accent)]"
                    : "text-[var(--text-muted)]",
                )}
              />
              <span
                className={cn(
                  "tn-text-13 tracking-wide",
                  isSubActive ? "font-black" : "font-semibold",
                )}
              >
                {tNav(sub.labelKey)}
              </span>
            </Link>
          );
        })}
      </div>
    </div>
  );
}
