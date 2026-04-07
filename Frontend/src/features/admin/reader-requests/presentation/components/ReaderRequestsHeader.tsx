import { ShieldCheck, Users } from 'lucide-react';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReaderRequestsHeaderProps {
 totalCount: number;
 labels: {
  tag: string;
  title: string;
  subtitle: string;
  totalLabel: string;
 };
}

export function ReaderRequestsHeader({
 totalCount,
 labels,
}: ReaderRequestsHeaderProps) {
 return (
  <div className={cn('flex flex-col md:flex-row md:items-end justify-between gap-6')}>
   <SectionHeader
    tag={labels.tag}
    tagIcon={<ShieldCheck className={cn('w-3 h-3 text-[var(--purple-accent)]')} />}
    title={labels.title}
    subtitle={labels.subtitle}
    className={cn('mb-0 text-left items-start')}
   />
   <div className={cn('flex items-center gap-4 tn-panel rounded-[2rem] p-2 pr-4 shadow-inner min-w-max')}>
    <div className={cn('w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 shadow-inner')}>
     <Users className={cn('w-4 h-4 text-[var(--purple-accent)]')} />
    </div>
    <div className={cn('space-y-0.5')}>
     <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
      {labels.totalLabel}
     </div>
     <div className={cn('text-sm font-black tn-text-primary italic tracking-tighter drop-shadow-sm')}>
      {totalCount}
     </div>
    </div>
   </div>
  </div>
 );
}
