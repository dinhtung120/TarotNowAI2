import { Sparkles } from 'lucide-react';
import { cn } from '@/lib/utils';

interface CollectionHeaderProps {
 totalCollected: number;
 totalCardCount: number;
 progressRatio: number;
 labels: {
  tag: string;
  title: string;
  subtitle: string;
  progressLabel: string;
 };
}

export function CollectionHeader({
 totalCollected,
 totalCardCount,
 progressRatio,
 labels,
}: CollectionHeaderProps) {
 return (
  <div className={cn('tn-flex-col-row-lg tn-items-end-lg justify-between gap-8 mb-16 animate-in fade-in slide-in-from-bottom-4 duration-700')}>
   <div className={cn('space-y-4')}>
    <div className={cn('inline-flex items-center gap-2 px-3 py-1 rounded-full tn-bg-warning-5 border tn-border-warning-10 tn-text-9 uppercase tn-tracking-02 font-black tn-text-warning shadow-xl')}>
     <Sparkles className={cn('w-3 h-3')} />
     {labels.tag}
    </div>
    <h1 className={cn('tn-text-4-5-md font-black tracking-tighter tn-text-primary')}>{labels.title}</h1>
    <p className={cn('tn-text-muted max-w-md text-sm font-medium leading-relaxed')}>{labels.subtitle}</p>
   </div>

   <div className={cn('tn-w-full-72-lg space-y-3 tn-panel-soft p-4 rounded-3xl shadow-xl')}>
    <div className={cn('flex justify-between items-end')}>
     <span className={cn('tn-text-overline tn-text-muted')}>
      {labels.progressLabel}
     </span>
     <span className={cn('text-sm font-black tn-text-warning tracking-tighter')}>
      {totalCollected} <span className={cn('tn-text-muted')}>/ {totalCardCount}</span>
     </span>
    </div>
    <progress
     className={cn('tn-progress tn-progress-xs tn-progress-warning')}
     max={100}
     value={Math.max(0, Math.min(100, progressRatio))}
    />
   </div>
  </div>
 );
}
