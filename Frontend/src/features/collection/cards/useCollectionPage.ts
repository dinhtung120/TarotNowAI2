'use client';

import { useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { UserCollectionDto } from '@/features/collection/cards/actions/actions';
import { fetchJsonOrThrow } from '@/shared/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';
import { TAROT_CARD_COUNT, TAROT_DECK } from '@/shared/models/tarotData';

export type CollectionFilter = 'all' | 'owned' | 'unowned';
export type CollectionSortOrder = 'id' | 'level' | 'atk' | 'def';

export function useCollectionPage() {
  const [activeFilter, setActiveFilter] = useState<CollectionFilter>('all');
  /* 
   * Trạng thái sortBy lưu trữ tiêu chí sắp xếp hiện tại của bộ sưu tập.
   * Mặc định được thiết lập là 'id' để hiển thị các lá bài theo thứ tự số thứ tự gốc trong bộ bài Tarot.
   * Trước đó giá trị này là 'level', nhưng đã được thay đổi theo yêu cầu người dùng
   * để tạo ra cái nhìn nhất quán hơn khi bắt đầu vào trang.
   */
  const [sortBy, setSortBy] = useState<CollectionSortOrder>('id'); 
  const [selectedCardId, setSelectedCardId] = useState<number | null>(null);

 const { data, error: queryError, isLoading, isFetching } = useQuery({
  queryKey: userStateQueryKeys.collection.mine(),
 queryFn: () => fetchJsonOrThrow<UserCollectionDto[]>(
  '/api/collection',
  {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to load collection.',
   8_000,
  ),
  staleTime: 120_000,
  gcTime: 1000 * 60 * 20,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
 });

 const collection = useMemo<UserCollectionDto[]>(
  () => (Array.isArray(data) ? data : []),
  [data]
 );
 const error = useMemo(
  () => (queryError instanceof Error ? queryError.message : ''),
  [queryError]
 );
 const loading = isLoading || isFetching;
 const collectionByCardId = useMemo(
  () => new Map(collection.map((card) => [card.cardId, card])),
  [collection],
 );
 const ownedCardIds = useMemo(
  () => new Set(collectionByCardId.keys()),
  [collectionByCardId],
 );

  const selectedCardData = useMemo(() => {
    if (selectedCardId === null) return null;
    return TAROT_DECK.find((card) => card.id === selectedCardId) ?? null;
  }, [selectedCardId]);

  const selectedUserCard = useMemo(() => {
    if (selectedCardId === null) return null;
    return collectionByCardId.get(selectedCardId) ?? null;
  }, [collectionByCardId, selectedCardId]);

  
  const filteredDeck = useMemo(() => {
    const baseDeck = TAROT_DECK.filter((deckCard) => {
      const isOwned = ownedCardIds.has(deckCard.id);
      if (activeFilter === 'owned') return isOwned;
      if (activeFilter === 'unowned') return !isOwned;
      return true;
    });

    if (sortBy === 'id') return baseDeck;

    return [...baseDeck].sort((cardA, cardB) => {
      const userCardA = collectionByCardId.get(cardA.id);
      const userCardB = collectionByCardId.get(cardB.id);

      let valA = 0;
      let valB = 0;

      switch (sortBy) {
        case 'level':
          valA = userCardA?.level ?? 0;
          valB = userCardB?.level ?? 0;
          break;
        case 'atk':
          valA = userCardA?.atk ?? 0;
          valB = userCardB?.atk ?? 0;
          break;
        case 'def':
          valA = userCardA?.def ?? 0;
          valB = userCardB?.def ?? 0;
          break;
      }

      if (valB !== valA) return valB - valA;
      return cardA.id - cardB.id;
    });
  }, [ownedCardIds, collectionByCardId, activeFilter, sortBy]);

  const totalCollected = collection.length;
  const totalCardCount = TAROT_CARD_COUNT;
  const progressRatio = (totalCollected / TAROT_CARD_COUNT) * 100;

  return {
    collection,
    isLoading: loading,
    error,
    activeFilter,
    setActiveFilter,
    sortBy,
    setSortBy,
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
