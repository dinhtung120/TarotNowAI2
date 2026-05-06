import type { InventoryResponse, UseInventoryItemPayload } from '@/features/inventory/shared/inventoryTypes';

function normalizeItemCode(itemCode: string): string {
 return itemCode.trim().toLowerCase();
}

function resolveConsumedQuantity(quantity: number): number {
 return Math.max(1, quantity);
}

export function applyOptimisticInventoryUsage(
 currentInventory: InventoryResponse | undefined,
 payload: Readonly<UseInventoryItemPayload>,
): InventoryResponse | undefined {
 if (!currentInventory?.items?.length) {
  return currentInventory;
 }

 const targetCode = normalizeItemCode(payload.itemCode);
 const consumedQuantity = resolveConsumedQuantity(payload.quantity);
 const adjustedItems = currentInventory.items.map((item) => {
  if (normalizeItemCode(item.itemCode) !== targetCode) {
   return item;
  }

  const nextQuantity = Math.max(0, item.quantity - consumedQuantity);
  return { ...item, quantity: nextQuantity };
 });

 return {
  ...currentInventory,
  items: adjustedItems,
 };
}
