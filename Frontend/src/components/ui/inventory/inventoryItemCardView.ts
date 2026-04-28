import type { InventoryItem, InventoryRarity } from '@/shared/infrastructure/inventory/inventoryTypes';

export const inventoryRarityConfig: Record<InventoryRarity, { badgeVariant: 'amber' | 'purple' | 'info' | 'success' | 'default'; glowClass: string; label: string }> = {
 common: { badgeVariant: 'default', glowClass: 'tn-border-soft', label: 'Phổ thông' },
 uncommon: { badgeVariant: 'success', glowClass: 'tn-inventory-glow-uncommon', label: 'Bất thường' },
 rare: { badgeVariant: 'info', glowClass: 'tn-inventory-glow-rare', label: 'Hiếm' },
 epic: { badgeVariant: 'purple', glowClass: 'tn-inventory-glow-epic', label: 'Sử thi' },
 legendary: { badgeVariant: 'amber', glowClass: 'tn-inventory-glow-legendary', label: 'Huyền thoại' },
};

export const inventoryRarityGlowClass: Record<InventoryRarity, string> = {
 common: 'bg-slate-500',
 uncommon: 'bg-emerald-500',
 rare: 'bg-sky-500',
 epic: 'bg-purple-500',
 legendary: 'bg-amber-500',
};

export function resolveInventoryRarity(rarity: string): InventoryRarity {
 return rarity in inventoryRarityConfig ? rarity as InventoryRarity : 'common';
}

export function getLocalizedInventoryText(item: InventoryItem, locale: string) {
 if (locale === 'en') return { name: item.nameEn, description: item.descriptionEn };
 if (locale === 'zh') return { name: item.nameZh, description: item.descriptionZh };
 return { name: item.nameVi, description: item.descriptionVi };
}
