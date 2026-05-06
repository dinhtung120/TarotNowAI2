import { cn } from '@/lib/utils';
import Button from '@/shared/ui/Button';
import type { PullGachaResult } from '@/features/gacha/shared/gachaTypes';
import { GachaResultItem } from '@/features/gacha/result/GachaResultItem';

interface GachaResultModalResultStageProps {
  labels: {
    title: string;
    pityTriggered: string;
    close: string;
  };
  locale: string;
  result: PullGachaResult;
  onClose: () => void;
}

export function GachaResultModalResultStage({ labels, locale, result, onClose }: GachaResultModalResultStageProps) {
  return (
    <div className={cn('animate-in fade-in zoom-in-95 w-full space-y-8 duration-700')}>
      <div className={cn('space-y-2 text-center')}>
        <h2 className={cn('lunar-metallic-text text-2xl font-black uppercase tracking-[0.2em]')}>{labels.title}</h2>
        <p className={cn('tn-text-secondary text-[10px] font-black uppercase tracking-widest opacity-60')}>{result.poolCode}</p>
      </div>

      {result.wasPityTriggered ? (
        <div className={cn('mx-auto max-w-xs animate-bounce')}>
          <div className={cn('rounded-full border border-amber-500/30 bg-amber-500/10 px-4 py-1.5 text-center')}>
            <span className={cn('text-[10px] font-black uppercase tracking-widest text-amber-400')}>{labels.pityTriggered}</span>
          </div>
        </div>
      ) : null}

      <div className={cn('custom-scrollbar grid max-h-[50vh] grid-cols-1 gap-4 overflow-y-auto px-2', result.rewards.length > 1 && 'sm:grid-cols-2')}>
        {result.rewards.map((reward, index) => (
          <GachaResultItem key={`${reward.itemCode}_${index}`} reward={reward} locale={locale} />
        ))}
      </div>

      <div className={cn('pt-4')}>
        <Button variant="brand" size="lg" fullWidth onClick={onClose} className={cn('shadow-[0_0_20px_rgba(16,185,129,0.2)]')}>
          {labels.close}
        </Button>
      </div>
    </div>
  );
}
