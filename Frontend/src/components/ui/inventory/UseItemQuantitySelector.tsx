'use client';

import { Minus, Plus } from 'lucide-react';
import { cn } from '@/lib/utils';

interface UseItemQuantitySelectorProps {
  value: number;
  max: number;
  totalQuantity: number;
  label: string;
  onChange: (value: number) => void;
  disabled?: boolean;
}

/**
 * UseItemQuantitySelector - Thành phần cho phép chọn số lượng vật phẩm muốn dùng.
 * Thiết kế tinh xảo với hiển thị tổng số lượng sở hữu ở bên phải.
 */
export default function UseItemQuantitySelector({
  value,
  max,
  totalQuantity,
  label,
  onChange,
  disabled
}: UseItemQuantitySelectorProps) {
  const actualMax = Math.min(max, 10);

  const increment = () => {
    if (value < actualMax) onChange(value + 1);
  };

  const decrement = () => {
    if (value > 1) onChange(value - 1);
  };

  return (
    <div className={cn('space-y-3')}>
      <p className={cn('tn-text-muted text-[10px] font-black tracking-[0.2em] uppercase opacity-40')}>
        {label}
      </p>
      
      <div className={cn(
        'flex items-center gap-3',
        disabled && 'opacity-50 pointer-events-none'
      )}>
        {/* Selector Control Box */}
        <div className={cn(
          'flex flex-1 items-center justify-between rounded-2xl border tn-border-soft bg-white/[0.02] p-2'
        )}>
          <button
            type="button"
            onClick={decrement}
            disabled={value <= 1}
            className={cn(
              'flex h-10 w-10 items-center justify-center rounded-xl border tn-border-soft transition-all',
              'bg-white/[0.03] hover:bg-white/[0.08] hover:border-violet-500/30 active:scale-95 disabled:opacity-20'
            )}
          >
            <Minus className="h-4 w-4" />
          </button>

          <span className="text-xl font-black tn-text-primary tracking-tighter">
            {value}
          </span>

          <button
            type="button"
            onClick={increment}
            disabled={value >= actualMax}
            className={cn(
              'flex h-10 w-10 items-center justify-center rounded-xl border tn-border-soft transition-all',
              'bg-white/[0.03] hover:bg-white/[0.08] hover:border-violet-500/30 active:scale-95 disabled:opacity-20'
            )}
          >
            <Plus className="h-4 w-4" />
          </button>
        </div>

        {/* Total Quantity Box (Right Side) */}
        <div className={cn(
          'flex h-[60px] min-w-[70px] flex-col items-center justify-center rounded-2xl border tn-border-soft bg-white/[0.02] px-3'
        )}>
           <span className={cn('text-[9px] font-black tn-text-muted uppercase tracking-widest opacity-30 mb-0.5')}>
             Tổng
           </span>
           <span className="text-lg font-black tn-text-primary tracking-tighter">
             x{totalQuantity}
           </span>
        </div>
      </div>
    </div>
  );
}
  );
}
