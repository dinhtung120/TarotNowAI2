/*
 * ===================================================================
 * FILE: PostCard.tsx
 * ===================================================================
 * MỤC ĐÍCH:
 *   Hiển thị thông tin 1 bài viết đầy đủ.
 * ===================================================================
 */

import React from 'react';
import { CommunityPost } from '../types';
import { ReactionBar } from './ReactionBar';
import { CommentSection } from './CommentSection';
import { MessageCircle } from 'lucide-react';

interface PostCardProps {
  post: CommunityPost;
  currentVisibilityTab?: string;
  onReportClick: (postId: string) => void;
}

export const PostCard: React.FC<PostCardProps> = ({ post, currentVisibilityTab, onReportClick }) => {
  const [showComments, setShowComments] = React.useState(false);
  const dateStr = new Date(post.createdAt).toLocaleDateString();
  const timeStr = new Date(post.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

  return (
    <div className="bg-[#1a1b26] rounded-xl p-5 shadow-lg border border-[#2a2b3d] mb-4">
      {/* Header: Author + Meta */}
      <div className="flex justify-between items-start mb-4">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-full bg-gradient-to-tr from-[#8a2be2] to-[#ff00ff] p-0.5">
            <div className="w-full h-full bg-[#0f0f16] rounded-full overflow-hidden flex items-center justify-center">
              {post.authorAvatarUrl ? (
                // eslint-disable-next-line @next/next/no-img-element
                <img src={post.authorAvatarUrl} alt={post.authorDisplayName} className="w-full h-full object-cover" />
              ) : (
                <span className="text-[#8a2be2] font-semibold text-sm">
                  {post.authorDisplayName.charAt(0).toUpperCase()}
                </span>
              )}
            </div>
          </div>
          <div>
            <h4 className="text-gray-100 font-semibold">{post.authorDisplayName}</h4>
            <div className="text-xs text-gray-500 flex items-center gap-2">
              <span>{dateStr} lúc {timeStr}</span>
              <span>•</span>
              <span className="capitalize">{post.visibility.replace('_', ' ')}</span>
            </div>
          </div>
        </div>

        {/* Menu (chỉ nháp Report / Xoa) */}
        <button 
          onClick={() => onReportClick(post.id)}
          className="text-gray-500 hover:text-red-400 transition-colors"
          title="Báo cáo vi phạm"
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
            <path fillRule="evenodd" d="M3 6a3 3 0 013-3h10a1 1 0 01.8 1.6L14.25 8l2.55 3.4A1 1 0 0116 13H6a1 1 0 00-1 1v3a1 1 0 11-2 0V6z" clipRule="evenodd" />
          </svg>
        </button>
      </div>

      {/* Content */}
      {/* TODO: [TECH-DEBT] Thay thế Regex parsing bằng thư viện react-markdown để xử lý những nội dung Markdown phức tạp hơn */}
      <div className="text-gray-300 text-sm leading-relaxed mb-4 whitespace-pre-wrap">
        {post.content.split(/(?:!\[.*?\]\((.*?)\))/g).map((part, i) => {
          if (part.startsWith('http') || part.startsWith('/')) {
            // Hiển thị ảnh (vì đã extract url ra, markdown ![]() sẽ bẻ chuỗi thành 3 đoạn)
            // eslint-disable-next-line @next/next/no-img-element
            return <img key={i} src={part} alt="Post media" className="rounded-xl mt-3 max-h-96 w-full object-cover border border-[#2a2b3d]" />;
          }
          return <span key={i}>{part}</span>;
        })}
      </div>

      {/* Footer: Reactions & Actions*/}
      <div className="flex items-center gap-6">
        <ReactionBar 
          postId={post.id} 
          reactionsCount={post.reactionsCount} 
          viewerReaction={post.viewerReaction} 
          currentVisibilityTab={currentVisibilityTab} 
        />
        <button 
          onClick={() => setShowComments(!showComments)}
          className={`flex items-center gap-1.5 transition-colors ${showComments ? 'text-[#ff00ff]' : 'text-gray-400 hover:text-gray-200'}`}
        >
          <MessageCircle className="w-5 h-5" />
          <span className="text-sm font-medium">{post.commentsCount || 0}</span>
        </button>
      </div>

      {showComments && <CommentSection postId={post.id} />}
    </div>
  );
};
