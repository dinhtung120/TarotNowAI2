'use client';

import { useEffect, useRef, useState } from 'react';
import { getHistorySessionsAction, type HistorySessionsResponse } from '@/features/reading/application/actions/history';

interface UseHistorySessionsPageParams {
 isAuthenticated: boolean;
 networkErrorMessage: string;
 onUnauthorized: () => void;
}

export function useHistorySessionsPage({
 isAuthenticated,
 networkErrorMessage,
 onUnauthorized,
}: UseHistorySessionsPageParams) {
 const [historyData, setHistoryData] = useState<HistorySessionsResponse | null>(null);
 const [isLoading, setIsLoading] = useState(true);
 const [error, setError] = useState<string | null>(null);
 const [currentPage, setCurrentPage] = useState(1);
 const [filterType, setFilterType] = useState('all');
 const [filterDate, setFilterDate] = useState('');
 const pageSize = 10;
 const onUnauthorizedRef = useRef(onUnauthorized);

 useEffect(() => {
  onUnauthorizedRef.current = onUnauthorized;
 }, [onUnauthorized]);

 useEffect(() => {
  setCurrentPage(1);
 }, [filterDate, filterType]);

 useEffect(() => {
  if (!isAuthenticated) {
   return;
  }

  const fetchHistory = async () => {
   setIsLoading(true);
   setError(null);

   try {
    const result = await getHistorySessionsAction(currentPage, pageSize, filterType, filterDate);
    if (result.error) {
     if (result.error === 'unauthorized') {
      onUnauthorizedRef.current();
      return;
     }

     setError(result.error);
     return;
    }

    if (result.success && result.data) {
     setHistoryData(result.data);
    }
   } catch {
    setError(networkErrorMessage);
   } finally {
    setIsLoading(false);
   }
  };

  void fetchHistory();
 }, [currentPage, filterDate, filterType, isAuthenticated, networkErrorMessage]);

 const goToPrevPage = () => {
  if (currentPage > 1) {
   setCurrentPage((prev) => prev - 1);
  }
 };

 const goToNextPage = () => {
  if (historyData && currentPage < historyData.totalPages) {
   setCurrentPage((prev) => prev + 1);
  }
 };

 return {
  historyData,
  isLoading,
  error,
  currentPage,
  filterType,
  filterDate,
  setFilterType,
  setFilterDate,
  goToPrevPage,
  goToNextPage,
 };
}
