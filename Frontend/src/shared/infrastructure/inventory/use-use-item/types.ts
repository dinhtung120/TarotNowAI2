import type { InventoryResponse } from '@/shared/infrastructure/inventory/inventoryTypes';

export interface UseItemMutationContext {
 intentKey: string;
 idempotencyKey: string;
 previousInventory: InventoryResponse | undefined;
}
