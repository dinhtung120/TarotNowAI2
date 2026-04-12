
'use client';

import React, { useRef, useState, type ChangeEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { postFormDataToApiV1 } from '@/shared/infrastructure/http/clientMultipartUpload';
import { useCreatePost } from '@/features/community/hooks/useCreatePost';
import { PostVisibility } from '@/features/community/types';
import { cn } from '@/lib/utils';
import { prepareUserImageForUpload, UserImageValidationError } from '@/shared/media';
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
      let toSend: File;
      try {
        toSend = await prepareUserImageForUpload(file, 'community');
      } catch (prepErr) {
        const msg =
          prepErr instanceof UserImageValidationError
            ? prepErr.message
            : t('composer.upload_error');
        toast.error(msg);
        return;
      }

      const formData = new FormData();
      formData.append('file', toSend, toSend.name);
      const response = await postFormDataToApiV1<{ url?: string }>('/community/images', formData, {
        fallbackErrorMessage: t('composer.upload_failed'),
      });
      if (response.ok && response.data.url) {
        const currentContent = watch('content') ?? '';
        setValue('content', `${currentContent}\n\n![post-image](${response.data.url})\n`, {
          shouldDirty: true,
          shouldValidate: true,
        });
        toast.success(t('composer.upload_success'));
      } else {
        toast.error(!response.ok ? response.error : t('composer.upload_failed'));
      }
    } catch {
      toast.error(t('composer.upload_error'));
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
    <div className={cn("tn-community-card rounded-xl p-4 shadow-lg border mb-6")}>
      <form onSubmit={onSubmit}>
        <textarea
          {...register('content')}
          className={cn("w-full tn-community-input border rounded-lg p-3 resize-none transition-colors tn-minh-100")}
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
