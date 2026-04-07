import type { MutableRefObject } from "react";
import { getStackPlacement } from "@/features/reading/session/presentation/session-page/utils";
import { cn } from "@/lib/utils";

interface PickedCardsStackProps {
  pickedCards: number[];
  isRevealing: boolean;
  stackAnchorRef: MutableRefObject<HTMLDivElement | null>;
  onRemove: (cardId: number) => void;
}

export default function PickedCardsStack({
  pickedCards,
  isRevealing,
  stackAnchorRef,
  onRemove,
}: PickedCardsStackProps) {
  return (
    <div className={cn("mb-6 flex w-full tn-justify-center-end-sm px-2")}>
      <div className={cn("relative tn-size-picked-card")}>
        <div
          ref={stackAnchorRef}
          className={cn(
            "absolute left-0 top-0 aspect-[14/22] w-[72px] rounded-md border border-dashed border-[var(--purple-accent)]/30 bg-[var(--purple-accent)]/10",
          )}
        />

        {pickedCards.map((cardId, stackIndex) => {
          const placement = getStackPlacement(stackIndex);

          return (
            <button
              key={`${cardId}-${stackIndex}`}
              type="button"
              onClick={() => onRemove(cardId)}
              disabled={isRevealing}
              className={cn(
                "absolute left-0 top-0 aspect-[14/22] w-[72px] rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-md transition-transform duration-200 hover:-translate-y-1 disabled:pointer-events-none tarot-deck-card",
              )}
              style={{
                transform: `translate(${placement.xOffset}px, ${placement.yOffset}px) rotate(${placement.rotate}deg)`,
                zIndex: stackIndex + 1,
              }}
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
            </button>
          );
        })}
      </div>
    </div>
  );
}
