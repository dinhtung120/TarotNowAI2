'use client';

import { useQuery } from '@tanstack/react-query';
import { listUsers, listDeposits } from '@/features/admin/application/actions';
import { adminQueryKeys } from '@/features/admin/application/adminQueryKeys';
import { listPromotions } from '@/features/admin/application/actions/promotion';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';
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
   const [usersRes, depositsRes, promosRes, readingsRes] = await Promise.all([
    listUsers(1, 1),
    listDeposits(1, 1),
    listPromotions(false),
    getAllHistorySessionsAdminAction({ page: 1, pageSize: 1 }),
   ]);

   const users = queryFnOrThrow(usersRes, 'Failed to load admin users for dashboard stats');
   const deposits = queryFnOrThrow(depositsRes, 'Failed to load admin deposits for dashboard stats');
   const promotions = queryFnOrThrow(promosRes, 'Failed to load admin promotions for dashboard stats');
   const readings = queryFnOrThrow(readingsRes, 'Failed to load admin readings for dashboard stats');

   return {
    users: users.totalCount,
    deposits: deposits.totalCount,
    promotions: promotions.length,
    readings: readings.totalCount,
   };
  },
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
