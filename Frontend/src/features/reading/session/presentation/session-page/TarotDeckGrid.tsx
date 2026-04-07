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
  <div className={cn("mx-auto mb-20 w-full max-w-[1520px] transition-opacity duration-300", pickedCards.length === cardsToDraw && "pointer-events-none opacity-35")} style={{ "--deck-card-w": deckCardWidth } as CSSProperties}>
   <div className={cn("relative mx-auto w-full max-w-[1420px] px-1 sm:px-2")}>
    {activeDeckRows.map((row, rowIndex) => <div key={`row-${rowIndex}`} className={cn("flex items-end justify-center")} style={rowIndex === 0 ? undefined : { marginTop: rowOverlapMargin }}>{row.map((cardId, cardIndex) => { const picked = pickedCardSet.has(cardId); const yOffset = Math.sin(cardId * 0.55) * DECK_WAVE_Y_AMPLITUDE; const rotation = Math.sin(cardId * 0.35) * DECK_WAVE_ROTATION_AMPLITUDE; return <div key={cardId} className={cn("group relative aspect-[14/22] w-[var(--deck-card-w)] tarot-card-fan")} style={{ marginLeft: cardIndex === 0 ? 0 : `calc(var(--deck-card-w) * -${horizontalOverlapFactor})`, transform: `translateY(${yOffset}px) rotate(${rotation}deg)`, zIndex: picked ? 0 : cardIndex + rowIndex * 100 }}><div ref={setDeckCardRef(cardId)} onClick={(event) => !isRevealing && !picked && pickedCards.length < cardsToDraw && onPickCard(cardId, event.currentTarget)} className={cn("relative h-full w-full rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-sm transition-all duration-150 ease-out tarot-deck-card", picked && "pointer-events-none scale-95 opacity-0", !picked && pickedCards.length < cardsToDraw && "opacity-90 hover:z-40 hover:scale-105 hover:-translate-y-3 hover:border-[var(--purple-accent)] hover:shadow-[0_0_10px_var(--c-168-85-247-70)] md:hover:-translate-y-5", !picked && pickedCards.length >= cardsToDraw && "cursor-default opacity-45")} role="button" tabIndex={0} aria-label={`Pick card ${cardId + 1}`} onKeyDown={(event) => (event.key === "Enter" || event.key === " ") && (event.preventDefault(), !isRevealing && !picked && pickedCards.length < cardsToDraw && onPickCard(cardId, event.currentTarget))}><div className={cn("pointer-events-none absolute inset-1 rounded-sm border border-[var(--purple-accent)]/30 opacity-60")} /><div className={cn("pointer-events-none absolute inset-0 rounded-md bg-[radial-gradient(circle_at_24%_22%,var(--c-255-255-255-15)_0,transparent_34%),linear-gradient(160deg,var(--c-61-49-80-20),transparent_65%)]")} /></div></div>; })}</div>)}
   </div>
  </div>
 );
}
