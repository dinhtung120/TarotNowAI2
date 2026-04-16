import type { QueryClient } from '@tanstack/react-query';
import { routing } from '@/i18n/routing';
import { listPromotions } from '@/features/admin/application/actions/promotion';
import { listConversations, getUnreadConversationCount } from '@/features/chat/application/actions';
import { getUserCollection } from '@/features/collection/application/actions';
import { getUnreadNotificationCount } from '@/features/notifications/application/actions/unread-count';
import { getNotifications } from '@/features/notifications/application/actions';
import { getProfileAction } from '@/features/profile/application/actions/get-profile';
import { getMfaStatus } from '@/features/profile/mfa/application/actions/status';
import {
 getHistoryDetailAction,
 getHistorySessionsAction,
} from '@/features/reading/application/actions/history';
import {
 HISTORY_PAGE_SIZE,
 historyDetailQueryKey,
 historySessionsListQueryKey,
} from '@/features/reading/history/domain/historyQueryKeys';
import {
 getMyReaderRequest,
 getReaderProfile,
 listReaders,
} from '@/features/reader/application/actions';
import { getLedger } from '@/features/wallet/application/actions';
import { listMyWithdrawals } from '@/features/wallet/application/actions/withdrawal';
import { getCardsCatalogAction } from '@/features/reading/application/actions/cards-catalog';
import { getReadingSetupSnapshotAction } from '@/shared/application/actions/reading-setup-snapshot';
import {
 gachaQueryKeys,
} from '@/shared/infrastructure/gacha/gachaConstants';
import {
 fetchGachaHistoryServer,
 fetchGachaPoolsServer,
} from '@/shared/infrastructure/gacha/gachaServerActions';
import {
 inventoryQueryKeys,
} from '@/shared/infrastructure/inventory/inventoryConstants';
import { fetchInventoryServer } from '@/shared/infrastructure/inventory/inventoryServerActions';

interface RouteQuerySpec {
 queryKey: readonly unknown[];
 queryFn: () => Promise<unknown>;
 staleTime?: number;
}

const READERS_DEFAULT_QUERY_KEY = ['readers', 1, 12, '', '', ''] as const;

const startsWithSegment = (pathname: string, segment: string): boolean =>
 pathname === segment || pathname.startsWith(`${segment}/`);

const stripQueryHash = (href: string): string => href.split('#')[0]?.split('?')[0] ?? href;

const stripLocalePrefix = (pathname: string): string => {
 const candidate = pathname.split('/')[1];
 if (!candidate) {
  return pathname;
 }

 if (!routing.locales.includes(candidate as (typeof routing.locales)[number])) {
  return pathname;
 }

 const rest = pathname.split('/').slice(2).join('/');
 return `/${rest}`.replace(/\/+$/, '') || '/';
};

/** Chuẩn hoá URL trước khi map route prefetch (remove locale/query/hash). */
export function normalizeNavigationPath(href: string): string {
 const rawPath = stripQueryHash(href);
 if (!rawPath.startsWith('/')) {
  return rawPath;
 }

 const withoutLocale = stripLocalePrefix(rawPath);
 return withoutLocale.replace(/\/+$/, '') || '/';
}

