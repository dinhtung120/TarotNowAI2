'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { listReaders, type ReaderProfile } from '@/features/reader/application/actions';

export function useReadersDirectoryPage() {
  const [page, setPage] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedSpecialty, setSelectedSpecialty] = useState('');
  const [selectedStatus, setSelectedStatus] = useState('');
  const pageSize = 12;

  useEffect(() => {
    const debounceTimer = window.setTimeout(() => {
      setSearchTerm(searchInput.trim());
    }, 300);

    return () => window.clearTimeout(debounceTimer);
  }, [searchInput]);

 const { data, isLoading } = useQuery({
  queryKey: ['readers', page, pageSize, selectedSpecialty, selectedStatus, searchTerm],
  queryFn: async () => {
   const result = await listReaders(
    page,
    pageSize,
    selectedSpecialty,
    selectedStatus,
    searchTerm
   );
   if (result.success && result.data) {
    return result.data;
   }
   return { readers: [] as ReaderProfile[], totalCount: 0 };
  },
  refetchOnWindowFocus: true,
  refetchOnReconnect: true,
  refetchOnMount: true,
 });

 const readers = data?.readers ?? [];
 const totalCount = data?.totalCount ?? 0;
 const loading = isLoading;

  const totalPages = useMemo(
    () => Math.ceil(totalCount / pageSize),
    [totalCount, pageSize]
  );

  return {
    readers,
    totalCount,
    page,
    setPage,
    loading,
    searchInput,
    setSearchInput,
    selectedSpecialty,
    setSelectedSpecialty,
    selectedStatus,
    setSelectedStatus,
    totalPages,
  };
}
