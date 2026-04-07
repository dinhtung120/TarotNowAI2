import type { UserCollectionDto } from "@/features/collection/application/actions";
import type { CollectionFilter } from "@/features/collection/application/useCollectionPage";
import { CollectionDeckCard } from "@/features/collection/presentation/components/CollectionDeckCard";
import { CollectionEmptyState } from "@/features/collection/presentation/components/CollectionEmptyState";
import CollectionDeckError from "@/features/collection/presentation/components/collection-grid/CollectionDeckError";
import type { TarotCardMeta } from "@/shared/domain/tarotData";
import { cn } from "@/lib/utils";

interface CollectionDeckGridProps {
  activeFilter: CollectionFilter;
  collection: UserCollectionDto[];
  error: string;
  filteredDeck: TarotCardMeta[];
  getCardImageUrl: (cardId: number) => string | undefined;
  getCardName: (cardId: number) => string | undefined;
  labels: {
    emptyCta: string;
    emptyDescription: string;
    emptyTitle: string;
    unknownCard: string;
  };
  onSelectCard: (cardId: number) => void;
}

export function CollectionDeckGrid({
  activeFilter,
  collection,
  error,
  filteredDeck,
  getCardImageUrl,
  getCardName,
  labels,
  onSelectCard,
}: CollectionDeckGridProps) {
  if (error) return <CollectionDeckError error={error} />;

  if (collection.length === 0 && activeFilter !== "unowned") {
    return (
      <CollectionEmptyState
        title={labels.emptyTitle}
        description={labels.emptyDescription}
        cta={labels.emptyCta}
      />
    );
  }

  return (
    <div
      className={cn(
        "animate-in fade-in slide-in-from-bottom-8 grid grid-cols-2 gap-4 delay-500 duration-1000 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-7",
      )}
    >
      {filteredDeck.map((deckCard) => (
        <CollectionDeckCard
          key={deckCard.id}
          deckCard={deckCard}
          userCard={collection.find((card) => card.cardId === deckCard.id)}
          cardImageUrl={getCardImageUrl(deckCard.id)}
          cardName={getCardName(deckCard.id) ?? ""}
          unknownCardLabel={labels.unknownCard}
          onSelect={onSelectCard}
        />
      ))}
    </div>
  );
}
