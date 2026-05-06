'use client';

import Image from 'next/image';
import { useTranslations } from 'next-intl';
import type { CommunityComment } from '@/features/community/shared/types';
import { cn } from '@/lib/utils';
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from '@/shared/infrastructure/http/assetUrl';
import { isRenderableImageUrl, parseMarkdownSegments } from '@/features/community/shared/markdownImageParser';

interface CommentItemProps {
  comment: CommunityComment;
}

export function CommentItem({ comment }: CommentItemProps) {
  const t = useTranslations('Community');
  const avatarSrc = resolveAvatarUrl(comment.authorAvatarUrl);
  const unoptimizedAvatar = shouldUseUnoptimizedImage(avatarSrc);

  return (
    <div className={cn('flex gap-3')}>
      <div className={cn('relative w-8 h-8 shrink-0 rounded-full bg-slate-900 border border-slate-700 overflow-hidden flex items-center justify-center')}>
        {avatarSrc ? (
          <Image
            src={avatarSrc}
            alt={comment.authorDisplayName}
            fill
            sizes="32px"
            unoptimized={unoptimizedAvatar}
            loading="lazy"
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
        <div className={cn('space-y-2 text-gray-300 text-sm whitespace-pre-wrap')}>
          {parseMarkdownSegments(comment.content).map((segment, index) => {
            if (segment.kind === 'image' && isRenderableImageUrl(segment.url)) {
              return (
                <Image
                  key={`${segment.url}-${index}`}
                  src={segment.url}
                  alt={segment.alt || t('post.post_media_alt')}
                  width={720}
                  height={400}
                  sizes="100vw"
                  unoptimized={shouldUseUnoptimizedImage(segment.url)}
                  loading="lazy"
                  className={cn('h-auto max-h-72 w-full rounded-lg border border-slate-700 object-cover')}
                />
              );
            }

            if (segment.kind === 'image') {
              return <span key={`${segment.url}-${index}`} />;
            }

            return <span key={`${segment.value}-${index}`}>{segment.value}</span>;
          })}
        </div>
      </div>
    </div>
  );
}
