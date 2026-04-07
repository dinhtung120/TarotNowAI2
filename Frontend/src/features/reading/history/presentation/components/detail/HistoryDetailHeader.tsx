import { ArrowLeft, Calendar, Clock, Sparkles } from 'lucide-react';
import type { HistoryDetailResponse } from '@/features/reading/application/actions/history';
import { useRouter } from '@/i18n/routing';
import { cn } from '@/lib/utils';

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
 const router = useRouter();

 return (
  <div className={cn('flex flex-col md:flex-row md:items-end justify-between gap-8 mb-12 sm:mb-16 animate-in fade-in slide-in-from-bottom-4 duration-700')}>
   <div className={cn('flex flex-col gap-6')}>
    <button type="button" onClick={() => router.push('/reading/history')} className={cn('group flex items-center gap-2 text-[var(--text-secondary)] hover:tn-text-primary transition-all w-fit min-h-11 px-2 rounded-xl hover:tn-surface-soft')}>
     <div className={cn('w-8 h-8 rounded-full tn-surface flex items-center justify-center group-hover:bg-[var(--purple-accent)] group-hover:tn-text-ink transition-all')}>
      <ArrowLeft className={cn('w-4 h-4')} />
     </div>
     <span className={cn('text-[10px] uppercase font-black tracking-widest')}>{labels.back}</span>
    </button>
    <div className={cn('flex items-center gap-6')}>
     <div className={cn('w-16 h-16 bg-[var(--bg-glass)] border tn-border rounded-[1.5rem] flex items-center justify-center shadow-2xl')}>
      <Sparkles className={cn('w-8 h-8 text-[var(--warning)]')} />
     </div>
     <div>
      <h1 className={cn('text-3xl sm:text-4xl md:text-5xl font-black tracking-tighter tn-text-primary uppercase italic')}>{spreadName || '...'}</h1>
      <div className={cn('flex flex-wrap items-center gap-3 mt-3 text-[var(--text-secondary)] font-medium text-xs')}>
       <span className={cn('flex items-center gap-1.5 px-3 py-1.5 tn-surface rounded-full')}><Calendar className={cn('w-3.5 h-3.5')} />{detail ? new Date(detail.createdAt).toLocaleDateString(locale) : '...'}</span>
       <span className={cn('flex items-center gap-1.5 px-3 py-1.5 tn-surface rounded-full')}><Clock className={cn('w-3.5 h-3.5')} />{detail ? new Date(detail.createdAt).toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit' }) : '...'}</span>
      </div>
     </div>
    </div>
   </div>
   <div className={cn('flex flex-col items-start md:items-end gap-2')}>
    <span className={cn('text-[10px] px-4 py-1.5 rounded-full font-black uppercase tracking-[0.2em] border', detail?.isCompleted ? 'bg-[var(--success-bg)] text-[var(--success)] border-[var(--success)]' : 'bg-[var(--danger-bg)] text-[var(--danger)] border-[var(--danger)]')}>
     {detail?.isCompleted ? labels.statusCompleted : labels.statusInterrupted}
    </span>
    <p className={cn('text-[10px] text-[var(--text-tertiary)] font-black tracking-widest uppercase mt-1')}>
     {labels.detailFragment(sessionId.split('-')[0])}
    </p>
   </div>
  </div>
 );
}
