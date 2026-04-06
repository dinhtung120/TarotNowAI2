/*
 * ===================================================================
 * FILE: PostComposer.tsx
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thành phần Giao diện cho Hộp Soạn Thảo.
 *   Nơi người dùng gõ text, chọn quyền nhìn thấy và Bấm Đăng.
 * ===================================================================
 */

import React, { useState, useRef } from 'react';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useCreatePost } from '../hooks/useCreatePost';
import { PostVisibility } from '../types';
import { Image as ImageIcon, Loader2 } from 'lucide-react';
import { uploadPostImageAction } from '../application/actions/communityActions';

interface PostComposerProps {
  currentVisibilityTab?: string;
}

const postComposerSchema = z.object({
  content: z.string().trim().min(1).max(5000),
  visibility: z.enum(['public', 'private'])
});

type PostComposerValues = z.infer<typeof postComposerSchema>;

export const PostComposer: React.FC<PostComposerProps> = ({ currentVisibilityTab }) => {
  const t = useTranslations('Community');
  const [isUploading, setIsUploading] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    watch
  } = useForm<PostComposerValues>({
    resolver: zodResolver(postComposerSchema),
    defaultValues: {
      content: '',
      visibility: 'public'
    }
  });

  const content = watch('content') ?? '';
  const createPost = useCreatePost(currentVisibilityTab);

  const handleImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    setIsUploading(true);
    try {
      const formData = new FormData();
      formData.append('file', file);
      
      const res = await uploadPostImageAction(formData);
      if (res.success && res.data) {
        const currentContent = watch('content') ?? '';
        setValue('content', `${currentContent}\n\n![post-image](${res.data.url})\n`, { shouldDirty: true });
      } else {
        alert(res.error || t('composer.upload_failed'));
      }
    } catch {
      alert(t('composer.upload_error'));
    } finally {
      setIsUploading(false);
      // Reset input file
      if (fileInputRef.current) {
        fileInputRef.current.value = '';
      }
    }
  };

  const onSubmit = handleSubmit((values) => {
    createPost.mutate(
      {
        content: values.content,
        visibility: values.visibility as PostVisibility
      },
      {
        onSuccess: () => {
          reset({ content: '', visibility: values.visibility }); // Thành công thì xóa trắng hộp
        },
      }
    );
  });

  return (
    <div className="bg-[#1a1b26] rounded-xl p-4 shadow-lg border border-[#2a2b3d] mb-6">
      <form onSubmit={onSubmit}>
        <textarea
          {...register('content')}
          className="w-full bg-[#0f0f16] text-gray-200 border border-[#2a2b3d] rounded-lg p-3 resize-none focus:outline-none focus:ring-2 focus:ring-[#8a2be2] transition-colors min-h-[100px]"
          placeholder={t('composer.placeholder')}
          disabled={createPost.isPending}
        />
        
        <div className="flex justify-between items-center mt-3">
          <div className="flex items-center gap-4">
            <select
              {...register('visibility')}
              className="bg-[#0f0f16] text-gray-400 text-sm border-none focus:ring-0 rounded-md py-1 px-2 cursor-pointer"
              disabled={createPost.isPending}
            >
              <option value="public">{t('composer.visibility_public')}</option>
              <option value="private">{t('composer.visibility_private')}</option>
            </select>
            
            <button
              type="button"
              disabled={isUploading}
              onClick={() => fileInputRef.current?.click()}
              className="text-[#8a2be2] hover:text-[#ff00ff] transition-colors p-2 rounded-full hover:bg-[#8a2be2]/10"
              title={t('composer.attach_image')}
            >
              {isUploading ? <Loader2 className="w-5 h-5 animate-spin" /> : <ImageIcon className="w-5 h-5" />}
            </button>
            <input 
              type="file" 
              accept="image/*" 
              className="hidden" 
              ref={fileInputRef}
              onChange={handleImageUpload}
            />
          </div>

          <button
            type="submit"
            disabled={createPost.isPending || (!content.trim() && !isUploading)}
            className="bg-gradient-to-r from-[#8a2be2] to-[#4b0082] text-white px-6 py-2 rounded-lg font-medium hover:opacity-90 disabled:opacity-50 disabled:cursor-not-allowed transition-opacity"
          >
            {createPost.isPending ? t('composer.posting') : t('composer.submit')}
          </button>
        </div>
      </form>
    </div>
  );
};
