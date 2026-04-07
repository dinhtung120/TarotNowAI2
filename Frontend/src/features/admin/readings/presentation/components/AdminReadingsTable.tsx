'use client';

import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import type { PaginatedResponse } from '@/features/admin/readings/application/useAdminReadings';
import { AdminReadingsPagination } from './AdminReadingsPagination';
import { AdminReadingsTableContent } from './AdminReadingsTableContent';
import { AdminReadingsTableHead } from './AdminReadingsTableHead';

interface AdminReadingsTableProps {
 data: PaginatedResponse | null | undefined;
 loading: boolean;
 page: number;
 onPageChange: (page: number) => void;
 getSpreadLabel: (type: string) => string;
}

export function AdminReadingsTable({
 data,
 loading,
 page,
 onPageChange,
 getSpreadLabel,
}: AdminReadingsTableProps) {
 return (
  <GlassCard className={cn('!p-0 !tn-rounded-2_5xl overflow-hidden text-left')}>
   <div className={cn('overflow-x-auto custom-scrollbar')}>
    <table className={cn('w-full text-left')}>
     <AdminReadingsTableHead />
     <tbody className={cn('divide-y divide-white/5')}>
      <AdminReadingsTableContent
       data={data}
       loading={loading}
       getSpreadLabel={getSpreadLabel}
      />
     </tbody>
    </table>
   </div>
   <AdminReadingsPagination
    data={data}
    loading={loading}
    page={page}
    onPageChange={onPageChange}
   />
  </GlassCard>
 );
}
