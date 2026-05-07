'use client';

import { useRef } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import {
  GACHA_API_ROUTES,
  GACHA_IDEMPOTENCY_HEADER,
  gachaQueryKeys,
} from '@/features/gacha/shared/gachaConstants';
import { markLocalGachaCacheSynced } from '@/features/gacha/shared/gachaRealtimeDedup';
import { fetchJsonOrThrow } from '@/shared/http/clientFetch';
import type {
 GachaHistoryEntry,
 GachaHistoryPage,
 GachaHistoryReward,
 GachaPool,
 PullGachaPayload,
 PullGachaResult,
 PullGachaReward,
} from '@/features/gacha/shared/gachaTypes';
import { inventoryQueryKeys } from '@/features/inventory/shared/inventoryConstants';
import type { InventoryItem, InventoryResponse } from '@/features/inventory/shared/inventoryTypes';

const GACHA_HISTORY_QUERY_KEY = [...gachaQueryKeys.all, 'history'] as const;
const GACHA_PULL_TIMEOUT_MS = 12_000;

function createInventoryItemFromReward(reward: PullGachaReward, quantity: number): InventoryItem {
 const itemCode = reward.itemCode?.trim() ?? '';
 const now = new Date().toISOString();
 return {
  itemDefinitionId: reward.itemDefinitionId ?? itemCode,
  itemCode,
  itemType: reward.kind,
  enhancementType: null,
  rarity: reward.rarity,
  isConsumable: true,
  isPermanent: false,
  effectValue: 0,
  successRatePercent: 0,
  nameVi: reward.nameVi,
  nameEn: reward.nameEn,
  nameZh: reward.nameZh,
  descriptionVi: '',
  descriptionEn: '',
  descriptionZh: '',
  iconUrl: reward.iconUrl ?? null,
  quantity,
  canUse: quantity > 0,
  requiresTargetCard: false,
  blockedReason: null,
  acquiredAtUtc: now,
 };
}

function updateInventoryCacheFromGachaResult(
 queryClient: ReturnType<typeof useQueryClient>,
 result: PullGachaResult,
): void {
 queryClient.setQueryData<InventoryResponse | undefined>(inventoryQueryKeys.mine(), (currentInventory) => {
  if (!currentInventory) {
   return currentInventory;
  }

  const rewardQuantityByItemCode = new Map<string, { reward: PullGachaReward; quantity: number }>();
  for (const reward of result.rewards) {
   if (!reward.itemCode) {
    continue;
   }

   const normalizedCode = reward.itemCode.trim().toLowerCase();
   if (!normalizedCode) {
    continue;
   }

   const current = rewardQuantityByItemCode.get(normalizedCode);
   rewardQuantityByItemCode.set(normalizedCode, {
    reward,
    quantity: (current?.quantity ?? 0) + Math.max(0, reward.quantityGranted),
   });
  }

  if (rewardQuantityByItemCode.size === 0) {
   return currentInventory;
  }

  const seenItemCodes = new Set<string>();
  const updatedItems = currentInventory.items.map((item) => {
   const normalizedCode = item.itemCode.trim().toLowerCase();
   seenItemCodes.add(normalizedCode);
   const rewardEntry = rewardQuantityByItemCode.get(normalizedCode);
   const delta = rewardEntry?.quantity ?? 0;
   if (delta === 0) {
    return item;
   }

   return {
    ...item,
    quantity: item.quantity + delta,
    canUse: item.isConsumable ? item.quantity + delta > 0 : item.canUse,
   };
  });

  const newItems = Array.from(rewardQuantityByItemCode.entries())
   .filter(([itemCode, entry]) => entry.quantity > 0 && !seenItemCodes.has(itemCode))
   .map(([, entry]) => createInventoryItemFromReward(entry.reward, entry.quantity));

  return {
   ...currentInventory,
   items: [...updatedItems, ...newItems],
  };
 });
}

function createIdempotencyKey(): string {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return crypto.randomUUID();
 }

 return `${Date.now()}_${Math.random().toString(16).slice(2)}`;
}