function buildRouteQuerySpecs(pathname: string): RouteQuerySpec[] {
 const specs: RouteQuerySpec[] = [];

 if (startsWithSegment(pathname, '/wallet')) {
  specs.push({
   queryKey: ['wallet', 'ledger', 1],
   queryFn: async () => {
    const result = await getLedger(1, 10);
    return result.success && result.data ? result.data : null;
   },
   staleTime: 30_000,
  });
 }

 if (pathname === '/wallet/deposit') {
  specs.push({
   queryKey: ['wallet', 'deposit-promotions'],
   queryFn: async () => {
    const result = await listPromotions(true);
    return result.success && result.data ? result.data : [];
   },
   staleTime: 60_000,
  });
 }

 if (pathname === '/wallet/withdraw') {
  specs.push({
   queryKey: ['wallet', 'withdrawals', 'mine'],
   queryFn: async () => {
    const result = await listMyWithdrawals();
    return result.success && result.data ? result.data : [];
   },
   staleTime: 30_000,
  });
 }

 if (startsWithSegment(pathname, '/notifications')) {
  specs.push(
   {
    queryKey: ['notifications', 'dropdown'],
    queryFn: async () => {
     const result = await getNotifications(1, 10);
     return result.success ? result.data ?? null : null;
    },
    staleTime: 60_000,
   },
   {
    queryKey: ['notifications', 'unread-count'],
    queryFn: async () => {
     const result = await getUnreadNotificationCount();
     return result.success ? result.data ?? 0 : 0;
    },
    staleTime: 60_000,
   },
  );
 }

 if (startsWithSegment(pathname, '/chat')) {
  specs.push(
   {
    queryKey: ['chat', 'inbox', 'active'],
    queryFn: async () => {
     const result = await listConversations('active', 1, 100);
     if (result.success && result.data) {
      return result.data;
     }
     return { conversations: [], currentUserId: '', totalCount: 0 };
    },
    staleTime: 30_000,
   },
   {
    queryKey: ['chat', 'unread-badge'],
    queryFn: async () => {
     const result = await getUnreadConversationCount();
     return result.success && result.data ? result.data.count : 0;
    },
    staleTime: 30_000,
   },
  );
 }

 if (pathname === '/readers') {
  specs.push({
   queryKey: READERS_DEFAULT_QUERY_KEY,
   queryFn: async () => {
    const result = await listReaders(1, 12, '', '', '');
    if (result.success && result.data) {
     return result.data;
    }
    return { readers: [], totalCount: 0 };
   },
   staleTime: 30_000,
  });
 }

 const readerProfileMatch = pathname.match(/^\/readers\/([^/]+)$/);
 if (readerProfileMatch?.[1]) {
  const readerId = decodeURIComponent(readerProfileMatch[1]);
  specs.push({
   queryKey: ['reader-profile', readerId],
   queryFn: async () => {
    const result = await getReaderProfile(readerId);
    return result.success ? result.data ?? null : null;
   },
   staleTime: 30_000,
  });
 }

 if (pathname === '/profile') {
  specs.push(
   {
    queryKey: ['profile', 'me'],
    queryFn: async () => {
     const result = await getProfileAction();
     return result.success
      ? { profile: result.data ?? null, error: '' }
      : { profile: null, error: result.error };
    },
    staleTime: 30_000,
   },
   {
    queryKey: ['reader', 'my-request'],
    queryFn: async () => {
     const result = await getMyReaderRequest();
     return result.success ? result.data ?? null : null;
    },
    staleTime: 30_000,
   },
  );
 }

 if (pathname === '/profile/mfa') {
  specs.push({
   queryKey: ['profile', 'mfa-status'],
   queryFn: async () => {
    const result = await getMfaStatus();
    return result.success ? result.data ?? false : false;
   },
   staleTime: 30_000,
  });
 }

 if (pathname === '/reading') {
  specs.push({
   queryKey: ['me', 'reading-setup-snapshot'],
   queryFn: async () => {
    const result = await getReadingSetupSnapshotAction();
    if (!result.success || !result.data) {
     throw new Error(result.error || 'reading-setup-snapshot');
    }
    return result.data;
   },
   staleTime: 60_000,
  });
 }

 if (pathname === '/reading/history') {
  specs.push({
   queryKey: historySessionsListQueryKey(1, 'all', ''),
   queryFn: async () => {
    const result = await getHistorySessionsAction(1, HISTORY_PAGE_SIZE, 'all', '');
    if (result.success && result.data) {
     return result.data;
    }
    throw new Error(result.error || 'history');
   },
   staleTime: 30_000,
  });
 }

 const historyDetailMatch = pathname.match(/^\/reading\/history\/([^/]+)$/);
 if (historyDetailMatch?.[1]) {
  const sessionId = decodeURIComponent(historyDetailMatch[1]);
  specs.push({
   queryKey: historyDetailQueryKey(sessionId),
   queryFn: async () => {
    const result = await getHistoryDetailAction(sessionId);
    if (result.success && result.data) {
     return result.data;
    }
    throw new Error(result.error || 'history-detail');
   },
   staleTime: 30_000,
  });
 }

 if (pathname === '/collection') {
  specs.push(
   {
    queryKey: ['collection', 'user'],
    queryFn: async () => getUserCollection(),
    staleTime: 60_000,
   },
   {
    queryKey: ['reading', 'cards-catalog'],
    queryFn: async () => getCardsCatalogAction(),
    staleTime: 60_000,
   },
  );
 }

 if (pathname === '/inventory') {
  specs.push({
   queryKey: inventoryQueryKeys.mine(),
   queryFn: async () => fetchInventoryServer(),
   staleTime: 20_000,
  });
 }

 if (pathname === '/gacha') {
  specs.push({
   queryKey: gachaQueryKeys.pools(),
   queryFn: async () => fetchGachaPoolsServer(),
   staleTime: 20_000,
  });
 }

 if (pathname === '/gacha/history') {
  specs.push({
   queryKey: gachaQueryKeys.history(1, 20),
   queryFn: async () => fetchGachaHistoryServer(1, 20),
   staleTime: 20_000,
  });
 }

 return specs;
}

export async function prefetchRouteQueries(queryClient: QueryClient, href: string): Promise<void> {
 const pathname = normalizeNavigationPath(href);
 if (!pathname.startsWith('/')) {
  return;
 }

 const specs = buildRouteQuerySpecs(pathname);
 if (specs.length === 0) {
  return;
 }

 await Promise.all(
  specs.map(async (spec) => {
   try {
    await queryClient.prefetchQuery({
     queryKey: spec.queryKey,
     queryFn: spec.queryFn,
     staleTime: spec.staleTime,
    });
   } catch {
    // Query prefetch là best-effort để không chặn navigation.
   }
  }),
 );
}
