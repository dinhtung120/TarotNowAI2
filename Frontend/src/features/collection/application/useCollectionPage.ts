'use client';

import { useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { getUserCollection, type UserCollectionDto } from '@/features/collection/application/actions';
import { TAROT_CARD_COUNT, TAROT_DECK } from '@/shared/domain/tarotData';

export type CollectionFilter = 'all' | 'owned' | 'unowned';

export function useCollectionPage() {
  const [activeFilter, setActiveFilter] = useState<CollectionFilter>('all');
  const [selectedCardId, setSelectedCardId] = useState<number | null>(null);

 const { data, isLoading, isFetching } = useQuery({
  queryKey: ['collection', 'user'],
  queryFn: getUserCollection,
 });

 const collection = useMemo<UserCollectionDto[]>(
  () => (data?.success && data.data ? data.data : []),
  [data]
 );
 const error = useMemo(
  () => (data && !data.success ? data.error || 'Failed to load collection' : ''),
  [data]
 );
 const loading = isLoading || isFetching;

  const selectedCardData = useMemo(() => {
    if (selectedCardId === null) return null;
    return TAROT_DECK.find((card) => card.id === selectedCardId) ?? null;
  }, [selectedCardId]);

  const selectedUserCard = useMemo(() => {
    return collection.find((card) => card.cardId === selectedCardId) ?? null;
  }, [collection, selectedCardId]);

  const filteredDeck = useMemo(() => {
    return TAROT_DECK.filter((deckCard) => {
      const isOwned = collection.some((card) => card.cardId === deckCard.id);
      if (activeFilter === 'owned') return isOwned;
      if (activeFilter === 'unowned') return !isOwned;
      return true;
    });
  }, [collection, activeFilter]);

  const totalCollected = collection.length;
  const totalCardCount = TAROT_CARD_COUNT;
  const progressRatio = (totalCollected / TAROT_CARD_COUNT) * 100;

  return {
    collection,
    isLoading: loading,
    error,
    activeFilter,
    setActiveFilter,
    selectedCardId,
    setSelectedCardId,
    selectedCardData,
    selectedUserCard,
    filteredDeck,
    totalCollected,
    totalCardCount,
    progressRatio,
  };
}
