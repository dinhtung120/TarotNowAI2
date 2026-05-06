

import { useInfiniteQuery } from '@tanstack/react-query';
import { getFeedAction } from '../shared/actions/communityActions';
import type { CommunityFeedResponse } from '../shared/types';

export type { CommunityFeedResponse as FeedResponse };

export const useFeed = (visibility?: string) => {
  return useInfiniteQuery<CommunityFeedResponse, Error>({
    queryKey: ['community', 'feed', visibility],
    queryFn: async ({ pageParam = 1 }) => {
      const res = await getFeedAction(pageParam as number, 10, visibility);
      if (!res.success) throw new Error(res.error);
      return res.data as CommunityFeedResponse;
    },
    initialPageParam: 1, 
    getNextPageParam: (lastPage) => {
      const { page, pageSize, totalCount } = lastPage.metadata;
      const hasMore = page * pageSize < totalCount;
      return hasMore ? page + 1 : undefined;
    },
  });
};
