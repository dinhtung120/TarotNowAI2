import { CreditCard, History, ShieldCheck, Users } from "lucide-react";
import type { LucideIcon } from "lucide-react";
import type { AdminRoute } from "./types";

interface AdminShortcutItem {
  borderHoverClass: string;
  href: AdminRoute;
  icon: LucideIcon;
  iconHoverClass: string;
  label: string;
}

export function getAdminShortcutItems(
  t: (key: string) => string,
): AdminShortcutItem[] {
  return [
    {
      icon: Users,
      label: t("dashboard.shortcuts.users"),
      href: "/admin/users",
      borderHoverClass: "hover:border-[var(--purple-accent)]/30",
      iconHoverClass: "group-hover:text-[var(--purple-accent)]",
    },
    {
      icon: CreditCard,
      label: t("dashboard.shortcuts.deposits"),
      href: "/admin/deposits",
      borderHoverClass: "hover:border-[var(--success)]/30",
      iconHoverClass: "group-hover:text-[var(--success)]",
    },
    {
      icon: History,
      label: t("dashboard.shortcuts.readings"),
      href: "/admin/readings",
      borderHoverClass: "hover:border-[var(--warning)]/30",
      iconHoverClass: "group-hover:text-[var(--warning)]",
    },
    {
      icon: ShieldCheck,
      label: t("dashboard.shortcuts.reader_requests"),
      href: "/admin/reader-requests",
      borderHoverClass: "hover:border-[var(--danger)]/30",
      iconHoverClass: "group-hover:text-[var(--danger)]",
    },
  ];
}
