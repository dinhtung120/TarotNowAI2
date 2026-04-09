'use client';

import { useQuery } from '@tanstack/react-query';
import { listUsers, listDeposits } from '@/features/admin/application/actions';
import { listPromotions } from '@/features/admin/application/actions/promotion';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';

interface AdminStats {
  users: number;
  deposits: number;
  promotions: number;
  readings: number;
}

export function useAdminDashboard() {
 const { data, isLoading, isFetching } = useQuery<AdminStats>({
  queryKey: ['admin', 'dashboard-stats'],
  queryFn: async () => {
   try {
    const [usersRes, depositsRes, promosRes, readingsRes] = await Promise.all([
     listUsers(1, 1),
     listDeposits(1, 1),
     listPromotions(false),
     getAllHistorySessionsAdminAction({ page: 1, pageSize: 1 }),
    ]);

    const readingCount =
     readingsRes && 'success' in readingsRes && readingsRes.success
      ? readingsRes.data?.totalCount ?? 0
      : 0;

    return {
     users: usersRes.success && usersRes.data ? usersRes.data.totalCount : 0,
     deposits: depositsRes.success && depositsRes.data ? depositsRes.data.totalCount : 0,
     promotions: promosRes.success && promosRes.data ? promosRes.data.length : 0,
     readings: readingCount,
    };
   } catch {
    return {
     users: 0,
     deposits: 0,
     promotions: 0,
     readings: 0,
    };
   }
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
  };
}
