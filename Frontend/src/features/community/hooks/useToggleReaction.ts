/*
 * ===================================================================
 * FILE: useToggleReaction.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Hook thả cảm xúc vào bài viết. 
 *   Áp dụng Optimistic Update: Thấy phản hồi ngay lập tức trên UI trước 
 *   khi Backend báo thành công. Cực kỳ mượt mà.
 * ===================================================================
 */

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { toggleReactionAction } from '../application/actions/communityActions';
import { CommunityPost, ReactionType } from '../types';

interface UseToggleReactionProps {
  postId: string;
  visibility?: string; // Dùng để xác định queryKey của Feed hiện hành
}

export const useToggleReaction = ({ postId, visibility }: UseToggleReactionProps) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (type: ReactionType) => {
      const res = await toggleReactionAction(postId, { type });
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    
    // Cập nhật giao diện tự tin (Optimistic Updates) trước khi mạng kịp chạy xong
    onMutate: async (reactionType) => {
      // Hủy mọi request đang chờ để khỏi bị đè khi sửa tay
      await queryClient.cancelQueries({ queryKey: ['community', 'feed', visibility] });

      // Lưu lại cái cũ đề phòng Backend trả lỗi thì Rollback
      const previousFeed = queryClient.getQueryData(['community', 'feed', visibility]);

      // Thay mận đổi đào trong Cache của React Query
      queryClient.setQueryData(['community', 'feed', visibility], (old: any) => {
        if (!old) return old;

        return {
          ...old,
          pages: old.pages.map((page: any) => ({
            ...page,
            data: page.data.map((post: CommunityPost) => {
              if (post.id !== postId) return post; // Không phải con mình không rớ

              let newReactionsCount = { ...post.reactionsCount };
              let currentReaction = post.viewerReaction;

              // Logic cập nhật state nháp:
              if (currentReaction === reactionType) {
                // Hủy React do ấn trùng
                currentReaction = null;
                newReactionsCount[reactionType] = Math.max(0, (newReactionsCount[reactionType] || 0) - 1);
              } else {
                // Đổi React hoặc Thêm Mới
                if (currentReaction) {
                    // Trừ cái cũ
                    newReactionsCount[currentReaction] = Math.max(0, (newReactionsCount[currentReaction] || 0) - 1);
                }
                // Cộng cái mới
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
    
    // Dù lỗi hay thành công thì cũng cập nhật lại Data Thật
    onError: (err, newReaction, context) => {
      if (context?.previousFeed) {
        queryClient.setQueryData(['community', 'feed', visibility], context.previousFeed);
      }
    },
    onSettled: () => {
      // Trigger update ngầm dù sao đi nữa (Eventual Consistency)
      // queryClient.invalidateQueries({ queryKey: ['community', 'feed', visibility] });
    },
  });
};
