import React from "react";
import type { InfiniteData } from "@tanstack/react-query";
import type { CommunityFeedResponse } from "@/features/community/shared/types";
import { cn } from "@/lib/utils";
import { PostCard } from "@/features/community/post/components/PostCard";

interface FeedPostsSectionProps {
 activeVisibility: string;
 data: InfiniteData<CommunityFeedResponse> | undefined;
 hasNextPage: boolean;
 isFetchingNextPage: boolean;
 labels: {
  loadingMore: string;
  loadMore: string;
  end: string;
  empty: string;
 };
 onReport: (postId: string) => void;
 onLoadMore: () => void;
}

export function FeedPostsSection({ activeVisibility, data, hasNextPage, isFetchingNextPage, labels, onReport, onLoadMore }: FeedPostsSectionProps) {
 const firstPageItems = data?.pages[0]?.data ?? [];

 return (
  <div className={cn("space-y-4")}>
   {data?.pages.map((page) => {
    const pageKey = `feed-page-${page.metadata.page}-${page.metadata.pageSize}`;
    return <React.Fragment key={pageKey}>{page.data.map((post) => <PostCard key={post.id} post={post} currentVisibilityTab={activeVisibility} onReportClick={onReport} />)}</React.Fragment>;
   })}
   {hasNextPage ? (
    <button
     type="button"
     onClick={onLoadMore}
     disabled={isFetchingNextPage}
     className={cn("mt-4", "w-full", "rounded-xl", "border", "border-slate-700", "px-3", "py-3", "text-sm", "text-violet-400", "transition-colors")}
    >
     {isFetchingNextPage ? labels.loadingMore : labels.loadMore}
    </button>
   ) : null}
   {!hasNextPage && firstPageItems.length > 0 ? <div className={cn("py-6", "text-center", "text-sm", "text-gray-600")}>{labels.end}</div> : null}
   {firstPageItems.length === 0 ? <div className={cn("rounded-xl", "border", "border-dashed", "border-slate-700", "py-10", "text-center", "text-gray-500")}>{labels.empty}</div> : null}
  </div>
 );
}
