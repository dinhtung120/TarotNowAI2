'use client';

import { ChevronLeft, ChevronRight } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import type { PaginatedResponse } from '@/features/admin/readings/application/useAdminReadings';

interface AdminReadingsPaginationProps {
 data: PaginatedResponse | null | undefined;
 loading: boolean;
 page: number;
 onPageChange: (page: number) => void;
}

export function AdminReadingsPagination({
 data,
 loading,
 page,
 onPageChange,
}: AdminReadingsPaginationProps) {
 const t = useTranslations('Admin');

 if (!data || data.totalPages <= 1) {
  return null;
 }

 return (
  <div className={cn('px-8 py-6 tn-surface-soft flex flex-col md:flex-row md:items-center justify-between gap-4 border-t tn-border-soft')}>
   <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] text-left')}>
    {t('readings.pagination.summary', { page: data.page, total: data.totalPages })}
   </div>
   <div className={cn('flex items-center gap-3')}>
    <button
     type="button"
     onClick={() => onPageChange(Math.max(1, page - 1))}
     disabled={page === 1 || loading}
     className={cn('p-3.5 rounded-2xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all shadow-inner')}
    >
     <ChevronLeft className={cn('w-4 h-4 text-[var(--text-secondary)]')} />
    </button>
    <span className={cn('text-xs font-black tn-text-primary italic mx-4 drop-shadow-sm')}>{page}</span>
    <button
     type="button"
     onClick={() => onPageChange(Math.min(data.totalPages, page + 1))}
     disabled={page === data.totalPages || loading}
     className={cn('p-3.5 rounded-2xl tn-panel hover:tn-surface-strong disabled:opacity-30 disabled:cursor-not-allowed transition-all shadow-inner')}
    >
     <ChevronRight className={cn('w-4 h-4 text-[var(--text-secondary)]')} />
    </button>
   </div>
  </div>
 );
}
