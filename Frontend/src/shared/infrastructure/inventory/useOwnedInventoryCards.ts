'use client';

import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { UserCollectionDto } from '@/features/collection/application/actions';
import type { CardOption } from '@/shared/application/inventory/cardOption';
import { useCardsCatalog } from '@/shared/application/hooks/useCardsCatalog';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

function resolveCardName(
 locale: string,
 card: { nameVi: string; nameEn: string; nameZh: string },
): string {
 if (locale === 'en') return card.nameEn;
 if (locale === 'zh') return card.nameZh;
 return card.nameVi;
}

interface UseOwnedInventoryCardsResult {
 cardOptions: CardOption[];
 isLoading: boolean;
 error?: string;
}

interface UseOwnedInventoryCardsOptions {
 enabled?: boolean;
}

export function useOwnedInventoryCards(
 locale: string,
 options: UseOwnedInventoryCardsOptions = {},
): UseOwnedInventoryCardsResult {
 const enabled = options.enabled ?? true;
 const cardsCatalog = useCardsCatalog({ enabled });
 const collectionQuery = useQuery({
  queryKey: userStateQueryKeys.collection.mine(),
  enabled,
  queryFn: () => fetchJsonOrThrow<UserCollectionDto[]>(
   '/api/collection',
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to load user collection.',
   8_000,
  ),
  staleTime: 20_000,
 });

 const collection = useMemo(
  () => (Array.isArray(collectionQuery.data) ? collectionQuery.data : []),
  [collectionQuery.data],
 );
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

 const collectionError = collectionQuery.error instanceof Error ? collectionQuery.error.message : '';
 const errorMessage = collectionError || cardsCatalog.error || undefined;

 return {
  cardOptions,
  isLoading: enabled && (cardsCatalog.isLoading || collectionQuery.isLoading || collectionQuery.isFetching),
  error: errorMessage,
 };
}
