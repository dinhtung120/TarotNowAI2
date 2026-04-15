'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
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

async function useItemRequest(payload: UseInventoryItemPayload): Promise<UseInventoryItemResponse> {
 const idempotencyKey = payload.idempotencyKey || createIdempotencyKey();
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
  let message = 'Failed to use item.';
  try {
   const payloadJson = (await response.json()) as { detail?: string; error?: string };
   message = payloadJson.detail || payloadJson.error || message;
  } catch {
   // Ignore parse failure and keep fallback message.
  }

  throw new Error(message);
 }

 return (await response.json()) as UseInventoryItemResponse;
}

export function useUseItem() {
 const queryClient = useQueryClient();

 return useMutation({
  mutationFn: useItemRequest,
  onSuccess: async () => {
   await queryClient.invalidateQueries({ queryKey: inventoryQueryKeys.mine() });
  },
 });
}
