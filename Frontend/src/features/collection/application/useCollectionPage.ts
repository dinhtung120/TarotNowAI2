'use client';

import { useEffect, useMemo, useState } from 'react';
import { getUserCollection, type UserCollectionDto } from '@/actions/collectionActions';
import { TAROT_CARD_COUNT, TAROT_DECK } from '@/lib/tarotData';

export type CollectionFilter = 'all' | 'owned' | 'unowned';

export function useCollectionPage() {
  const [collection, setCollection] = useState<UserCollectionDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [activeFilter, setActiveFilter] = useState<CollectionFilter>('all');
  const [selectedCardId, setSelectedCardId] = useState<number | null>(null);

  useEffect(() => {
    const fetchCollection = async () => {
      const result = await getUserCollection();
      if (result.success && result.data) {
        setCollection(result.data);
      } else {
        setError(result.error || 'Failed to load collection');
      }
      setIsLoading(false);
    };

    void fetchCollection();
  }, []);

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
    isLoading,
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
