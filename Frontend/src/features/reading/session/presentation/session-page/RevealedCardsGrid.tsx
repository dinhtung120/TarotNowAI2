import RevealedCardItem from "@/features/reading/session/presentation/session-page/RevealedCardItem";
import { cn } from "@/lib/utils";

interface RevealedCardsGridProps {
  cards: number[];
  flippedIndex: number;
  meaningLabel: string;
  getCardImageUrl: (cardId: number) => string | undefined;
  getCardMeaning: (cardId: number) => string | null | undefined;
  getCardName: (cardId: number) => string | null | undefined;
}

export default function RevealedCardsGrid({
  cards,
  flippedIndex,
  meaningLabel,
  getCardImageUrl,
  getCardMeaning,
  getCardName,
}: RevealedCardsGridProps) {
  if (cards.length === 0) return null;

  return (
    <div className={cn("tn-revealed-grid perspective-1000")}>
      {cards.map((cardId, index) => (
        <RevealedCardItem
          key={`revealed-card-${cardId}`}
          cardId={cardId}
          cardImageUrl={getCardImageUrl(cardId)}
          cardMeaning={getCardMeaning(cardId) ?? ""}
          cardName={getCardName(cardId) ?? `Card ${cardId + 1}`}
          index={index}
          isFlipped={flippedIndex >= index}
          meaningLabel={meaningLabel}
        />
      ))}
    </div>
  );
}
