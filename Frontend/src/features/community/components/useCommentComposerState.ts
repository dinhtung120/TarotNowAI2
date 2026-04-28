import { useCallback, useEffect, useRef } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { toast } from 'react-hot-toast';
import { useCommunityImageUpload } from '@/features/community/hooks/useCommunityImageUpload';

const commentSchema = z.object({
 content: z.string().trim().min(1).max(1000),
});

type CommentFormValues = z.infer<typeof commentSchema>;

interface UseCommentComposerStateOptions {
 getLabel: (key: string, values?: Record<string, string | number>) => string;
 onSubmitComment: (payload: { content: string; contextDraftId: string }) => void;
 onResetComment: () => void;
}

export function useCommentComposerState(options: UseCommentComposerStateOptions) {
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
  messages: {
   alreadyUploading: options.getLabel('composer.upload_in_progress'),
   prepareFailed: options.getLabel('composer.upload_prepare_failed'),
   uploadFailed: options.getLabel('composer.upload_failed'),
  },
 });

 const submitComment = handleSubmit((values) => {
  options.onSubmitComment({
   content: values.content,
   contextDraftId: imageUpload.contextDraftId,
  });
 });

 const handleAttachImage = useCallback(async (file: File) => {
  const result = await imageUpload.uploadImage(file);
  if (result.success) {
   toast.success(options.getLabel('composer.upload_success'));
   return;
  }

  toast.error(result.error || options.getLabel('composer.upload_failed'));
 }, [imageUpload, options]);

 const handleSuccess = () => {
  reset({ content: '' });
  imageUpload.resetContextDraftId();
  options.onResetComment();
 };

 return {
  content,
  imageUpload,
  registerField: register('content'),
  submitComment,
  handleAttachImage,
  handleSuccess,
 };
}
