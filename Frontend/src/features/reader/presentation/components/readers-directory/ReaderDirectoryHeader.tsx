import { Users } from 'lucide-react';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReaderDirectoryHeaderProps {
 totalCount: number;
 labels: {
  tag: string;
  title: string;
  subtitle: string;
  totalLabel: string;
 };
}

export function ReaderDirectoryHeader({ totalCount, labels }: ReaderDirectoryHeaderProps) {
 return (
  <div className={cn('flex flex-col md:flex-row md:items-end justify-between gap-6 mb-2')}>
   <SectionHeader
    tag={labels.tag}
    tagIcon={<Users className={cn('w-3 h-3 text-[var(--purple-accent)]')} />}
    title={labels.title}
    subtitle={labels.subtitle}
    className={cn('mb-0 text-left items-start md:mb-0')}
   />
   <div className={cn('tn-panel rounded-2xl px-5 py-3 flex items-center gap-3 self-start md:self-auto')}>
    <div className={cn('w-10 h-10 rounded-full bg-[var(--purple-accent)]/20 flex items-center justify-center')}>
     <Users className={cn('w-5 h-5 text-[var(--purple-accent)]')} />
    </div>
    <div>
     <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
      {labels.totalLabel}
     </div>
     <div className={cn('text-2xl font-black tn-text-primary leading-none')}>{totalCount}</div>
    </div>
   </div>
  </div>
 );
}
