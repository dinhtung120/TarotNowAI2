'use client';

import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { GACHA_API_ROUTES, gachaQueryKeys } from '@/shared/infrastructure/gacha/gachaConstants';
import type { GachaHistoryPage, GachaPool, GachaPoolOdds } from '@/shared/infrastructure/gacha/gachaTypes';

interface UseGachaArgs {
  selectedPoolCode?: string;
  historyPreviewSize?: number;
}

async function fetchPools(): Promise<GachaPool[]> {
  const response = await fetch(GACHA_API_ROUTES.pools, { method: 'GET', credentials: 'include', cache: 'no-store' });
  if (!response.ok) throw new Error(await parseApiError(response, 'Failed to load gacha pools.'));
  return (await response.json()) as GachaPool[];
}

async function fetchPoolOdds(poolCode: string): Promise<GachaPoolOdds> {
  const response = await fetch(GACHA_API_ROUTES.poolOdds(poolCode), { method: 'GET', credentials: 'include', cache: 'no-store' });
  if (!response.ok) throw new Error(await parseApiError(response, 'Failed to load gacha odds.'));
  return (await response.json()) as GachaPoolOdds;
}

async function fetchHistoryPreview(pageSize: number): Promise<GachaHistoryPage> {
  const query = new URLSearchParams({ page: '1', pageSize: String(pageSize) });
  const response = await fetch(`${GACHA_API_ROUTES.history}?${query.toString()}`, {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
  });
  if (!response.ok) throw new Error(await parseApiError(response, 'Failed to load gacha history.'));
  return (await response.json()) as GachaHistoryPage;
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
