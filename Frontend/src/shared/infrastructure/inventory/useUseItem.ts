'use client';

import { useRef } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { markLocalInventoryCacheSynced } from '@/shared/infrastructure/inventory/inventoryRealtimeDedup';
import {
 INVENTORY_API_ROUTE,
 INVENTORY_IDEMPOTENCY_HEADER,
 inventoryQueryKeys,
} from '@/shared/infrastructure/inventory/inventoryConstants';
import type {
 InventoryResponse,
 UseInventoryCardStatSnapshot,
 UseInventoryItemEffectSummary,
 UseInventoryItemPayload,
 UseInventoryItemResponse,
} from '@/shared/infrastructure/inventory/inventoryTypes';
import { invalidateUserStateQueries } from '@/shared/infrastructure/query/invalidateUserStateQueries';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import type { UserCollectionDto } from '@/features/collection/application/actions';
import type { ReadingSetupSnapshotDto } from '@/shared/application/actions/reading-setup-snapshot';

function createIdempotencyKey(): string {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return crypto.randomUUID();
 }

 return `${Date.now()}_${Math.random().toString(16).slice(2)}`;
}

function normalizeIntentKey(payload: UseInventoryItemPayload): string {
 const normalizedCode = payload.itemCode.trim().toLowerCase();
 const targetCard = payload.targetCardId ?? 'none';
 const quantity = payload.quantity;
 return `${normalizedCode}:${targetCard}:${quantity}`;
}

interface InventoryEffectSummaryPayload {
 EffectType?: unknown;
 effectType?: unknown;
 RolledValue?: unknown;
 rolledValue?: unknown;
 CardId?: unknown;
 cardId?: unknown;
 BeforeValue?: unknown;
 beforeValue?: unknown;
 AfterValue?: unknown;
 afterValue?: unknown;
 Before?: InventorySnapshotPayload | null;
 before?: InventorySnapshotPayload | null;
 After?: InventorySnapshotPayload | null;
 after?: InventorySnapshotPayload | null;
}

interface InventorySnapshotPayload {
 Level?: unknown;
 level?: unknown;
 CurrentExp?: unknown;
 currentExp?: unknown;
 ExpToNextLevel?: unknown;
 expToNextLevel?: unknown;
 BaseAtk?: unknown;
 baseAtk?: unknown;
 BaseDef?: unknown;
 baseDef?: unknown;
 BonusAtkPercent?: unknown;
 bonusAtkPercent?: unknown;
 BonusDefPercent?: unknown;
 bonusDefPercent?: unknown;
 TotalAtk?: unknown;
 totalAtk?: unknown;
 TotalDef?: unknown;
 totalDef?: unknown;
}

interface UseItemResponsePayload {
 ItemCode?: unknown;
 itemCode?: unknown;
 TargetCardId?: unknown;
 targetCardId?: unknown;
 IsIdempotentReplay?: unknown;
 isIdempotentReplay?: unknown;
 Message?: unknown;
 message?: unknown;
 EffectSummaries?: unknown;
 effectSummaries?: unknown;
}

interface UseItemMutationContext {
 intentKey: string;
 previousInventory: InventoryResponse | undefined;
}

function isCardEnhancementSummary(summary: UseInventoryItemEffectSummary): boolean {
 return Boolean(summary.cardId) && Boolean(summary.after);
}

function isFreeDrawSummary(summary: UseInventoryItemEffectSummary): boolean {
 return summary.effectType.trim().toLowerCase() === 'free_draw'
  && typeof summary.afterValue === 'number';
}

function resolveSpreadCardCountFromItemCode(itemCode: string): 3 | 5 | 10 | null {
 const normalizedCode = itemCode.trim().toLowerCase();
 if (normalizedCode.includes('free_draw_ticket_3')) {
  return 3;
 }

 if (normalizedCode.includes('free_draw_ticket_5')) {
  return 5;
 }

 if (normalizedCode.includes('free_draw_ticket_10')) {
  return 10;
 }

 return null;
}

function patchReadingSetupSnapshot(
 queryClient: ReturnType<typeof useQueryClient>,
 result: UseInventoryItemResponse,
): void {
 const spreadCardCount = resolveSpreadCardCountFromItemCode(result.itemCode);
 if (!spreadCardCount) {
  return;
 }

 const freeDrawSummary = result.effectSummaries.find((summary) => isFreeDrawSummary(summary));
 const afterValue = freeDrawSummary?.afterValue;
 if (typeof afterValue !== 'number') {
  return;
 }

 queryClient.setQueryData<ReadingSetupSnapshotDto | undefined>(
  userStateQueryKeys.reading.setupSnapshot(),
  (currentSnapshot) => {
   if (!currentSnapshot) {
    return currentSnapshot;
   }

   if (spreadCardCount === 3) {
    return {
      ...currentSnapshot,
      freeDrawQuotas: {
       ...currentSnapshot.freeDrawQuotas,
       spread3: afterValue,
      },
     };
   }

   if (spreadCardCount === 5) {
    return {
      ...currentSnapshot,
      freeDrawQuotas: {
       ...currentSnapshot.freeDrawQuotas,
       spread5: afterValue,
      },
     };
   }

   return {
     ...currentSnapshot,
     freeDrawQuotas: {
      ...currentSnapshot.freeDrawQuotas,
      spread10: afterValue,
     },
    };
  },
 );
}

