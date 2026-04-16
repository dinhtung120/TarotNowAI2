'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import {
  GACHA_API_ROUTES,
  GACHA_IDEMPOTENCY_HEADER,
  gachaQueryKeys,
} from '@/shared/infrastructure/gacha/gachaConstants';
import type { GachaPool, PullGachaPayload, PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';
import { invalidateUserStateQueries } from '@/shared/infrastructure/query/invalidateUserStateQueries';

function createIdempotencyKey(): string {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return crypto.randomUUID();
 }

 return `${Date.now()}_${Math.random().toString(16).slice(2)}`;
}

async function sendPullRequest(payload: PullGachaPayload, idempotencyKey: string): Promise<PullGachaResult> {
 const response = await fetch(GACHA_API_ROUTES.pull, {
  method: 'POST',
  credentials: 'include',
  headers: {
   'Content-Type': 'application/json',
   [GACHA_IDEMPOTENCY_HEADER]: idempotencyKey,
  },
  body: JSON.stringify({
   poolCode: payload.poolCode,
   count: payload.count,
   idempotencyKey,
  }),
 });

 if (!response.ok) throw new Error(await parseApiError(response, 'Failed to pull gacha.'));
 return (await response.json()) as PullGachaResult;
}

export function usePullGacha() {
 const queryClient = useQueryClient();

 return useMutation({
  mutationFn: (payload: PullGachaPayload) => {
   const activeIdempotencyKey = payload.idempotencyKey || createIdempotencyKey();
   return sendPullRequest(payload, activeIdempotencyKey);
  },
  onSuccess: async (result, variables) => {
   queryClient.setQueryData<GachaPool[] | undefined>(gachaQueryKeys.pools(), (currentPools) => {
    if (!currentPools?.length) {
     return currentPools;
    }

    const normalizedPoolCode = variables.poolCode.trim().toLowerCase();
    return currentPools.map((pool) => (
     pool.code === normalizedPoolCode
      ? { ...pool, userCurrentPity: result.currentPityCount }
      : pool
    ));
   });

   await invalidateUserStateQueries(queryClient, [
    'gacha',
    'wallet',
    'inventory',
    'collection',
    'readingSetup',
    'gamification',
    'profile',
    'notifications',
   ]);
   void useWalletStore.getState().fetchBalance();
  },
 });
}
