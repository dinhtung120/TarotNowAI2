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
import { useCreatePost } from '../hooks/useCreatePost';
import { PostVisibility } from '../types';
import { Image as ImageIcon, Loader2 } from 'lucide-react';
import { uploadPostImageAction } from '../application/actions/communityActions';

interface PostComposerProps {
  currentVisibilityTab?: string;
}

export const PostComposer: React.FC<PostComposerProps> = ({ currentVisibilityTab }) => {
  const [content, setContent] = useState('');
  const [visibility, setVisibility] = useState<PostVisibility>('public');
  const [isUploading, setIsUploading] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);
  
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
        setContent(prev => prev + `\n\n![post-image](${res.data!.url})\n`);
      } else {
        alert(res.error || 'Upload ảnh thất bại');
      }
    } catch (err) {
      alert('Lỗi khi tải ảnh lên');
    } finally {
      setIsUploading(false);
      // Reset input file
      if (fileInputRef.current) {
        fileInputRef.current.value = '';
      }
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!content.trim()) return;

    createPost.mutate(
      { content, visibility },
      {
        onSuccess: () => {
          setContent(''); // Thành công thì xóa trắng hộp
        },
      }
    );
  };

  return (
    <div className="bg-[#1a1b26] rounded-xl p-4 shadow-lg border border-[#2a2b3d] mb-6">
      <form onSubmit={handleSubmit}>
        <textarea
          className="w-full bg-[#0f0f16] text-gray-200 border border-[#2a2b3d] rounded-lg p-3 resize-none focus:outline-none focus:ring-2 focus:ring-[#8a2be2] transition-colors min-h-[100px]"
          placeholder="Hôm nay vũ trụ gửi thông điệp gì đến bạn?..."
          value={content}
          onChange={(e) => setContent(e.target.value)}
          disabled={createPost.isPending}
        />
        
        <div className="flex justify-between items-center mt-3">
          <div className="flex items-center gap-4">
            <select
              className="bg-[#0f0f16] text-gray-400 text-sm border-none focus:ring-0 rounded-md py-1 px-2 cursor-pointer"
              value={visibility}
              onChange={(e) => setVisibility(e.target.value as PostVisibility)}
              disabled={createPost.isPending}
            >
              <option value="public">🌐 Công khai</option>
              <option value="private">🔒 Riêng tư</option>
            </select>
            
            <button
              type="button"
              disabled={isUploading}
              onClick={() => fileInputRef.current?.click()}
              className="text-[#8a2be2] hover:text-[#ff00ff] transition-colors p-2 rounded-full hover:bg-[#8a2be2]/10"
              title="Đính kèm ảnh"
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
            {createPost.isPending ? 'Đang đăng...' : 'Đăng Bài'}
          </button>
        </div>
      </form>
    </div>
  );
};
