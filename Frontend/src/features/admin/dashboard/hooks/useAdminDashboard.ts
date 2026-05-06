'use client';

import { useQuery } from '@tanstack/react-query';
import { getAdminDashboardSummary } from '@/features/admin/dashboard/actions/dashboard';
import { ADMIN_QUERY_POLICY } from '@/features/admin/shared/adminQueryPolicy';
import { adminQueryKeys } from '@/features/admin/shared/adminQueryKeys';
import { queryFnOrThrow } from '@/shared/application/utils/queryPolicy';

interface AdminStats {
  users: number;
  deposits: number;
  promotions: number;
  readings: number;
}

export function useAdminDashboard() {
 const { data, isLoading, isFetching, error } = useQuery<AdminStats>({
  queryKey: adminQueryKeys.dashboardStats(),
  queryFn: async () => {
   const result = await getAdminDashboardSummary();
   return queryFnOrThrow(result, 'Failed to load admin dashboard summary');
  },
  ...ADMIN_QUERY_POLICY.dashboard,
 });

  return {
  stats: data ?? {
   users: 0,
   deposits: 0,
   promotions: 0,
   readings: 0,
  },
  loading: isLoading || isFetching,
  statsError: error instanceof Error ? error.message : '',
  };
}
