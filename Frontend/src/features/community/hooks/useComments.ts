import { useInfiniteQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getCommentsAction, addCommentAction } from '../application/actions/communityActions';
import { CommunityComment } from '../types';

export function useComments(postId: string) {
  const query = useInfiniteQuery({
    queryKey: ['community-comments', postId],
    queryFn: async ({ pageParam = 1 }) => {
      const res = await getCommentsAction(postId, pageParam, 10);
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage, allPages) => {
      return lastPage?.hasNextPage ? allPages.length + 1 : undefined;
    },
  });

  return query;
}

export function useAddComment(postId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (content: string) => {
      const res = await addCommentAction(postId, content);
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['community-comments', postId] });
      queryClient.invalidateQueries({ queryKey: ['community', 'feed'] });
    },
  });
}
