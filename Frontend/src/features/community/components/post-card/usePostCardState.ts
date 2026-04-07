'use client';

import { useCallback, useMemo, useState } from 'react';
import type { CommunityPost } from '@/features/community/types';

interface UsePostCardStateArgs {
  post: CommunityPost;
}

export function usePostCardState({ post }: UsePostCardStateArgs) {
  const [showComments, setShowComments] = useState(false);

  const date = useMemo(() => new Date(post.createdAt), [post.createdAt]);
  const dateText = useMemo(() => date.toLocaleDateString(), [date]);
  const timeText = useMemo(
    () => date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
    [date]
  );

  const toggleComments = useCallback(() => {
    setShowComments((prev) => !prev);
  }, []);

  return {
    dateText,
    timeText,
    showComments,
    toggleComments,
  };
}
