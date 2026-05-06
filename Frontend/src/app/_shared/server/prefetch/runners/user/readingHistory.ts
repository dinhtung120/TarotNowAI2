import type { QueryClient } from '@tanstack/react-query';
import { AUTH_ERROR, isUnauthorizedError } from '@/shared/models/authErrors';
import {
 getHistoryDetailAction,
 getHistorySessionsAction,
} from '@/features/reading/history/actions';
import {
 HISTORY_PAGE_SIZE,
 historyDetailQueryKey,
 historySessionsListQueryKey,
} from '@/features/reading/history/historyQueryKeys';
import { swallowPrefetch } from '@/app/_shared/server/prefetch/runners/user/shared';

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
