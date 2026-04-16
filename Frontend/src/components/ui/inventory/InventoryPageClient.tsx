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
  const [selectedItem, setSelectedItem] = useState<InventoryItem | null>(null);
  
  /* Lấy dữ liệu kho đồ tổng quát */
  const inventoryQuery = useInventory();
  
  /* Lấy dữ liệu các lá bài người dùng đang sở hữu (để phục vụ việc chọn mục tiêu sử dụng vật phẩm) */
  const ownedCards = useOwnedInventoryCards(locale);
  
  /* Hook thực thi hành động sử dụng vật phẩm */
  const useItemMutation = useUseItem();

  return (
    <div className={cn('mx-auto w-full max-w-6xl space-y-10 px-4 pb-24 pt-28 sm:px-6')}>
      
      {/* 
          Phần giới thiệu trang: Hiển thị tiêu đề metallic và icon kho đồ.
      */}
      <InventoryPageHero title={t('title')} subtitle={t('subtitle')} />

      {/* 
          Quản lý trạng thái tải dữ liệu và hiển thị lỗi:
          Đảm bảo người dùng luôn biết được hệ thống đang xử lý hay gặp vấn đề.
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
          Hiển thị danh sách các vật phẩm dưới dạng các Interactive Glass Cards.
      */}
      <div className={cn('relative')}>
        <div className={cn('absolute -right-20 -top-20 h-64 w-64 rounded-full bg-purple-500/5 blur-3xl')} />
        <InventoryGrid
          items={inventoryQuery.data?.items ?? []}
          locale={locale}
          emptyLabel={t('empty')}
          onSelect={(item) => setSelectedItem(item)}
        />
      </div>

      {/* 
          Modal sử dụng vật phẩm:
          Chỉ hiển thị khi người dùng nhấn vào một vật phẩm hợp lệ trong kho.
      */}
      {selectedItem ? (
        <UseItemModal
          key={selectedItem.itemDefinitionId}
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
          }}
          isPending={useItemMutation.isPending}
          onClose={() => setSelectedItem(null)}
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
