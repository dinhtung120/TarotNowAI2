import RevealedCardItem from "@/features/reading/session/components/RevealedCardItem";
import type { RevealedReadingCard } from "@/features/reading/shared/actions/types";
import { cn } from "@/lib/utils";

interface RevealedCardsGridProps {
  cards: RevealedReadingCard[];
  flippedIndex: number;
  meaningLabel: string;
  uprightLabel: string;
  reversedLabel: string;
  getCardImageUrl: (cardId: number) => string | undefined;
  getCardMeaning: (
    cardId: number,
    orientation?: "upright" | "reversed",
  ) => string | null | undefined;
  getCardName: (cardId: number) => string | null | undefined;
}

export default function RevealedCardsGrid({
  cards,
  flippedIndex,
  meaningLabel,
  uprightLabel,
  reversedLabel,
  getCardImageUrl,
  getCardMeaning,
  getCardName,
}: RevealedCardsGridProps) {
  if (cards.length === 0) return null;

  return (
    <div className={cn("tn-revealed-grid perspective-1000")}>
      {cards.map((card, index) => (
        <RevealedCardItem
          key={`revealed-card-${card.cardId}-${card.position}`}
          cardId={card.cardId}
          cardImageUrl={getCardImageUrl(card.cardId)}
          cardMeaning={getCardMeaning(card.cardId, card.orientation) ?? ""}
          cardName={getCardName(card.cardId) ?? `Card ${card.cardId + 1}`}
          index={index}
          isFlipped={flippedIndex >= index}
          meaningLabel={meaningLabel}
          orientation={card.orientation}
          orientationLabel={
            card.orientation === "reversed" ? reversedLabel : uprightLabel
          }
        />
      ))}
    </div>
  );
}
