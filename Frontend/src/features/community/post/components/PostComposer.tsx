'use client';

import React, { useRef } from 'react';
import { useTranslations } from 'next-intl';
import { useCreatePost } from '@/features/community/feed/useCreatePost';
import { cn } from '@/lib/utils';
import { PostComposerActions } from './post-composer/PostComposerActions';
import { usePostComposerState } from './post-composer/usePostComposerState';

interface PostComposerProps {
  currentVisibilityTab?: string;
}

export const PostComposer: React.FC<PostComposerProps> = ({ currentVisibilityTab }) => {
  const t = useTranslations('Community');
  const createPost = useCreatePost(currentVisibilityTab);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const composer = usePostComposerState({
    fileInputRef,
    getLabel: (key, values) => t(key as never, values as never),
    onCreatePost: ({ content, visibility, contextDraftId, resetVisibility }) => {
      createPost.mutate(
        { content, visibility, contextDraftId },
        { onSuccess: () => composer.handleSuccess(resetVisibility) },
      );
    },
  });

  return (
    <div className={cn('tn-community-card rounded-xl p-4 shadow-lg border mb-6')}>
      <form onSubmit={composer.onSubmit}>
        <textarea
          {...composer.registerContent}
          className={cn('w-full tn-community-input border rounded-lg p-3 resize-none transition-colors tn-minh-100')}
          placeholder={t('composer.placeholder')}
          disabled={createPost.isPending}
        />
        {composer.imageUpload.isUploading ? (
          <p className={cn('mt-2 text-xs text-gray-400')}>{t('composer.uploading_progress', { percent: composer.imageUpload.uploadProgress })}</p>
        ) : null}
        <PostComposerActions
          content={composer.content}
          fileInputRef={fileInputRef}
          isPending={createPost.isPending}
          isUploading={composer.imageUpload.isUploading}
          onChangeImage={composer.handleImageUpload}
          t={t}
          visibilityField={composer.registerVisibility}
        />
      </form>
    </div>
  );
};
