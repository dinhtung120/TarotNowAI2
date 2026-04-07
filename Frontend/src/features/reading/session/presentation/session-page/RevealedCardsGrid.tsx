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
    <div className={cn("grid grid-cols-2 items-start gap-3 perspective-1000 sm:grid-cols-3 md:gap-4")}>
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
