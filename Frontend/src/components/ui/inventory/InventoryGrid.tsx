'use client';

import { cn } from '@/lib/utils';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';
import InventoryItemCard from '@/components/ui/inventory/InventoryItemCard';

interface InventoryGridProps {
 items: InventoryItem[];
 locale: string;
 emptyLabel: string;
 onSelect: (item: InventoryItem) => void;
}

export default function InventoryGrid({ items, locale, emptyLabel, onSelect }: InventoryGridProps) {
 if (!items.length) {
  return (
   <div className={cn('rounded-2xl border border-dashed border-slate-300 bg-slate-50/70 px-4 py-10 text-center text-sm text-slate-600 dark:border-slate-700 dark:bg-slate-900/30 dark:text-slate-300')}>
    {emptyLabel}
   </div>
  );
 }

 return (
  <div className={cn('grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-3')}>
   {items.map((item) => (
    <InventoryItemCard key={item.itemDefinitionId} item={item} locale={locale} onSelect={onSelect} />
   ))}
  </div>
 );
}
