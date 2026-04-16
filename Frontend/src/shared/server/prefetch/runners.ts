import type { QueryClient } from '@tanstack/react-query';
import { listDeposits, listReaderRequests, listUsers } from '@/features/admin/application/actions';
import { listPromotions } from '@/features/admin/application/actions/promotion';
import { getFeedAction } from '@/features/community/application/actions/communityActions';
import type { CommunityFeedResponse } from '@/features/community/types';
import { getUserCollection } from '@/features/collection/application/actions';
import { getNavbarSnapshotAction } from '@/shared/application/actions/navbar-snapshot';
import { getReadingSetupSnapshotAction } from '@/shared/application/actions/reading-setup-snapshot';
import { actionOk } from '@/shared/domain/actionResult';
import { checkinQueryKeys } from '@/features/checkin/domain/checkinQueryKeys';
import { listAdminDisputes, listConversations } from '@/features/chat/application/actions';
import {
 fetchGamificationAchievements,
 fetchGamificationLeaderboard,
 fetchGamificationQuests,
 fetchGamificationTitles,
} from '@/features/gamification/application/gamificationServerActions';
import { gamificationKeys } from '@/features/gamification/gamificationQueryKeys';
import { getNotifications } from '@/features/notifications/application/actions';
import { getProfileAction } from '@/features/profile/application/actions/get-profile';
import { getMfaStatus } from '@/features/profile/mfa/application/actions/status';
import { getCardsCatalogAction } from '@/features/reading/application/actions/cards-catalog';
import {
 getHistoryDetailAction,
 getHistorySessionsAction,
} from '@/features/reading/application/actions/history';
import {
 HISTORY_PAGE_SIZE,
 historyDetailQueryKey,
 historySessionsListQueryKey,
} from '@/features/reading/history/domain/historyQueryKeys';
import { listReaders, getMyReaderRequest, getReaderProfile } from '@/features/reader/application/actions';
import { getAllHistorySessionsAdminAction } from '@/features/reading/public';
import { getLedger } from '@/features/wallet/application/actions';
import { listMyWithdrawals } from '@/features/wallet/application/actions/withdrawal';
import { listWithdrawalQueue } from '@/features/wallet/public';
import { AUTH_ERROR, isUnauthorizedError } from '@/shared/domain/authErrors';
import { gachaQueryKeys } from '@/shared/infrastructure/gacha/gachaConstants';
import {
 fetchGachaHistoryServer,
 fetchGachaPoolOddsServer,
 fetchGachaPoolsServer,
} from '@/shared/infrastructure/gacha/gachaServerActions';
import type { GachaPool } from '@/shared/infrastructure/gacha/gachaTypes';
import { inventoryQueryKeys } from '@/shared/infrastructure/inventory/inventoryConstants';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { fetchInventoryServer } from '@/shared/infrastructure/inventory/inventoryServerActions';

const readersDirectoryQueryKey = ['readers', 1, 12, '', '', ''] as const;

/** Mặc định khớp LeaderboardTable lúc mở trang (gold + daily); đổi tab vẫn fetch trên client. */
const DEFAULT_LEADERBOARD_TRACK = 'spent_gold_daily';

async function swallowPrefetch(run: () => Promise<void>): Promise<void> {
 try {
  await run();
 } catch {
  /* Prefetch trên server là best-effort khi thiếu session hoặc API lỗi. */
 }
}

async function prefetchCommunityFeedInfinite(qc: QueryClient, visibility: 'public' | 'private'): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchInfiniteQuery({
   queryKey: ['community', 'feed', visibility],
   queryFn: async ({ pageParam = 1 }) => {
    const res = await getFeedAction(pageParam as number, 10, visibility);
    if (!res.success) {
     throw new Error(res.error);
    }
    return res.data!;
   },
   initialPageParam: 1,
   getNextPageParam: (lastPage: CommunityFeedResponse) => {
    const { page, pageSize, totalCount } = lastPage.metadata;
    const hasMore = page * pageSize < totalCount;
    return hasMore ? page + 1 : undefined;
   },
  });
 });
}

async function readersDirectoryQueryFn() {
 const result = await listReaders(1, 12, '', '', '');
 if (result.success && result.data) {
  return result.data;
 }
 return { readers: [], totalCount: 0 };
}

