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
      {/* 
          Hiển thị số lượng hiện có: 
          Sử dụng GlassCard variant 'default' với padding sm để tiết kiệm không gian.
      */}
      <GlassCard variant="default" padding="sm" className="bg-white/[0.03]">
        <p className={cn('tn-text-muted text-[10px] font-black uppercase tracking-widest mb-1 opacity-60')}>
          {quantityLabel}
        </p>
        <p className={cn('tn-text-primary font-black text-lg')}>
          x{quantity}
        </p>
      </GlassCard>
      
      {/* Hiển thị giá trị hiệu ứng của vật phẩm */}
      <GlassCard variant="default" padding="sm" className="bg-white/[0.03]">
        <p className={cn('tn-text-muted text-[10px] font-black uppercase tracking-widest mb-1 opacity-60')}>
          {effectValueLabel}
        </p>
        <p className={cn('tn-text-accent font-black text-lg')}>
          {effectValue}
        </p>
      </GlassCard>
    </div>
  );
}
