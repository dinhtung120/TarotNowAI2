'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';

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
  queryFn: () => fetchJsonOrThrow<{ readers: ReaderProfile[]; totalCount: number }>(
   `/api/readers?page=${page}&pageSize=${pageSize}&specialty=${encodeURIComponent(selectedSpecialty)}&status=${encodeURIComponent(selectedStatus)}&searchTerm=${encodeURIComponent(searchTerm)}`,
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to load readers.',
   8_000,
  ),
  staleTime: 30_000,
  refetchOnWindowFocus: false,
  refetchOnReconnect: true,
  refetchOnMount: false,
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
