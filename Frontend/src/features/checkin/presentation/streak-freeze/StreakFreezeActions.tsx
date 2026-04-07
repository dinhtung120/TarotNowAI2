import { cn } from '@/lib/utils';

interface StreakFreezeActionsProps {
  isPending: boolean;
  onBuy: () => void;
  onClose: () => void;
}

export default function StreakFreezeActions({ isPending, onBuy, onClose }: StreakFreezeActionsProps) {
  return (
    <div className={cn('flex gap-3')}>
      <button type="button" onClick={onClose} className={cn('flex-1 rounded-lg bg-slate-800 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-slate-700')}>Mặc Kệ</button>
      <button type="button" onClick={onBuy} disabled={isPending} className={cn('flex-1 rounded-lg bg-gradient-to-r from-blue-600 to-cyan-500 px-4 py-2 text-sm font-medium text-white transition-all shadow-[0_0_15px_rgba(56,189,248,0.4)] hover:from-blue-500 hover:to-cyan-400 hover:shadow-[0_0_20px_rgba(56,189,248,0.6)] disabled:opacity-50')}>
        {isPending ? 'Đang Mua...' : 'Mua Khôi Phục'}
      </button>
    </div>
  );
}
