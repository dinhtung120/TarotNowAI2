'use client';

import { useQuery } from '@tanstack/react-query';
import { fetchJsonOrThrow } from '@/shared/infrastructure/http/clientFetch';
import { INVENTORY_API_ROUTE, inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import type { InventoryResponse } from '@/shared/infrastructure/inventory/inventoryTypes';

async function fetchInventory(): Promise<InventoryResponse> {
 return fetchJsonOrThrow<InventoryResponse>(
  INVENTORY_API_ROUTE,
  {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  },
  'Failed to load inventory.',
  8_000,
 );
}

export function useInventory() {
 return useQuery({
  queryKey: inventoryQueryKeys.mine(),
  queryFn: fetchInventory,
  select: (response: InventoryResponse) => ({
   ...response,
   items: response.items.filter((item) => item.quantity > 0),
  }),
  staleTime: 20_000,
 });
}
