'use client';

import { useMemo, useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import InventoryGrid from '@/components/ui/inventory/InventoryGrid';
import InventoryPageHero from '@/components/ui/inventory/InventoryPageHero';
import InventoryQueryState from '@/components/ui/inventory/InventoryQueryState';
import UseItemModal from '@/components/ui/inventory/UseItemModal';
import { useCardsCatalog } from '@/shared/application/hooks/useCardsCatalog';
import { useInventory } from '@/shared/infrastructure/inventory/useInventory';
import { useUseItem } from '@/shared/infrastructure/inventory/useUseItem';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';
export default function InventoryPageClient() {
 const locale = useLocale();
 const t = useTranslations('Inventory');
 const [selectedItem, setSelectedItem] = useState<InventoryItem | null>(null);
 const inventoryQuery = useInventory();
 const useItemMutation = useUseItem();
 const cardsCatalog = useCardsCatalog();
 const cardOptions = useMemo(
  () =>
   cardsCatalog.cards.map((card) => ({
    id: card.id,
    name: locale === 'en' ? card.nameEn : locale === 'zh' ? card.nameZh : card.nameVi,
   })),
  [cardsCatalog.cards, locale],
 );

 return (
  <div className={cn('mx-auto w-full max-w-6xl px-4 pb-16 pt-24 sm:px-6')}>
   <InventoryPageHero title={t('title')} subtitle={t('subtitle')} />
   <InventoryQueryState
    isLoading={inventoryQuery.isLoading}
    errorMessage={inventoryQuery.error ? (inventoryQuery.error as Error).message : undefined}
    loadingLabel={t('loading')}
   />
   <InventoryGrid
    items={inventoryQuery.data?.items ?? []}
    locale={locale}
    emptyLabel={t('empty')}
    onSelect={(item) => setSelectedItem(item)}
   />
   {selectedItem ? (
    <UseItemModal
     isOpen
     item={selectedItem}
     locale={locale}
     cardOptions={cardOptions}
     labels={{
       useNow: t('useNow'),
       selectCard: t('selectCard'),
       quantity: t('quantity'),
       effectValue: t('effectValue'),
     }}
     isPending={useItemMutation.isPending}
     onClose={() => setSelectedItem(null)}
     onUse={async (payload) => {
       await useItemMutation.mutateAsync(payload);
       setSelectedItem(null);
     }}
    />
   ) : null}
  </div>
 );
}
