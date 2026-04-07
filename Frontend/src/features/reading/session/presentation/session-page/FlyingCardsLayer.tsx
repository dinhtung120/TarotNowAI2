import type { CSSProperties } from "react";
import type { FlyingCard } from "@/features/reading/session/presentation/session-page/types";
import { cn } from "@/lib/utils";

interface FlyingCardsLayerProps {
  flyingCards: FlyingCard[];
}

export default function FlyingCardsLayer({
  flyingCards,
}: FlyingCardsLayerProps) {
  if (flyingCards.length === 0) return null;

  return (
    <div className={cn("pointer-events-none fixed inset-0 tn-z-60")}>
      {flyingCards.map((card) => (
        <div
          key={card.key}
          className={cn(
            "tarot-flying-card rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-md",
          )}
          style={{
            top: `${card.startY}px`,
            left: `${card.startX}px`,
            zIndex: 120 + card.stackIndex,
            "--fly-x": `${card.deltaX}px`,
            "--fly-y": `${card.deltaY}px`,
            "--fly-rotate": `${card.rotate}deg`,
          } as CSSProperties}
        >
          <div
            className={cn(
              "pointer-events-none absolute inset-1 rounded-sm border border-[var(--purple-accent)]/30 opacity-60",
            )}
          />
          <div
            className={cn(
              "pointer-events-none absolute inset-0 rounded-md bg-[radial-gradient(circle_at_24%_22%,var(--c-255-255-255-15)_0,transparent_34%),linear-gradient(160deg,var(--c-61-49-80-20),transparent_65%)]",
            )}
          />
        </div>
      ))}
    </div>
  );
}
