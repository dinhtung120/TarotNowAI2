'use client';

import { useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { getUserCollection, type UserCollectionDto } from '@/features/collection/application/actions';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { TAROT_CARD_COUNT, TAROT_DECK } from '@/shared/domain/tarotData';

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

 const { data, isLoading, isFetching } = useQuery({
  queryKey: userStateQueryKeys.collection.mine(),
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
    const baseDeck = TAROT_DECK.filter((deckCard) => {
      const isOwned = collection.some((card) => card.cardId === deckCard.id);
      if (activeFilter === 'owned') return isOwned;
      if (activeFilter === 'unowned') return !isOwned;
      return true;
    });

    if (sortBy === 'id') return baseDeck;

    return [...baseDeck].sort((cardA, cardB) => {
      const userCardA = collection.find(c => c.cardId === cardA.id);
      const userCardB = collection.find(c => c.cardId === cardB.id);

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
  }, [collection, activeFilter, sortBy]);

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
