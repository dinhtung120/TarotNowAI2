import { useCallback, useEffect, useRef, type ChangeEvent, type RefObject } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { toast } from 'react-hot-toast';
import { useCommunityImageUpload } from '@/features/community/feed/useCommunityImageUpload';
import type { PostVisibility } from '@/features/community/shared/types';

const postComposerFormSchema = z.object({
 content: z.string().trim().min(1).max(4000),
 visibility: z.enum(['public', 'private']),
});

type PostComposerFormValues = z.infer<typeof postComposerFormSchema>;

interface UsePostComposerStateOptions {
 fileInputRef: RefObject<HTMLInputElement | null>;
 getLabel: (key: string, values?: Record<string, string | number>) => string;
 onCreatePost: (payload: {
  content: string;
  visibility: PostVisibility;
  contextDraftId: string;
  resetVisibility: PostComposerFormValues['visibility'];
 }) => void;
}

export function usePostComposerState(options: UsePostComposerStateOptions) {
 const { fileInputRef, getLabel, onCreatePost } = options;
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
   alreadyUploading: getLabel('composer.upload_in_progress'),
   prepareFailed: getLabel('composer.upload_prepare_failed'),
   uploadFailed: getLabel('composer.upload_failed'),
  },
 });

 const handleImageUpload = useCallback(async (event: ChangeEvent<HTMLInputElement>) => {
  const file = event.target.files?.[0];
  if (!file) {
   return;
  }

  const result = await imageUpload.uploadImage(file);
  if (result.success) toast.success(getLabel('composer.upload_success'));
  else toast.error(result.error || getLabel('composer.upload_failed'));

  if (fileInputRef.current) {
   fileInputRef.current.value = '';
  }
 }, [fileInputRef, getLabel, imageUpload]);

 const onSubmit = handleSubmit((values) => {
  onCreatePost({
   content: values.content,
   visibility: values.visibility as PostVisibility,
   contextDraftId: imageUpload.contextDraftId,
   resetVisibility: values.visibility,
  });
 });

 const handleSuccess = (visibility: PostComposerFormValues['visibility']) => {
  reset({ content: '', visibility });
  imageUpload.resetContextDraftId();
 };

 return {
  content,
  imageUpload,
  registerContent: register('content'),
  registerVisibility: register('visibility'),
  handleImageUpload,
  onSubmit,
  handleSuccess,
 };
}
