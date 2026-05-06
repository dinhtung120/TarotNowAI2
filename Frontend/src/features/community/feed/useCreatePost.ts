

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createPostAction } from '../shared/actions/communityActions';
import { CreatePostPayload } from '../shared/types';

export const useCreatePost = (visibility?: string) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (payload: CreatePostPayload) => {
      const res = await createPostAction(payload);
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['community', 'feed', visibility] });
    },
  });
};
