'use client';

/* 
 * Import các hooks và thư viện cốt lõi.
 */
import { useState } from 'react';
import { useLocale, useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

/* 
 * Import các thành phần nội bộ của trang Kho đồ.
 */
import InventoryGrid from '@/components/ui/inventory/InventoryGrid';
import InventoryPageHero from '@/components/ui/inventory/InventoryPageHero';
import InventoryQueryState from '@/components/ui/inventory/InventoryQueryState';
import UseItemModal from '@/components/ui/inventory/UseItemModal';

/* 
 * Import hạ tầng dữ liệu để quản lý trạng thái kho đồ và sử dụng vật phẩm.
 */
import { useInventory } from '@/shared/infrastructure/inventory/useInventory';
import { useOwnedInventoryCards } from '@/shared/infrastructure/inventory/useOwnedInventoryCards';
import { useUseItem } from '@/shared/infrastructure/inventory/useUseItem';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';

/**
 * InventoryPageClient - Thành phần điều hướng chính cho giao diện Kho đồ người dùng.
 * @returns JSX.Element
 */
export default function InventoryPageClient() {
  const locale = useLocale();
  const t = useTranslations('Inventory');
  
  /* Quản lý trạng thái vật phẩm đang được chọn để sử dụng */
  const [selectedItemCode, setSelectedItemCode] = useState<string | null>(null);
  
  /* Lấy dữ liệu kho đồ tổng quát */
  const inventoryQuery = useInventory();
  
  /* Lấy dữ liệu các lá bài người dùng đang sở hữu */
  const ownedCards = useOwnedInventoryCards(locale);
  
  /* Hook thực thi hành động sử dụng vật phẩm */
  const useItemMutation = useUseItem();
  const inventoryItems = inventoryQuery.data?.items ?? [];
  const selectedItem = selectedItemCode
    ? (inventoryItems.find((item) => item.itemCode === selectedItemCode) ?? null)
    : null;

  return (
    <div className={cn('relative mx-auto w-full max-w-7xl px-4 pb-24 pt-32 sm:px-6 lg:px-8')}>
      {/* Decorative Background Elements */}
      <div className="pointer-events-none absolute left-0 top-0 -z-10 h-[500px] w-full bg-[radial-gradient(circle_at_50%_0%,rgba(139,92,246,0.08),transparent_70%)]" />
      <div className="pointer-events-none absolute right-0 top-1/2 -z-10 h-[300px] w-[300px] rounded-full bg-violet-600/5 blur-[120px]" />
      
      {/* 
          Phần giới thiệu trang: Hiển thị tiêu đề metallic và icon kho đồ.
      */}
      <InventoryPageHero title={t('title')} subtitle={t('subtitle')} />

      {/* 
          Quản lý trạng thái tải dữ liệu và hiển thị lỗi
      */}
      <InventoryQueryState
        isLoading={inventoryQuery.isLoading || ownedCards.isLoading}
        errorMessage={inventoryQuery.error
          ? (inventoryQuery.error as Error).message
          : ownedCards.error}
        loadingLabel={t('loading')}
      />

      {/* 
          Lưới vật phẩm (Inventory Grid):
          Đảm bảo min-h để tránh layout shift khi danh sách trống.
      */}
      <div className={cn('relative min-h-[400px]')}>
        <div className={cn('absolute -right-20 -top-20 -z-10 h-64 w-64 rounded-full bg-purple-500/5 blur-3xl')} />
        <InventoryGrid
          items={inventoryItems}
          locale={locale}
          emptyLabel={t('empty')}
          onSelect={(item: InventoryItem) => setSelectedItemCode(item.itemCode)}
        />
      </div>

      {/* 
          Modal sử dụng vật phẩm:
          Được ghim bằng key của item để ổn định trạng thái.
      */}
      {selectedItem ? (
        <UseItemModal
          key={`${selectedItem.itemCode}-${selectedItem.quantity}`}
          isOpen={!!selectedItem}
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
            useAgain: t('useAgain') || 'Sử dụng tiếp',
            title: t('useItemTitle') || 'Sử dụng vật phẩm'
          }}
          isPending={useItemMutation.isPending}
          onClose={() => setSelectedItemCode(null)}
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
