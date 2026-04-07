
'use client';

import React, { useRef, useState, type ChangeEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { uploadPostImageAction } from '@/features/community/application/actions/communityActions';
import { useCreatePost } from '@/features/community/hooks/useCreatePost';
import { PostVisibility } from '@/features/community/types';
import { cn } from '@/lib/utils';
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
  const [isUploading, setIsUploading] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const { handleSubmit, register, reset, setValue, watch } = useForm<PostComposerFormValues>({
    resolver: zodResolver(postComposerFormSchema),
    defaultValues: {
      content: '',
      visibility: 'public',
    },
  });

  const content = watch('content') ?? '';

  const handleImageUpload = async (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;
    setIsUploading(true);
    try {
      const formData = new FormData();
      formData.append('file', file);
      const response = await uploadPostImageAction(formData);
      if (response.success && response.data) {
        const currentContent = watch('content') ?? '';
        setValue('content', `${currentContent}\n\n![post-image](${response.data.url})\n`, {
          shouldDirty: true,
          shouldValidate: true,
        });
      } else {
        alert(response.error || t('composer.upload_failed'));
      }
    } catch {
      alert(t('composer.upload_error'));
    } finally {
      setIsUploading(false);
      if (fileInputRef.current) fileInputRef.current.value = '';
    }
  };

  const onSubmit = handleSubmit((values) => {
    createPost.mutate(
      {
        content: values.content,
        visibility: values.visibility as PostVisibility,
      },
      {
        onSuccess: () => reset({ content: '', visibility: values.visibility }),
      },
    );
  });

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
