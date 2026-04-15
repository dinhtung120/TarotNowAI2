'use client';

import { memo } from 'react';
import { Package2 } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { InventoryItem, InventoryRarity } from '@/shared/infrastructure/inventory/inventoryTypes';

interface InventoryItemCardProps {
 item: InventoryItem;
 locale: string;
 onSelect: (item: InventoryItem) => void;
}

const rarityClasses: Record<InventoryRarity, string> = {
 common: 'border-slate-300/70 bg-slate-100/70 dark:border-slate-700 dark:bg-slate-900/30',
 uncommon: 'border-emerald-300/80 bg-emerald-50/70 dark:border-emerald-700 dark:bg-emerald-900/20',
 rare: 'border-sky-300/80 bg-sky-50/70 dark:border-sky-700 dark:bg-sky-900/20',
 epic: 'border-fuchsia-300/80 bg-fuchsia-50/70 dark:border-fuchsia-700 dark:bg-fuchsia-900/20',
 legendary: 'border-amber-300/80 bg-amber-50/70 dark:border-amber-700 dark:bg-amber-900/20',
};

function isKnownRarity(rarity: string): rarity is InventoryRarity {
 return rarity in rarityClasses;
}

function getLocalizedText(item: InventoryItem, locale: string) {
 if (locale === 'en') return { name: item.nameEn, description: item.descriptionEn };
 if (locale === 'zh') return { name: item.nameZh, description: item.descriptionZh };
 return { name: item.nameVi, description: item.descriptionVi };
}

function InventoryItemCardComponent({ item, locale, onSelect }: InventoryItemCardProps) {
 const text = getLocalizedText(item, locale);
 const rarityClass = isKnownRarity(item.rarity) ? rarityClasses[item.rarity] : rarityClasses.common;

 return (
  <button
   type="button"
   onClick={() => onSelect(item)}
   className={cn(
    'group flex w-full flex-col gap-3 rounded-2xl border p-4 text-left transition-all duration-200',
    'hover:-translate-y-0.5 hover:shadow-md focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-sky-500',
    rarityClass,
   )}
   aria-label={text.name}
  >
   <div className={cn('flex items-start justify-between gap-3')}>
    <div className={cn('min-w-0')}>
     <p className={cn('truncate text-sm font-semibold text-slate-900 dark:text-slate-100')}>{text.name}</p>
     <p className={cn('line-clamp-2 text-xs text-slate-600 dark:text-slate-300')}>{text.description}</p>
    </div>
    <div className={cn('rounded-lg border border-slate-200 bg-white/70 p-2 dark:border-slate-700 dark:bg-slate-900/50')}>
     <Package2 className={cn('h-4 w-4 text-slate-700 dark:text-slate-200')} />
    </div>
   </div>
   <div className={cn('flex items-center justify-between text-xs')}>
    <span className={cn('rounded-md bg-black/5 px-2 py-1 font-medium uppercase tracking-wide text-slate-700 dark:bg-white/10 dark:text-slate-200')}>
     {item.rarity}
    </span>
    <span className={cn('font-semibold text-slate-800 dark:text-slate-100')}>x{item.quantity}</span>
   </div>
  </button>
 );
}

const InventoryItemCard = memo(InventoryItemCardComponent);
InventoryItemCard.displayName = 'InventoryItemCard';

export default InventoryItemCard;
