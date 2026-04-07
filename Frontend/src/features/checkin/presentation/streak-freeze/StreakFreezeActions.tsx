import { cn } from '@/lib/utils';

interface StreakFreezeActionsProps {
  isPending: boolean;
  onBuy: () => void;
  onClose: () => void;
}

export default function StreakFreezeActions({ isPending, onBuy, onClose }: StreakFreezeActionsProps) {
  return (
    <div className={cn('flex gap-3')}>
      <button type="button" onClick={onClose} className={cn('flex-1 rounded-lg bg-slate-800 px-4 py-2 text-sm font-medium text-white transition-colors tn-streak-freeze-skip-btn')}>Mặc Kệ</button>
      <button type="button" onClick={onBuy} disabled={isPending} className={cn('flex-1 rounded-lg px-4 py-2 text-sm font-medium text-white transition-all tn-streak-freeze-buy-btn')}>
        {isPending ? 'Đang Mua...' : 'Mua Khôi Phục'}
      </button>
    </div>
  );
}
