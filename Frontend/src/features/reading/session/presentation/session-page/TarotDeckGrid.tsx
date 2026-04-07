import type { CSSProperties } from "react";
import { DECK_WAVE_ROTATION_AMPLITUDE, DECK_WAVE_Y_AMPLITUDE } from "@/features/reading/session/presentation/session-page/constants";
import { cn } from "@/lib/utils";

interface TarotDeckGridProps {
 activeDeckRows: number[][];
 cardsToDraw: number;
 deckCardWidth: string;
 horizontalOverlapFactor: number;
 isRevealing: boolean;
 pickedCards: number[];
 pickedCardSet: Set<number>;
 rowOverlapMargin: string;
 onPickCard: (cardId: number, source: HTMLDivElement | null) => void;
 setDeckCardRef: (cardId: number) => (node: HTMLDivElement | null) => void;
}

export default function TarotDeckGrid({ activeDeckRows, cardsToDraw, deckCardWidth, horizontalOverlapFactor, isRevealing, pickedCards, pickedCardSet, rowOverlapMargin, onPickCard, setDeckCardRef }: TarotDeckGridProps) {
 return (
  <div className={cn("mx-auto mb-20 w-full tn-maxw-1520 transition-opacity duration-300", pickedCards.length === cardsToDraw && "pointer-events-none opacity-35")} style={{ "--deck-card-w": deckCardWidth } as CSSProperties}>
   <div className={cn("relative mx-auto w-full tn-maxw-1420 tn-px-1-2-sm")}>
    {activeDeckRows.map((row, rowIndex) => (
     <div key={`row-${rowIndex}`} className={cn("flex items-end justify-center")} style={rowIndex === 0 ? undefined : { marginTop: rowOverlapMargin }}>
      {row.map((cardId, cardIndex) => {
       const picked = pickedCardSet.has(cardId);
       const yOffset = Math.sin(cardId * 0.55) * DECK_WAVE_Y_AMPLITUDE;
       const rotation = Math.sin(cardId * 0.35) * DECK_WAVE_ROTATION_AMPLITUDE;

       return (
        <div key={cardId} className={cn("group relative tn-tarot-card-wrap tarot-card-fan")} style={{ marginLeft: cardIndex === 0 ? 0 : `calc(var(--deck-card-w) * -${horizontalOverlapFactor})`, transform: `translateY(${yOffset}px) rotate(${rotation}deg)`, zIndex: picked ? 0 : cardIndex + rowIndex * 100 }}>
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
