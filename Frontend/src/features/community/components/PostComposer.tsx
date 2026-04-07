
'use client';

import React from 'react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import { PostComposerActions } from './post-composer/PostComposerActions';
import { usePostComposerState } from './post-composer/usePostComposerState';

interface PostComposerProps {
  currentVisibilityTab?: string;
}

export const PostComposer: React.FC<PostComposerProps> = ({ currentVisibilityTab }) => {
  const t = useTranslations('Community');
  const { content, createPost, fileInputRef, handleImageUpload, isUploading, onSubmit, register } = usePostComposerState(
    currentVisibilityTab,
    t,
  );

  return (
    <div className={cn("bg-[#1a1b26] rounded-xl p-4 shadow-lg border border-[#2a2b3d] mb-6")}>
      <form onSubmit={onSubmit}>
        <textarea
          {...register('content')}
          className={cn("w-full bg-[#0f0f16] text-gray-200 border border-[#2a2b3d] rounded-lg p-3 resize-none focus:outline-none focus:ring-2 focus:ring-[#8a2be2] transition-colors min-h-[100px]")}
          placeholder={t('composer.placeholder')}
          disabled={createPost.isPending}
        />
        <PostComposerActions
          content={content}
          fileInputRef={fileInputRef}
          isPending={createPost.isPending}
          isUploading={isUploading}
          onChangeImage={handleImageUpload}
          t={t}
          visibilityField={register('visibility')}
        />
      </form>
    </div>
  );
};
