'use client';

import { useEffect, useRef } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
 getHistoryDetailAction,
 type HistoryDetailResponse,
} from '@/features/reading/history/actions';
import { historyDetailQueryKey } from '@/features/reading/history/historyQueryKeys';
import { AUTH_ERROR, isUnauthorizedError } from '@/shared/models/authErrors';

interface UseHistoryDetailPageParams {
 isAuthenticated: boolean;
 sessionId: string;
 networkErrorMessage: string;
 onUnauthorized: () => void;
}

export function useHistoryDetailPage({
 isAuthenticated,
 sessionId,
 networkErrorMessage,
 onUnauthorized,
}: UseHistoryDetailPageParams) {
 const onUnauthorizedRef = useRef(onUnauthorized);

 useEffect(() => {
  onUnauthorizedRef.current = onUnauthorized;
 }, [onUnauthorized]);

 const query = useQuery<HistoryDetailResponse, Error>({
  queryKey: historyDetailQueryKey(sessionId),
  enabled: isAuthenticated && Boolean(sessionId),
  queryFn: async () => {
   const result = await getHistoryDetailAction(sessionId);
   if (result.success && result.data) {
    return result.data;
   }
   if (isUnauthorizedError(result.error)) {
    onUnauthorizedRef.current();
    throw new Error(AUTH_ERROR.UNAUTHORIZED);
   }
   throw new Error(result.error || networkErrorMessage);
  },
 });

 const detail = query.data ?? null;
 const isLoading = query.isPending || (query.isFetching && !query.data);
 const error = query.isError && query.error?.message !== AUTH_ERROR.UNAUTHORIZED
  ? query.error.message || networkErrorMessage
  : null;

 const parsedCards = parseCardsDrawn(detail?.cardsDrawn);

 return {
  detail,
  isLoading,
  error,
  parsedCards,
 };
}

function parseCardsDrawn(rawValue?: string | null): number[] {
 if (!rawValue) {
  return [];
 }

 try {
  const parsed = JSON.parse(rawValue) as unknown;
  if (!Array.isArray(parsed)) {
   return [];
  }

  return parsed.filter((item): item is number => typeof item === 'number');
 } catch {
  return [];
 }
}
