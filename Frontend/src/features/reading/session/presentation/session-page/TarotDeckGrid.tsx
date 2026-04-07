import { jsx as n, jsxs as D } from "react/jsx-runtime";
import {
  DECK_WAVE_ROTATION_AMPLITUDE as k,
  DECK_WAVE_Y_AMPLITUDE as _,
} from "@/features/reading/session/presentation/session-page/constants";
import { cn as r } from "@/lib/utils";
function w({
  activeDeckRows: v,
  cardsToDraw: o,
  deckCardWidth: u,
  horizontalOverlapFactor: p,
  isRevealing: s,
  pickedCards: i,
  pickedCardSet: m,
  rowOverlapMargin: f,
  onPickCard: c,
  setDeckCardRef: b,
}) {
  return n("div", {
    className: r(
      "mx-auto mb-20 w-full max-w-[1520px] transition-opacity duration-300",
      i.length === o && "pointer-events-none opacity-35",
    ),
    style: { "--deck-card-w": u },
    children: n("div", {
      className: r("relative mx-auto w-full max-w-[1420px] px-1 sm:px-2"),
      children: v.map((g, l) =>
        n(
          "div",
          {
            className: r("flex items-end justify-center"),
            style: l === 0 ? void 0 : { marginTop: f },
            children: g.map((e, d) => {
              const t = m.has(e),
                h = Math.sin(e * 0.55) * _,
                y = Math.sin(e * 0.35) * k;
              return n(
                "div",
                {
                  className: r(
                    "group relative aspect-[14/22] w-[var(--deck-card-w)] tarot-card-fan",
                  ),
                  style: {
                    marginLeft:
                      d === 0 ? 0 : `calc(var(--deck-card-w) * -${p})`,
                    transform: `translateY(${h}px) rotate(${y}deg)`,
                    zIndex: t ? 0 : d + l * 100,
                  },
                  children: D("div", {
                    ref: b(e),
                    onClick: (a) => {
                      s || t || i.length >= o || c(e, a.currentTarget);
                    },
                    className: r(
                      "relative h-full w-full rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-sm transition-all duration-150 ease-out tarot-deck-card",
                      t && "pointer-events-none scale-95 opacity-0",
                      !t &&
                        i.length < o &&
                        "opacity-90 hover:z-40 hover:scale-105 hover:-translate-y-3 hover:border-[var(--purple-accent)] hover:shadow-[0_0_10px_var(--c-168-85-247-70)] md:hover:-translate-y-5",
                      !t && i.length >= o && "cursor-default opacity-45",
                    ),
                    role: "button",
                    tabIndex: 0,
                    "aria-label": `Pick card ${e + 1}`,
                    onKeyDown: (a) => {
                      (a.key !== "Enter" && a.key !== " ") ||
                        (a.preventDefault(),
                        !(s || t || i.length >= o) && c(e, a.currentTarget));
                    },
                    children: [
                      n("div", {
                        className: r(
                          "pointer-events-none absolute inset-1 rounded-sm border border-[var(--purple-accent)]/30 opacity-60",
                        ),
                      }),
                      n("div", {
                        className: r(
                          "pointer-events-none absolute inset-0 rounded-md bg-[radial-gradient(circle_at_24%_22%,var(--c-255-255-255-15)_0,transparent_34%),linear-gradient(160deg,var(--c-61-49-80-20),transparent_65%)]",
                        ),
                      }),
                    ],
                  }),
                },
                e,
              );
            }),
          },
          `row-${l}`,
        ),
      ),
    }),
  });
}
export { w as default };
