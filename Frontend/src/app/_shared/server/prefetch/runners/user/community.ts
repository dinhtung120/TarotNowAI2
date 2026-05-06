import type { QueryClient } from '@tanstack/react-query';
import { getFeedAction } from '@/features/community/shared/actions/communityActions';
import type { CommunityFeedResponse } from '@/features/community/shared/types';
import { swallowPrefetch } from '@/app/_shared/server/prefetch/runners/user/shared';

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

/** Chỉ SSR feed công khai; tab private tải khi user chuyển tab trên client. */
export async function prefetchCommunityFeedsPage(qc: QueryClient): Promise<void> {
 await prefetchCommunityFeedInfinite(qc, 'public');
}
