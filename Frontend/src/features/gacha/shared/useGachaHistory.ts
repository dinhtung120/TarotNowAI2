'use client';

import { useQuery } from '@tanstack/react-query';
import { GACHA_API_ROUTES, gachaQueryKeys } from '@/features/gacha/shared/gachaConstants';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import type { GachaHistoryPage } from '@/features/gacha/shared/gachaTypes';

interface UseGachaHistoryArgs {
  page: number;
  pageSize: number;
}

async function fetchHistory(page: number, pageSize: number): Promise<GachaHistoryPage> {
  const query = new URLSearchParams({ page: String(page), pageSize: String(pageSize) });
  return fetchJsonOrThrow<GachaHistoryPage>(
    `${GACHA_API_ROUTES.history}?${query.toString()}`,
    {
      method: 'GET',
      credentials: 'include',
      cache: 'no-store',
    },
    'Failed to load gacha history.',
    8_000,
  );
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
