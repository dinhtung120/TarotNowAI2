'use client';

import { useEffect, useRef, useState } from 'react';
import { keepPreviousData, useQuery } from '@tanstack/react-query';
import {
 getHistorySessionsAction,
 type HistorySessionsResponse,
} from '@/features/reading/application/actions/history';
import {
 HISTORY_PAGE_SIZE,
 historySessionsListQueryKey,
} from '@/features/reading/history/domain/historyQueryKeys';

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
 const [currentPage, setCurrentPage] = useState(1);
 const [filterType, setFilterType] = useState('all');
 const [filterDate, setFilterDate] = useState('');
 const onUnauthorizedRef = useRef(onUnauthorized);

 useEffect(() => {
  onUnauthorizedRef.current = onUnauthorized;
 }, [onUnauthorized]);

 useEffect(() => {
  setCurrentPage(1);
 }, [filterDate, filterType]);

 const query = useQuery<HistorySessionsResponse, Error>({
  queryKey: historySessionsListQueryKey(currentPage, filterType, filterDate),
  enabled: isAuthenticated,
  placeholderData: keepPreviousData,
  queryFn: async () => {
   const result = await getHistorySessionsAction(
    currentPage,
    HISTORY_PAGE_SIZE,
    filterType,
    filterDate
   );
   if (result.success && result.data) {
    return result.data;
   }
   if (result.error === 'unauthorized') {
    onUnauthorizedRef.current();
    throw new Error('unauthorized');
   }
   throw new Error(result.error || networkErrorMessage);
  },
 });

 const historyData = query.data ?? null;
 const isLoading = query.isPending || (query.isFetching && !query.data);
 const error =
  query.isError && query.error?.message !== 'unauthorized'
   ? query.error.message || networkErrorMessage
   : null;

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