function toHistoryReward(reward: PullGachaReward, wasPityTriggered: boolean): GachaHistoryReward {
 return {
  kind: reward.kind,
  rarity: reward.rarity,
  currency: reward.currency ?? null,
  amount: reward.amount ?? null,
  itemCode: reward.itemCode ?? null,
  quantityGranted: reward.quantityGranted,
  iconUrl: reward.iconUrl ?? null,
  nameVi: reward.nameVi,
  nameEn: reward.nameEn,
  nameZh: reward.nameZh,
  isHardPityReward: wasPityTriggered,
 };
}

function createOptimisticHistoryEntry(result: PullGachaResult, pullCount: number): GachaHistoryEntry {
 const pullOperationId = `local_${Date.now()}_${Math.random().toString(36).slice(2, 10)}`;
 const pityAfter = Math.max(0, result.currentPityCount);
 const pityBefore = Math.max(0, pityAfter - Math.max(1, pullCount));
 const rewards = result.rewards.map((reward) => toHistoryReward(reward, result.wasPityTriggered));

 return {
  pullOperationId,
  poolCode: result.poolCode,
  pullCount: Math.max(1, pullCount),
  pityBefore,
  pityAfter,
  wasPityReset: result.wasPityTriggered,
  createdAtUtc: new Date().toISOString(),
  rewards,
 };
}

function patchHistoryCaches(
 queryClient: ReturnType<typeof useQueryClient>,
 result: PullGachaResult,
 pullCount: number,
): void {
 const optimisticEntry = createOptimisticHistoryEntry(result, pullCount);
 queryClient.setQueriesData<GachaHistoryPage>({ queryKey: GACHA_HISTORY_QUERY_KEY }, (currentPage) => {
  if (!currentPage || currentPage.page !== 1) {
   return currentPage;
  }

  const nextItems = [optimisticEntry, ...currentPage.items];
  return {
   ...currentPage,
   totalCount: currentPage.totalCount + 1,
   items: nextItems.slice(0, currentPage.pageSize),
  };
 });
}

async function sendPullRequest(payload: PullGachaPayload, idempotencyKey: string): Promise<PullGachaResult> {
 return fetchJsonOrThrow<PullGachaResult>(
  GACHA_API_ROUTES.pull,
  {
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
  },
  'Failed to pull gacha.',
  GACHA_PULL_TIMEOUT_MS,
 );
}

export function usePullGacha() {
 const queryClient = useQueryClient();
 const pendingIdempotencyByPayloadRef = useRef<WeakMap<PullGachaPayload, string>>(new WeakMap());

 const resolveRequestIdempotencyKey = (payload: PullGachaPayload): string => {
  const cached = pendingIdempotencyByPayloadRef.current.get(payload);
  if (cached) {
   return cached;
  }

  if (payload.idempotencyKey?.trim()) {
   return payload.idempotencyKey.trim();
  }

  return createIdempotencyKey();
 };

  return useMutation({
  onMutate: (payload) => {
   const idempotencyKey = resolveRequestIdempotencyKey(payload);
   pendingIdempotencyByPayloadRef.current.set(payload, idempotencyKey);
   markLocalGachaCacheSynced(idempotencyKey);
   return { idempotencyKey };
  },
  mutationFn: (payload: PullGachaPayload) => {
   const activeIdempotencyKey = resolveRequestIdempotencyKey(payload);
   return sendPullRequest({ ...payload, idempotencyKey: activeIdempotencyKey }, activeIdempotencyKey);
  },
  onSuccess: (result, variables, context) => {
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

   patchHistoryCaches(queryClient, result, variables.count);
   updateInventoryCacheFromGachaResult(queryClient, result);
   markLocalGachaCacheSynced(context?.idempotencyKey);
   void queryClient.invalidateQueries({ queryKey: GACHA_HISTORY_QUERY_KEY, refetchType: 'none' });
  },
  onSettled: (_result, _error, variables) => {
   pendingIdempotencyByPayloadRef.current.delete(variables);
  },
 });
}
