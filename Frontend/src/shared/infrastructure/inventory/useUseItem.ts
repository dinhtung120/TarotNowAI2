'use client';

import { useRef } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { useWalletStore } from '@/store/walletStore';
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
  const response = await fetch(INVENTORY_API_ROUTE, {
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
  });

  if (!response.ok) {
    const message = await parseApiError(response, 'Failed to use item.');
    throw new Error(message);
  }

  const rawData = await response.json();
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

   pendingIntentKeysRef.current.set(intentKey, currentIdempotencyKey);
   return sendUseItemRequest(payload, currentIdempotencyKey);
  },
  onSuccess: async (_, variables, context) => {
   const intentKey = context?.intentKey ?? normalizeIntentKey(variables);
   pendingIntentKeysRef.current.delete(intentKey);
   await invalidateUserStateQueries(queryClient, [
    'inventory',
    'collection',
    'readingSetup',
    'wallet',
    'gamification',
    'profile',
    'notifications',
   ]);
   void useWalletStore.getState().fetchBalance();
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
