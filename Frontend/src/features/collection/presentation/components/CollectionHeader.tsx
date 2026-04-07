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
  <div className={cn('flex flex-col lg:flex-row lg:items-end justify-between gap-8 mb-16 animate-in fade-in slide-in-from-bottom-4 duration-700')}>
   <div className={cn('space-y-4')}>
    <div className={cn('inline-flex items-center gap-2 px-3 py-1 rounded-full bg-[var(--warning)]/5 border border-[var(--warning)]/10 text-[9px] uppercase tracking-[0.2em] font-black text-[var(--warning)] shadow-xl')}>
     <Sparkles className={cn('w-3 h-3')} />
     {labels.tag}
    </div>
    <h1 className={cn('text-4xl md:text-5xl font-black tracking-tighter tn-text-primary')}>{labels.title}</h1>
    <p className={cn('tn-text-muted max-w-md text-sm font-medium leading-relaxed')}>{labels.subtitle}</p>
   </div>

   <div className={cn('w-full lg:w-72 space-y-3 tn-panel-soft p-4 rounded-3xl shadow-xl')}>
    <div className={cn('flex justify-between items-end')}>
     <span className={cn('text-[10px] font-black uppercase tracking-widest tn-text-muted')}>
      {labels.progressLabel}
     </span>
     <span className={cn('text-sm font-black text-[var(--warning)] tracking-tighter')}>
      {totalCollected} <span className={cn('tn-text-muted')}>/ {totalCardCount}</span>
     </span>
    </div>
    <div className={cn('h-1 w-full tn-surface rounded-full overflow-hidden')}>
     <div
      className={cn('h-full bg-gradient-to-r from-[var(--warning)] via-[var(--warning)] to-[var(--warning)] rounded-full transition-all duration-1000 ease-out shadow-[0_0_10px_var(--c-251-191-36-20)]')}
      style={{ width: `${progressRatio}%` }}
     />
    </div>
   </div>
  </div>
 );
}
