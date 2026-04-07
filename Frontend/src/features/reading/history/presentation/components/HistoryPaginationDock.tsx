import { ChevronLeft, ChevronRight } from 'lucide-react';
import { cn } from '@/lib/utils';

interface HistoryPaginationDockProps {
 currentPage: number;
 totalPages: number;
 prevLabel: string;
 nextLabel: string;
 pageInfo: (current: number, total: number) => string;
 onPrev: () => void;
 onNext: () => void;
}

export function HistoryPaginationDock({
 currentPage,
 totalPages,
 prevLabel,
 nextLabel,
 pageInfo,
 onPrev,
 onNext,
}: HistoryPaginationDockProps) {
 return (
  <div className={cn('fixed bottom-[calc(6.5rem+env(safe-area-inset-bottom))] md:bottom-10 left-1/2 -translate-x-1/2 z-50 flex items-center gap-3 sm:gap-6 px-4 sm:px-6 py-2.5 sm:py-3 bg-[var(--bg-glass)]/90 border tn-border rounded-full shadow-[0_20px_50px_var(--c-0-0-0-80)] animate-in fade-in slide-in-from-bottom-4 duration-1000 max-w-[calc(100vw-1rem)]')}>
   <button onClick={onPrev} disabled={currentPage === 1} className={cn('p-2.5 min-h-11 min-w-11 rounded-full tn-surface tn-text-secondary disabled:opacity-20 hover:tn-surface-strong hover:tn-text-primary transition-all active:scale-90')} title={prevLabel}><ChevronLeft className={cn('w-4 h-4')} /></button>
   <div className={cn('flex flex-col items-center min-w-[72px]')}>
    <span className={cn('text-[9px] font-black uppercase tracking-[0.15em] sm:tracking-[0.2em] text-[var(--text-tertiary)] text-center')}>
     {pageInfo(currentPage, totalPages)}
    </span>
   </div>
   <button onClick={onNext} disabled={currentPage === totalPages} className={cn('p-2.5 min-h-11 min-w-11 rounded-full tn-surface tn-text-secondary disabled:opacity-20 hover:tn-surface-strong hover:tn-text-primary transition-all active:scale-90')} title={nextLabel}><ChevronRight className={cn('w-4 h-4')} /></button>
  </div>
 );
}
