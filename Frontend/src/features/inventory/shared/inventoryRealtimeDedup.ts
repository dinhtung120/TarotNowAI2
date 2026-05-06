const LOCAL_INVENTORY_CORRELATION_TTL_MS = 2 * 60_000;
const localInventoryCorrelations = new Map<string, number>();

function cleanupExpiredCorrelations(now: number): void {
 for (const [correlationKey, expiresAt] of localInventoryCorrelations) {
  if (expiresAt <= now) {
   localInventoryCorrelations.delete(correlationKey);
  }
 }
}

/**
 * Marks that current tab already synced inventory cache for a specific local mutation correlation key.
 */
export function markLocalInventoryCacheSynced(correlationKey?: string): void {
 if (!correlationKey) {
  return;
 }

 const normalizedKey = correlationKey.trim();
 if (!normalizedKey) {
  return;
 }

 const now = Date.now();
 cleanupExpiredCorrelations(now);
 localInventoryCorrelations.set(normalizedKey, now + LOCAL_INVENTORY_CORRELATION_TTL_MS);
}

/**
 * Skips realtime inventory invalidation only when event carries the same correlation key as a local mutation.
 */
export function shouldSkipRealtimeInventoryInvalidation(correlationKey?: string, now: number = Date.now()): boolean {
 cleanupExpiredCorrelations(now);
 if (!correlationKey) {
  return false;
 }

 const normalizedKey = correlationKey.trim();
 if (!normalizedKey) {
  return false;
 }

 const expiresAt = localInventoryCorrelations.get(normalizedKey);
 if (!expiresAt || expiresAt <= now) {
  localInventoryCorrelations.delete(normalizedKey);
  return false;
 }

 // Consume once to avoid over-skipping follow-up realtime events.
 localInventoryCorrelations.delete(normalizedKey);
 return true;
}
