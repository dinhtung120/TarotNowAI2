'use client';

import { useRef, useState, type ChangeEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { useCreatePost } from '@/features/community/hooks/useCreatePost';
import { uploadPostImageAction } from '@/features/community/application/actions/communityActions';
import { PostVisibility } from '@/features/community/types';
import { postComposerSchema, type PostComposerValues } from './postComposerSchema';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function usePostComposerState(currentVisibilityTab: string | undefined, t: TranslateFn) {
 const [isUploading, setIsUploading] = useState(false);
 const fileInputRef = useRef<HTMLInputElement>(null);
 const { register, handleSubmit, setValue, reset, watch } = useForm<PostComposerValues>({
  resolver: zodResolver(postComposerSchema),
  defaultValues: { content: '', visibility: 'public' },
 });

 const content = watch('content') ?? '';
 const createPost = useCreatePost(currentVisibilityTab);

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
    setValue('content', `${currentContent}\n\n![post-image](${response.data.url})\n`, { shouldDirty: true });
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
   { content: values.content, visibility: values.visibility as PostVisibility },
   { onSuccess: () => reset({ content: '', visibility: values.visibility }) }
  );
 });

 return {
  content,
  createPost,
  fileInputRef,
  handleImageUpload,
  isUploading,
  onSubmit,
  register,
 };
}
