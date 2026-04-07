import { jsx as r, jsxs as D } from "react/jsx-runtime";
import { AlertCircle as u } from "lucide-react";
import { cn as i } from "@/lib/utils";
import { CollectionDeckCard as C } from "./CollectionDeckCard";
import { CollectionEmptyState as y } from "./CollectionEmptyState";
function N({
  collection: o,
  filteredDeck: a,
  activeFilter: d,
  error: n,
  getCardImageUrl: l,
  getCardName: c,
  onSelectCard: m,
  labels: t,
}) {
  return n
    ? D("div", {
        className: i(
          "mb-10 p-4 bg-[var(--danger)]/10 border border-[var(--danger)]/20 rounded-2xl flex items-center gap-3 text-[var(--danger)] text-xs animate-in zoom-in-95",
        ),
        children: [
          r(u, { className: i("w-4 h-4 flex-shrink-0") }),
          r("p", { className: i("font-medium"), children: n }),
        ],
      })
    : o.length === 0 && d !== "unowned"
      ? r(y, {
          title: t.emptyTitle,
          description: t.emptyDescription,
          cta: t.emptyCta,
        })
      : r("div", {
          className: i(
            "grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-7 gap-4 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-500",
          ),
          children: a.map((e) => {
            const s = o.find((f) => f.cardId === e.id),
              p = l(e.id),
              g = c(e.id) ?? "";
            return r(
              C,
              {
                deckCard: e,
                userCard: s,
                cardImageUrl: p,
                cardName: g,
                unknownCardLabel: t.unknownCard,
                onSelect: m,
              },
              e.id,
            );
          }),
        });
}
export { N as CollectionDeckGrid };
