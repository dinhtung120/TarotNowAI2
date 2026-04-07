import React from "react";
import type { InfiniteData } from "@tanstack/react-query";
import type { FeedResponse } from "@/features/community/hooks/useFeed";
import { PostCard } from "./PostCard";

interface FeedPostsSectionProps {
 activeVisibility: string;
 data: InfiniteData<FeedResponse> | undefined;
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
  <div className="space-y-4">
   {data?.pages.map((page, pageIndex) => <React.Fragment key={pageIndex}>{page.data.map((post) => <PostCard key={post.id} post={post} currentVisibilityTab={activeVisibility} onReportClick={onReport} />)}</React.Fragment>)}
   {hasNextPage ? <button type="button" onClick={onLoadMore} disabled={isFetchingNextPage} className="w-full py-3 mt-4 text-sm text-[#8a2be2] border border-[#2a2b3d] rounded-xl hover:bg-[#2a2b3d]/50 transition-colors">{isFetchingNextPage ? labels.loadingMore : labels.loadMore}</button> : null}
   {!hasNextPage && firstPageItems.length > 0 ? <div className="text-center text-gray-600 py-6 text-sm">{labels.end}</div> : null}
   {firstPageItems.length === 0 ? <div className="text-center text-gray-500 py-10 border border-dashed border-[#2a2b3d] rounded-xl">{labels.empty}</div> : null}
  </div>
 );
}
