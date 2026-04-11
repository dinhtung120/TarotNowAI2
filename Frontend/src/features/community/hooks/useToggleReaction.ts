

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { toggleReactionAction } from '../application/actions/communityActions';
import { type CommunityFeedResponse, type CommunityPost, ReactionType } from '../types';

interface UseToggleReactionProps {
  postId: string;
  visibility?: string; 
}

interface FeedPagesState {
  pages: CommunityFeedResponse[];
  pageParams: unknown[];
}

export const useToggleReaction = ({ postId, visibility }: UseToggleReactionProps) => {
  const queryClient = useQueryClient();
  const queryKey = ['community', 'feed', visibility] as const;

  return useMutation({
    mutationFn: async (type: ReactionType) => {
      const res = await toggleReactionAction(postId, { type });
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    
    onMutate: async (reactionType) => {
      await queryClient.cancelQueries({ queryKey });

      const previousFeed = queryClient.getQueryData<FeedPagesState>(queryKey);

      queryClient.setQueryData<FeedPagesState>(queryKey, (old) => {
        if (!old) return old;

        return {
          ...old,
          pages: old.pages.map((page) => ({
            ...page,
            data: page.data.map((post: CommunityPost) => {
              if (post.id !== postId) return post; 

              const newReactionsCount = { ...post.reactionsCount };
              let currentReaction = post.viewerReaction;

              if (currentReaction === reactionType) {
                currentReaction = null;
                newReactionsCount[reactionType] = Math.max(0, (newReactionsCount[reactionType] || 0) - 1);
              } else {
                if (currentReaction) {
                    newReactionsCount[currentReaction] = Math.max(0, (newReactionsCount[currentReaction] || 0) - 1);
                }
                currentReaction = reactionType;
                newReactionsCount[reactionType] = (newReactionsCount[reactionType] || 0) + 1;
              }

              return {
                ...post,
                viewerReaction: currentReaction,
                reactionsCount: newReactionsCount,
              };
            }),
          })),
        };
      });

      return { previousFeed };
    },
    
    onError: (_error, _newReaction, context) => {
      if (context?.previousFeed) {
        queryClient.setQueryData(queryKey, context.previousFeed);
      }
    },
    onSettled: () => {
    },
  });
};
