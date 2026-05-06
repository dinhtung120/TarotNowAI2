import { ArrowLeft, Calendar, Clock, Sparkles } from 'lucide-react';
import type { HistoryDetailResponse } from '@/features/reading/history/actions';
import { cn } from '@/lib/utils';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';

interface HistoryDetailHeaderProps {
 locale: string;
 sessionId: string;
 detail: HistoryDetailResponse | null;
 spreadName: string;
 labels: {
  back: string;
  statusCompleted: string;
  statusInterrupted: string;
  detailFragment: (id: string) => string;
 };
}

export function HistoryDetailHeader({
 locale,
 sessionId,
 detail,
 spreadName,
 labels,
}: HistoryDetailHeaderProps) {
 const navigation = useOptimizedNavigation();

 return (
  <div className={cn('tn-flex-col-row-md tn-items-end-md justify-between gap-8 tn-mb-12-16-sm animate-in fade-in slide-in-from-bottom-4 duration-700')}>
   <div className={cn('flex flex-col gap-6')}>
    <button type="button" onClick={() => navigation.push('/reading/history')} className={cn('group flex items-center gap-2 tn-text-secondary tn-hover-text-primary transition-all w-fit min-h-11 px-2 rounded-xl tn-hover-surface-soft')}>
     <div className={cn('w-8 h-8 rounded-full tn-surface flex items-center justify-center transition-all')}>
      <ArrowLeft className={cn('w-4 h-4')} />
     </div>
     <span className={cn('tn-text-overline')}>{labels.back}</span>
    </button>
    <div className={cn('flex items-center gap-6')}>
     <div className={cn('w-16 h-16 tn-bg-glass border tn-border rounded-2xl flex items-center justify-center shadow-2xl')}>
      <Sparkles className={cn('w-8 h-8 tn-text-warning')} />
     </div>
     <div>
      <h1 className={cn('tn-text-3-4-5-responsive font-black tracking-tighter tn-text-primary uppercase italic')}>{spreadName || '...'}</h1>
      <div className={cn('flex flex-wrap items-center gap-3 mt-3 tn-text-secondary font-medium text-xs')}>
       <span className={cn('flex items-center gap-1.5 px-3 py-1.5 tn-surface rounded-full')}><Calendar className={cn('w-3.5 h-3.5')} />{detail ? new Date(detail.createdAt).toLocaleDateString(locale) : '...'}</span>
       <span className={cn('flex items-center gap-1.5 px-3 py-1.5 tn-surface rounded-full')}><Clock className={cn('w-3.5 h-3.5')} />{detail ? new Date(detail.createdAt).toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit' }) : '...'}</span>
      </div>
     </div>
    </div>
   </div>
   <div className={cn('flex flex-col tn-items-end-md gap-2')}>
    <span className={cn('tn-text-overline tn-tracking-02 px-4 py-1.5 rounded-full border', detail?.isCompleted ? 'tn-bg-success-soft tn-text-success tn-border-success' : 'tn-bg-danger-soft tn-text-danger tn-border-danger')}>
     {detail?.isCompleted ? labels.statusCompleted : labels.statusInterrupted}
    </span>
    <p className={cn('tn-text-overline tn-text-tertiary mt-1')}>
     {labels.detailFragment(sessionId.split('-')[0])}
    </p>
   </div>
  </div>
 );
}
