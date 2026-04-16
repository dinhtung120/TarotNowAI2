'use client';

import { useRef } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import {
 INVENTORY_API_ROUTE,
 INVENTORY_IDEMPOTENCY_HEADER,
 inventoryQueryKeys,
} from '@/shared/infrastructure/inventory/inventoryConstants';
import type {
 UseInventoryItemPayload,
 UseInventoryItemResponse,
} from '@/shared/infrastructure/inventory/inventoryTypes';

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

function normalizeUseItemResponse(data: any): UseInventoryItemResponse {
  if (!data) return data;

  const normalizeSnapshot = (s: any) => s ? ({
    level: s.level ?? s.Level ?? 0,
    currentExp: s.currentExp ?? s.CurrentExp ?? 0,
    expToNextLevel: s.expToNextLevel ?? s.ExpToNextLevel ?? 0,
    baseAtk: s.baseAtk ?? s.BaseAtk ?? 0,
    baseDef: s.baseDef ?? s.BaseDef ?? 0,
    bonusAtkPercent: s.bonusAtkPercent ?? s.BonusAtkPercent ?? 0,
    bonusDefPercent: s.bonusDefPercent ?? s.BonusDefPercent ?? 0,
    totalAtk: s.totalAtk ?? s.TotalAtk ?? 0,
    totalDef: s.totalDef ?? s.TotalDef ?? 0,
  }) : undefined;

  const normalizeSummary = (s: any) => ({
    effectType: s.effectType ?? s.EffectType ?? '',
    rolledValue: s.rolledValue ?? s.RolledValue ?? 0,
    cardId: s.cardId ?? s.CardId,
    beforeValue: s.beforeValue ?? s.BeforeValue,
    afterValue: s.afterValue ?? s.AfterValue,
    before: normalizeSnapshot(s.before ?? s.Before),
    after: normalizeSnapshot(s.after ?? s.After),
  });

  return {
    itemCode: data.itemCode ?? data.ItemCode ?? '',
    targetCardId: data.targetCardId ?? data.TargetCardId,
    isIdempotentReplay: data.isIdempotentReplay ?? data.IsIdempotentReplay ?? false,
    message: data.message ?? data.Message ?? '',
    effectSummaries: (data.effectSummaries ?? data.EffectSummaries ?? []).map(normalizeSummary),
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
  mutationFn: async (payload: UseInventoryItemPayload) => {
   const intentKey = normalizeIntentKey(payload);
   const currentIdempotencyKey = payload.idempotencyKey
    || pendingIntentKeysRef.current.get(intentKey)
    || createIdempotencyKey();

   pendingIntentKeysRef.current.set(intentKey, currentIdempotencyKey);
   return sendUseItemRequest(payload, currentIdempotencyKey);
  },
  onSuccess: async (_, variables) => {
   const intentKey = normalizeIntentKey(variables);
   pendingIntentKeysRef.current.delete(intentKey);
   await queryClient.invalidateQueries({ queryKey: inventoryQueryKeys.mine() });
   await queryClient.invalidateQueries({ queryKey: ['collection', 'user'] });
   await queryClient.invalidateQueries({ queryKey: ['me', 'reading-setup-snapshot'] });
  },
  onError: (_, variables) => {
   const intentKey = normalizeIntentKey(variables);
   pendingIntentKeysRef.current.delete(intentKey);
  },
 });
}
