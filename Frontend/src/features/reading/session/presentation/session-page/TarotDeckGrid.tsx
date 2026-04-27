import { DECK_WAVE_ROTATION_AMPLITUDE, DECK_WAVE_Y_AMPLITUDE } from "@/features/reading/session/presentation/session-page/constants";
import { cn } from "@/lib/utils";

interface TarotDeckGridProps {
 activeDeckRows: number[][];
 cardsToDraw: number;
 isRevealing: boolean;
 pickedCards: number[];
 pickedCardSet: Set<number>;
 onPickCard: (cardId: number, source: HTMLDivElement | null) => void;
 setDeckCardRef: (cardId: number) => (node: HTMLDivElement | null) => void;
}

function resolveDeckYOffsetClass(cardId: number): string {
 const yOffset = Math.round(Math.sin(cardId * 0.55) * DECK_WAVE_Y_AMPLITUDE);
 if (yOffset >= 0) return `tn-deck-y-pos-${Math.min(yOffset, 6)}`;
 return `tn-deck-y-neg-${Math.min(Math.abs(yOffset), 6)}`;
}

function resolveDeckRotationClass(cardId: number): string {
 const rotation = Math.round(Math.sin(cardId * 0.35) * DECK_WAVE_ROTATION_AMPLITUDE);
 if (rotation >= 0) return `tn-deck-rot-pos-${Math.min(rotation, 7)}`;
 return `tn-deck-rot-neg-${Math.min(Math.abs(rotation), 7)}`;
}

export default function TarotDeckGrid({ activeDeckRows, cardsToDraw, isRevealing, pickedCards, pickedCardSet, onPickCard, setDeckCardRef }: TarotDeckGridProps) {
 return (
  <div className={cn("tn-reading-deck-root mx-auto mb-20 w-full tn-maxw-1520 transition-opacity duration-300", pickedCards.length === cardsToDraw && "pointer-events-none opacity-35")}>
   <div className={cn("relative mx-auto w-full tn-maxw-1420 tn-px-1-2-sm")}>
    {activeDeckRows.map((row, rowIndex) => (
     <div key={`row-${rowIndex}`} className={cn("flex items-end justify-center", rowIndex > 0 && "tn-reading-deck-row-overlap")}>
      {row.map((cardId, cardIndex) => {
       const picked = pickedCardSet.has(cardId);

       return (
        <div
         key={cardId}
         className={cn(
          "group relative tn-tarot-card-wrap tarot-card-fan tn-deck-fan-transform",
          cardIndex > 0 && "tn-reading-deck-card-overlap",
          resolveDeckYOffsetClass(cardId),
          resolveDeckRotationClass(cardId),
         )}
        >
         <div
          ref={setDeckCardRef(cardId)}
          onClick={(event) => !isRevealing && !picked && pickedCards.length < cardsToDraw && onPickCard(cardId, event.currentTarget)}
          className={cn("tn-tarot-card-face relative h-full w-full rounded-md shadow-sm transition-all duration-150 ease-out tarot-deck-card", picked && "tn-tarot-card-face-picked pointer-events-none scale-95 opacity-0", !picked && pickedCards.length < cardsToDraw && "tn-tarot-card-face-ready tn-tarot-card-face-hover", !picked && pickedCards.length >= cardsToDraw && "tn-tarot-card-face-disabled cursor-default opacity-45")}
          role="button"
          tabIndex={0}
          aria-label={`Pick card ${cardId + 1}`}
          onKeyDown={(event) => (event.key === "Enter" || event.key === " ") && (event.preventDefault(), !isRevealing && !picked && pickedCards.length < cardsToDraw && onPickCard(cardId, event.currentTarget))}
         >
          <div className={cn("tn-tarot-card-inner-border pointer-events-none absolute inset-1 rounded-sm opacity-60")} />
          <div className={cn("tn-tarot-card-overlay pointer-events-none absolute inset-0 rounded-md")} />
         </div>
        </div>
       );
      })}
     </div>
    ))}
   </div>
  </div>
 );
}
