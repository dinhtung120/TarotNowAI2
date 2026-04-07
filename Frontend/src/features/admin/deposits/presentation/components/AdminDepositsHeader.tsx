import { CreditCard, Filter } from 'lucide-react';
import { FilterTabs, SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminDepositsHeaderProps {
 statusFilter: string;
 labels: {
  tag: string;
  title: string;
  subtitle: string;
  filterLabel: string;
  all: string;
  pending: string;
  success: string;
  failed: string;
 };
 onStatusFilterChange: (value: string) => void;
}

export function AdminDepositsHeader({
 statusFilter,
 labels,
 onStatusFilterChange,
}: AdminDepositsHeaderProps) {
 return (
  <div className={cn('flex flex-col md:flex-row md:items-end justify-between gap-6')}>
   <SectionHeader
    tag={labels.tag}
    tagIcon={<CreditCard className={cn('w-3 h-3 text-[var(--purple-accent)]')} />}
    title={labels.title}
    subtitle={labels.subtitle}
    className={cn('mb-0 text-left items-start')}
   />
   <div className={cn('flex items-center gap-4 tn-panel rounded-[2rem] p-3 px-6 shadow-inner shrink-0')}>
    <div className={cn('flex items-center gap-2')}>
     <Filter className={cn('w-4 h-4 text-[var(--text-secondary)]')} />
     <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>{labels.filterLabel}:</span>
    </div>
    <FilterTabs
     value={statusFilter}
     options={[
      { value: '', label: labels.all, activeClassName: 'bg-[var(--purple-accent)] tn-text-ink shadow-md' },
      { value: 'Pending', label: labels.pending, activeClassName: 'bg-[var(--purple-accent)] tn-text-ink shadow-md' },
      { value: 'Success', label: labels.success, activeClassName: 'bg-[var(--purple-accent)] tn-text-ink shadow-md' },
      { value: 'Failed', label: labels.failed, activeClassName: 'bg-[var(--purple-accent)] tn-text-ink shadow-md' },
     ]}
     containerClassName="flex gap-2"
     buttonClassName="px-4 py-2.5 min-h-11 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all"
     inactiveClassName="tn-surface text-[var(--text-tertiary)] hover:tn-surface-strong hover:tn-text-primary"
     onChange={onStatusFilterChange}
    />
   </div>
  </div>
 );
}
