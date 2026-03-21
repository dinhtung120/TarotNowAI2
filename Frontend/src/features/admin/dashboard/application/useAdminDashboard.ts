'use client';

import { useEffect, useState } from 'react';
import { listUsers, listDeposits } from '@/actions/adminActions';
import { listPromotions } from '@/actions/promotionActions';
import { getAllHistorySessionsAdminAction } from '@/actions/historyActions';

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
            ? readingsRes.data.totalCount
            : 0;

        setStats({
          users: usersRes?.totalCount ?? 0,
          deposits: depositsRes?.totalCount ?? 0,
          promotions: promosRes?.length ?? 0,
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
