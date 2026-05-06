import { cn } from '@/lib/utils';

export function HistoryLoadingGrid() {
 return (
  <div className={cn('space-y-4 animate-pulse')}>
   {[1, 2, 3, 4].map((index) => (
    <div key={`history-list-skeleton-${index}`} className={cn('h-24 tn-surface rounded-2xl border tn-border-soft')} />
   ))}
  </div>
 );
}
