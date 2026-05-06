import { memo } from 'react';
import { cn } from '@/lib/utils';

type DepositHistoryStatusFilter = 'all' | 'pending' | 'success' | 'failed';

interface DepositHistoryFiltersProps {
 currentFilter: DepositHistoryStatusFilter;
 onChange: (filter: DepositHistoryStatusFilter) => void;
 labels: {
  all: string;
  pending: string;
  success: string;
  failed: string;
 };
}

function DepositHistoryFiltersComponent({
 currentFilter,
 onChange,
 labels,
}: DepositHistoryFiltersProps) {
 return (
  <div className={cn('flex flex-wrap gap-2')}>
   <FilterButton
    active={currentFilter === 'all'}
    onClick={() => onChange('all')}
    label={labels.all}
   />
   <FilterButton
    active={currentFilter === 'pending'}
    onClick={() => onChange('pending')}
    label={labels.pending}
   />
   <FilterButton
    active={currentFilter === 'success'}
    onClick={() => onChange('success')}
    label={labels.success}
   />
   <FilterButton
    active={currentFilter === 'failed'}
    onClick={() => onChange('failed')}
    label={labels.failed}
   />
  </div>
 );
}

interface FilterButtonProps {
 active: boolean;
 label: string;
 onClick: () => void;
}

function FilterButton({ active, label, onClick }: FilterButtonProps) {
 return (
  <button
   type="button"
   onClick={onClick}
   className={cn(
    'min-h-11 rounded-xl border px-4 py-2 text-xs font-black uppercase tracking-wider transition-colors',
    active
     ? 'border-emerald-400/60 bg-emerald-500/15 text-emerald-100'
     : 'tn-border-soft tn-surface tn-text-secondary hover:tn-text-primary',
   )}
  >
   {label}
  </button>
 );
}

export const DepositHistoryFilters = memo(DepositHistoryFiltersComponent);
DepositHistoryFilters.displayName = 'DepositHistoryFilters';
