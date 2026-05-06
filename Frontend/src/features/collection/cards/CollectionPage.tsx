"use client";

import { CollectionDeckGrid } from "@/features/collection/cards/components/CollectionDeckGrid";
import { CollectionFilterBar } from "@/features/collection/cards/components/CollectionFilterBar";
import { CollectionHeader } from "@/features/collection/cards/components/CollectionHeader";
import { CollectionLoadingState } from "@/features/collection/cards/components/CollectionLoadingState";
import { CollectionZoomModal } from "@/features/collection/cards/components/CollectionZoomModal";
import { useCollectionPageViewModel } from "@/features/collection/cards/useCollectionPageViewModel";
import { cn } from "@/lib/utils";

export default function CollectionPage() {
  const vm = useCollectionPageViewModel();

  if (vm.isLoading) {
    return <CollectionLoadingState />;
  }

  return (
    <div
      className={cn(
        "tn-surface tn-text-primary min-h-screen overflow-hidden font-sans selection:bg-[var(--purple-accent)]/40",
      )}
    >
      <CollectionZoomModal
        cardData={vm.selectedCardData}
        userCard={vm.selectedUserCard}
        cardImageUrl={vm.selectedCardImageUrl}
        cardPreviewImageUrl={vm.selectedCardPreviewImageUrl}
        cardName={vm.selectedCardName}
        cardMeaning={vm.selectedCardMeaning}
        suitLabel={vm.selectedSuitLabel}
        labels={vm.zoomLabels}
        onClose={() => vm.setSelectedCardId(null)}
      />

      <main
        className={cn(
          "relative z-10 container mx-auto px-4 pt-28 pb-20 sm:px-6",
        )}
      >
        <CollectionHeader
          totalCollected={vm.totalCollected}
          totalCardCount={vm.totalCardCount}
          progressRatio={vm.progressRatio}
          labels={vm.headerLabels}
        />
        <CollectionFilterBar
          activeFilter={vm.activeFilter}
          sortBy={vm.sortBy}
          labels={vm.filterBarLabels}
          onFilterChange={vm.setActiveFilter}
          onSortChange={vm.setSortBy}
        />
        <CollectionDeckGrid
          activeFilter={vm.activeFilter}
          collection={vm.collection}
          filteredDeck={vm.visibleDeck}
          error={vm.error}
          labels={vm.deckGridLabels}
          getCardImageUrl={vm.getCardImageUrl}
          getCardName={vm.getCardName}
          onSelectCard={vm.setSelectedCardId}
          hasMore={vm.hasMoreVisibleCards}
          onLoadMore={vm.requestNextVisibleChunkWindow}
          loadingMore={vm.isCatalogChunkLoading}
        />
      </main>
    </div>
  );
}