async function chatInboxActiveQueryFn() {
 const result = await listConversations('active', 1, 100);
 if (result.success && result.data) {
  return result.data;
 }
 return {
  conversations: [],
  currentUserId: '',
  totalCount: 0,
 };
}

async function profileMeQueryFn() {
 const result = await getProfileAction();
 return result.success ? { profile: result.data ?? null, error: '' } : { profile: null, error: result.error };
}

async function gachaPoolsQueryFn() {
 return fetchGachaPoolsServer();
}

export async function prefetchCollectionPage(qc: QueryClient): Promise<void> {
 await Promise.all([
  qc.prefetchQuery({ queryKey: userStateQueryKeys.collection.mine(), queryFn: getUserCollection }),
  qc.prefetchQuery({ queryKey: userStateQueryKeys.reading.cardsCatalog(), queryFn: getCardsCatalogAction }),
 ]);
}

export async function prefetchInventoryPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: inventoryQueryKeys.mine(),
   queryFn: fetchInventoryServer,
  });
 });
}

/** Gộp ví + catalog bài (GET /me/reading-setup-snapshot) → hydrate cache catalog cho session sau. */
export async function prefetchReadingSetupPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: userStateQueryKeys.reading.setupSnapshot(),
   queryFn: async () => {
    const result = await getReadingSetupSnapshotAction();
    if (!result.success || !result.data) {
     throw new Error(result.error || 'reading-setup-snapshot');
    }
    const { cardsCatalog } = result.data;
    qc.setQueryData(userStateQueryKeys.reading.cardsCatalog(), actionOk(cardsCatalog));
    return result.data;
   },
   staleTime: 60_000,
  });
 });
}

export async function prefetchReadersDirectoryPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: readersDirectoryQueryKey,
  queryFn: readersDirectoryQueryFn,
 });
}

export async function prefetchWalletOverviewPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.wallet.ledger(1),
  queryFn: async () => {
   const result = await getLedger(1, 10);
   return result.success && result.data ? result.data : null;
  },
 });
}

export async function prefetchChatInboxShell(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.chat.inboxActive(),
  queryFn: chatInboxActiveQueryFn,
  staleTime: 30_000,
 });
}

export async function prefetchProfilePage(qc: QueryClient): Promise<void> {
 await Promise.all([
  qc.prefetchQuery({
   queryKey: userStateQueryKeys.profile.me(),
   queryFn: profileMeQueryFn,
  }),
  qc.prefetchQuery({
   queryKey: userStateQueryKeys.reader.myRequest(),
   queryFn: async () => {
    const result = await getMyReaderRequest();
    return result.success ? result.data ?? null : null;
   },
  }),
 ]);
}

export async function prefetchNotificationsPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: ['notifications', 1, false],
  queryFn: async () => {
   const result = await getNotifications(1, 20, undefined);
   return result.success ? result.data ?? null : null;
  },
 });
}

export async function prefetchGachaPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: gachaQueryKeys.pools(),
  queryFn: gachaPoolsQueryFn,
 });

 await swallowPrefetch(async () => {
  const pools = qc.getQueryData<GachaPool[]>(gachaQueryKeys.pools());
  if (!pools?.length) {
   return;
  }
  const firstCode = pools[0].code;
   await Promise.all([
    swallowPrefetch(async () => {
     await qc.prefetchQuery({
      queryKey: gachaQueryKeys.poolOdds(firstCode),
      queryFn: () => fetchGachaPoolOddsServer(firstCode),
     });
    }),
    swallowPrefetch(async () => {
     await qc.prefetchQuery({
      queryKey: gachaQueryKeys.history(1, 6),
      queryFn: () => fetchGachaHistoryServer(1, 6),
     });
    }),
   ]);
 });
}

export async function prefetchGachaHistoryPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: gachaQueryKeys.history(1, 20),
   queryFn: () => fetchGachaHistoryServer(1, 20),
  });
 });
}

export async function prefetchReaderApplyPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.reader.myRequest(),
  queryFn: async () => {
   const result = await getMyReaderRequest();
   return result.success ? result.data ?? null : null;
  },
 });
}

export async function prefetchDepositPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.wallet.depositPromotions(),
  queryFn: async () => {
   const result = await listPromotions(true);
   return result.success && result.data ? result.data : [];
  },
 });
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

export async function prefetchWithdrawPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.wallet.withdrawalsMine(),
  queryFn: async () => {
   const result = await listMyWithdrawals();
   return result.success && result.data ? result.data : [];
  },
 });
}

