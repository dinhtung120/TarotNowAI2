

import { useInfiniteQuery } from '@tanstack/react-query';
import { getFeedAction } from '../application/actions/communityActions';
import { CommunityPost } from '../types';

export interface FeedResponse {
  data: CommunityPost[];
  metadata: {
    totalCount: number;
    page: number;
    pageSize: number;
  };
}

export const useFeed = (visibility?: string) => {
  return useInfiniteQuery<FeedResponse, Error>({
    queryKey: ['community', 'feed', visibility],
    queryFn: async ({ pageParam = 1 }) => {
      const res = await getFeedAction(pageParam as number, 10, visibility);
      if (!res.success) throw new Error(res.error);
      return res.data as FeedResponse;
    },
    initialPageParam: 1, 
    getNextPageParam: (lastPage) => {
      const { page, pageSize, totalCount } = lastPage.metadata;
      const hasMore = page * pageSize < totalCount;
      return hasMore ? page + 1 : undefined;
    },
  });
};
