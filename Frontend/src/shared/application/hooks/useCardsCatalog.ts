'use client';

import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
 type CardCatalogItemDto,
} from '@/features/reading/application/actions/cards-catalog';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

interface UseCardsCatalogOptions {
 enabled?: boolean;
}

export function useCardsCatalog(options: UseCardsCatalogOptions = {}) {
 const enabled = options.enabled ?? true;
 const { data, error: queryError, isLoading, isFetching } = useQuery({
  queryKey: userStateQueryKeys.reading.cardsCatalog(),
  enabled,
  queryFn: () => fetchJsonOrThrow<CardCatalogItemDto[]>(
   '/api/reading/cards-catalog',
   {
    method: 'GET',
    credentials: 'include',
    cache: 'force-cache',
   },
   'Failed to load cards catalog.',
   8_000,
  ),
  staleTime: Infinity,
  gcTime: 1000 * 60 * 60 * 12,
 });

 const cards = useMemo<CardCatalogItemDto[]>(
  () => (Array.isArray(data) ? data : []),
  [data]
 );

 const cardsById = useMemo(() => {
  return new Map(cards.map((card) => [card.id, card]));
 }, [cards]);

 const error = useMemo(
  () => (queryError instanceof Error ? queryError.message : ''),
  [queryError]
 );

 const loading = enabled && (isLoading || isFetching);

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
