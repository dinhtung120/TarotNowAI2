'use client';

import type { CommunityComment } from '@/features/community/types';
import { cn } from '@/lib/utils';

interface CommentItemProps {
 comment: CommunityComment;
}

export function CommentItem({ comment }: CommentItemProps) {
 return (
  <div className={cn('flex gap-3')}>
   <div className={cn('w-8 h-8 shrink-0 rounded-full bg-[#1a1b26] border border-[#2a2b3d] overflow-hidden flex items-center justify-center')}>
    {comment.authorAvatarUrl ? (
     
     <img src={comment.authorAvatarUrl} alt={comment.authorDisplayName} className={cn('w-full h-full object-cover')} />
    ) : (
     <span className={cn('text-[#8a2be2] font-semibold text-xs')}>{comment.authorDisplayName.charAt(0).toUpperCase()}</span>
    )}
   </div>
   <div className={cn('flex-1 bg-[#15161f] rounded-2xl rounded-tl-none p-3 border border-[#2a2b3d]/50')}>
    <div className={cn('flex items-baseline justify-between mb-1')}>
     <span className={cn('font-semibold text-gray-200 text-sm')}>{comment.authorDisplayName}</span>
     <span className={cn('text-xs text-gray-500')}>{new Date(comment.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</span>
    </div>
    <p className={cn('text-gray-300 text-sm whitespace-pre-wrap')}>{comment.content}</p>
   </div>
  </div>
 );
}
