'use client';

import { useCallback, useEffect, useRef, useState } from 'react';
import { keepPreviousData, useQuery } from '@tanstack/react-query';
import {
 getHistorySessionsAction,
 type HistorySessionsResponse,
} from '@/features/reading/application/actions/history';
import {
 HISTORY_PAGE_SIZE,
 historySessionsListQueryKey,
} from '@/features/reading/history/domain/historyQueryKeys';
import { AUTH_ERROR, isUnauthorizedError } from '@/shared/domain/authErrors';

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
   if (isUnauthorizedError(result.error)) {
    onUnauthorizedRef.current();
    throw new Error(AUTH_ERROR.UNAUTHORIZED);
   }
   throw new Error(result.error || networkErrorMessage);
  },
 });

 const historyData = query.data ?? null;
 const isLoading = query.isPending || (query.isFetching && !query.data);
 const error = query.isError && query.error?.message !== AUTH_ERROR.UNAUTHORIZED
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

 const updateFilterType = useCallback((nextType: string) => {
  setCurrentPage(1);
  setFilterType(nextType);
 }, []);

 const updateFilterDate = useCallback((nextDate: string) => {
  setCurrentPage(1);
  setFilterDate(nextDate);
 }, []);

 return {
  historyData,
  isLoading,
  error,
  currentPage,
  filterType,
  filterDate,
  setFilterType: updateFilterType,
  setFilterDate: updateFilterDate,
  goToPrevPage,
  goToNextPage,
 };
}
