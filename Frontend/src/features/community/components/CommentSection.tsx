import React from 'react';
import { useTranslations } from 'next-intl';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useComments, useAddComment } from '../hooks/useComments';
import { useAuthStore } from '@/store/authStore';
import { cn } from '@/lib/utils';
import { CommentComposer } from './comment-section/CommentComposer';
import { CommentsList } from './comment-section/CommentsList';
interface CommentSectionProps {
  postId: string;
}

const commentSchema = z.object({
  content: z.string().trim().min(1).max(1000)
});

type CommentFormValues = z.infer<typeof commentSchema>;

export const CommentSection: React.FC<CommentSectionProps> = ({ postId }) => {
  const t = useTranslations('Community');
  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading, isError } = useComments(postId);
  const addComment = useAddComment(postId);
  const isAuthenticated = useAuthStore(state => state.isAuthenticated);
  const {
    control,
    register,
    handleSubmit,
    reset
  } = useForm<CommentFormValues>({
    resolver: zodResolver(commentSchema),
    defaultValues: {
      content: ''
    }
  });
  const content = useWatch({ control, name: 'content' }) ?? '';

  const submitComment = handleSubmit((values) => {
    addComment.mutate(values.content, {
      onSuccess: () => reset({ content: '' })
    });
  });

  return (
    <div className={cn("mt-4 pt-4 border-t border-[#2a2b3d]")}>
      <CommentsList
        commentsPages={data?.pages}
        emptyLabel={t('comments.empty')}
        errorLabel={t('comments.load_error')}
        hasNextPage={Boolean(hasNextPage)}
        isError={isError}
        isFetchingNextPage={isFetchingNextPage}
        isLoading={isLoading}
        loadMoreLabel={t('comments.load_more')}
        loadingMoreLabel={t('comments.loading_more')}
        onLoadMore={() => fetchNextPage()}
      />
      <CommentComposer
        content={content}
        isAuthenticated={isAuthenticated}
        isPending={addComment.isPending}
        loginRequiredLabel={t('comments.login_required')}
        placeholder={t('comments.placeholder')}
        registerField={register('content')}
        submit={() => void submitComment()}
      />
    </div>
  );
};
