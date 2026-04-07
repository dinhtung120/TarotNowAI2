import StreakFreezeActions from '@/features/checkin/presentation/streak-freeze/StreakFreezeActions';
import StreakFreezeSummary from '@/features/checkin/presentation/streak-freeze/StreakFreezeSummary';
import { cn } from '@/lib/utils';

interface StreakFreezeModalCardProps {
  errorDesc: string | null;
  freezePrice: number;
  hours: number;
  isPending: boolean;
  minutes: number;
  preBreakStreak: number;
  onBuy: () => void;
  onClose: () => void;
}

export default function StreakFreezeModalCard({ errorDesc, freezePrice, hours, isPending, minutes, preBreakStreak, onBuy, onClose }: StreakFreezeModalCardProps) {
  return (
    <div className={cn('relative w-full max-w-sm rounded-2xl border border-slate-800 bg-slate-900 p-6 shadow-2xl')}>
      <button type="button" onClick={onClose} className={cn('absolute right-4 top-4 text-slate-400 transition-colors hover:text-white')}>
        <svg className={cn('h-5 w-5')} fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" /></svg>
      </button>
      <StreakFreezeSummary freezePrice={freezePrice} hours={hours} minutes={minutes} preBreakStreak={preBreakStreak} />
      {errorDesc ? <p className={cn('mb-4 text-xs text-red-400')}>{errorDesc}</p> : null}
      <StreakFreezeActions isPending={isPending} onClose={onClose} onBuy={onBuy} />
    </div>
  );
}
