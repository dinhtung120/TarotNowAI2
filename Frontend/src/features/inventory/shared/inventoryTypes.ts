export type InventoryRarity = 'common' | 'uncommon' | 'rare' | 'epic' | 'legendary';

export interface InventoryItem {
 itemDefinitionId: string;
 itemCode: string;
 itemType: string;
 enhancementType?: string | null;
 rarity: InventoryRarity | string;
 isConsumable: boolean;
 isPermanent: boolean;
 effectValue: number;
 successRatePercent: number;
 nameVi: string;
 nameEn: string;
 nameZh: string;
 descriptionVi: string;
 descriptionEn: string;
 descriptionZh: string;
 iconUrl?: string | null;
 quantity: number;
 canUse: boolean;
 requiresTargetCard: boolean;
 blockedReason?: string | null;
 acquiredAtUtc: string;
}

export interface InventoryResponse {
 items: InventoryItem[];
}

export interface UseInventoryItemPayload {
 readonly itemCode: string;
 readonly quantity: number;
 readonly targetCardId?: number;
 readonly idempotencyKey?: string;
}

export interface UseInventoryCardStatSnapshot {
 level: number;
 currentExp: number;
 expToNextLevel: number;
 baseAtk: number;
 baseDef: number;
 bonusAtkPercent: number;
 bonusDefPercent: number;
 totalAtk: number;
 totalDef: number;
}

export interface UseInventoryItemEffectSummary {
 effectType: string;
 rolledValue: number;
 cardId?: number | null;
 beforeValue?: number | null;
 afterValue?: number | null;
 before?: UseInventoryCardStatSnapshot | null;
 after?: UseInventoryCardStatSnapshot | null;
}

export interface UseInventoryItemResponse {
 itemCode: string;
 targetCardId?: number;
 isIdempotentReplay: boolean;
 message: string;
 effectSummaries: UseInventoryItemEffectSummary[];
}