function patchCollectionCache(
 queryClient: ReturnType<typeof useQueryClient>,
 result: UseInventoryItemResponse,
): void {
 const enhancementSummary = result.effectSummaries.find((summary) => isCardEnhancementSummary(summary));
 if (!enhancementSummary?.cardId || !enhancementSummary.after) {
  return;
 }

 queryClient.setQueryData<UserCollectionDto[] | undefined>(
  userStateQueryKeys.collection.mine(),
  (currentCollection) => {
   if (!currentCollection?.length) {
    return currentCollection;
   }

   const updatedCollection = currentCollection.map((card) => {
    if (card.cardId !== enhancementSummary.cardId) {
     return card;
    }

    const nextStats = enhancementSummary.after as UseInventoryCardStatSnapshot;
    return {
     ...card,
     level: nextStats.level,
     currentExp: nextStats.currentExp,
     expToNextLevel: nextStats.expToNextLevel,
     baseAtk: nextStats.baseAtk,
     baseDef: nextStats.baseDef,
     bonusAtkPercent: nextStats.bonusAtkPercent,
     bonusDefPercent: nextStats.bonusDefPercent,
     totalAtk: nextStats.totalAtk,
     totalDef: nextStats.totalDef,
     atk: nextStats.totalAtk,
     def: nextStats.totalDef,
    };
   });

   return updatedCollection;
  },
 );
}

function resolveInvalidationDomains(result: UseInventoryItemResponse) {
 const domains: Array<Parameters<typeof invalidateUserStateQueries>[1][number]> = ['inventory'];
 const normalizedItemCode = result.itemCode.trim().toLowerCase();

 if (result.effectSummaries.some((summary) => isCardEnhancementSummary(summary))) {
  domains.push('collection');
 }

 if (result.effectSummaries.some((summary) => isFreeDrawSummary(summary))) {
  domains.push('readingSetup');
 }

 if (normalizedItemCode === 'rare_title_lucky_star') {
  domains.push('profile', 'gamification', 'wallet');
 }

 return domains;
}

function toNumber(value: unknown): number {
 if (typeof value === 'number' && Number.isFinite(value)) {
  return value;
 }

 if (typeof value === 'string') {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : 0;
 }

 return 0;
}

function toStringValue(value: unknown): string {
 return typeof value === 'string' ? value : '';
}

function toBoolean(value: unknown): boolean {
 return value === true;
}

function hasValue(value: unknown): boolean {
 if (value === null || value === undefined) {
  return false;
 }

 if (typeof value === 'string') {
  return value.trim().length > 0;
 }

 return true;
}

function normalizeSnapshot(snapshot: InventorySnapshotPayload | null | undefined): UseInventoryCardStatSnapshot | null {
 if (!snapshot) {
  return null;
 }

 return {
  level: toNumber(snapshot.level ?? snapshot.Level),
  currentExp: toNumber(snapshot.currentExp ?? snapshot.CurrentExp),
  expToNextLevel: toNumber(snapshot.expToNextLevel ?? snapshot.ExpToNextLevel),
  baseAtk: toNumber(snapshot.baseAtk ?? snapshot.BaseAtk),
  baseDef: toNumber(snapshot.baseDef ?? snapshot.BaseDef),
  bonusAtkPercent: toNumber(snapshot.bonusAtkPercent ?? snapshot.BonusAtkPercent),
  bonusDefPercent: toNumber(snapshot.bonusDefPercent ?? snapshot.BonusDefPercent),
  totalAtk: toNumber(snapshot.totalAtk ?? snapshot.TotalAtk),
  totalDef: toNumber(snapshot.totalDef ?? snapshot.TotalDef),
 };
}

function normalizeSummary(rawSummary: InventoryEffectSummaryPayload): UseInventoryItemEffectSummary {
 const cardId = rawSummary.cardId ?? rawSummary.CardId;
 const beforeValue = rawSummary.beforeValue ?? rawSummary.BeforeValue;
 const afterValue = rawSummary.afterValue ?? rawSummary.AfterValue;

 return {
  effectType: toStringValue(rawSummary.effectType ?? rawSummary.EffectType),
  rolledValue: toNumber(rawSummary.rolledValue ?? rawSummary.RolledValue),
  cardId: hasValue(cardId) ? toNumber(cardId) : null,
  beforeValue: hasValue(beforeValue) ? toNumber(beforeValue) : null,
  afterValue: hasValue(afterValue) ? toNumber(afterValue) : null,
  before: normalizeSnapshot(rawSummary.before ?? rawSummary.Before),
  after: normalizeSnapshot(rawSummary.after ?? rawSummary.After),
 };
}

