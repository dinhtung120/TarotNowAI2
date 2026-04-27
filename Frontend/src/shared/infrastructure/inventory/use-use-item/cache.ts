import type { QueryClient } from '@tanstack/react-query';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import type { UserCollectionDto } from '@/features/collection/application/actions';
import type { ReadingSetupSnapshotDto } from '@/shared/application/actions/reading-setup-snapshot';
import type {
 UseInventoryCardStatSnapshot,
 UseInventoryItemEffectSummary,
 UseInventoryItemResponse,
} from '@/shared/infrastructure/inventory/inventoryTypes';

function isCardEnhancementSummary(summary: UseInventoryItemEffectSummary): boolean {
 return Boolean(summary.cardId) && Boolean(summary.after);
}

function isFreeDrawSummary(summary: UseInventoryItemEffectSummary): boolean {
 return summary.effectType.trim().toLowerCase() === 'free_draw'
  && typeof summary.afterValue === 'number';
}

function resolveSpreadCardCountFromItemCode(itemCode: string): 3 | 5 | 10 | null {
 const normalizedCode = itemCode.trim().toLowerCase();
 if (normalizedCode.includes('free_draw_ticket_3')) {
  return 3;
 }

 if (normalizedCode.includes('free_draw_ticket_5')) {
  return 5;
 }

 if (normalizedCode.includes('free_draw_ticket_10')) {
  return 10;
 }

 return null;
}

export function patchReadingSetupSnapshot(
 queryClient: QueryClient,
 result: UseInventoryItemResponse,
): void {
 const spreadCardCount = resolveSpreadCardCountFromItemCode(result.itemCode);
 if (!spreadCardCount) {
  return;
 }

 const freeDrawSummary = result.effectSummaries.find((summary) => isFreeDrawSummary(summary));
 const afterValue = freeDrawSummary?.afterValue;
 if (typeof afterValue !== 'number') {
  return;
 }

 queryClient.setQueryData<ReadingSetupSnapshotDto | undefined>(
  userStateQueryKeys.reading.setupSnapshot(),
  (currentSnapshot) => {
   if (!currentSnapshot) {
    return currentSnapshot;
   }

   if (spreadCardCount === 3) {
    return {
     ...currentSnapshot,
     freeDrawQuotas: {
      ...currentSnapshot.freeDrawQuotas,
      spread3: afterValue,
     },
    };
   }

   if (spreadCardCount === 5) {
    return {
     ...currentSnapshot,
     freeDrawQuotas: {
      ...currentSnapshot.freeDrawQuotas,
      spread5: afterValue,
     },
    };
   }

   return {
    ...currentSnapshot,
    freeDrawQuotas: {
     ...currentSnapshot.freeDrawQuotas,
     spread10: afterValue,
    },
   };
  },
 );
}

export function patchCollectionCache(
 queryClient: QueryClient,
 result: UseInventoryItemResponse,
): void {
 const enhancementSummary = result.effectSummaries.find((summary) => isCardEnhancementSummary(summary));
 if (!enhancementSummary?.cardId || !enhancementSummary.after) {
  return;
 }

 queryClient.setQueryData<UserCollectionDto[] | undefined>(
  userStateQueryKeys.collection.mine(),
  (currentCollection) => {
   if (!currentCollection?.length) {
    return currentCollection;
   }

   const updatedCollection = currentCollection.map((card) => {
    if (card.cardId !== enhancementSummary.cardId) {
     return card;
    }

    const nextStats = enhancementSummary.after as UseInventoryCardStatSnapshot;
    return {
     ...card,
     level: nextStats.level,
     currentExp: nextStats.currentExp,
     expToNextLevel: nextStats.expToNextLevel,
     baseAtk: nextStats.baseAtk,
     baseDef: nextStats.baseDef,
     bonusAtkPercent: nextStats.bonusAtkPercent,
     bonusDefPercent: nextStats.bonusDefPercent,
     totalAtk: nextStats.totalAtk,
     totalDef: nextStats.totalDef,
     atk: nextStats.totalAtk,
     def: nextStats.totalDef,
    };
   });

   return updatedCollection;
  },
 );
}

export function resolveInvalidationDomains(result: UseInventoryItemResponse) {
 const domains: Array<'inventory' | 'collection' | 'readingSetup' | 'profile' | 'gamification' | 'wallet'> = ['inventory'];
 const normalizedItemCode = result.itemCode.trim().toLowerCase();

 if (result.effectSummaries.some((summary) => isCardEnhancementSummary(summary))) {
  domains.push('collection');
 }

 if (result.effectSummaries.some((summary) => isFreeDrawSummary(summary))) {
  domains.push('readingSetup');
 }

 if (normalizedItemCode === 'rare_title_lucky_star') {
  domains.push('profile', 'gamification', 'wallet');
 }

 return domains;
}
