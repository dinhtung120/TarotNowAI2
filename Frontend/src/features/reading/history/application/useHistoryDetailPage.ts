'use client';

import { useEffect, useMemo, useState } from 'react';
import { getHistoryDetailAction, type HistoryDetailResponse } from '@/features/reading/application/actions/history';

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
 const [detail, setDetail] = useState<HistoryDetailResponse | null>(null);
 const [isLoading, setIsLoading] = useState(true);
 const [error, setError] = useState<string | null>(null);

 useEffect(() => {
  if (!isAuthenticated) {
   return;
  }

  const fetchDetail = async () => {
   setIsLoading(true);
   setError(null);

   try {
    const result = await getHistoryDetailAction(sessionId);
    if (result.error) {
     if (result.error === 'unauthorized') {
      onUnauthorized();
      return;
     }

     setError(result.error);
     return;
    }

    if (result.success && result.data) {
     setDetail(result.data);
    }
   } catch {
    setError(networkErrorMessage);
   } finally {
    setIsLoading(false);
   }
  };

  void fetchDetail();
 }, [isAuthenticated, networkErrorMessage, onUnauthorized, sessionId]);

 const parsedCards = useMemo<number[]>(() => {
  if (!detail?.cardsDrawn) {
   return [];
  }

  try {
   const parsed = JSON.parse(detail.cardsDrawn) as unknown;
   if (!Array.isArray(parsed)) {
    return [];
   }
   return parsed.filter((item): item is number => typeof item === 'number');
  } catch {
   return [];
  }
 }, [detail?.cardsDrawn]);

 return {
  detail,
  isLoading,
  error,
  parsedCards,
 };
}
