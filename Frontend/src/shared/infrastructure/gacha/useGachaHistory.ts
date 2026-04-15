'use client';

import { useQuery } from '@tanstack/react-query';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { GACHA_API_ROUTES, gachaQueryKeys } from '@/shared/infrastructure/gacha/gachaConstants';
import type { GachaHistoryPage } from '@/shared/infrastructure/gacha/gachaTypes';

interface UseGachaHistoryArgs {
  page: number;
  pageSize: number;
}

async function fetchHistory(page: number, pageSize: number): Promise<GachaHistoryPage> {
  const query = new URLSearchParams({ page: String(page), pageSize: String(pageSize) });
  const response = await fetch(`${GACHA_API_ROUTES.history}?${query.toString()}`, {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
  });
  if (!response.ok) throw new Error(await parseApiError(response, 'Failed to load gacha history.'));
  return (await response.json()) as GachaHistoryPage;
}

export function useGachaHistory({ page, pageSize }: UseGachaHistoryArgs) {
  const normalizedPage = Math.max(1, page);
  const normalizedPageSize = Math.max(1, Math.min(pageSize, 50));

  return useQuery({
    queryKey: gachaQueryKeys.history(normalizedPage, normalizedPageSize),
    queryFn: () => fetchHistory(normalizedPage, normalizedPageSize),
    staleTime: 10_000,
  });
}

