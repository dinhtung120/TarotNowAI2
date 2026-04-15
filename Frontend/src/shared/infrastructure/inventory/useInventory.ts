'use client';

import { useQuery } from '@tanstack/react-query';
import { INVENTORY_API_ROUTE, inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import type { InventoryResponse } from '@/shared/infrastructure/inventory/inventoryTypes';

async function fetchInventory(): Promise<InventoryResponse> {
 const response = await fetch(INVENTORY_API_ROUTE, {
  method: 'GET',
  credentials: 'include',
  cache: 'no-store',
 });

 if (!response.ok) {
  let message = 'Failed to load inventory.';
  try {
   const payload = (await response.json()) as { detail?: string; error?: string };
   message = payload.detail || payload.error || message;
  } catch {
   // Ignore parse failure and keep fallback message.
  }

  throw new Error(message);
 }

 return (await response.json()) as InventoryResponse;
}

export function useInventory() {
 return useQuery({
  queryKey: inventoryQueryKeys.mine(),
  queryFn: fetchInventory,
  staleTime: 20_000,
 });
}
