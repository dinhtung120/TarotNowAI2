const LOCAL_GACHA_SYNC_SUPPRESSION_MS = 45_000;

let lastLocalGachaSyncAt = 0;

/**
 * Marks that current tab already synced gacha caches after a local pull mutation.
 */
export function markLocalGachaCacheSynced(): void {
 lastLocalGachaSyncAt = Date.now();
}

/**
 * Prevents duplicate invalidation when realtime event arrives right after local mutation.
 */
export function shouldSkipRealtimeGachaInvalidation(now: number = Date.now()): boolean {
 return now - lastLocalGachaSyncAt < LOCAL_GACHA_SYNC_SUPPRESSION_MS;
}
