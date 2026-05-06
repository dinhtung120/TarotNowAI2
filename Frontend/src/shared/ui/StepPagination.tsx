import { ChevronLeft, ChevronRight } from 'lucide-react';
import { cn } from '@/lib/utils';

interface StepPaginationProps {
 summary?: string;
 canPrev: boolean;
 canNext: boolean;
 onPrev: () => void;
 onNext: () => void;
 currentLabel?: string;
 className?: string;
}

export default function StepPagination({
 summary,
 canPrev,
 canNext,
 onPrev,
 onNext,
 currentLabel,
 className,
}: StepPaginationProps) {
 return (
  <div className={cn('tn-step-pagination-layout px-8 py-6 tn-surface-soft justify-between gap-4 border-t tn-border-soft', className)}>
   {summary ? (
    <div className={cn("tn-text-10 font-black uppercase tracking-widest tn-text-tertiary text-left")}>{summary}</div>
   ) : null}
   <div className={cn("flex items-center gap-3")}>
    <button
     type="button"
     onClick={onPrev}
     disabled={!canPrev}
     className={cn("p-2.5 min-h-11 min-w-11 rounded-xl tn-panel tn-pagination-btn transition-all")}
    >
     <ChevronLeft className={cn("w-4 h-4 tn-text-secondary")} />
    </button>
    {currentLabel ? (
     <span className={cn("text-xs font-black tn-text-accent italic mx-2")}>{currentLabel}</span>
    ) : null}
    <button
     type="button"
     onClick={onNext}
     disabled={!canNext}
     className={cn("p-2.5 min-h-11 min-w-11 rounded-xl tn-panel tn-pagination-btn transition-all")}
    >
     <ChevronRight className={cn("w-4 h-4 tn-text-secondary")} />
    </button>
   </div>
  </div>
 );
}
