'use client';

/* 
 * Import các thành phần và utility.
 * - GlassCard: Để tạo cấu trúc cho trạng thái rỗng.
 */
import { cn } from '@/lib/utils';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';
import InventoryItemCard from '@/components/ui/inventory/InventoryItemCard';

/**
 * Props cho thành phần InventoryGrid.
 */
interface InventoryGridProps {
  items: InventoryItem[];
  locale: string;
  emptyLabel: string;
  onSelect: (item: InventoryItem) => void;
}

/**
 * InventoryGrid - Hiển thị danh sách vật phẩm theo dạng lưới (Grid).
 * Hỗ trợ hiển thị trạng thái rỗng một cách thẩm mỹ.
 */
export default function InventoryGrid({ items, locale, emptyLabel, onSelect }: InventoryGridProps) {
  
  /* Xử lý trạng thái khi kho đồ trống */
  if (!items.length) {
    return (
      <div className={cn(
        'flex flex-col items-center justify-center rounded-3xl border border-dashed tn-border-soft bg-white/[0.01] px-6 py-20 text-center'
      )}>
        <div className={cn('mb-6 flex h-20 w-20 items-center justify-center rounded-full bg-slate-500/10')}>
          <span className={cn('text-3xl opacity-30')}>✧</span>
        </div>
        <p className={cn('tn-text-muted text-base font-medium italic opacity-60')}>
          {emptyLabel}
        </p>
      </div>
    );
  }

  /* Hiển thị lưới vật phẩm với khoảng cách (gap) được tối ưu hóa cho thẻ kính */
  return (
    <div className={cn('grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4')}>
      {items.map((item) => (
        <InventoryItemCard 
            key={item.itemDefinitionId} 
            item={item} 
            locale={locale} 
            onSelect={onSelect} 
        />
      ))}
    </div>
  );
}
