import React, { useCallback, useEffect, useRef } from 'react';
import { useTranslations } from 'next-intl';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { toast } from 'react-hot-toast';
import { useComments, useAddComment } from '../hooks/useComments';
import { useCommunityImageUpload } from '@/features/community/hooks/useCommunityImageUpload';
import { useAuthStore } from '@/store/authStore';
import { cn } from '@/lib/utils';
import { CommentComposer } from './comment-section/CommentComposer';
import { CommentsList } from './comment-section/CommentsList';

interface CommentSectionProps {
  postId: string;
}

const commentSchema = z.object({
  content: z.string().trim().min(1).max(1000),
});

type CommentFormValues = z.infer<typeof commentSchema>;

export const CommentSection: React.FC<CommentSectionProps> = ({ postId }) => {
  const t = useTranslations('Community');
  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading, isError } = useComments(postId);
  const addComment = useAddComment(postId);
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const { control, register, handleSubmit, reset, setValue } = useForm<CommentFormValues>({
    resolver: zodResolver(commentSchema),
    defaultValues: { content: '' },
  });

  const content = useWatch({ control, name: 'content' }) ?? '';
  const contentRef = useRef(content);
  useEffect(() => {
    contentRef.current = content;
  }, [content]);

  const setCommentContent = useCallback((nextValue: string) => {
    contentRef.current = nextValue;
    setValue('content', nextValue, {
      shouldDirty: true,
      shouldValidate: true,
    });
  }, [setValue]);

  const imageUpload = useCommunityImageUpload({
    contextType: 'comment',
    getContent: () => contentRef.current,
    setContent: setCommentContent,
  });

  const submitComment = handleSubmit((values) => {
    addComment.mutate(
      {
        content: values.content,
        contextDraftId: imageUpload.contextDraftId,
      },
      {
        onSuccess: () => {
          reset({ content: '' });
          imageUpload.resetContextDraftId();
        },
      },
    );
  });

  const handleAttachImage = useCallback(async (file: File) => {
    const result = await imageUpload.uploadImage(file);
    if (result.success) {
      toast.success(t('composer.upload_success'));
      return;
    }

    toast.error(result.error || t('composer.upload_failed'));
  }, [imageUpload, t]);

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
        content={content}
        isAuthenticated={isAuthenticated}
        isPending={addComment.isPending}
        isUploadingImage={imageUpload.isUploading}
        attachImageLabel={t('comments.attach_image')}
        uploadingProgressLabel={t('comments.uploading_progress', { percent: imageUpload.uploadProgress })}
        loginRequiredLabel={t('comments.login_required')}
        placeholder={t('comments.placeholder')}
        registerField={register('content')}
        submit={() => void submitComment()}
        onAttachImage={handleAttachImage}
      />
    </div>
  );
};
