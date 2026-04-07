import { jsx as t, jsxs as r } from "react/jsx-runtime";
import c from "next/image";
import { Sparkles as m } from "lucide-react";
import { cn as e } from "@/lib/utils";
function f({
  cardId: l,
  cardImageUrl: a,
  cardMeaning: d,
  cardName: n,
  index: i,
  isFlipped: s,
  meaningLabel: o,
}) {
  return r(
    "div",
    {
      className: e(
        "mx-auto flex w-full max-w-[180px] flex-col items-center gap-3",
      ),
      children: [
        r("div", {
          className: e(
            "group relative aspect-[14/22] w-full cursor-pointer preserve-3d transition-transform duration-700 ease-out tarot-card-flip",
          ),
          style: { transform: s ? "rotateY(180deg)" : "rotateY(0deg)" },
          children: [
            t("div", {
              className: e(
                "absolute inset-0 flex h-full w-full backface-hidden flex-col items-center justify-center rounded-xl border-2 shadow-xl tn-border tn-grad-lunar",
              ),
              children: t("div", {
                className: e(
                  "flex h-[90%] w-[85%] items-center justify-center rounded-lg border tn-border-soft tn-starfield",
                ),
                children: t(m, { className: e("h-8 w-8 tn-text-muted") }),
              }),
            }),
            t("div", {
              className: e(
                "absolute inset-0 flex h-full w-full backface-hidden items-center justify-center bg-transparent",
              ),
              style: { transform: "rotateY(180deg)" },
              children: r("div", {
                className: e(
                  "relative flex h-full w-full items-center justify-center overflow-hidden rounded-xl border shadow-xl tn-border-soft tn-surface-strong",
                ),
                children: [
                  t("div", {
                    className: e(
                      "pointer-events-none absolute inset-0 bg-gradient-to-tr from-[var(--purple-accent)]/20 to-transparent",
                    ),
                  }),
                  t("div", {
                    className: e(
                      "pointer-events-none absolute inset-2 rounded-lg border border-[var(--purple-accent)]/10",
                    ),
                  }),
                  a
                    ? t(c, {
                        src: a,
                        alt: n,
                        fill: !0,
                        unoptimized: !0,
                        sizes: "(max-width: 1024px) 45vw, 220px",
                        className: e("h-full w-full object-cover"),
                      })
                    : t("span", {
                        className: e(
                          "text-5xl font-black drop-shadow-sm font-serif tn-text-primary/10",
                        ),
                        children: i + 1,
                      }),
                ],
              }),
            }),
          ],
        }),
        r("div", {
          className: e(
            "mt-4 text-center transition-opacity duration-1000 delay-500",
            s ? "opacity-100" : "opacity-0",
          ),
          children: [
            t("h3", {
              className: e(
                "mb-2 px-2 text-sm font-bold leading-tight font-serif tn-text-primary",
              ),
              children: n,
            }),
            t("p", {
              className: e(
                "mb-1 text-[10px] font-semibold uppercase tracking-widest text-[var(--purple-accent)]",
              ),
              children: o,
            }),
            t("p", {
              className: e(
                "line-clamp-3 px-2 text-xs leading-relaxed tn-text-secondary",
              ),
              children: d,
            }),
          ],
        }),
      ],
    },
    `revealed-card-${l}`,
  );
}
export { f as default };
