import { ArrowUpRight, Gem } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminDepositsSummaryProps {
 labels: {
  confirmTitle: string;
  confirmDescription: string;
  ledgerTitle: string;
  ledgerSubtitle: string;
 };
}

export function AdminDepositsSummary({ labels }: AdminDepositsSummaryProps) {
 return (
  <div className={cn('grid grid-cols-1 md:grid-cols-2 gap-6 animate-in fade-in slide-in-from-bottom-4 duration-1000')}>
   <GlassCard className={cn('!p-8 relative overflow-hidden group hover:border-[var(--purple-accent)]/30 transition-all text-left')}>
    <div className={cn('absolute -bottom-4 -right-4 opacity-5 group-hover:scale-110 transition-transform duration-700')}>
     <ArrowUpRight size={150} />
    </div>
    <div className={cn('relative z-10 space-y-4')}>
     <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] drop-shadow-sm')}>
      {labels.confirmTitle}
     </div>
     <p className={cn('text-xs text-[var(--text-secondary)] leading-relaxed font-medium')}>{labels.confirmDescription}</p>
    </div>
   </GlassCard>
   <GlassCard className={cn('!p-8 flex items-center gap-6 group hover:tn-border transition-all text-left')}>
    <div className={cn('w-14 h-14 rounded-2xl tn-overlay flex items-center justify-center border tn-border group-hover:scale-110 transition-transform shadow-inner')}>
     <Gem className={cn('w-7 h-7 text-[var(--purple-accent)]')} />
    </div>
    <div>
     <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>{labels.ledgerTitle}</div>
     <div className={cn('text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md')}>{labels.ledgerSubtitle}</div>
    </div>
   </GlassCard>
  </div>
 );
}
