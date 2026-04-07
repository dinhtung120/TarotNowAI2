"use client";
import { jsx as t, jsxs as s } from "react/jsx-runtime";
import {
  BookOpen as c,
  CheckCircle2 as o,
  Clock as d,
  Hash as l,
  User as p,
} from "lucide-react";
import { useLocale as m, useTranslations as x } from "next-intl";
import { cn as e } from "@/lib/utils";
function f({ reading: a, getSpreadLabel: n }) {
  const r = x("Admin"),
    i = m();
  return s("tr", {
    className: e("group/row hover:tn-surface transition-colors"),
    children: [
      t("td", {
        className: e("px-8 py-6 whitespace-nowrap"),
        children: s("div", {
          className: e("flex flex-col text-left"),
          children: [
            t("div", {
              className: e(
                "text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter italic",
              ),
              children: new Date(a.createdAt).toLocaleDateString(i),
            }),
            t("div", {
              className: e(
                "text-[10px] font-bold text-[var(--text-tertiary)] italic",
              ),
              children: new Date(a.createdAt).toLocaleTimeString(i),
            }),
          ],
        }),
      }),
      t("td", {
        className: e("px-8 py-6"),
        children: s("div", {
          className: e("flex items-center gap-4"),
          children: [
            t("div", {
              className: e(
                "w-10 h-10 rounded-[1rem] tn-panel-overlay flex items-center justify-center shadow-inner group-hover/row:scale-110 transition-transform",
              ),
              children: t(p, {
                className: e("w-4 h-4 text-[var(--text-secondary)]"),
              }),
            }),
            s("div", {
              children: [
                t("div", {
                  className: e(
                    "text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm",
                  ),
                  children: a.username || r("readings.row.user_unknown"),
                }),
                s("div", {
                  className: e(
                    "text-[9px] font-bold text-[var(--text-tertiary)] uppercase tracking-tighter flex items-center gap-1 mt-0.5",
                  ),
                  children: [
                    t(l, { className: e("w-2.5 h-2.5 opacity-50") }),
                    a.userId.split("-")[0],
                    "...",
                  ],
                }),
              ],
            }),
          ],
        }),
      }),
      t("td", {
        className: e("px-8 py-6"),
        children: s("div", {
          className: e(
            "inline-flex items-center gap-2 px-3 py-1.5 rounded-lg tn-panel-soft text-[10px] font-black tn-text-primary uppercase tracking-widest italic text-left shadow-inner",
          ),
          children: [
            t(c, { className: e("w-3.5 h-3.5 text-[var(--purple-accent)]") }),
            n(a.spreadType),
          ],
        }),
      }),
      t("td", {
        className: e("px-8 py-6 max-w-[200px] truncate text-left"),
        children: t("p", {
          className: e(
            "text-[11px] font-bold text-[var(--text-secondary)] italic uppercase leading-relaxed tracking-tight text-left",
          ),
          children: a.question || r("readings.row.question_empty"),
        }),
      }),
      t("td", {
        className: e("px-8 py-6 text-center"),
        children: s("div", {
          className: e(
            "inline-flex items-center gap-2 px-3 py-1.5 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all shadow-inner",
            a.isCompleted
              ? "bg-[var(--success)]/10 border-[var(--success)]/30 text-[var(--success)] shadow-md"
              : "bg-[var(--accent)]/10 border-[var(--accent)]/30 text-[var(--accent)]",
          ),
          children: [
            a.isCompleted
              ? t(o, { className: e("w-3 h-3") })
              : t(d, { className: e("w-3 h-3") }),
            a.isCompleted
              ? r("readings.status.completed")
              : r("readings.status.processing"),
          ],
        }),
      }),
    ],
  });
}
export { f as AdminReadingsTableRow };
