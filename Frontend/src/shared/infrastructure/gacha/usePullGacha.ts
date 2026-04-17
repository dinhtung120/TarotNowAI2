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
import { inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import type { InventoryResponse } from '@/shared/infrastructure/inventory/inventoryTypes';

const CACHE_SYNC_DELAY_MS = 0;

function updateInventoryCacheFromGachaResult(
 queryClient: ReturnType<typeof useQueryClient>,
 result: PullGachaResult,
): void {
 queryClient.setQueryData<InventoryResponse | undefined>(inventoryQueryKeys.mine(), (currentInventory) => {
  if (!currentInventory?.items?.length) {
   return currentInventory;
  }

  const rewardQuantityByItemCode = new Map<string, number>();
  for (const reward of result.rewards) {
   if (!reward.itemCode) {
    continue;
   }

   const normalizedCode = reward.itemCode.trim().toLowerCase();
   if (!normalizedCode) {
    continue;
   }

   const currentQuantity = rewardQuantityByItemCode.get(normalizedCode) ?? 0;
   rewardQuantityByItemCode.set(normalizedCode, currentQuantity + Math.max(0, reward.quantityGranted));
  }

  if (rewardQuantityByItemCode.size === 0) {
   return currentInventory;
  }

  const updatedItems = currentInventory.items.map((item) => {
   const delta = rewardQuantityByItemCode.get(item.itemCode.trim().toLowerCase()) ?? 0;
   if (delta === 0) {
    return item;
   }

   return {
    ...item,
    quantity: item.quantity + delta,
    canUse: item.isConsumable ? item.quantity + delta > 0 : item.canUse,
   };
  });

  return {
   ...currentInventory,
   items: updatedItems,
  };
 });
}

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
  onSuccess: (result, variables) => {
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

   updateInventoryCacheFromGachaResult(queryClient, result);

   window.setTimeout(() => {
    void invalidateUserStateQueries(queryClient, [
     'gacha',
     'wallet',
     'inventory',
     'collection',
    ]);
    void useWalletStore.getState().fetchBalance();
   }, CACHE_SYNC_DELAY_MS);
  },
 });
}
