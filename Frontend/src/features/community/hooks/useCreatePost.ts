/*
 * ===================================================================
 * FILE: useCreatePost.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Hook xử lý tạo bài viết mới.
 *   Khi thành công sẽ tự động Invalidate cache để Feed load lại.
 * ===================================================================
 */

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createPostAction } from '../application/actions/communityActions';
import { CreatePostPayload } from '../types';

export const useCreatePost = (visibility?: string) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (payload: CreatePostPayload) => {
      const res = await createPostAction(payload);
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    onSuccess: () => {
      // Gọi lệnh phá cache, yêu cầu tải lại Feed để bài mới nhất hiện lên đầu
      queryClient.invalidateQueries({ queryKey: ['community', 'feed', visibility] });
    },
  });
};
