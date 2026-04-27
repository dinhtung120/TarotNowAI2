import type { QueryClient } from '@tanstack/react-query';
import { getUserCollection } from '@/features/collection/application/actions';
import { getReadingSetupSnapshotAction } from '@/shared/application/actions/reading-setup-snapshot';
import { getCardsCatalogAction } from '@/features/reading/application/actions/cards-catalog';
import { inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import { fetchInventoryServer } from '@/shared/infrastructure/inventory/inventoryServerActions';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { swallowPrefetch } from '@/shared/server/prefetch/runners/user/shared';

export async function prefetchCollectionPage(qc: QueryClient): Promise<void> {
 await Promise.all([
  qc.prefetchQuery({
   queryKey: userStateQueryKeys.collection.mine(),
   queryFn: async () => {
    const result = await getUserCollection();
    return result.success && Array.isArray(result.data) ? result.data : [];
   },
  }),
  qc.prefetchQuery({
   queryKey: userStateQueryKeys.reading.cardsCatalog(),
   queryFn: async () => {
    const result = await getCardsCatalogAction();
    return result.success && Array.isArray(result.data) ? result.data : [];
   },
  }),
 ]);
}

export async function prefetchInventoryPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: inventoryQueryKeys.mine(),
   queryFn: fetchInventoryServer,
  });
 });
}

/** Gộp ví + catalog bài (GET /me/reading-setup-snapshot) → hydrate cache catalog cho session sau. */
export async function prefetchReadingSetupPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: userStateQueryKeys.reading.setupSnapshot(),
   queryFn: async () => {
    const result = await getReadingSetupSnapshotAction();
    if (!result.success || !result.data) {
     throw new Error(result.error || 'reading-setup-snapshot');
    }
    const { cardsCatalog } = result.data;
    qc.setQueryData(
     userStateQueryKeys.reading.cardsCatalog(),
     Array.isArray(cardsCatalog) ? cardsCatalog : [],
    );
    return result.data;
   },
   staleTime: 60_000,
  });
 });
}
