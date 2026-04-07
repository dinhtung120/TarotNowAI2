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
 className = 'px-8 py-6 tn-surface-soft flex flex-col md:flex-row md:items-center justify-between gap-4 border-t tn-border-soft',
}: StepPaginationProps) {
 return (
  <div className={className}>
   {summary ? (
    <div className={cn("text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] text-left")}>{summary}</div>
   ) : null}
   <div className={cn("flex items-center gap-3")}>
    <button
     type="button"
     onClick={onPrev}
     disabled={!canPrev}
     className={cn("p-2.5 min-h-11 min-w-11 rounded-xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md")}
    >
     <ChevronLeft className={cn("w-4 h-4 text-[var(--text-secondary)]")} />
    </button>
    {currentLabel ? (
     <span className={cn("text-xs font-black text-[var(--purple-accent)] italic mx-2")}>{currentLabel}</span>
    ) : null}
    <button
     type="button"
     onClick={onNext}
     disabled={!canNext}
     className={cn("p-2.5 min-h-11 min-w-11 rounded-xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md")}
    >
     <ChevronRight className={cn("w-4 h-4 text-[var(--text-secondary)]")} />
    </button>
   </div>
  </div>
 );
}
