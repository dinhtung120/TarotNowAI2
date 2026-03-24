'use client';

import { useEffect, useMemo, useState } from 'react';
import { listReaders, type ReaderProfile } from '@/features/reader/application/actions';

export function useReadersDirectoryPage() {
  const [readers, setReaders] = useState<ReaderProfile[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
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

  useEffect(() => {
    const fetchReaders = async () => {
      setLoading(true);
      const result = await listReaders(
        page,
        pageSize,
        selectedSpecialty,
        selectedStatus,
        searchTerm
      );
      if (result.success && result.data) {
        setReaders(result.data.readers);
        setTotalCount(result.data.totalCount);
      }
      setLoading(false);
    };

    void fetchReaders();
  }, [page, selectedSpecialty, selectedStatus, searchTerm]);

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
