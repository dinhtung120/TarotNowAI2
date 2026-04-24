'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

export function useReadersDirectoryPage() {
 const runtimePoliciesQuery = useRuntimePolicies();
 const directoryPageSize = runtimePoliciesQuery.data?.ui.readers.directoryPageSize
  ?? RUNTIME_POLICY_FALLBACKS.ui.readers.directoryPageSize;
 const searchDebounceMs = runtimePoliciesQuery.data?.ui.search.debounceMs
  ?? RUNTIME_POLICY_FALLBACKS.ui.search.debounceMs;
 const directoryStaleMs = runtimePoliciesQuery.data?.ui.readers.directoryStaleMs
  ?? RUNTIME_POLICY_FALLBACKS.ui.readers.directoryStaleMs;
 const clientTimeoutMs = runtimePoliciesQuery.data?.http.clientTimeoutMs
  ?? RUNTIME_POLICY_FALLBACKS.http.clientTimeoutMs;
 const [page, setPage] = useState(1);
 const [searchInput, setSearchInput] = useState('');
 const [searchTerm, setSearchTerm] = useState('');
 const [selectedSpecialty, setSelectedSpecialty] = useState('');
 const [selectedStatus, setSelectedStatus] = useState('');
 const pageSize = directoryPageSize;

 useEffect(() => {
  const debounceTimer = window.setTimeout(() => {
   setSearchTerm(searchInput.trim());
  }, searchDebounceMs);

  return () => window.clearTimeout(debounceTimer);
 }, [searchDebounceMs, searchInput]);

 const { data, isLoading } = useQuery({
  queryKey: ['readers', page, pageSize, selectedSpecialty, selectedStatus, searchTerm],
  queryFn: ({ signal }) => fetchJsonOrThrow<{ readers: ReaderProfile[]; totalCount: number }>(
   `/api/readers?page=${page}&pageSize=${pageSize}&specialty=${encodeURIComponent(selectedSpecialty)}&status=${encodeURIComponent(selectedStatus)}&searchTerm=${encodeURIComponent(searchTerm)}`,
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
    signal,
   },
   'Failed to load readers.',
   clientTimeoutMs,
  ),
  staleTime: directoryStaleMs,
  refetchOnWindowFocus: false,
  refetchOnReconnect: true,
  refetchOnMount: false,
 });

 const readers = data?.readers ?? [];
 const totalCount = data?.totalCount ?? 0;
 const loading = isLoading;

 const totalPages = useMemo(
  () => Math.ceil(totalCount / pageSize),
  [totalCount, pageSize],
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
