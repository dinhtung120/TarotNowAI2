'use client';

import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import { getUserCollection } from '@/features/collection/application/actions';
import { useCardsCatalog } from '@/shared/application/hooks/useCardsCatalog';
import type { CardOption } from '@/components/ui/inventory/UseItemCardSelector';

function resolveCardName(
 locale: string,
 card: { nameVi: string; nameEn: string; nameZh: string }
): string {
 if (locale === 'en') return card.nameEn;
 if (locale === 'zh') return card.nameZh;
 return card.nameVi;
}

function resolveCollectionError(collectionResponse: Awaited<ReturnType<typeof getUserCollection>> | undefined): string {
 if (!collectionResponse || collectionResponse.success) {
  return '';
 }

 return collectionResponse.error || 'Failed to load user collection.';
}

interface UseOwnedInventoryCardsResult {
 cardOptions: CardOption[];
 isLoading: boolean;
 error?: string;
}

export function useOwnedInventoryCards(locale: string): UseOwnedInventoryCardsResult {
 const cardsCatalog = useCardsCatalog();
 const collectionQuery = useQuery({
  queryKey: ['collection', 'user'],
  queryFn: getUserCollection,
  staleTime: 20_000,
 });

 const collection = useMemo(() => {
  if (!collectionQuery.data?.success || !collectionQuery.data.data) {
   return [];
  }

  return collectionQuery.data.data;
 }, [collectionQuery.data]);

 const collectionMap = useMemo(
  () => new Map(collection.map((card) => [card.cardId, card])),
  [collection],
 );

 const cardOptions = useMemo<CardOption[]>(
  () => cardsCatalog.cards
   .filter((card) => collectionMap.has(card.id))
   .map((card) => {
    const userCard = collectionMap.get(card.id);
     return {
      id: card.id,
      name: resolveCardName(locale, card),
      imageUrl: card.imageUrl ?? undefined,
      stats: userCard
       ? {
        level: userCard.level,
        currentExp: userCard.currentExp,
        expToNextLevel: userCard.expToNextLevel,
        totalAtk: userCard.totalAtk,
        totalDef: userCard.totalDef,
       }
      : undefined,
    };
   })
   .sort((left, right) => left.id - right.id),
  [cardsCatalog.cards, collectionMap, locale],
 );

 const collectionError = resolveCollectionError(collectionQuery.data);
 const errorMessage = collectionError || cardsCatalog.error || undefined;

 return {
  cardOptions,
  isLoading: cardsCatalog.isLoading || collectionQuery.isLoading || collectionQuery.isFetching,
  error: errorMessage,
 };
}
