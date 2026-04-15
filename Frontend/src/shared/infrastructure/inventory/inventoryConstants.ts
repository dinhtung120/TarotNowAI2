export const INVENTORY_API_ROUTE = '/api/inventory';
export const INVENTORY_IDEMPOTENCY_HEADER = 'x-idempotency-key';

export const inventoryQueryKeys = {
 all: ['inventory'] as const,
 mine: () => [...inventoryQueryKeys.all, 'mine'] as const,
};

export const inventoryItemTypes = {
 cardEnhancer: 'card_enhancer',
 readingBooster: 'reading_booster',
 consumableSpecial: 'consumable_special',
 rareTitle: 'rare_title',
} as const;
