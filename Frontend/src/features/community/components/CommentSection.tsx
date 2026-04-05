import React, { useState } from 'react';
import { useComments, useAddComment } from '../hooks/useComments';
import { Loader2, Send } from 'lucide-react';
import { useAuthStore } from '@/store/authStore';

interface CommentSectionProps {
  postId: string;
}

export const CommentSection: React.FC<CommentSectionProps> = ({ postId }) => {
  const [content, setContent] = useState('');
  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading, isError } = useComments(postId);
  const addComment = useAddComment(postId);
  const isAuthenticated = useAuthStore(state => state.isAuthenticated);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!content.trim()) return;
    
    addComment.mutate(content, {
      onSuccess: () => setContent('')
    });
  };

  return (
    <div className="mt-4 pt-4 border-t border-[#2a2b3d]">
      {/* Danh sách bình luận */}
      <div className="space-y-4 mb-4 max-h-[400px] overflow-y-auto custom-scrollbar pr-2">
        {isLoading && <div className="text-center py-2 text-gray-500"><Loader2 className="w-5 h-5 animate-spin mx-auto" /></div>}
        {isError && <div className="text-center py-2 text-red-400">Lỗi khi tải bình luận</div>}
        
        {data?.pages?.map((page, i) => (
          <React.Fragment key={i}>
            {page?.items?.map((comment) => (
              <div key={comment.id} className="flex gap-3">
                {/* Avatar */}
                <div className="w-8 h-8 shrink-0 rounded-full bg-[#1a1b26] border border-[#2a2b3d] overflow-hidden flex items-center justify-center">
                  {comment.authorAvatarUrl ? (
                    // eslint-disable-next-line @next/next/no-img-element
                    <img src={comment.authorAvatarUrl} alt={comment.authorDisplayName} className="w-full h-full object-cover" />
                  ) : (
                    <span className="text-[#8a2be2] font-semibold text-xs">
                      {comment.authorDisplayName.charAt(0).toUpperCase()}
                    </span>
                  )}
                </div>
                
                {/* Nội dung */}
                <div className="flex-1 bg-[#15161f] rounded-2xl rounded-tl-none p-3 border border-[#2a2b3d]/50">
                  <div className="flex items-baseline justify-between mb-1">
                    <span className="font-semibold text-gray-200 text-sm">{comment.authorDisplayName}</span>
                    <span className="text-xs text-gray-500">
                      {new Date(comment.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                    </span>
                  </div>
                  <p className="text-gray-300 text-sm whitespace-pre-wrap">{comment.content}</p>
                </div>
              </div>
            ))}
          </React.Fragment>
        ))}

        {hasNextPage && (
          <button 
            onClick={() => fetchNextPage()} 
            disabled={isFetchingNextPage}
            className="text-xs font-semibold text-[#8a2be2] hover:text-[#ff00ff] block w-full text-center py-2"
          >
            {isFetchingNextPage ? 'Đang tải...' : 'Xem thêm bình luận'}
          </button>
        )}
        
        {!isLoading && data?.pages[0]?.items.length === 0 && (
          <div className="text-center py-4 text-gray-500 text-sm">Chưa có bình luận nào. Hãy là người đầu tiên!</div>
        )}
      </div>

      {/* Box nhập bình luận */}
      {isAuthenticated ? (
        <form onSubmit={handleSubmit} className="flex gap-2 items-end relative">
          <textarea
            value={content}
            onChange={(e) => setContent(e.target.value)}
            placeholder="Viết bình luận..."
            maxLength={1000}
            className="flex-1 bg-[#0f0f16] border border-[#2a2b3d] rounded-xl px-4 py-3 text-sm text-gray-200 focus:outline-none focus:border-[#8a2be2] resize-none h-[46px] min-h-[46px] custom-scrollbar focus:ring-1 focus:ring-[#8a2be2]/50 transition-all"
            rows={1}
            disabled={addComment.isPending}
            onKeyDown={(e) => {
              if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                handleSubmit(e);
              }
            }}
          />
          {content.length > 800 && (
            <span className={`absolute -top-5 right-14 text-xs ${content.length >= 1000 ? 'text-red-400' : 'text-gray-500'}`}>
              {content.length}/1000
            </span>
          )}
          <button
            type="submit"
            disabled={!content.trim() || addComment.isPending}
            className="w-[46px] h-[46px] shrink-0 rounded-xl bg-gradient-to-tr from-[#8a2be2] to-[#4b0082] flex items-center justify-center text-white hover:opacity-90 transition-opacity disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {addComment.isPending ? <Loader2 className="w-5 h-5 animate-spin" /> : <Send className="w-5 h-5 ml-1" />}
          </button>
        </form>
      ) : (
        <div className="text-center py-3 text-xs text-gray-500 bg-[#0f0f16] rounded-xl border border-[#2a2b3d]/50">
          Vui lòng đăng nhập để bình luận
        </div>
      )}
    </div>
  );
};
