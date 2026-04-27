'use client';

/* 
 * Import các thành phần hỗ trợ UI.
 */
import { cn } from '@/lib/utils';

/**
 * Props của thành phần UseItemStats.
 */
interface UseItemStatsProps {
  quantityLabel: string;
  effectValueLabel: string;
  quantity: number;
  effectValue: number;
}

/**
 * UseItemStats - Hiển thị các chỉ số của vật phẩm trong Modal sử dụng.
 * Thiết kế gọn gàng với hiệu ứng Glassmorphism nhẹ.
 */
export default function UseItemStats({
  quantityLabel,
  quantity,
}: Pick<UseItemStatsProps, 'quantityLabel' | 'quantity'>) {
  return (
    <div className={cn('flex')}>
      <div className={cn(
        'relative overflow-hidden rounded-xl border tn-border-soft bg-white/[0.02] px-3 py-2 transition-all hover:bg-white/[0.04]',
        'min-w-[120px]'
      )}>
        <div className={cn("absolute -right-1 -top-1 flex h-6 w-6 items-center justify-center opacity-5")}>
          <div className={cn("h-full w-full rounded-full bg-violet-500 blur-xl")} />
        </div>
        <p className={cn('tn-text-muted text-[9px] font-black uppercase tracking-[0.1em] mb-0.5 opacity-30')}>
          {quantityLabel}
        </p>
        <p className={cn('tn-text-primary font-black text-lg tracking-tighter')}>
          x{quantity}
        </p>
      </div>
    </div>
  );
}
