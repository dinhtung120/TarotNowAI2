import type { UserCollectionDto } from "@/features/collection/cards/actions/actions";
import type { CollectionFilter } from "@/features/collection/cards/useCollectionPage";
import { CollectionDeckCard } from "@/features/collection/cards/components/CollectionDeckCard";
import { CollectionEmptyState } from "@/features/collection/cards/components/CollectionEmptyState";
import CollectionDeckError from "@/features/collection/cards/components/collection-grid/CollectionDeckError";
import type { TarotCardMeta } from "@/shared/models/tarotData";
import { useEffect, useRef } from "react";

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
  hasMore: boolean;
  onLoadMore: () => void;
  loadingMore: boolean;
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
  hasMore,
  onLoadMore,
  loadingMore,
}: CollectionDeckGridProps) {
  const loadMoreSentinelRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    if (!hasMore) return undefined;
    const sentinel = loadMoreSentinelRef.current;
    if (!sentinel) return undefined;

    const observer = new IntersectionObserver((entries) => {
      for (const entry of entries) {
        if (entry.isIntersecting && !loadingMore) {
          onLoadMore();
          break;
        }
      }
    }, { rootMargin: "320px 0px" });

    observer.observe(sentinel);
    return () => observer.disconnect();
  }, [hasMore, loadingMore, onLoadMore, filteredDeck.length]);

  if (error) return <CollectionDeckError error={error} />;

  if (collection.length === 0 && activeFilter !== "unowned") {
    return <CollectionEmptyState title={labels.emptyTitle} description={labels.emptyDescription} cta={labels.emptyCta} />;
  }

  return (
    <div className="animate-in fade-in slide-in-from-bottom-8 grid grid-cols-2 gap-4 delay-500 duration-1000 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-7">
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
      {hasMore && (
        <div
          ref={loadMoreSentinelRef}
          className="col-span-full flex min-h-14 items-center justify-center rounded-2xl border border-dashed tn-border-soft tn-overlay-soft text-xs font-semibold tracking-wide tn-text-muted"
          aria-live="polite"
        >
          {loadingMore ? "Loading more cards..." : "Scroll to load more cards"}
        </div>
      )}
    </div>
  );
}
