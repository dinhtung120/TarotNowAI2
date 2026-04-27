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
        'group relative flex flex-col items-center justify-center rounded-[2.5rem] border border-dashed tn-border-soft',
        'bg-white/[0.01] px-6 py-32 text-center transition-all duration-700 hover:bg-white/[0.02]'
      )}>
        <div className={cn("absolute inset-0 -z-10 bg-[radial-gradient(circle_at_50%_50%,rgba(139,92,246,0.03),transparent_70%)] opacity-0 transition-opacity duration-700 group-hover:opacity-100")} />
        
        <div className={cn("relative mb-8")}>
          <div className={cn('flex h-24 w-24 items-center justify-center rounded-full bg-slate-500/5 border tn-border-soft shadow-inner')}>
            <span className={cn('text-5xl opacity-20 grayscale transition-all duration-700 group-hover:scale-110 group-hover:opacity-40 group-hover:grayscale-0')}>
              📦
            </span>
          </div>
          <div className={cn("absolute -right-2 -top-2 flex h-6 w-6 animate-pulse items-center justify-center rounded-full bg-violet-500/10")}>
             <div className={cn("h-2 w-2 rounded-full bg-violet-400 blur-[1px]")} />
          </div>
        </div>

        <div className={cn("space-y-4")}>
          <p className={cn('lunar-metallic-text text-xl font-black uppercase tracking-[0.2em] opacity-30')}>
            {emptyLabel}
          </p>
          <div className={cn("mx-auto h-[1px] w-12 bg-gradient-to-r from-transparent via-violet-500/30 to-transparent")} />
          <p className={cn('tn-text-muted text-xs font-medium tracking-widest uppercase opacity-40')}>
            Hãy quay lại sau khi thu thập thêm vật phẩm
          </p>
        </div>
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
