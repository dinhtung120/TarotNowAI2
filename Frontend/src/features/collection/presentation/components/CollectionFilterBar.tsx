import { jsx as n, jsxs as l } from "react/jsx-runtime";
import {
  CheckCircle2 as d,
  Filter as p,
  LayoutGrid as m,
  Lock as f,
  Sparkles as x,
} from "lucide-react";
import { cn as e } from "@/lib/utils";
const b = [
  { id: "id", label: "ID", icon: "\u{1F4D6}" },
  { id: "level", label: "Level", icon: "\u2B50" },
  { id: "atk", label: "ATK", icon: "\u2694\uFE0F" },
  { id: "def", label: "DEF", icon: "\u{1F6E1}\uFE0F" },
];
function v({
  activeFilter: a,
  sortBy: i,
  onFilterChange: o,
  onSortChange: s,
  labels: r,
}) {
  const c = [
    { id: "all", label: r.filterAll, icon: n(m, { className: e("w-3 h-3") }) },
    {
      id: "owned",
      label: r.filterOwned,
      icon: n(d, { className: e("w-3 h-3") }),
    },
    {
      id: "unowned",
      label: r.filterUnowned,
      icon: n(f, { className: e("w-3 h-3") }),
    },
  ];
  return l("div", {
    className: e(
      "flex flex-col md:flex-row md:items-center justify-between gap-6 mb-10 animate-in fade-in duration-700 delay-300",
    ),
    children: [
      l("div", {
        className: e("flex flex-wrap items-center gap-2"),
        children: [
          l("div", {
            className: e("flex items-center gap-2 mr-4 tn-text-muted"),
            children: [
              n(p, { className: e("w-3.5 h-3.5") }),
              n("span", {
                className: e(
                  "text-[10px] uppercase font-black tracking-widest",
                ),
                children: r.filtersLabel,
              }),
            ],
          }),
          c.map((t) =>
            l(
              "button",
              {
                type: "button",
                onClick: () => o(t.id),
                className: e(
                  "flex items-center gap-2 px-5 py-2 rounded-full text-xs font-black transition-all duration-300 border",
                  a === t.id
                    ? "tn-surface-strong tn-text-ink tn-border shadow-xl scale-105"
                    : "tn-surface tn-text-muted tn-border-soft hover:tn-border-strong hover:tn-text-secondary",
                ),
                children: [t.icon, t.label],
              },
              t.id,
            ),
          ),
        ],
      }),
      l("div", {
        className: e("flex flex-wrap items-center gap-2"),
        children: [
          l("div", {
            className: e("flex items-center gap-2 mr-4 tn-text-muted"),
            children: [
              n(x, { className: e("w-3.5 h-3.5") }),
              n("span", {
                className: e(
                  "text-[10px] uppercase font-black tracking-widest",
                ),
                children: r.sortLabel,
              }),
            ],
          }),
          b.map((t) =>
            l(
              "button",
              {
                type: "button",
                onClick: () => s(t.id),
                className: e(
                  "flex items-center gap-2 px-4 py-1.5 rounded-xl text-[10px] font-black transition-all duration-300 border",
                  i === t.id
                    ? "bg-[var(--warning)]/10 border-[var(--warning)]/30 text-[var(--warning)] shadow-lg scale-105"
                    : "tn-surface tn-text-muted tn-border-soft hover:tn-border-strong hover:tn-text-secondary",
                ),
                children: [
                  n("span", { className: e("text-xs"), children: t.icon }),
                  t.label,
                ],
              },
              t.id,
            ),
          ),
        ],
      }),
    ],
  });
}
export { v as CollectionFilterBar };
