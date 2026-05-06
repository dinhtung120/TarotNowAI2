'use client';

import { History, Loader2, TriangleAlert } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import type { PaginatedResponse } from '@/features/admin/readings/hooks/useAdminReadings';
import { AdminReadingsTableRow } from './AdminReadingsTableRow';

interface AdminReadingsTableContentProps {
 data: PaginatedResponse | null | undefined;
 loading: boolean;
 listError: string;
 getSpreadLabel: (type: string) => string;
}

export function AdminReadingsTableContent({ data, loading, listError, getSpreadLabel }: AdminReadingsTableContentProps) {
 const t = useTranslations('Admin');

 if (loading) {
  return (
   <tr>
    <td colSpan={5} className={cn('px-8 py-20 text-center')}>
     <div className={cn('flex flex-col items-center justify-center space-y-4')}>
      <Loader2 className={cn('w-8 h-8 animate-spin tn-text-accent mx-auto')} />
      <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
       {t('readings.states.loading')}
      </span>
     </div>
    </td>
   </tr>
  );
 }

 if (!data || !data.items || data.items.length === 0) {
  if (listError) {
   return (
    <tr>
     <td colSpan={5} className={cn('px-8 py-20 text-center')}>
      <div className={cn('flex flex-col items-center justify-center space-y-4')}>
       <TriangleAlert className={cn('w-8 h-8 text-red-300')} />
       <span className={cn('tn-text-10 font-black uppercase tracking-widest text-red-300')}>
        {listError}
       </span>
      </div>
     </td>
    </tr>
   );
  }

  return (
   <tr>
    <td colSpan={5} className={cn('px-8 py-20 text-center')}>
     <div className={cn('flex flex-col items-center justify-center space-y-4')}>
      <div className={cn('w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center')}>
       <History className={cn('w-8 h-8 tn-text-tertiary opacity-50')} />
      </div>
      <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-tertiary')}>
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
