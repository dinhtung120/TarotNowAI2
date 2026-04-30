import { useCallback, useMemo } from 'react';
import { useTranslations } from 'next-intl';
import { toCollectionImageProxyUrl } from '@/features/collection/application/collectionCatalogChunked';
import { useCollectionCatalogChunked } from '@/features/collection/application/useCollectionCatalogChunked';
import { useCollectionPage } from '@/features/collection/application/useCollectionPage';
import type { CollectionFilterBarLabels } from '@/features/collection/presentation/components/CollectionFilterBar.types';

const COLLECTION_BACK_CARD_SRC = '/images/collection/back-card.svg';

export function useCollectionPageViewModel() {
  const t = useTranslations('Collection');
  const tCommon = useTranslations('Common');
  const tTarot = useTranslations('Tarot');
  const pageState = useCollectionPage();

  const orderedCardIds = useMemo(
    () => pageState.filteredDeck.map((card) => card.id),
    [pageState.filteredDeck],
  );

  const chunkedCatalog = useCollectionCatalogChunked({
    orderedCardIds,
    selectedCardId: pageState.selectedCardData?.id ?? null,
  });

  const ownedCardIds = useMemo(
    () => new Set(pageState.collection.map((card) => card.cardId)),
    [pageState.collection],
  );

  const visibleCardIdsSet = useMemo(
    () => new Set(chunkedCatalog.visibleCardIds),
    [chunkedCatalog.visibleCardIds],
  );

  const visibleDeck = useMemo(
    () => pageState.filteredDeck.filter((card) => visibleCardIdsSet.has(card.id)),
    [pageState.filteredDeck, visibleCardIdsSet],
  );

  const getCardName = useCallback((cardId: number): string | undefined => {
    return chunkedCatalog.cardSummaryById.get(cardId)?.name ?? undefined;
  }, [chunkedCatalog.cardSummaryById]);

  const resolveOwnedCardImageUrl = useCallback((cardId: number, mode: 'thumb' | 'full'): string | undefined => {
    const summary = chunkedCatalog.cardSummaryById.get(cardId);
    if (!summary || !chunkedCatalog.manifest) return undefined;
    const sourceUrl = mode === 'full' ? summary.fullUrl : summary.thumbUrl;
    return toCollectionImageProxyUrl(sourceUrl, chunkedCatalog.manifest.version) ?? undefined;
  }, [chunkedCatalog.cardSummaryById, chunkedCatalog.manifest]);

  const getCardImageUrl = (cardId: number): string | undefined => {
    if (!ownedCardIds.has(cardId)) {
      return COLLECTION_BACK_CARD_SRC;
    }
    return resolveOwnedCardImageUrl(cardId, 'thumb');
  };

  const selectedCardId = pageState.selectedCardData?.id ?? null;
  const isSelectedCardOwned = selectedCardId !== null && ownedCardIds.has(selectedCardId);

  const selectedCardImageUrl = useMemo(() => {
    if (!pageState.selectedCardData) {
      return undefined;
    }

    if (!isSelectedCardOwned) {
      return COLLECTION_BACK_CARD_SRC;
    }

    const cardId = pageState.selectedCardData.id;
    const detailImage = chunkedCatalog.selectedCardDetail
      ? toCollectionImageProxyUrl(chunkedCatalog.selectedCardDetail.fullUrl, chunkedCatalog.selectedCardDetail.version)
      : null;
    if (detailImage) return detailImage;
    return resolveOwnedCardImageUrl(cardId, 'full');
  }, [chunkedCatalog.selectedCardDetail, isSelectedCardOwned, pageState.selectedCardData, resolveOwnedCardImageUrl]);

  const selectedCardName = useMemo(() => {
    if (!pageState.selectedCardData) return '';
    return getCardName(pageState.selectedCardData.id) ?? '';
  }, [getCardName, pageState.selectedCardData]);

  const selectedCardMeaning = useMemo(() => {
    if (!isSelectedCardOwned) return '';
    return chunkedCatalog.selectedCardDetail?.uprightDescription ?? '';
  }, [chunkedCatalog.selectedCardDetail, isSelectedCardOwned]);

  const selectedSuitLabel = useMemo(
    () => (
      pageState.selectedCardData
        ? tTarot(`suits.${pageState.selectedCardData.suit}.full`)
        : ''
    ),
    [pageState.selectedCardData, tTarot],
  );

  const filterBarLabels: CollectionFilterBarLabels = {
    filterAll: t('filter_all'),
    filterOwned: t('filter_owned'),
    filterUnowned: t('filter_unowned'),
    filtersLabel: t('filters_label'),
    sortLabel: tCommon('sort_by') || 'Sắp xếp theo',
  };

  return {
    ...pageState,
    isLoading: pageState.isLoading || chunkedCatalog.isManifestLoading,
    visibleDeck,
    hasMoreVisibleCards: chunkedCatalog.hasMoreVisibleCards,
    requestNextVisibleChunkWindow: chunkedCatalog.requestNextVisibleChunkWindow,
    isCatalogChunkLoading: chunkedCatalog.isChunkLoading,
    deckGridLabels: {
      emptyCta: t('cta_draw'),
      emptyDescription: t('empty_desc'),
      emptyTitle: t('empty_title'),
      unknownCard: t('unknown_card'),
    },
    getCardImageUrl,
    getCardName,
    filterBarLabels,
    headerLabels: {
      progressLabel: t('progress_label'),
      subtitle: t('subtitle'),
      tag: t('tag'),
      title: t('title'),
    },
    selectedCardImageUrl,
    selectedCardMeaning,
    selectedCardName,
    selectedSuitLabel,
    zoomLabels: {
      closeLabel: tCommon('close'),
      copiesLabel: t('copies_label'),
      levelLabel: t('level_label'),
      lockedMeaning: t('locked_meaning'),
      unknownCard: t('unknown_card'),
    },
  };
}
