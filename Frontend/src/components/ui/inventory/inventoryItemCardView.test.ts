import { describe, expect, it } from 'vitest';
import {
 getLocalizedInventoryText,
 inventoryRarityConfig,
 inventoryRarityGlowClass,
 resolveInventoryRarity,
} from '@/components/ui/inventory/inventoryItemCardView';

const ITEM = {
 rarity: 'legendary',
 nameVi: 'La bai',
 nameEn: 'Card',
 nameZh: 'Ka pai',
 descriptionVi: 'Mo ta',
 descriptionEn: 'Description',
 descriptionZh: 'Miao shu',
} as never;

describe('inventoryItemCardView', () => {
 it('normalizes rarity config and localized copy', () => {
  expect(resolveInventoryRarity('epic')).toBe('epic');
  expect(resolveInventoryRarity('unknown')).toBe('common');
  expect(getLocalizedInventoryText(ITEM, 'en')).toEqual({ name: 'Card', description: 'Description' });
  expect(getLocalizedInventoryText(ITEM, 'zh')).toEqual({ name: 'Ka pai', description: 'Miao shu' });
  expect(getLocalizedInventoryText(ITEM, 'vi')).toEqual({ name: 'La bai', description: 'Mo ta' });
  expect(inventoryRarityConfig.legendary.label).toBe('Huyền thoại');
  expect(inventoryRarityGlowClass.rare).toBe('bg-sky-500');
 });
});
