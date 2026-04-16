'use client';

import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
 getCardsCatalogAction,
 type CardCatalogItemDto,
} from '@/features/reading/application/actions/cards-catalog';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

export function useCardsCatalog() {
 const { data, isLoading, isFetching } = useQuery({
  queryKey: userStateQueryKeys.reading.cardsCatalog(),
  queryFn: getCardsCatalogAction,
  staleTime: Infinity,
  gcTime: 1000 * 60 * 60 * 12,
 });

 const cards = useMemo<CardCatalogItemDto[]>(
  () => (data?.success && data.data ? data.data : []),
  [data]
 );

 const cardsById = useMemo(() => {
  return new Map(cards.map((card) => [card.id, card]));
 }, [cards]);

 const error = useMemo(
  () => (data && !data.success ? data.error || 'Failed to load cards catalog' : ''),
  [data]
 );

 const loading = isLoading || isFetching;

 const getCard = (cardId: number) => cardsById.get(cardId);

 const getCardImageUrl = (cardId: number): string | undefined => {
  return getCard(cardId)?.imageUrl ?? undefined;
 };

 const getCardName = (cardId: number): string | undefined => {
  const card = getCard(cardId);
  if (!card) return undefined;

  return card.nameEn || undefined;
 };

 const getCardMeaning = (
  cardId: number,
  orientation: 'upright' | 'reversed' = 'upright',
 ): string | undefined => {
  const card = getCard(cardId);
  if (!card) return undefined;

  if (orientation === 'reversed') {
   return card.reversedDescription || card.uprightDescription || undefined;
  }

  return card.uprightDescription || undefined;
 };

 return {
  cards,
  cardsById,
  isLoading: loading,
  error,
  getCard,
  getCardImageUrl,
  getCardName,
  getCardMeaning,
 };
}
