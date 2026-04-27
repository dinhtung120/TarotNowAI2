import type { QueryClient } from '@tanstack/react-query';
import { listDeposits, listReaderRequests, listUsers } from '@/features/admin/application/actions';
import { listPromotions } from '@/features/admin/application/actions/promotion';
import { listAdminDisputes } from '@/features/chat/application/actions';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';
import { listWithdrawalQueue } from '@/features/wallet/public';

async function swallowPrefetch(run: () => Promise<void>): Promise<void> {
 try {
  await run();
 } catch {
  /* Prefetch server-side là best-effort nếu thiếu session hoặc API lỗi. */
 }
}

export async function prefetchAdminDashboardPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
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
}

export async function prefetchAdminUsersPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'users', 1, ''] as const,
   queryFn: async () => {
    const result = await listUsers(1, 10, '');
    if (!result.success || !result.data) {
     return { users: [], totalCount: 0 };
    }
    return {
     users: result.data.users.map((item) => ({
      ...item,
      isLocked: item.status?.toLowerCase() === 'locked',
     })),
     totalCount: result.data.totalCount,
    };
   },
  });
 });
}

export async function prefetchAdminDepositsPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'deposits', 1, ''],
   queryFn: async () => {
    const result = await listDeposits(1, 10, '');
    if (!result.success || !result.data) {
     return { deposits: [], totalCount: 0 };
    }
    return result.data;
   },
  });
 });
}

export async function prefetchAdminReaderRequestsPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  const pageSize = 10;
  const statusFilter = 'pending';
  await qc.prefetchQuery({
   queryKey: ['admin', 'reader-requests', 1, statusFilter],
   queryFn: async () => {
    const result = await listReaderRequests(1, pageSize, statusFilter);
    if (!result.success || !result.data) {
     return { requests: [], totalCount: 0 };
    }
    return result.data;
   },
  });
 });
}

export async function prefetchAdminReadingsPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'readings', 1, '', '', '', ''],
   queryFn: async () => {
    const result = await getAllHistorySessionsAdminAction({
     page: 1,
     pageSize: 10,
     username: '',
     spreadType: '',
     startDate: undefined,
     endDate: undefined,
    });
    if (result.success && result.data) {
     return result.data;
    }
    return null;
   },
  });
 });
}

export async function prefetchAdminWithdrawalsQueuePage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'withdrawals', 'queue'] as const,
   queryFn: async () => {
    const result = await listWithdrawalQueue();
    return result.success && result.data ? result.data : [];
   },
  });
 });
}

export async function prefetchAdminDisputesPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'disputes', 'list'],
   queryFn: async () => {
    const result = await listAdminDisputes(1, 100);
    if (result.success && result.data) {
     return result.data.items;
    }
    return [];
   },
  });
 });
}
