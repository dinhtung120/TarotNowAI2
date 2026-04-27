'use client';

import React, { useCallback, useEffect, useRef, type ChangeEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { useCreatePost } from '@/features/community/hooks/useCreatePost';
import { useCommunityImageUpload } from '@/features/community/hooks/useCommunityImageUpload';
import { PostVisibility } from '@/features/community/types';
import { cn } from '@/lib/utils';
import { toast } from 'react-hot-toast';
import { PostComposerActions } from './post-composer/PostComposerActions';

interface PostComposerProps {
  currentVisibilityTab?: string;
}

const postComposerFormSchema = z.object({
  content: z.string().trim().min(1).max(4000),
  visibility: z.enum(['public', 'private']),
});

type PostComposerFormValues = z.infer<typeof postComposerFormSchema>;

export const PostComposer: React.FC<PostComposerProps> = ({ currentVisibilityTab }) => {
  const t = useTranslations('Community');
  const createPost = useCreatePost(currentVisibilityTab);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const { control, handleSubmit, register, reset, setValue } = useForm<PostComposerFormValues>({
    resolver: zodResolver(postComposerFormSchema),
    defaultValues: { content: '', visibility: 'public' },
  });

  const content = useWatch({ control, name: 'content' }) ?? '';
  const contentRef = useRef(content);
  useEffect(() => {
    contentRef.current = content;
  }, [content]);

  const setComposerContent = useCallback((nextValue: string) => {
    contentRef.current = nextValue;
    setValue('content', nextValue, { shouldDirty: true, shouldValidate: true });
  }, [setValue]);

  const imageUpload = useCommunityImageUpload({
    contextType: 'post',
    getContent: () => contentRef.current,
    setContent: setComposerContent,
    messages: {
      alreadyUploading: t('composer.upload_in_progress'),
      prepareFailed: t('composer.upload_prepare_failed'),
      uploadFailed: t('composer.upload_failed'),
    },
  });

  const handleImageUpload = useCallback(async (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) {
      return;
    }

    const result = await imageUpload.uploadImage(file);
    if (result.success) toast.success(t('composer.upload_success'));
    else toast.error(result.error || t('composer.upload_failed'));

    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  }, [imageUpload, t]);

  const onSubmit = handleSubmit((values) => {
    createPost.mutate(
      {
        content: values.content,
        visibility: values.visibility as PostVisibility,
        contextDraftId: imageUpload.contextDraftId,
      },
      {
        onSuccess: () => {
          reset({ content: '', visibility: values.visibility });
          imageUpload.resetContextDraftId();
        },
      },
    );
  });

  return (
    <div className={cn('tn-community-card rounded-xl p-4 shadow-lg border mb-6')}>
      <form onSubmit={onSubmit}>
        <textarea
          {...register('content')}
          className={cn('w-full tn-community-input border rounded-lg p-3 resize-none transition-colors tn-minh-100')}
          placeholder={t('composer.placeholder')}
          disabled={createPost.isPending}
        />
        {imageUpload.isUploading ? (
          <p className={cn('mt-2 text-xs text-gray-400')}>{t('composer.uploading_progress', { percent: imageUpload.uploadProgress })}</p>
        ) : null}
        <PostComposerActions
          content={content}
          fileInputRef={fileInputRef}
          isPending={createPost.isPending}
          isUploading={imageUpload.isUploading}
          onChangeImage={handleImageUpload}
          t={t}
          visibilityField={register('visibility')}
        />
      </form>
    </div>
  );
};
