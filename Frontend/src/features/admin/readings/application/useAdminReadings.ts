'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import { adminQueryKeys } from '@/features/admin/application/adminQueryKeys';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';
import { isUnauthorizedError } from '@/shared/domain/authErrors';

export interface AdminReading {
 id: string;
 userId: string;
 username: string;
 spreadType: string;
 question: string | null;
 isCompleted: boolean;
 createdAt: string;
}

export interface PaginatedResponse {
 page: number;
 pageSize: number;
 totalPages: number;
 totalCount: number;
 items: AdminReading[];
}

export interface AdminReadingsFilters {
 username: string;
 spreadType: string;
 startDate: string;
 endDate: string;
}

const EMPTY_FILTERS: AdminReadingsFilters = {
 username: '',
 spreadType: '',
 startDate: '',
 endDate: '',
};

function normalizeFilters(filters: AdminReadingsFilters): AdminReadingsFilters {
 return {
  username: filters.username.trim(),
  spreadType: filters.spreadType.trim(),
  startDate: filters.startDate.trim(),
  endDate: filters.endDate.trim(),
 };
}

export function useAdminReadings() {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [page, setPage] = useState(1);
 const [draftFilters, setDraftFilters] = useState<AdminReadingsFilters>(EMPTY_FILTERS);
 const [appliedFilters, setAppliedFilters] = useState<AdminReadingsFilters>(EMPTY_FILTERS);

 const { data, isLoading, isFetching, error } = useQuery<PaginatedResponse>({
  queryKey: adminQueryKeys.readings(page, appliedFilters),
  queryFn: async () => {
   const result = await getAllHistorySessionsAdminAction({
    page,
    pageSize: 10,
    username: appliedFilters.username,
    spreadType: appliedFilters.spreadType,
    startDate: appliedFilters.startDate
     ? new Date(appliedFilters.startDate).toISOString()
     : undefined,
    endDate: appliedFilters.endDate
     ? new Date(appliedFilters.endDate).toISOString()
     : undefined,
   });

   if (result.success && result.data) {
    return result.data as PaginatedResponse;
   }

   if (isUnauthorizedError(result.error)) {
    toast.error(t('readings.toast.unauthorized'));
   }

   throw new Error(result.error || 'Failed to load admin readings');
  },
 });

 const applyFilters = (nextDraftFilters: AdminReadingsFilters) => {
  const normalizedFilters = normalizeFilters(nextDraftFilters);
  setDraftFilters(normalizedFilters);
  setAppliedFilters(normalizedFilters);
  setPage(1);
 };

 const getSpreadLabel = (type: string) => {
  switch (type) {
   case 'daily_1':
    return t('readings.filters.spread_daily');
   case 'spread_3':
    return t('readings.filters.spread_3');
   case 'spread_5':
    return t('readings.filters.spread_5');
   case 'spread_10':
    return t('readings.filters.spread_10');
   default:
    return type.replace('_', ' ');
  }
 };

 return {
  t,
  locale,
  data,
  loading: isLoading || isFetching,
  page,
  setPage,
  filters: draftFilters,
  applyFilters,
  listError: error instanceof Error ? error.message : '',
  getSpreadLabel,
 };
}
