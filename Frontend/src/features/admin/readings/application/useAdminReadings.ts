'use client';

import { useCallback, useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';

interface AdminReading {
 id: string;
 userId: string;
 username: string;
 spreadType: string;
 question: string | null;
 isCompleted: boolean;
 createdAt: string;
}

interface PaginatedResponse {
 page: number;
 pageSize: number;
 totalPages: number;
 totalCount: number;
 items: AdminReading[];
}

export function useAdminReadings() {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [data, setData] = useState<PaginatedResponse | null>(null);
 const [loading, setLoading] = useState(true);
 const [page, setPage] = useState(1);
 const [username, setUsername] = useState('');
 const [spreadType, setSpreadType] = useState('');
 const [startDate, setStartDate] = useState('');
 const [endDate, setEndDate] = useState('');

 const fetchReadings = useCallback(
  async (
   pageNumber: number,
   currentFilters: { uname: string; type: string; start: string; end: string }
  ) => {
   setLoading(true);

   try {
    const result = await getAllHistorySessionsAdminAction({
     page: pageNumber,
     pageSize: 10,
     username: currentFilters.uname,
     spreadType: currentFilters.type,
     startDate: currentFilters.start ? new Date(currentFilters.start).toISOString() : undefined,
     endDate: currentFilters.end ? new Date(currentFilters.end).toISOString() : undefined,
    });

    if (result.success && result.data) {
     setData(result.data as PaginatedResponse);
     return;
    }

    if (result.error === 'unauthorized') {
     toast.error(t('readings.toast.unauthorized'));
    }
   } finally {
    setLoading(false);
   }
  },
  [t]
 );

 useEffect(() => {
  void fetchReadings(page, {
   uname: username,
   type: spreadType,
   start: startDate,
   end: endDate,
  });
 }, [endDate, fetchReadings, page, spreadType, startDate, username]);

 const handleSearch = (event: React.FormEvent) => {
  event.preventDefault();
  setPage(1);
  void fetchReadings(1, {
   uname: username,
   type: spreadType,
   start: startDate,
   end: endDate,
  });
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
  loading,
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
