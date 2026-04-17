const LOCAL_INVENTORY_SYNC_SUPPRESSION_MS = 30_000;

let lastLocalInventorySyncAt = 0;

/**
 * Marks that current tab already synced inventory cache after a local mutation.
 */
export function markLocalInventoryCacheSynced(): void {
 lastLocalInventorySyncAt = Date.now();
}

/**
 * Skips realtime inventory invalidation when local mutation just finished.
 */
export function shouldSkipRealtimeInventoryInvalidation(now: number = Date.now()): boolean {
 return now - lastLocalInventorySyncAt < LOCAL_INVENTORY_SYNC_SUPPRESSION_MS;
}
