'use client';

import Image from 'next/image';
import type { CommunityComment } from '@/features/community/types';
import { cn } from '@/lib/utils';
import { resolveAvatarUrl } from '@/shared/infrastructure/http/assetUrl';

interface CommentItemProps {
 comment: CommunityComment;
}

export function CommentItem({ comment }: CommentItemProps) {
 const avatarSrc = resolveAvatarUrl(comment.authorAvatarUrl);

 return (
  <div className={cn('flex gap-3')}>
   <div className={cn('relative w-8 h-8 shrink-0 rounded-full bg-slate-900 border border-slate-700 overflow-hidden flex items-center justify-center')}>
    {avatarSrc ? (
     <Image
      src={avatarSrc}
      alt={comment.authorDisplayName}
      fill
      sizes="32px"
      unoptimized
      className={cn('object-cover')}
     />
    ) : (
     <span className={cn('text-violet-500 font-semibold text-xs')}>{comment.authorDisplayName.charAt(0).toUpperCase()}</span>
    )}
   </div>
   <div className={cn('flex-1 bg-slate-900 rounded-2xl rounded-tl-none p-3 border border-slate-700/50')}>
    <div className={cn('flex items-baseline justify-between mb-1')}>
     <span className={cn('font-semibold text-gray-200 text-sm')}>{comment.authorDisplayName}</span>
     <span className={cn('text-xs text-gray-500')}>{new Date(comment.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</span>
    </div>
    <p className={cn('text-gray-300 text-sm whitespace-pre-wrap')}>{comment.content}</p>
   </div>
  </div>
 );
}
