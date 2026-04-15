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
 itemCode: string;
 targetCardId?: number;
 idempotencyKey?: string;
}

export interface UseInventoryItemResponse {
 itemCode: string;
 targetCardId?: number;
 isIdempotentReplay: boolean;
 message: string;
}