export async function prefetchProfileMfaPage(qc: QueryClient): Promise<void> {
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.profile.mfaStatus(),
  queryFn: async () => {
   const result = await getMfaStatus();
   return result.success ? result.data ?? false : false;
  },
 });
}

/**
 * Một round-trip GET /me/navbar-snapshot → hydrate các query key mà navbar đang dùng.
 */
export async function prefetchUserSegmentShell(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['me', 'navbar-snapshot'],
   queryFn: async () => {
    const result = await getNavbarSnapshotAction();
    if (!result.success || !result.data) {
     throw new Error(result.error || 'navbar-snapshot');
    }
    const d = result.data;
    qc.setQueryData(userStateQueryKeys.notifications.unreadCount(), d.unreadNotificationCount);
    qc.setQueryData(userStateQueryKeys.chat.unreadBadge(), d.unreadChatCount);
    qc.setQueryData(checkinQueryKeys.streakStatus, d.streak);
    qc.setQueryData(userStateQueryKeys.notifications.dropdown(), d.dropdownPreview);
    return d;
   },
   staleTime: 60_000,
  });
 });
}

export async function prefetchReaderPublicProfilePage(qc: QueryClient, readerId: string): Promise<void> {
 if (!readerId) return;
 await qc.prefetchQuery({
  queryKey: userStateQueryKeys.reader.profile(readerId),
  queryFn: async () => {
   const result = await getReaderProfile(readerId);
   return result.success ? result.data ?? null : null;
  },
 });
}

export async function prefetchReadingHistoryListPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: historySessionsListQueryKey(1, 'all', ''),
   queryFn: async () => {
   const result = await getHistorySessionsAction(1, HISTORY_PAGE_SIZE, 'all', '');
   if (result.success && result.data) {
    return result.data;
   }
    if (isUnauthorizedError(result.error)) {
     throw new Error(AUTH_ERROR.UNAUTHORIZED);
    }
    throw new Error(result.error || 'error');
   },
  });
 });
}

export async function prefetchReadingHistoryDetailPage(qc: QueryClient, sessionId: string): Promise<void> {
 if (!sessionId) return;
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: historyDetailQueryKey(sessionId),
   queryFn: async () => {
   const result = await getHistoryDetailAction(sessionId);
   if (result.success && result.data) {
    return result.data;
   }
    if (isUnauthorizedError(result.error)) {
     throw new Error(AUTH_ERROR.UNAUTHORIZED);
    }
    throw new Error(result.error || 'error');
   },
  });
 });
}

export async function prefetchGamificationHubPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await Promise.all([
   qc.prefetchQuery({
    queryKey: gamificationKeys.quests('daily'),
    queryFn: () => fetchGamificationQuests('daily'),
   }),
   qc.prefetchQuery({
    queryKey: gamificationKeys.quests('weekly'),
    queryFn: () => fetchGamificationQuests('weekly'),
   }),
   qc.prefetchQuery({
    queryKey: gamificationKeys.achievements(),
    queryFn: () => fetchGamificationAchievements(),
   }),
   qc.prefetchQuery({
    queryKey: gamificationKeys.titles(),
    queryFn: () => fetchGamificationTitles(),
   }),
  ]);
 });
}

export async function prefetchLeaderboardPage(qc: QueryClient): Promise<void> {
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: gamificationKeys.leaderboard(DEFAULT_LEADERBOARD_TRACK),
   queryFn: () => fetchGamificationLeaderboard(DEFAULT_LEADERBOARD_TRACK),
  });
 });
}

/** Chỉ SSR feed công khai; tab private tải khi user chuyển tab trên client. */
export async function prefetchCommunityFeedsPage(qc: QueryClient): Promise<void> {
 await prefetchCommunityFeedInfinite(qc, 'public');
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

export async function prefetchProfileReaderSettingsPage(qc: QueryClient): Promise<void> {
 const profileResult = await getProfileAction();
 if (!profileResult.success || !profileResult.data) {
  return;
 }
 if (profileResult.data.role !== 'tarot_reader') {
  return;
 }
 const userId = profileResult.data.id;
 await swallowPrefetch(async () => {
  await qc.prefetchQuery({
   queryKey: ['reader-profile-settings', userId],
   queryFn: async () => {
    const result = await getReaderProfile(userId);
    return result.success ? result.data ?? null : null;
   },
  });
 });
}
