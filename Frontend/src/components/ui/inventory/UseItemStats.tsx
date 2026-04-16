'use client';

/* 
 * Import các thành phần hỗ trợ UI.
 */
import { cn } from '@/lib/utils';
import GlassCard from '@/shared/components/ui/GlassCard';

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
  effectValueLabel,
  quantity,
  effectValue,
}: UseItemStatsProps) {
  return (
    <div className={cn('grid grid-cols-2 gap-4 text-sm')}>
      <div className={cn('relative overflow-hidden rounded-2xl border tn-border-soft bg-white/[0.02] p-4 transition-all hover:bg-white/[0.04]')}>
        <div className="absolute -right-2 -top-2 flex h-8 w-8 items-center justify-center opacity-10">
          <div className="h-full w-full rounded-full bg-violet-500 blur-xl" />
        </div>
        <p className={cn('tn-text-muted text-[10px] font-black uppercase tracking-[0.2em] mb-2 opacity-40')}>
          {quantityLabel}
        </p>
        <p className={cn('tn-text-primary font-black text-2xl tracking-tighter')}>
          x{quantity}
        </p>
      </div>
      
      <div className={cn('relative overflow-hidden rounded-2xl border tn-border-soft bg-white/[0.02] p-4 transition-all hover:bg-white/[0.04]')}>
        <div className="absolute -right-2 -top-2 flex h-8 w-8 items-center justify-center opacity-10">
          <div className="h-full w-full rounded-full bg-fuchsia-500 blur-xl" />
        </div>
        <p className={cn('tn-text-muted text-[10px] font-black uppercase tracking-[0.2em] mb-2 opacity-40')}>
          {effectValueLabel}
        </p>
        <p className={cn('tn-text-accent font-black text-2xl tracking-tighter')}>
          {effectValue}
        </p>
      </div>
    </div>
  );
}
