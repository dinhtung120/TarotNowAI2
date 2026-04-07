import { Fragment as p, jsx as t, jsxs as a } from "react/jsx-runtime";
import f from "next/image";
import { Lock as m } from "lucide-react";
import { cn as e } from "@/lib/utils";
function g({
  deckCard: l,
  userCard: o,
  cardImageUrl: i,
  cardName: s,
  unknownCardLabel: n,
  onSelect: d,
}) {
  const r = !!o,
    c = o ? ((o.copies % 5) / 5) * 100 : 0;
  return a("div", {
    onClick: () => d(l.id),
    className: e(
      "group relative rounded-3xl p-3 flex flex-col items-center transition-transform duration-500 transform-gpu antialiased will-change-transform border overflow-hidden cursor-pointer",
      r
        ? "tn-panel hover:border-[var(--warning)]/40 hover:tn-surface-strong shadow-sm hover:shadow-[0_10px_30px_var(--c-245-158-11-08)]"
        : "tn-panel-overlay-soft opacity-50 grayscale",
    ),
    children: [
      t("div", {
        className: e("w-full mb-2"),
        children:
          r && o
            ? a("div", {
                className: e("flex flex-col gap-1.5 w-full"),
                children: [
                  a("div", {
                    className: e(
                      "flex justify-between items-center text-[10px] font-black uppercase tracking-tighter tn-text-primary px-1",
                    ),
                    children: [
                      a("span", { children: ["Lv. ", o.level] }),
                      a("span", {
                        className: e("text-[var(--warning)]"),
                        children: [o.copies % 5, " / 5"],
                      }),
                    ],
                  }),
                  t("div", {
                    className: e(
                      "w-full h-1 tn-surface rounded-full overflow-hidden",
                    ),
                    children: t("div", {
                      className: e(
                        "h-full bg-[var(--warning)] shadow-[0_0_5px_var(--c-245-158-11-30)] transition-all duration-1000",
                      ),
                      style: { width: `${c}%` },
                    }),
                  }),
                ],
              })
            : t("div", { className: e("h-4 w-full") }),
      }),
      a("div", {
        className: e(
          "w-full aspect-[14/22] rounded-2xl mb-3 flex items-center justify-center relative overflow-hidden transition-transform duration-500 transform-gpu antialiased will-change-transform",
          r ? "group-hover:scale-[1.03]" : "",
          r
            ? "bg-gradient-to-br from-[color:var(--c-215-189-226-26)] to-[color:var(--c-168-156-255-28)] border tn-border-soft"
            : "tn-overlay",
        ),
        children: [
          i
            ? t(f, {
                src: i,
                alt: s || n,
                fill: !0,
                unoptimized: !0,
                priority: l.id < 7,
                sizes: "(max-width: 768px) 33vw, 220px",
                className: e(
                  "h-full w-full object-cover",
                  r ? "backface-hidden" : "blur-[6px]",
                ),
              })
            : t("span", {
                className: e(
                  "text-4xl font-serif font-black tracking-tighter opacity-20",
                  r ? "text-[var(--warning)]" : "tn-text-muted blur-[4px]",
                ),
                children: l.id + 1,
              }),
          r
            ? a(p, {
                children: [
                  t("div", {
                    className: e(
                      "absolute inset-0 bg-gradient-to-t from-[var(--warning)]/20 via-transparent to-transparent opacity-60",
                    ),
                  }),
                  t("div", {
                    className: e(
                      "absolute bottom-[-10%] left-[-10%] w-[120%] h-1/2 bg-[var(--warning)]/[0.03] blur-2xl rounded-full",
                    ),
                  }),
                ],
              })
            : null,
        ],
      }),
      t("h4", {
        className: e(
          "text-[11px] font-black text-center leading-snug mb-3 px-1 tracking-tight min-h-[1.5rem] flex items-center justify-center",
          r ? "tn-text-primary" : "tn-text-muted",
        ),
        children: (r && s) || n,
      }),
      r && o
        ? a("div", {
            className: e(
              "w-full mt-auto flex gap-1 text-[9px] font-bold text-center",
            ),
            children: [
              a("div", {
                className: e(
                  "flex-1 bg-red-500/10 border border-red-500/20 text-red-400 rounded-lg py-1 shadow-inner shadow-red-500/10",
                ),
                children: ["\u2694\uFE0F ", o.atk || 0],
              }),
              a("div", {
                className: e(
                  "flex-1 bg-blue-500/10 border border-blue-500/20 text-blue-400 rounded-lg py-1 shadow-inner shadow-blue-500/10",
                ),
                children: ["\u{1F6E1}\uFE0F ", o.def || 0],
              }),
            ],
          })
        : t("div", {
            className: e(
              "w-full mt-auto py-1.5 tn-surface rounded-xl border tn-border-soft flex items-center justify-center",
            ),
            children: t(m, { className: e("w-2.5 h-2.5 tn-text-muted") }),
          }),
    ],
  });
}
export { g as CollectionDeckCard };
