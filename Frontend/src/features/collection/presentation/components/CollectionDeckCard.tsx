import type { UserCollectionDto } from "@/features/collection/application/actions";
import type { TarotCardMeta } from "@/shared/domain/tarotData";
import CollectionDeckCardFooter from "@/features/collection/presentation/components/deck-card/CollectionDeckCardFooter";
import CollectionDeckCardHeader from "@/features/collection/presentation/components/deck-card/CollectionDeckCardHeader";
import CollectionDeckCardVisual from "@/features/collection/presentation/components/deck-card/CollectionDeckCardVisual";
import { cn } from "@/lib/utils";

interface CollectionDeckCardProps {
  cardImageUrl?: string;
  cardName: string;
  deckCard: TarotCardMeta;
  onSelect: (cardId: number) => void;
  unknownCardLabel: string;
  userCard?: UserCollectionDto | null;
}

export function CollectionDeckCard({
  cardImageUrl,
  cardName,
  deckCard,
  onSelect,
  unknownCardLabel,
  userCard,
}: CollectionDeckCardProps) {
  const isOwned = Boolean(userCard);

  return (
    <button
      type="button"
      onClick={() => onSelect(deckCard.id)}
      data-tn-collection-card="true"
      data-tn-card-id={deckCard.id}
      className={cn(
        "tn-panel group relative flex cursor-pointer flex-col items-center overflow-hidden rounded-3xl border p-3 text-left transition-transform duration-500",
        isOwned
          ? "hover:tn-surface-strong hover:border-[var(--warning)]/40 hover:shadow-[0_10px_30px_var(--c-245-158-11-08)]"
          : "tn-panel-overlay-soft opacity-50 grayscale",
      )}
    >
      <div className={cn("mb-2 w-full")}>
        <CollectionDeckCardHeader userCard={userCard ?? null} />
      </div>
      <CollectionDeckCardVisual
        cardImageUrl={cardImageUrl}
        cardName={cardName}
        deckCard={deckCard}
        isOwned={isOwned}
        unknownCardLabel={unknownCardLabel}
      />
      <h4
        className={cn(
          "mb-3 flex min-h-[1.5rem] items-center justify-center px-1 text-center text-[11px] leading-snug font-black tracking-tight",
          isOwned ? "tn-text-primary" : "tn-text-muted",
        )}
      >
        {isOwned ? cardName || unknownCardLabel : unknownCardLabel}
      </h4>
      <CollectionDeckCardFooter isOwned={isOwned} userCard={userCard ?? null} />
    </button>
  );
}
