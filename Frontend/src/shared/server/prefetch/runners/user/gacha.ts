import type { QueryClient } from '@tanstack/react-query';
import { gachaQueryKeys } from '@/shared/infrastructure/gacha/gachaConstants';
import {
 fetchGachaHistoryServer,
 fetchGachaPoolOddsServer,
 fetchGachaPoolsServer,
} from '@/shared/infrastructure/gacha/gachaServerActions';
import type { GachaPool } from '@/shared/infrastructure/gacha/gachaTypes';
import { swallowPrefetch } from '@/shared/server/prefetch/runners/user/shared';

async function gachaPoolsQueryFn() {
 return fetchGachaPoolsServer();
}

export async function prefetchGachaPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: gachaQueryKeys.pools(),
  queryFn: gachaPoolsQueryFn,
 });

 await swallowPrefetch(async () => {
  const pools = qc.getQueryData<GachaPool[]>(gachaQueryKeys.pools());
  if (!pools?.length) {
   return;
  }
  const firstCode = pools[0].code;
  await Promise.all([
   swallowPrefetch(async () => {
    await qc.prefetchQuery({
     queryKey: gachaQueryKeys.poolOdds(firstCode),
     queryFn: () => fetchGachaPoolOddsServer(firstCode),
    });
   }),
   swallowPrefetch(async () => {
    await qc.prefetchQuery({
     queryKey: gachaQueryKeys.history(1, 6),
     queryFn: () => fetchGachaHistoryServer(1, 6),
    });
   }),
  ]);
 });
}

export async function prefetchGachaHistoryPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: gachaQueryKeys.history(1, 20),
   queryFn: () => fetchGachaHistoryServer(1, 20),
  });
 });
}
