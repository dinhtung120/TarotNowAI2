'use client';

import { useCallback, useMemo, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import {
 listMyDepositOrders,
 reconcileDepositOrder,
 type MyDepositOrderHistoryItemResponse,
} from '@/features/wallet/application/actions/deposit';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

const DEFAULT_PAGE_SIZE = 10;

type DepositHistoryStatusFilter = 'all' | 'pending' | 'success' | 'failed';

export function useDepositHistoryPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const queryClient = useQueryClient();

 const [page, setPage] = useState(1);
 const [statusFilter, setStatusFilter] = useState<DepositHistoryStatusFilter>('all');

 const queryKey = useMemo(() => {
  return userStateQueryKeys.wallet.depositOrderHistory(page, DEFAULT_PAGE_SIZE, statusFilter);
 }, [page, statusFilter]);

 const historyQuery = useQuery({
  queryKey,
  queryFn: async () => {
   const status = statusFilter === 'all' ? null : statusFilter;
   const result = await listMyDepositOrders(page, DEFAULT_PAGE_SIZE, status);
   if (!result.success || !result.data) {
    throw new Error(result.error || 'Failed to load deposit history');
   }

   return result.data;
  },
  staleTime: 10_000,
 });

 const items = historyQuery.data?.items ?? [];
 const totalPages = historyQuery.data?.totalPages ?? 1;

 const setFilter = useCallback((nextFilter: DepositHistoryStatusFilter) => {
  setPage(1);
  setStatusFilter(nextFilter);
 }, []);

 const goNext = useCallback(() => {
  setPage((currentPage) => (currentPage < totalPages ? currentPage + 1 : currentPage));
 }, [totalPages]);

 const goPrev = useCallback(() => {
  setPage((currentPage) => (currentPage > 1 ? currentPage - 1 : currentPage));
 }, []);

 const reconcilePendingOrder = useCallback(async (orderId: string) => {
  await reconcileDepositOrder(orderId);
  await queryClient.invalidateQueries({ queryKey: queryKey });
  await queryClient.invalidateQueries({ queryKey: userStateQueryKeys.wallet.balance() });
 }, [queryClient, queryKey]);

 return {
  t,
  locale,
  page,
  totalPages,
  statusFilter,
  setFilter,
  items: items as MyDepositOrderHistoryItemResponse[],
  isLoading: historyQuery.isLoading,
  isFetching: historyQuery.isFetching,
  error: historyQuery.error instanceof Error ? historyQuery.error.message : null,
  goPrev,
  goNext,
  reconcilePendingOrder,
 };
}
