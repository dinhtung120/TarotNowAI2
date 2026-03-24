'use client';

import { useEffect, useState } from 'react';
import { listUsers, listDeposits } from '@/features/admin/application/actions';
import { listPromotions } from '@/features/admin/application/actions/promotion';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';

export interface AdminStats {
  users: number;
  deposits: number;
  promotions: number;
  readings: number;
}

export function useAdminDashboard() {
  const [stats, setStats] = useState<AdminStats>({
    users: 0,
    deposits: 0,
    promotions: 0,
    readings: 0,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      setLoading(true);
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

        setStats({
          users: usersRes.success && usersRes.data ? usersRes.data.totalCount : 0,
          deposits: depositsRes.success && depositsRes.data ? depositsRes.data.totalCount : 0,
          promotions: promosRes.success && promosRes.data ? promosRes.data.length : 0,
          readings: readingCount,
        });
      } catch {
        // Keep dashboard usable when backend stats are unavailable.
      } finally {
        setLoading(false);
      }
    };

    void fetchStats();
  }, []);

  return {
    stats,
    loading,
  };
}
