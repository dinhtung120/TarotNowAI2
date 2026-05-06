'use client';

import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import { GACHA_API_ROUTES, gachaQueryKeys } from '@/features/gacha/shared/gachaConstants';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import type { GachaHistoryPage, GachaPool, GachaPoolOdds } from '@/features/gacha/shared/gachaTypes';

interface UseGachaArgs {
  selectedPoolCode?: string;
  historyPreviewSize?: number;
}

async function fetchPools(): Promise<GachaPool[]> {
  return fetchJsonOrThrow<GachaPool[]>(
    GACHA_API_ROUTES.pools,
    { method: 'GET', credentials: 'include', cache: 'no-store' },
    'Failed to load gacha pools.',
    8_000,
  );
}

async function fetchPoolOdds(poolCode: string): Promise<GachaPoolOdds> {
  return fetchJsonOrThrow<GachaPoolOdds>(
    GACHA_API_ROUTES.poolOdds(poolCode),
    { method: 'GET', credentials: 'include', cache: 'no-store' },
    'Failed to load gacha odds.',
    8_000,
  );
}

async function fetchHistoryPreview(pageSize: number): Promise<GachaHistoryPage> {
  const query = new URLSearchParams({ page: '1', pageSize: String(pageSize) });
  return fetchJsonOrThrow<GachaHistoryPage>(
    `${GACHA_API_ROUTES.history}?${query.toString()}`,
    { method: 'GET', credentials: 'include', cache: 'no-store' },
    'Failed to load gacha history.',
    8_000,
  );
}

export function useGacha({ selectedPoolCode, historyPreviewSize = 6 }: UseGachaArgs = {}) {
  const poolsQuery = useQuery({ queryKey: gachaQueryKeys.pools(), queryFn: fetchPools, staleTime: 20_000 });

  const normalizedPreviewSize = Math.max(1, Math.min(historyPreviewSize, 20));
  const resolvedPoolCode = useMemo(() => {
    const normalizedSelection = selectedPoolCode?.trim().toLowerCase();
    if (normalizedSelection) return normalizedSelection;
    return poolsQuery.data?.[0]?.code ?? '';
  }, [poolsQuery.data, selectedPoolCode]);

  const poolOddsQuery = useQuery({
    queryKey: gachaQueryKeys.poolOdds(resolvedPoolCode),
    queryFn: () => fetchPoolOdds(resolvedPoolCode),
    enabled: Boolean(resolvedPoolCode),
    staleTime: 20_000,
  });

  const historyPreviewQuery = useQuery({
    queryKey: gachaQueryKeys.history(1, normalizedPreviewSize),
    queryFn: () => fetchHistoryPreview(normalizedPreviewSize),
    staleTime: 10_000,
  });

  return {
    poolsQuery,
    poolOddsQuery,
    historyPreviewQuery,
    resolvedPoolCode,
  };
}
