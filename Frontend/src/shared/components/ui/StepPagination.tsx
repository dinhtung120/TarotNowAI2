import { ChevronLeft, ChevronRight } from 'lucide-react';

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
    <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] text-left">{summary}</div>
   ) : null}
   <div className="flex items-center gap-3">
    <button
     onClick={onPrev}
     disabled={!canPrev}
     className="p-2.5 min-h-11 min-w-11 rounded-xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md"
    >
     <ChevronLeft className="w-4 h-4 text-[var(--text-secondary)]" />
    </button>
    {currentLabel ? (
     <span className="text-xs font-black text-[var(--purple-accent)] italic mx-2">{currentLabel}</span>
    ) : null}
    <button
     onClick={onNext}
     disabled={!canNext}
     className="p-2.5 min-h-11 min-w-11 rounded-xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all hover:shadow-md"
    >
     <ChevronRight className="w-4 h-4 text-[var(--text-secondary)]" />
    </button>
   </div>
  </div>
 );
}
