'use client';

import { History, Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import type { PaginatedResponse } from '@/features/admin/readings/application/useAdminReadings';
import { AdminReadingsTableRow } from './AdminReadingsTableRow';

interface AdminReadingsTableContentProps {
 data: PaginatedResponse | null | undefined;
 loading: boolean;
 getSpreadLabel: (type: string) => string;
}

export function AdminReadingsTableContent({ data, loading, getSpreadLabel }: AdminReadingsTableContentProps) {
 const t = useTranslations('Admin');

 if (loading) {
  return (
   <tr>
    <td colSpan={5} className={cn('px-8 py-20 text-center')}>
     <div className={cn('flex flex-col items-center justify-center space-y-4')}>
      <Loader2 className={cn('w-8 h-8 animate-spin text-[var(--purple-accent)] mx-auto')} />
      <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
       {t('readings.states.loading')}
      </span>
     </div>
    </td>
   </tr>
  );
 }

 if (!data || !data.items || data.items.length === 0) {
  return (
   <tr>
    <td colSpan={5} className={cn('px-8 py-20 text-center')}>
     <div className={cn('flex flex-col items-center justify-center space-y-4')}>
      <div className={cn('w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center')}>
       <History className={cn('w-8 h-8 text-[var(--text-tertiary)] opacity-50')} />
      </div>
      <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>
       {t('readings.states.empty')}
      </span>
     </div>
    </td>
   </tr>
  );
 }

 return (
  <>
   {data.items.map((reading) => (
    <AdminReadingsTableRow
     key={reading.id}
     reading={reading}
     getSpreadLabel={getSpreadLabel}
    />
   ))}
  </>
 );
}
