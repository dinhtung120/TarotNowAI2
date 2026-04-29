'use client';

import { cn } from '@/lib/utils';
import { useAdminReadings } from '@/features/admin/readings/application/useAdminReadings';
import {
 AdminReadingsFilters,
 AdminReadingsHeader,
 AdminReadingsInsights,
 AdminReadingsTable,
} from '@/features/admin/readings/presentation/components';

export default function AdminReadingsPage() {
 const {
  data,
  loading,
  page,
  setPage,
  filters,
  applyFilters,
  listError,
  getSpreadLabel,
 } = useAdminReadings();

 return (
  <div className={cn('space-y-8 pb-20 animate-in fade-in duration-700')}>
   <AdminReadingsHeader totalCount={data?.totalCount ?? 0} />

   <AdminReadingsFilters
    values={filters}
    onSubmit={applyFilters}
   />

   <AdminReadingsTable
    data={data}
    loading={loading}
    listError={listError}
    page={page}
    onPageChange={setPage}
    getSpreadLabel={getSpreadLabel}
   />

   <AdminReadingsInsights data={data} loading={loading} />
  </div>
 );
}
