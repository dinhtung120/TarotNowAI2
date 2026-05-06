'use client';

import { Fragment } from 'react';
import { Loader2 } from 'lucide-react';
import type { CommunityComment } from '@/features/community/shared/types';
import { cn } from '@/lib/utils';
import { CommentItem } from './CommentItem';

interface CommentsListProps {
 commentsPages?: Array<{ items: CommunityComment[] } | undefined>;
 emptyLabel: string;
 errorLabel: string;
 hasNextPage: boolean;
 isError: boolean;
 isFetchingNextPage: boolean;
 isLoading: boolean;
 loadMoreLabel: string;
 loadingMoreLabel: string;
 onLoadMore: () => void;
}

export function CommentsList(props: CommentsListProps) {
 const firstPageItems = props.commentsPages?.[0]?.items ?? [];
 return (
  <div className={cn('space-y-4 mb-4 tn-maxh-400 overflow-y-auto custom-scrollbar pr-2')}>
   {props.isLoading ? <div className={cn('text-center py-2 text-gray-500')}><Loader2 className={cn('w-5 h-5 animate-spin mx-auto')} /></div> : null}
   {props.isError ? <div className={cn('text-center py-2 text-red-400')}>{props.errorLabel}</div> : null}
   {props.commentsPages?.map((page, index) => {
    const firstCommentId = page?.items?.[0]?.id;
    const pageKey = firstCommentId ? `comment-page-${firstCommentId}` : `comment-page-${index}`;
    return <Fragment key={pageKey}>{page?.items?.map((comment) => <CommentItem key={comment.id} comment={comment} />)}</Fragment>;
   })}
   {props.hasNextPage ? <button type="button" onClick={props.onLoadMore} disabled={props.isFetchingNextPage} className={cn('text-xs font-semibold tn-comments-load-more block w-full text-center py-2')}>{props.isFetchingNextPage ? props.loadingMoreLabel : props.loadMoreLabel}</button> : null}
   {!props.isLoading && firstPageItems.length === 0 ? <div className={cn('text-center py-4 text-gray-500 text-sm')}>{props.emptyLabel}</div> : null}
  </div>
 );
}
