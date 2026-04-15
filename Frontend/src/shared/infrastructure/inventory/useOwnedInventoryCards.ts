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

 const ownedCardIds = useMemo(() => {
  const collection = collectionQuery.data?.success && collectionQuery.data.data
   ? collectionQuery.data.data
   : [];
  return new Set(collection.map((card) => card.cardId));
 }, [collectionQuery.data]);

 const cardOptions = useMemo<CardOption[]>(
  () => cardsCatalog.cards
   .filter((card) => ownedCardIds.has(card.id))
   .map((card) => ({
    id: card.id,
    name: resolveCardName(locale, card),
   }))
   .sort((left, right) => left.id - right.id),
  [cardsCatalog.cards, locale, ownedCardIds],
 );

 const collectionError = resolveCollectionError(collectionQuery.data);
 const errorMessage = collectionError || cardsCatalog.error || undefined;

 return {
  cardOptions,
  isLoading: cardsCatalog.isLoading || collectionQuery.isLoading || collectionQuery.isFetching,
  error: errorMessage,
 };
}
