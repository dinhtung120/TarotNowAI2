import { memo } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { cn } from '@/lib/utils';

interface DepositHistoryPaginationProps {
 page: number;
 totalPages: number;
 isLoading: boolean;
 pageLabel: (page: number, totalPages: number) => string;
 prevLabel: string;
 nextLabel: string;
 onPrev: () => void;
 onNext: () => void;
}

function DepositHistoryPaginationComponent({
 page,
 totalPages,
 isLoading,
 pageLabel,
 prevLabel,
 nextLabel,
 onPrev,
 onNext,
}: DepositHistoryPaginationProps) {
 return (
  <div className={cn('flex flex-wrap items-center justify-between gap-4 border-t tn-border-soft pt-5')}>
   <span className={cn('text-xs font-black uppercase tracking-[0.22em] tn-text-secondary')}>
    {pageLabel(page, totalPages)}
   </span>
   <div className={cn('flex items-center gap-2')}>
    <button
     type="button"
     onClick={onPrev}
     disabled={page <= 1 || isLoading}
     className={cn('min-h-11 rounded-xl border tn-border-soft px-3 py-2 text-xs font-black uppercase tracking-wider tn-text-secondary hover:tn-text-primary disabled:opacity-60')}
    >
     <span className={cn('inline-flex items-center gap-1')}>
      <ChevronLeft className={cn('h-4 w-4')} />
      {prevLabel}
     </span>
    </button>
    <button
     type="button"
     onClick={onNext}
     disabled={page >= totalPages || isLoading}
     className={cn('min-h-11 rounded-xl border tn-border-soft px-3 py-2 text-xs font-black uppercase tracking-wider tn-text-secondary hover:tn-text-primary disabled:opacity-60')}
    >
     <span className={cn('inline-flex items-center gap-1')}>
      {nextLabel}
      <ChevronRight className={cn('h-4 w-4')} />
     </span>
    </button>
   </div>
  </div>
 );
}

export const DepositHistoryPagination = memo(DepositHistoryPaginationComponent);
DepositHistoryPagination.displayName = 'DepositHistoryPagination';
