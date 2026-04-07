import { jsx as t, jsxs as a } from "react/jsx-runtime";
import { Suspense as c } from "react";
import { RefreshCw as m, Sparkles as u } from "lucide-react";
import { cn as e } from "@/lib/utils";
function v({
  allCardsFlipped: s,
  cards: r,
  footerNote: l,
  liveLabel: i,
  sessionId: d,
  subtitle: o,
  title: n,
  AiInterpretationStream: p,
}) {
  return r.length === 0
    ? null
    : t("div", {
        className: e("h-full md:sticky md:top-24 md:col-span-1"),
        children: a("div", {
          className: e(
            "flex flex-col overflow-hidden rounded-3xl border shadow-2xl tn-border tn-surface-strong md:max-h-[calc(100dvh-160px)]",
          ),
          children: [
            a("div", {
              className: e(
                "flex items-center justify-between border-b p-5 tn-border tn-overlay",
              ),
              children: [
                a("div", {
                  className: e("flex items-center gap-3"),
                  children: [
                    t("div", {
                      className: e(
                        "flex h-10 w-10 items-center justify-center rounded-full border border-[var(--purple-accent)]/20 bg-[var(--purple-accent)]/10",
                      ),
                      children: t(u, {
                        className: e(
                          "h-5 w-5 animate-pulse text-[var(--purple-accent)]",
                        ),
                      }),
                    }),
                    a("div", {
                      children: [
                        t("h2", {
                          className: e("text-lg font-bold tn-text-primary"),
                          children: n,
                        }),
                        t("p", {
                          className: e(
                            "text-[10px] font-mono uppercase tracking-tighter tn-text-muted",
                          ),
                          children: o,
                        }),
                      ],
                    }),
                  ],
                }),
                t("div", {
                  className: e(
                    "rounded-full border border-[var(--purple-accent)]/20 bg-[var(--purple-accent)]/10 px-3 py-1",
                  ),
                  children: t("span", {
                    className: e(
                      "text-[10px] font-bold uppercase tracking-widest text-[var(--purple-accent)]",
                    ),
                    children: i,
                  }),
                }),
              ],
            }),
            t("div", {
              className: e(
                "flex flex-1 flex-col overflow-hidden bg-gradient-to-b from-transparent to-[var(--purple-accent)]/5",
              ),
              children: t(c, {
                fallback: a("div", {
                  className: e(
                    "flex flex-1 items-center justify-center gap-3 tn-text-muted",
                  ),
                  children: [
                    t(m, {
                      className: e(
                        "h-4 w-4 animate-spin text-[var(--purple-accent)]",
                      ),
                    }),
                    t("span", {
                      className: e(
                        "text-[10px] font-black uppercase tracking-widest",
                      ),
                      children: n,
                    }),
                  ],
                }),
                children: t(p, { sessionId: d, cards: r, isReadyToShow: s }),
              }),
            }),
            a("div", {
              className: e(
                "flex items-center justify-between border-t px-6 py-4 tn-border tn-overlay",
              ),
              children: [
                t("p", {
                  className: e("text-[10px] italic tn-text-muted"),
                  children: l,
                }),
                a("div", {
                  className: e("flex gap-1"),
                  children: [
                    t("div", {
                      className: e(
                        "h-1.5 w-1.5 rounded-full bg-[var(--purple-accent)]/40",
                      ),
                    }),
                    t("div", {
                      className: e(
                        "h-1.5 w-1.5 rounded-full bg-[var(--warning)]/40",
                      ),
                    }),
                    t("div", {
                      className: e(
                        "h-1.5 w-1.5 rounded-full bg-[var(--danger)]/40",
                      ),
                    }),
                  ],
                }),
              ],
            }),
          ],
        }),
      });
}
export { v as default };
