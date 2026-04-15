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
 return `${normalizedCode}:${targetCard}`;
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
   targetCardId: payload.targetCardId,
   idempotencyKey,
  }),
 });

 if (!response.ok) {
  const message = await parseApiError(response, 'Failed to use item.');
  throw new Error(message);
 }

 return (await response.json()) as UseInventoryItemResponse;
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
  },
 });
}
