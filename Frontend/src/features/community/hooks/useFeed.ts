/*
 * ===================================================================
 * FILE: useFeed.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Hook dùng React Query (useInfiniteQuery) để cuộn vô tận (Infinite Scroll)
 *   danh sách các bài Post trên Feed.
 * ===================================================================
 */

import { useInfiniteQuery } from '@tanstack/react-query';
import { getFeedAction } from '../application/actions/communityActions';
import { CommunityPost } from '../types';

interface FeedResponse {
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
    initialPageParam: 1, // Đã fix đúng cú pháp React Query v5 syntax
    getNextPageParam: (lastPage) => {
      const { page, pageSize, totalCount } = lastPage.metadata;
      const hasMore = page * pageSize < totalCount;
      return hasMore ? page + 1 : undefined;
    },
  });
};
