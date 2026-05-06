import { ShieldCheck, Users } from 'lucide-react';
import { SectionHeader } from '@/shared/ui';
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
  <div className={cn('tn-flex-col-row-md tn-items-end-md justify-between gap-6')}>
   <SectionHeader
    tag={labels.tag}
    tagIcon={<ShieldCheck className={cn('w-3 h-3 tn-text-accent')} />}
    title={labels.title}
    subtitle={labels.subtitle}
    className={cn('mb-0 text-left items-start')}
   />
   <div className={cn('flex w-full items-center justify-center gap-4 p-2 pr-4 shadow-inner tn-panel tn-rounded-2rem sm:w-auto sm:justify-start')}>
    <div className={cn('w-10 h-10 rounded-xl tn-bg-accent-10 flex items-center justify-center border tn-border-accent-20 shadow-inner')}>
     <Users className={cn('w-4 h-4 tn-text-accent')} />
    </div>
    <div className={cn('space-y-0.5')}>
     <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
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
