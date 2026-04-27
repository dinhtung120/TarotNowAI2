import type { UseInventoryItemPayload } from '@/shared/infrastructure/inventory/inventoryTypes';

export function createIdempotencyKey(): string {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return crypto.randomUUID();
 }

 return `${Date.now()}_${Math.random().toString(16).slice(2)}`;
}

export function normalizeIntentKey(payload: UseInventoryItemPayload): string {
 const normalizedCode = payload.itemCode.trim().toLowerCase();
 const targetCard = payload.targetCardId ?? 'none';
 const quantity = payload.quantity;
 return `${normalizedCode}:${targetCard}:${quantity}`;
}
