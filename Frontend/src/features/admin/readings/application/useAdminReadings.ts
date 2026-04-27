'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
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

export function useAdminReadings() {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [page, setPage] = useState(1);
 const [username, setUsername] = useState('');
 const [spreadType, setSpreadType] = useState('');
 const [startDate, setStartDate] = useState('');
 const [endDate, setEndDate] = useState('');

 const { data, isLoading, isFetching } = useQuery<PaginatedResponse | null>({
  queryKey: ['admin', 'readings', page, username, spreadType, startDate, endDate],
  queryFn: async () => {
   const result = await getAllHistorySessionsAdminAction({
    page,
    pageSize: 10,
    username,
    spreadType,
    startDate: startDate ? new Date(startDate).toISOString() : undefined,
    endDate: endDate ? new Date(endDate).toISOString() : undefined,
   });

   if (result.success && result.data) {
    return result.data as PaginatedResponse;
   }

   if (isUnauthorizedError(result.error)) {
    toast.error(t('readings.toast.unauthorized'));
   }

   return null;
  },
 });

 const handleSearch = () => {
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
  username,
  setUsername,
  spreadType,
  setSpreadType,
  startDate,
  setStartDate,
  endDate,
  setEndDate,
  handleSearch,
  getSpreadLabel,
 };
}
