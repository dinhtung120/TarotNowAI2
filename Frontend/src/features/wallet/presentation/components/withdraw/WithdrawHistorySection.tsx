import { Clock, Loader2 } from 'lucide-react';
import type { WithdrawalResult } from '@/features/wallet/application/actions/withdrawal';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { WithdrawHistoryItem } from './WithdrawHistoryItem';

interface WithdrawHistorySectionProps {
 locale: string;
 history: WithdrawalResult[];
 loadingHistory: boolean;
 title: string;
 emptyLabel: string;
 adminNotePrefix: string;
 getStatusBadge: (status: string) => { text: string; className: string };
}

export function WithdrawHistorySection({
 locale,
 history,
 loadingHistory,
 title,
 emptyLabel,
 adminNotePrefix,
 getStatusBadge,
}: WithdrawHistorySectionProps) {
 return (
  <div className={cn('space-y-6 pt-8 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-400')}>
   <h2 className={cn('text-[11px] font-black tn-text-primary uppercase tracking-[0.3em] flex items-center gap-3')}>
    <Clock className={cn('w-4 h-4 text-[var(--text-secondary)]')} />
    {title}
   </h2>
   <GlassCard className={cn('!p-0 overflow-hidden tn-border-soft')}>
    {loadingHistory ? (
     <div className={cn('flex items-center justify-center py-12')}>
      <Loader2 className={cn('w-6 h-6 tn-text-muted animate-spin')} />
     </div>
    ) : null}
    {!loadingHistory && history.length === 0 ? (
      <div className={cn('text-[var(--text-secondary)] text-[11px] font-medium uppercase tracking-widest text-center py-12')}>
       {emptyLabel}
      </div>
     ) : null}
    <div className={cn('divide-y divide-white/5')}>
     {history.map((item) => (
      <WithdrawHistoryItem
       key={item.id}
       locale={locale}
       item={item}
       adminNotePrefix={adminNotePrefix}
       getStatusBadge={getStatusBadge}
      />
     ))}
    </div>
   </GlassCard>
  </div>
 );
}
