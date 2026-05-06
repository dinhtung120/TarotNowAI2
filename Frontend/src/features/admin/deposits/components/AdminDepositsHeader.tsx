import { CreditCard, Filter } from 'lucide-react';
import { FilterTabs, SectionHeader } from '@/shared/ui';
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
  <div className={cn('tn-flex-col-row-md tn-items-end-md justify-between gap-6')}>
   <SectionHeader
    tag={labels.tag}
    tagIcon={<CreditCard className={cn('w-3 h-3 tn-text-accent')} />}
    title={labels.title}
    subtitle={labels.subtitle}
    className={cn('mb-0 text-left items-start')}
   />
   <div className={cn('flex w-full flex-col gap-3 p-3 shadow-inner tn-panel tn-rounded-2rem sm:w-auto sm:flex-row sm:items-center sm:gap-4 sm:px-6')}>
    <div className={cn('flex items-center gap-2 self-start sm:self-auto')}>
     <Filter className={cn('w-4 h-4 tn-text-secondary')} />
     <span className={cn('tn-text-overline tn-text-secondary')}>{labels.filterLabel}:</span>
    </div>
    <FilterTabs
     value={statusFilter}
     options={[
      { value: '', label: labels.all, activeClassName: 'tn-bg-accent tn-text-ink shadow-md' },
      { value: 'Pending', label: labels.pending, activeClassName: 'tn-bg-accent tn-text-ink shadow-md' },
      { value: 'Success', label: labels.success, activeClassName: 'tn-bg-accent tn-text-ink shadow-md' },
      { value: 'Failed', label: labels.failed, activeClassName: 'tn-bg-accent tn-text-ink shadow-md' },
     ]}
     containerClassName="flex flex-wrap gap-2"
     buttonClassName="px-3 py-2.5 min-h-11 rounded-xl tn-text-overline transition-all sm:px-4"
     inactiveClassName="tn-surface tn-text-tertiary tn-hover-surface-strong tn-hover-text-primary"
     onChange={onStatusFilterChange}
    />
   </div>
  </div>
 );
}
