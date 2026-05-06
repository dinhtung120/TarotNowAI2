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
  <div className={cn('tn-history-pagination-dock animate-in fade-in slide-in-from-bottom-4 duration-1000')}>
   <button type="button" onClick={onPrev} disabled={currentPage === 1} className={cn('p-2.5 min-h-11 min-w-11 rounded-full tn-history-pagination-btn tn-disabled-opacity-20')} title={prevLabel}><ChevronLeft className={cn('w-4 h-4')} /></button>
   <div className={cn('flex flex-col items-center tn-minw-72')}>
    <span className={cn('tn-history-page-info font-black uppercase')}>
     {pageInfo(currentPage, totalPages)}
    </span>
   </div>
   <button type="button" onClick={onNext} disabled={currentPage === totalPages} className={cn('p-2.5 min-h-11 min-w-11 rounded-full tn-history-pagination-btn tn-disabled-opacity-20')} title={nextLabel}><ChevronRight className={cn('w-4 h-4')} /></button>
  </div>
 );
}
