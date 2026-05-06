import type { InventoryResponse } from '@/features/inventory/shared/inventoryTypes';

export interface UseItemMutationContext {
 intentKey: string;
 idempotencyKey: string;
 previousInventory: InventoryResponse | undefined;
}
