const LOCAL_GACHA_CORRELATION_TTL_MS = 2 * 60_000;
const localGachaCorrelations = new Map<string, number>();

function cleanupExpiredCorrelations(now: number): void {
 for (const [correlationKey, expiresAt] of localGachaCorrelations) {
  if (expiresAt <= now) {
   localGachaCorrelations.delete(correlationKey);
  }
 }
}

/**
 * Marks that current tab already synced gacha caches for a specific local mutation correlation key.
 */
export function markLocalGachaCacheSynced(correlationKey?: string): void {
 if (!correlationKey) {
  return;
 }

 const normalizedKey = correlationKey.trim();
 if (!normalizedKey) {
  return;
 }

 const now = Date.now();
 cleanupExpiredCorrelations(now);
 localGachaCorrelations.set(normalizedKey, now + LOCAL_GACHA_CORRELATION_TTL_MS);
}

/**
 * Prevents duplicate invalidation only when realtime event correlation key matches a local mutation.
 */
export function shouldSkipRealtimeGachaInvalidation(correlationKey?: string, now: number = Date.now()): boolean {
 cleanupExpiredCorrelations(now);
 if (!correlationKey) {
  return false;
 }

 const normalizedKey = correlationKey.trim();
 if (!normalizedKey) {
  return false;
 }

 const expiresAt = localGachaCorrelations.get(normalizedKey);
 if (!expiresAt || expiresAt <= now) {
  localGachaCorrelations.delete(normalizedKey);
  return false;
 }

 // Consume once to avoid over-skipping follow-up realtime events.
 localGachaCorrelations.delete(normalizedKey);
 return true;
}
