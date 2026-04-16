import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

export const INVENTORY_API_ROUTE = '/api/inventory';
export const INVENTORY_IDEMPOTENCY_HEADER = 'x-idempotency-key';

export const inventoryQueryKeys = {
 all: userStateQueryKeys.inventory.all,
 mine: userStateQueryKeys.inventory.mine,
};

export const inventoryItemTypes = {
 cardEnhancer: 'card_enhancer',
 readingBooster: 'reading_booster',
 consumableSpecial: 'consumable_special',
 rareTitle: 'rare_title',
} as const;
