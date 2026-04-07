import { ArrowRight, Calendar, Clock, Sparkles } from 'lucide-react';
import type { HistorySessionDto } from '@/features/reading/application/actions/history';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface HistorySessionCardProps {
 locale: string;
 session: HistorySessionDto;
 spreadName: string;
 completedLabel: string;
 interruptedLabel: string;
 onOpenSession: (id: string) => void;
}

export function HistorySessionCard({
 locale,
 session,
 spreadName,
 completedLabel,
 interruptedLabel,
 onOpenSession,
}: HistorySessionCardProps) {
 return (
  <GlassCard
   variant="interactive"
   onClick={() => onOpenSession(session.id)}
   className={cn('group relative flex items-center justify-between !py-4')}
  >
   <div className={cn('flex items-center gap-4')}>
    <div className={cn('tn-group-scale-110 w-12 h-12 rounded-2xl flex items-center justify-center border transition-all duration-500', session.isCompleted ? 'tn-bg-warning-soft tn-border-warning' : 'tn-surface tn-border tn-group-border-accent-50')}>
     <Sparkles className={cn('w-5 h-5', session.isCompleted ? 'tn-text-warning' : 'tn-text-muted')} />
    </div>
    <div>
     <h3 className={cn('tn-group-text-accent text-sm font-bold tn-text-primary transition-colors tracking-tight')}>{spreadName}</h3>
     <div className={cn('flex items-center gap-3 mt-1.5 opacity-60')}>
      <span className={cn('flex items-center tn-text-overline tn-text-tertiary')}><Calendar className={cn('w-3 h-3 mr-1')} />{new Date(session.createdAt).toLocaleDateString(locale)}</span>
      <span className={cn('flex items-center tn-text-overline tn-text-tertiary')}><Clock className={cn('w-3 h-3 mr-1')} />{new Date(session.createdAt).toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit' })}</span>
     </div>
    </div>
   </div>
   <div className={cn('flex items-center gap-4')}>
    <span className={cn('tn-hide-inline-show-sm tn-text-9 px-2.5 py-1 rounded-full font-black uppercase tracking-widest border', session.isCompleted ? 'tn-bg-success-soft tn-text-success tn-border-success' : 'tn-bg-danger-soft tn-text-danger tn-border-danger')}>
     {session.isCompleted ? completedLabel : interruptedLabel}
    </span>
    <div className={cn('tn-group-bg-accent tn-group-text-ink w-8 h-8 rounded-full tn-surface flex items-center justify-center transition-all duration-300')}>
     <ArrowRight className={cn('w-4 h-4')} />
    </div>
   </div>
  </GlassCard>
 );
}