function normalizeUseItemResponse(rawData: unknown): UseInventoryItemResponse {
 const data = (rawData as UseItemResponsePayload | null) ?? {};
 const effectSummariesRaw = data.effectSummaries ?? data.EffectSummaries;
 const targetCardId = data.targetCardId ?? data.TargetCardId;
 const effectSummaries = Array.isArray(effectSummariesRaw)
  ? effectSummariesRaw.map((summary) => normalizeSummary((summary as InventoryEffectSummaryPayload | null) ?? {}))
  : [];

 return {
  itemCode: toStringValue(data.itemCode ?? data.ItemCode),
  targetCardId: hasValue(targetCardId) ? toNumber(targetCardId) : undefined,
  isIdempotentReplay: toBoolean(data.isIdempotentReplay ?? data.IsIdempotentReplay),
  message: toStringValue(data.message ?? data.Message),
  effectSummaries,
 };
}

async function sendUseItemRequest(
  payload: UseInventoryItemPayload,
  idempotencyKey: string,
): Promise<UseInventoryItemResponse> {
  const rawData = await fetchJsonOrThrow<unknown>(
   INVENTORY_API_ROUTE,
   {
    method: 'POST',
    credentials: 'include',
    headers: {
     'Content-Type': 'application/json',
     [INVENTORY_IDEMPOTENCY_HEADER]: idempotencyKey,
    },
    body: JSON.stringify({
     itemCode: payload.itemCode,
     quantity: payload.quantity,
     targetCardId: payload.targetCardId,
     idempotencyKey,
    }),
   },
   'Failed to use item.',
   10_000,
  );

  return normalizeUseItemResponse(rawData);
}

export function useUseItem() {
 const queryClient = useQueryClient();
 const pendingIntentKeysRef = useRef<Map<string, string>>(new Map());

 return useMutation({
  onMutate: async (payload): Promise<UseItemMutationContext> => {
   const intentKey = normalizeIntentKey(payload);
   await queryClient.cancelQueries({ queryKey: inventoryQueryKeys.mine() });
   const previousInventory = queryClient.getQueryData<InventoryResponse>(inventoryQueryKeys.mine());

   queryClient.setQueryData<InventoryResponse | undefined>(inventoryQueryKeys.mine(), (currentInventory) => {
    if (!currentInventory?.items?.length) {
     return currentInventory;
    }

    const targetCode = payload.itemCode.trim().toLowerCase();
    const adjustedItems = currentInventory.items.map((item) => {
     if (item.itemCode.trim().toLowerCase() !== targetCode) {
      return item;
     }

     const nextQuantity = Math.max(0, item.quantity - Math.max(1, payload.quantity));
     return { ...item, quantity: nextQuantity };
    });

    return { ...currentInventory, items: adjustedItems };
   });

   return {
    intentKey,
    previousInventory,
   };
  },
  mutationFn: async (payload: UseInventoryItemPayload) => {
   const intentKey = normalizeIntentKey(payload);
   const currentIdempotencyKey = payload.idempotencyKey
    || pendingIntentKeysRef.current.get(intentKey)
    || createIdempotencyKey();

   payload.idempotencyKey = currentIdempotencyKey;
   pendingIntentKeysRef.current.set(intentKey, currentIdempotencyKey);
   markLocalInventoryCacheSynced(currentIdempotencyKey);
   return sendUseItemRequest(payload, currentIdempotencyKey);
  },
  onSuccess: (result, variables, context) => {
   const intentKey = context?.intentKey ?? normalizeIntentKey(variables);
   pendingIntentKeysRef.current.delete(intentKey);
   markLocalInventoryCacheSynced(variables.idempotencyKey);
   patchReadingSetupSnapshot(queryClient, result);
   patchCollectionCache(queryClient, result);

   const domains = resolveInvalidationDomains(result);
   const nonInventoryDomains = domains.filter((domain) => domain !== 'inventory');
   void queryClient.invalidateQueries({ queryKey: inventoryQueryKeys.mine(), refetchType: 'none' });
   if (nonInventoryDomains.length > 0) {
    void invalidateUserStateQueries(queryClient, nonInventoryDomains);
   }

   if (domains.includes('wallet')) {
    void useWalletStore.getState().fetchBalance();
   }
  },
  onError: (_, variables, context) => {
   const intentKey = context?.intentKey ?? normalizeIntentKey(variables);
   pendingIntentKeysRef.current.delete(intentKey);
   if (context?.previousInventory) {
    queryClient.setQueryData(inventoryQueryKeys.mine(), context.previousInventory);
   }
  },
 });
}
