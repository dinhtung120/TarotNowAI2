"use client";
import { jsx as t, jsxs as o } from "react/jsx-runtime";
import {
  ChevronRight as i,
  CreditCard as c,
  History as d,
  ShieldCheck as l,
  Users as h,
  Zap as v,
} from "lucide-react";
import { useTranslations as u } from "next-intl";
import { cn as r } from "@/lib/utils";
function f({ onNavigate: a }) {
  const s = u("Admin"),
    n = [
      {
        icon: h,
        label: s("dashboard.shortcuts.users"),
        href: "/admin/users",
        borderHoverClass: "hover:border-[var(--purple-accent)]/30",
        iconHoverClass: "group-hover:text-[var(--purple-accent)]",
      },
      {
        icon: c,
        label: s("dashboard.shortcuts.deposits"),
        href: "/admin/deposits",
        borderHoverClass: "hover:border-[var(--success)]/30",
        iconHoverClass: "group-hover:text-[var(--success)]",
      },
      {
        icon: d,
        label: s("dashboard.shortcuts.readings"),
        href: "/admin/readings",
        borderHoverClass: "hover:border-[var(--warning)]/30",
        iconHoverClass: "group-hover:text-[var(--warning)]",
      },
      {
        icon: l,
        label: s("dashboard.shortcuts.reader_requests"),
        href: "/admin/reader-requests",
        borderHoverClass: "hover:border-[var(--danger)]/30",
        iconHoverClass: "group-hover:text-[var(--danger)]",
      },
    ];
  return o("div", {
    className: r("space-y-6 text-left"),
    children: [
      o("h2", {
        className: r(
          "text-xl font-black tn-text-primary uppercase italic tracking-tighter flex items-center gap-3 drop-shadow-md",
        ),
        children: [
          t(v, { className: r("w-5 h-5 text-[var(--success)]") }),
          s("dashboard.shortcuts.title"),
        ],
      }),
      t("div", {
        className: r("space-y-4"),
        children: n.map((e) =>
          o(
            "button",
            {
              type: "button",
              onClick: () => a(e.href),
              className: r(
                "w-full flex items-center justify-between p-5 rounded-2xl tn-panel-soft hover:tn-surface-strong transition-all group shadow-sm hover:shadow-md",
                e.borderHoverClass,
              ),
              children: [
                o("div", {
                  className: r("flex items-center gap-4"),
                  children: [
                    t(e.icon, {
                      className: r(
                        "w-5 h-5 text-[var(--text-tertiary)] transition-colors",
                        e.iconHoverClass,
                      ),
                    }),
                    t("span", {
                      className: r(
                        "text-xs font-black uppercase tracking-widest text-[var(--text-secondary)] group-hover:tn-text-primary transition-colors",
                      ),
                      children: e.label,
                    }),
                  ],
                }),
                t(i, {
                  className: r(
                    "w-4 h-4 tn-text-muted transition-colors",
                    e.iconHoverClass,
                  ),
                }),
              ],
            },
            e.href,
          ),
        ),
      }),
    ],
  });
}
export { f as AdminDashboardShortcuts };
