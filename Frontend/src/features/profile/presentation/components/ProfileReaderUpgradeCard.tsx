"use client";
import { Fragment as v, jsx as a, jsxs as r } from "react/jsx-runtime";
import {
  ArrowUpRight as p,
  Clock as d,
  Sparkles as o,
  XCircle as g,
} from "lucide-react";
import { GlassCard as u } from "@/shared/components/ui";
import { cn as e } from "@/lib/utils";
function b({
  t,
  router: n,
  readerRequest: s,
  readerRequestLoading: l,
  isAdmin: i,
  isTarotReader: c,
}) {
  return i || c || l
    ? null
    : r(u, {
        className: e("!p-6 sm:!p-8 overflow-hidden relative group"),
        children: [
          a("div", {
            className: e(
              "absolute top-0 right-0 p-6 opacity-5 pointer-events-none transition-transform duration-700 group-hover:scale-110 group-hover:rotate-12",
            ),
            children: a(o, {
              className: e("w-32 h-32 text-[var(--purple-accent)]"),
            }),
          }),
          r("div", {
            className: e("relative z-10 space-y-5"),
            children: [
              r("div", {
                className: e("flex items-center gap-3"),
                children: [
                  a("div", {
                    className: e(
                      "w-12 h-12 rounded-2xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--warning)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 shadow-xl",
                    ),
                    children: a(o, {
                      className: e("w-6 h-6 text-[var(--purple-accent)]"),
                    }),
                  }),
                  r("div", {
                    children: [
                      a("h3", {
                        className: e(
                          "text-lg font-black tn-text-primary italic tracking-tight",
                        ),
                        children: t("upgrade.title"),
                      }),
                      a("p", {
                        className: e(
                          "text-[10px] font-bold uppercase tracking-widest text-[var(--text-secondary)]",
                        ),
                        children: t("upgrade.subtitle"),
                      }),
                    ],
                  }),
                ],
              }),
              s?.hasRequest
                ? null
                : r(v, {
                    children: [
                      a("p", {
                        className: e(
                          "text-sm text-[var(--text-secondary)] leading-relaxed",
                        ),
                        children: t("upgrade.desc"),
                      }),
                      r("button", {
                        type: "button",
                        onClick: () => n.push("/reader/apply"),
                        className: e(
                          "group/btn flex items-center gap-2.5 bg-gradient-to-r from-[var(--purple-accent)] to-[var(--purple-accent)] hover:brightness-110 px-6 py-3 min-h-11 rounded-xl text-[10px] font-black uppercase tracking-widest tn-text-primary transition-all hover:scale-[1.02] active:scale-95 shadow-xl cursor-pointer",
                        ),
                        children: [
                          a(p, {
                            className: e(
                              "w-3.5 h-3.5 transition-transform group-hover/btn:translate-x-0.5 group-hover/btn:-translate-y-0.5",
                            ),
                          }),
                          t("upgrade.cta"),
                        ],
                      }),
                    ],
                  }),
              s?.hasRequest && s.status === "pending"
                ? r("div", {
                    className: e(
                      "p-4 rounded-xl bg-[var(--warning)]/5 border border-[var(--warning)]/20 space-y-2",
                    ),
                    children: [
                      r("div", {
                        className: e("flex items-center gap-2"),
                        children: [
                          a(d, {
                            className: e("w-4 h-4 text-[var(--warning)]"),
                          }),
                          a("span", {
                            className: e(
                              "text-[10px] font-black uppercase tracking-widest text-[var(--warning)]",
                            ),
                            children: t("upgrade.pending_title"),
                          }),
                        ],
                      }),
                      a("p", {
                        className: e(
                          "text-xs text-[var(--text-secondary)] leading-relaxed",
                        ),
                        children: t("upgrade.pending_desc"),
                      }),
                    ],
                  })
                : null,
              s?.hasRequest && s.status === "rejected"
                ? r("div", {
                    className: e("space-y-3"),
                    children: [
                      r("div", {
                        className: e(
                          "p-4 rounded-xl bg-[var(--danger)]/5 border border-[var(--danger)]/20 space-y-2",
                        ),
                        children: [
                          r("div", {
                            className: e("flex items-center gap-2"),
                            children: [
                              a(g, {
                                className: e("w-4 h-4 text-[var(--danger)]"),
                              }),
                              a("span", {
                                className: e(
                                  "text-[10px] font-black uppercase tracking-widest text-[var(--danger)]",
                                ),
                                children: t("upgrade.rejected_title"),
                              }),
                            ],
                          }),
                          s.adminNote
                            ? a("p", {
                                className: e(
                                  "text-xs text-[var(--text-secondary)] leading-relaxed",
                                ),
                                children: t("upgrade.rejected_reason", {
                                  note: s.adminNote,
                                }),
                              })
                            : null,
                        ],
                      }),
                      r("button", {
                        type: "button",
                        onClick: () => n.push("/reader/apply"),
                        className: e(
                          "group/btn flex items-center gap-2.5 bg-[var(--purple-accent)]/10 hover:bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 text-[var(--purple-accent)] px-5 py-2.5 min-h-11 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all hover:scale-[1.02] active:scale-95 cursor-pointer",
                        ),
                        children: [
                          a(p, {
                            className: e(
                              "w-3.5 h-3.5 transition-transform group-hover/btn:translate-x-0.5 group-hover/btn:-translate-y-0.5",
                            ),
                          }),
                          t("upgrade.reapply_cta"),
                        ],
                      }),
                    ],
                  })
                : null,
            ],
          }),
        ],
      });
}
export { b as ProfileReaderUpgradeCard };
