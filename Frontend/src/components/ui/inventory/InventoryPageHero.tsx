'use client';

/* 
 * Import các thành phần hỗ trợ UI.
 * - PackageSearch: Icon trực quan cho kho đồ từ thư viện lucide-react.
 */
import { PackageSearch } from 'lucide-react';
import { cn } from '@/lib/utils';

/**
 * Props của thành phần InventoryPageHero.
 */
interface InventoryPageHeroProps {
  title: string;
  subtitle: string;
}

/**
 * InventoryPageHero - Thành phần hiển thị phần giới thiệu (Hero) của trang Kho đồ.
 * Sử dụng phong cách thiết kế cao cấp với tiêu đề Metallic.
 */
export default function InventoryPageHero({ title, subtitle }: InventoryPageHeroProps) {
  return (
    <div className={cn('mb-10 flex flex-col gap-4 sm:flex-row sm:items-center sm:gap-6')}>
      {/* 
          Container bao quanh Icon: Sử dụng hiệu ứng panel mờ (tn-panel-soft)
          để tạo sự chuyên nghiệp cho phần đồ họa.
      */}
      <div className={cn('flex h-14 w-14 shrink-0 items-center justify-center rounded-2xl border tn-border-soft bg-white/[0.03] shadow-inner')}>
        <PackageSearch className={cn('h-7 w-7 tn-text-primary opacity-80')} />
      </div>
      
      <div className="space-y-1">
        {/* 
            Tiêu đề trang: Áp dụng 'lunar-metallic-text' để tạo hiệu ứng chữ kim loại.
            Sử dụng font black và uppercase để tăng độ mạnh mẽ và quyền lực cho Kho đồ.
        */}
        <h1 className={cn('lunar-metallic-text text-3xl font-black uppercase tracking-[0.2em] sm:text-4xl')}>
          {title}
        </h1>
        
        {/* Phụ đề: Sử dụng 'tn-text-secondary' để cung cấp thông tin mô tả nhẹ nhàng */}
        <p className={cn('tn-text-secondary text-sm font-medium tracking-wide opacity-70')}>
          {subtitle}
        </p>
      </div>
    </div>
  );
}
