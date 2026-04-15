'use client';

import { useQuery } from '@tanstack/react-query';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { INVENTORY_API_ROUTE, inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import type { InventoryResponse } from '@/shared/infrastructure/inventory/inventoryTypes';

async function fetchInventory(): Promise<InventoryResponse> {
 const response = await fetch(INVENTORY_API_ROUTE, {
  method: 'GET',
  credentials: 'include',
  cache: 'no-store',
 });

 if (!response.ok) {
  const message = await parseApiError(response, 'Failed to load inventory.');
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
