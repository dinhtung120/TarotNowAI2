import React from 'react';
import { useTranslations } from 'next-intl';
import { useComments, useAddComment } from '../../feed/useComments';
import { useAuthStore } from '@/features/auth/public';
import { cn } from '@/lib/utils';
import { CommentComposer } from './comment-section/CommentComposer';
import { CommentsList } from './comment-section/CommentsList';
import { useCommentComposerState } from './useCommentComposerState';

interface CommentSectionProps {
  postId: string;
}

export const CommentSection: React.FC<CommentSectionProps> = ({ postId }) => {
  const t = useTranslations('Community');
  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading, isError } = useComments(postId);
  const addComment = useAddComment(postId);
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const composer = useCommentComposerState({
    getLabel: (key, values) => t(key as never, values as never),
    onSubmitComment: ({ content, contextDraftId }) => {
      addComment.mutate(
        { content, contextDraftId },
        { onSuccess: composer.handleSuccess },
      );
    },
    onResetComment: () => undefined,
  });

  return (
    <div className={cn('mt-4 pt-4 border-t tn-border-soft')}>
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
        content={composer.content}
        isAuthenticated={isAuthenticated}
        isPending={addComment.isPending}
        isUploadingImage={composer.imageUpload.isUploading}
        attachImageLabel={t('comments.attach_image')}
        uploadingProgressLabel={t('comments.uploading_progress', { percent: composer.imageUpload.uploadProgress })}
        loginRequiredLabel={t('comments.login_required')}
        placeholder={t('comments.placeholder')}
        registerField={composer.registerField}
        submit={() => void composer.submitComment()}
        onAttachImage={composer.handleAttachImage}
      />
    </div>
  );
};
