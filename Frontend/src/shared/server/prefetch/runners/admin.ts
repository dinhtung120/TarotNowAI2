import type { QueryClient } from '@tanstack/react-query';
import { listDeposits, listReaderRequests, listUsers } from '@/features/admin/application/actions';
import { listPromotions } from '@/features/admin/application/actions/promotion';
import { listAdminDisputes } from '@/features/chat/application/actions';
import { adminGamificationKeys } from '@/features/gamification/admin/application/adminGamificationKeys';
import {
 fetchAdminGamificationAchievementsServer,
 fetchAdminGamificationQuestsServer,
 fetchAdminGamificationTitlesServer,
} from '@/features/gamification/admin/infrastructure/adminGamificationServer';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';
import { listWithdrawalQueue } from '@/features/wallet/public';
import { logger } from '@/shared/application/gateways/logger';
import { queryFnOrThrow } from '@/shared/application/utils/queryPolicy';

async function swallowPrefetch(
 label: string,
 run: () => Promise<void>,
): Promise<void> {
 try {
  await run();
 } catch (error) {
  logger.warn('[AdminPrefetch]', `${label}.failed`, { error });
 }
}

export async function prefetchAdminDashboardPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: ['admin', 'dashboard-stats'],
  queryFn: async () => {
   const [usersRes, depositsRes, promosRes, readingsRes] = await Promise.all([
    listUsers(1, 1),
    listDeposits(1, 1),
    listPromotions(false),
    getAllHistorySessionsAdminAction({ page: 1, pageSize: 1 }),
   ]);

   const users = queryFnOrThrow(usersRes, 'Failed to load admin users for dashboard prefetch');
   const deposits = queryFnOrThrow(depositsRes, 'Failed to load admin deposits for dashboard prefetch');
   const promotions = queryFnOrThrow(promosRes, 'Failed to load promotions for dashboard prefetch');
   const readings = queryFnOrThrow(readingsRes, 'Failed to load readings for dashboard prefetch');

   return {
    users: users.totalCount,
    deposits: deposits.totalCount,
    promotions: promotions.length,
    readings: readings.totalCount,
   };
  },
 });
}

export async function prefetchAdminUsersPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch('prefetchAdminUsersPage', async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'users', 1, ''] as const,
   queryFn: async () => {
    const result = await listUsers(1, 10, '');
    const payload = queryFnOrThrow(result, 'Failed to prefetch admin users');
    return {
     users: payload.users.map((item) => ({
      ...item,
      isLocked: item.status?.toLowerCase() === 'locked',
     })),
     totalCount: payload.totalCount,
    };
   },
  });
 });
}

export async function prefetchAdminDepositsPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch('prefetchAdminDepositsPage', async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'deposits', 1, ''],
   queryFn: async () => {
    const result = await listDeposits(1, 10, '');
    return queryFnOrThrow(result, 'Failed to prefetch admin deposits');
   },
  });
 });
}

export async function prefetchAdminReaderRequestsPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch('prefetchAdminReaderRequestsPage', async () => {
  const pageSize = 10;
  const statusFilter = 'pending';
  await qc.prefetchQuery({
   queryKey: ['admin', 'reader-requests', 1, statusFilter],
   queryFn: async () => {
    const result = await listReaderRequests(1, pageSize, statusFilter);
    return queryFnOrThrow(result, 'Failed to prefetch admin reader requests');
   },
  });
 });
}

export async function prefetchAdminReadingsPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch('prefetchAdminReadingsPage', async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'readings', 1, { username: '', spreadType: '', startDate: '', endDate: '' }],
   queryFn: async () => {
    const result = await getAllHistorySessionsAdminAction({
     page: 1,
     pageSize: 10,
     username: '',
     spreadType: '',
     startDate: undefined,
     endDate: undefined,
    });
    return queryFnOrThrow(result, 'Failed to prefetch admin readings');
   },
  });
 });
}

export async function prefetchAdminWithdrawalsQueuePage(qc: QueryClient): Promise<void> {
 await swallowPrefetch('prefetchAdminWithdrawalsQueuePage', async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'withdrawals', 'queue'] as const,
   queryFn: async () => {
    const result = await listWithdrawalQueue();
    return queryFnOrThrow(result, 'Failed to prefetch withdrawal queue');
   },
  });
 });
}

export async function prefetchAdminDisputesPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch('prefetchAdminDisputesPage', async () => {
  await qc.prefetchQuery({
   queryKey: ['admin', 'disputes', 'list'],
   queryFn: async () => {
    const result = await listAdminDisputes(1, 100);
    return queryFnOrThrow(result, 'Failed to prefetch admin disputes').items;
   },
 });
});
}

export async function prefetchAdminGamificationPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch('prefetchAdminGamificationPage', async () => {
  await Promise.all([
   qc.prefetchQuery({
    queryKey: adminGamificationKeys.quests(),
    queryFn: fetchAdminGamificationQuestsServer,
   }),
   qc.prefetchQuery({
    queryKey: adminGamificationKeys.achievements(),
    queryFn: fetchAdminGamificationAchievementsServer,
   }),
   qc.prefetchQuery({
    queryKey: adminGamificationKeys.titles(),
    queryFn: fetchAdminGamificationTitlesServer,
   }),
  ]);
 });
}
