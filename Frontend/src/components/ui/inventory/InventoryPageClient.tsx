'use client';

import { useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import InventoryGrid from '@/components/ui/inventory/InventoryGrid';
import InventoryPageHero from '@/components/ui/inventory/InventoryPageHero';
import InventoryQueryState from '@/components/ui/inventory/InventoryQueryState';
import UseItemModal from '@/components/ui/inventory/UseItemModal';
import { useInventory } from '@/shared/infrastructure/inventory/useInventory';
import { useOwnedInventoryCards } from '@/shared/infrastructure/inventory/useOwnedInventoryCards';
import { useUseItem } from '@/shared/infrastructure/inventory/useUseItem';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';

export default function InventoryPageClient() {
  const locale = useLocale();
  const t = useTranslations('Inventory');
  const useAgainLabel = t.has('useAgain') ? t('useAgain') : 'Sử dụng tiếp';
  const useItemTitleLabel = t.has('useItemTitle') ? t('useItemTitle') : 'Sử dụng vật phẩm';

  const [selectedItemSnapshot, setSelectedItemSnapshot] = useState<InventoryItem | null>(null);
  const shouldLoadOwnedCards = selectedItemSnapshot !== null;
  const inventoryQuery = useInventory();
  const ownedCards = useOwnedInventoryCards(locale, { enabled: shouldLoadOwnedCards });
  const useItemMutation = useUseItem();

  const inventoryItems = inventoryQuery.data?.items ?? [];
  const selectedItemFromQuery = selectedItemSnapshot
    ? (inventoryItems.find((item) => item.itemCode === selectedItemSnapshot.itemCode) ?? null)
    : null;
  const selectedItem = selectedItemSnapshot
    ? selectedItemFromQuery ?? { ...selectedItemSnapshot, quantity: 0, canUse: false }
    : null;

  return (
    <div className={cn('relative mx-auto w-full max-w-7xl overflow-x-hidden px-4 pb-24 pt-32 sm:px-6 lg:px-8')}>
      <div className={cn('pointer-events-none absolute left-0 top-0 -z-10 h-[500px] w-full bg-[radial-gradient(circle_at_50%_0%,rgba(139,92,246,0.08),transparent_70%)]')} />
      <div className={cn('pointer-events-none absolute right-0 top-1/2 -z-10 h-[300px] w-[300px] rounded-full bg-violet-600/5 blur-[120px]')} />

      <InventoryPageHero title={t('title')} subtitle={t('subtitle')} />

      <InventoryQueryState
        isLoading={inventoryQuery.isLoading || ownedCards.isLoading}
        errorMessage={inventoryQuery.error ? (inventoryQuery.error as Error).message : ownedCards.error}
        loadingLabel={t('loading')}
      />

      <div className={cn('relative min-h-[400px]')}>
        <div className={cn('absolute -right-20 -top-20 -z-10 h-64 w-64 rounded-full bg-purple-500/5 blur-3xl')} />
        <InventoryGrid
          items={inventoryItems}
          locale={locale}
          emptyLabel={t('empty')}
          onSelect={(item: InventoryItem) => setSelectedItemSnapshot(item)}
        />
      </div>

      {selectedItem ? (
        <UseItemModal
          key={selectedItem.itemCode}
          isOpen={Boolean(selectedItem)}
          item={selectedItem}
          locale={locale}
          cardOptions={ownedCards.cardOptions}
          labels={{
            useNow: t('useNow'),
            selectCard: t('selectCard'),
            quantity: t('quantity'),
            effectValue: t('effectValue'),
            selectedCard: t('selectedCard'),
            effectType: t('effectType'),
            rolledValue: t('rolledValue'),
            beforeAfter: t('beforeAfter'),
            done: t('done'),
            useAgain: useAgainLabel,
            title: useItemTitleLabel,
          }}
          isPending={useItemMutation.isPending}
          onClose={() => setSelectedItemSnapshot(null)}
          onUse={async (payload) => {
            try {
              return await useItemMutation.mutateAsync(payload);
            } catch (error) {
              console.error('Lỗi khi sử dụng vật phẩm:', error);
              throw error;
            }
          }}
        />
      ) : null}
    </div>
  );
}
