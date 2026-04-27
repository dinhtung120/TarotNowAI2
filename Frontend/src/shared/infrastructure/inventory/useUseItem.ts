'use client';

import { useRef, type MutableRefObject } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { markLocalInventoryCacheSynced } from '@/shared/infrastructure/inventory/inventoryRealtimeDedup';
import { inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import type { InventoryResponse, UseInventoryItemPayload } from '@/shared/infrastructure/inventory/inventoryTypes';
import { invalidateUserStateQueries } from '@/shared/infrastructure/query/invalidateUserStateQueries';
import { createIdempotencyKey, normalizeIntentKey } from '@/shared/infrastructure/inventory/use-use-item/idempotency';
import { sendUseItemRequest } from '@/shared/infrastructure/inventory/use-use-item/normalizer';
import {
 patchCollectionCache,
 patchReadingSetupSnapshot,
 resolveInvalidationDomains,
} from '@/shared/infrastructure/inventory/use-use-item/cache';
import { applyOptimisticInventoryUsage } from '@/shared/infrastructure/inventory/use-use-item/optimistic';
import type { UseItemMutationContext } from '@/shared/infrastructure/inventory/use-use-item/types';

function resolvePayloadWithIdempotency(
 payload: Readonly<UseInventoryItemPayload>,
 pendingIntentKeysRef: MutableRefObject<Map<string, string>>,
): UseInventoryItemPayload {
 const intentKey = normalizeIntentKey(payload);
 const idempotencyKey = pendingIntentKeysRef.current.get(intentKey)
  || payload.idempotencyKey
  || createIdempotencyKey();

 pendingIntentKeysRef.current.set(intentKey, idempotencyKey);
 return {
  ...payload,
  idempotencyKey,
 };
}

export function useUseItem() {
 const queryClient = useQueryClient();
 const pendingIntentKeysRef = useRef<Map<string, string>>(new Map());

 return useMutation({
  onMutate: async (payload: UseInventoryItemPayload): Promise<UseItemMutationContext> => {
   const intentKey = normalizeIntentKey(payload);
   const idempotencyKey = payload.idempotencyKey
    || pendingIntentKeysRef.current.get(intentKey)
    || createIdempotencyKey();

   pendingIntentKeysRef.current.set(intentKey, idempotencyKey);
   await queryClient.cancelQueries({ queryKey: inventoryQueryKeys.mine() });
   const previousInventory = queryClient.getQueryData<InventoryResponse>(inventoryQueryKeys.mine());

   queryClient.setQueryData<InventoryResponse | undefined>(inventoryQueryKeys.mine(), (currentInventory) =>
    applyOptimisticInventoryUsage(currentInventory, payload)
   );

   return {
    intentKey,
    idempotencyKey,
    previousInventory,
   };
  },
  mutationFn: async (payload: UseInventoryItemPayload) => {
   const resolvedPayload = resolvePayloadWithIdempotency(payload, pendingIntentKeysRef);
   const idempotencyKey = resolvedPayload.idempotencyKey ?? createIdempotencyKey();
   pendingIntentKeysRef.current.set(normalizeIntentKey(resolvedPayload), idempotencyKey);
   markLocalInventoryCacheSynced(idempotencyKey);
   return sendUseItemRequest(resolvedPayload, idempotencyKey);
  },
  onSuccess: (result, variables, context) => {
   const intentKey = context?.intentKey ?? normalizeIntentKey(variables);
   pendingIntentKeysRef.current.delete(intentKey);
   if (context?.idempotencyKey) {
    markLocalInventoryCacheSynced(context.idempotencyKey);
   }

   patchReadingSetupSnapshot(queryClient, result);
   patchCollectionCache(queryClient, result);

   const domains = resolveInvalidationDomains(result);
   const nonInventoryDomains = domains.filter((domain) => domain !== 'inventory');
   void queryClient.invalidateQueries({ queryKey: inventoryQueryKeys.mine(), refetchType: 'none' });
   if (nonInventoryDomains.length > 0) {
    void invalidateUserStateQueries(queryClient, nonInventoryDomains);
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
