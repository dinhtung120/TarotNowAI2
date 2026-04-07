"use client";

import { ChevronRight, Zap } from "lucide-react";
import { useTranslations } from "next-intl";
import { getAdminShortcutItems } from "@/features/admin/dashboard/presentation/components/adminDashboardShortcuts.items";
import type { AdminRoute } from "@/features/admin/dashboard/presentation/components/types";
import { cn } from "@/lib/utils";

interface AdminDashboardShortcutsProps {
  onNavigate: (path: AdminRoute) => void;
}

export function AdminDashboardShortcuts({
  onNavigate,
}: AdminDashboardShortcutsProps) {
  const t = useTranslations("Admin");
  const shortcuts = getAdminShortcutItems(t);

  return (
    <div className={cn("space-y-6 text-left")}>
      <h2
        className={cn(
          "tn-text-primary flex items-center gap-3 text-xl font-black tracking-tighter uppercase italic drop-shadow-md",
        )}
      >
        <Zap className={cn("h-5 w-5 text-[var(--success)]")} />
        {t("dashboard.shortcuts.title")}
      </h2>
      <div className={cn("space-y-4")}>
        {shortcuts.map((shortcut) => (
          <button
            key={shortcut.href}
            type="button"
            onClick={() => onNavigate(shortcut.href)}
            className={cn(
              "group tn-panel-soft hover:tn-surface-strong flex w-full items-center justify-between rounded-2xl p-5 shadow-sm transition-all hover:shadow-md",
              shortcut.borderHoverClass,
            )}
          >
            <div className={cn("flex items-center gap-4")}>
              <shortcut.icon
                className={cn(
                  "h-5 w-5 text-[var(--text-tertiary)] transition-colors",
                  shortcut.iconHoverClass,
                )}
              />
              <span
                className={cn(
                  "group-hover:tn-text-primary text-xs font-black tracking-widest text-[var(--text-secondary)] uppercase transition-colors",
                )}
              >
                {shortcut.label}
              </span>
            </div>
            <ChevronRight
              className={cn(
                "tn-text-muted h-4 w-4 transition-colors",
                shortcut.iconHoverClass,
              )}
            />
          </button>
        ))}
      </div>
    </div>
  );
}
