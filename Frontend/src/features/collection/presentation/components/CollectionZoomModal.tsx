import { Fragment as f, jsx as e, jsxs as a } from "react/jsx-runtime";
import p from "next/image";
import { Sparkles as b } from "lucide-react";
import { cn as t } from "@/lib/utils";
function k({
  cardData: s,
  userCard: n,
  cardImageUrl: o,
  cardName: d,
  cardMeaning: c,
  suitLabel: m,
  onClose: i,
  labels: l,
}) {
  if (!s) return null;
  const r = (n && d) || l.unknownCard,
    x = (n && c) || l.lockedMeaning;
  return a("div", {
    className: t(
      "fixed inset-0 z-[100] flex items-center justify-center p-6 md:p-12 animate-in fade-in duration-500",
    ),
    children: [
      e("div", {
        className: t("absolute inset-0 tn-overlay-strong"),
        onClick: i,
      }),
      a("div", {
        className: t(
          "relative z-10 max-w-4xl w-full tn-panel rounded-[3rem] p-6 sm:p-8 md:p-12 shadow-[0_30px_100px_var(--c-0-0-0-80)] animate-in zoom-in-95 slide-in-from-bottom-10 duration-500 flex flex-col md:flex-row items-center md:items-stretch gap-8 md:gap-12 text-center md:text-left overflow-hidden",
        ),
        children: [
          e("div", {
            className: t(
              "absolute -top-24 -left-24 w-64 h-64 bg-[var(--purple-accent)]/[0.08] blur-[100px] rounded-full",
            ),
          }),
          e("div", {
            className: t(
              "absolute -bottom-24 -right-24 w-64 h-64 bg-[var(--warning)]/[0.05] blur-[100px] rounded-full",
            ),
          }),
          a("div", {
            className: t(
              "relative aspect-[14/22] w-52 sm:w-64 md:w-80 flex-shrink-0 bg-gradient-to-br from-[var(--warning)]/10 to-transparent rounded-[2.5rem] border tn-border p-5 sm:p-6 mb-2 md:mb-0 transition-transform duration-700 hover:scale-[1.02]",
              n
                ? "shadow-[0_0_50px_var(--c-251-191-36-15)]"
                : "grayscale opacity-50",
            ),
            children: [
              e("div", {
                className: t(
                  "absolute inset-4 border border-[var(--warning)]/10 rounded-[1.8rem] pointer-events-none",
                ),
              }),
              e("div", {
                className: t("h-full flex flex-col"),
                children: a("div", {
                  className: t(
                    "flex-1 rounded-2xl tn-overlay overflow-hidden relative shadow-inner mb-4 border tn-border-soft flex items-center justify-center",
                  ),
                  children: [
                    o
                      ? e(p, {
                          src: o,
                          alt: r,
                          fill: !0,
                          unoptimized: !0,
                          sizes: "(max-width: 768px) 13rem, 20rem",
                          className: t(
                            "h-full w-full object-cover transition-all duration-500",
                            n ? "" : "blur-[6px]",
                          ),
                        })
                      : a(f, {
                          children: [
                            e(b, {
                              className: t(
                                "w-20 h-20",
                                n
                                  ? "text-[var(--warning)]/30"
                                  : "tn-text-muted",
                              ),
                            }),
                            e("div", {
                              className: t(
                                "absolute inset-0 flex items-center justify-center",
                              ),
                              children: e("span", {
                                className: t(
                                  "text-8xl font-serif font-black tracking-tighter tn-text-primary opacity-5",
                                ),
                                children: s.id + 1,
                              }),
                            }),
                          ],
                        }),
                    a("div", {
                      className: t(
                        "absolute bottom-6 left-0 right-0 z-20 px-3 text-center",
                      ),
                      children: [
                        e("span", {
                          className: t(
                            "text-[10px] font-black uppercase tracking-[0.2em] text-[var(--warning)]/60 block mb-1",
                          ),
                          children: m,
                        }),
                        e("h3", {
                          className: t(
                            "text-xl font-black tn-text-primary italic tracking-tighter leading-tight drop-shadow-lg",
                          ),
                          children: r,
                        }),
                      ],
                    }),
                  ],
                }),
              }),
            ],
          }),
          a("div", {
            className: t("relative z-10 flex-1 flex flex-col justify-center"),
            children: [
              e("h2", {
                className: t(
                  "text-3xl md:text-4xl lg:text-5xl font-black tn-text-primary italic tracking-tight mb-4",
                ),
                children: r,
              }),
              e("div", {
                className: t(
                  "w-16 h-1 bg-[var(--warning)]/40 rounded-full mb-6 mx-auto md:mx-0",
                ),
              }),
              e("p", {
                className: t(
                  "tn-text-muted text-sm md:text-base font-medium leading-relaxed italic border-l-2 border-transparent md:border-[var(--warning)]/20 md:pl-4 py-2",
                ),
                children: x,
              }),
              n
                ? a("div", {
                    className: t(
                      "grid grid-cols-2 md:grid-cols-4 gap-4 mt-8 md:mt-10 mx-auto md:mx-0",
                    ),
                    children: [
                      a("div", {
                        className: t(
                          "tn-panel rounded-2xl p-4 text-center border tn-border-soft",
                        ),
                        children: [
                          e("span", {
                            className: t(
                              "block text-[10px] md:text-xs uppercase font-black tracking-widest tn-text-muted mb-1",
                            ),
                            children: l.levelLabel,
                          }),
                          e("span", {
                            className: t(
                              "text-2xl md:text-3xl font-black text-[var(--warning)]",
                            ),
                            children: n.level,
                          }),
                        ],
                      }),
                      a("div", {
                        className: t(
                          "tn-panel rounded-2xl p-4 text-center border tn-border-soft",
                        ),
                        children: [
                          e("span", {
                            className: t(
                              "block text-[10px] md:text-xs uppercase font-black tracking-widest tn-text-muted mb-1",
                            ),
                            children: l.copiesLabel,
                          }),
                          e("span", {
                            className: t(
                              "text-2xl md:text-3xl font-black tn-text-primary",
                            ),
                            children: n.copies,
                          }),
                        ],
                      }),
                      a("div", {
                        className: t(
                          "tn-panel rounded-2xl p-4 text-center border border-red-500/20 bg-red-500/5",
                        ),
                        children: [
                          e("span", {
                            className: t(
                              "block text-[10px] md:text-xs uppercase font-black tracking-widest text-red-500/70 mb-1",
                            ),
                            children: "ATK",
                          }),
                          e("span", {
                            className: t(
                              "text-2xl md:text-3xl font-black text-red-400 drop-shadow-[0_0_8px_rgba(248,113,113,0.5)]",
                            ),
                            children: n.atk || 0,
                          }),
                        ],
                      }),
                      a("div", {
                        className: t(
                          "tn-panel rounded-2xl p-4 text-center border border-blue-500/20 bg-blue-500/5",
                        ),
                        children: [
                          e("span", {
                            className: t(
                              "block text-[10px] md:text-xs uppercase font-black tracking-widest text-blue-500/70 mb-1",
                            ),
                            children: "DEF",
                          }),
                          e("span", {
                            className: t(
                              "text-2xl md:text-3xl font-black text-blue-400 drop-shadow-[0_0_8px_rgba(96,165,250,0.5)]",
                            ),
                            children: n.def || 0,
                          }),
                        ],
                      }),
                    ],
                  })
                : null,
              e("div", {
                className: t(
                  "mt-10 md:mt-auto pt-4 flex justify-center md:justify-start",
                ),
                children: e("button", {
                  type: "button",
                  onClick: i,
                  className: t(
                    "px-10 py-3.5 tn-surface-strong tn-text-ink rounded-full text-[10px] md:text-xs font-black uppercase tracking-widest hover:scale-105 active:scale-95 transition-all shadow-xl",
                  ),
                  children: l.closeLabel,
                }),
              }),
            ],
          }),
        ],
      }),
    ],
  });
}
export { k as CollectionZoomModal };
