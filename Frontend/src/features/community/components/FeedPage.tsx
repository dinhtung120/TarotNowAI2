"use client";

import { useState } from "react";
import { useTranslations } from "next-intl";
import { useFeed } from "@/features/community/hooks/useFeed";
import { cn } from "@/lib/utils";
import { FeedPostsSection } from "./FeedPostsSection";
import { FeedVisibilityTabs } from "./FeedVisibilityTabs";
import { PostReportModal } from "@/features/community/components/PostReportModal";
import { PostComposer } from "./PostComposer";

export function FeedPage() {
 const t = useTranslations("Community");
 const [activeVisibility, setActiveVisibility] = useState<"public" | "private">("public");
 const [reportPostId, setReportPostId] = useState<string | null>(null);
 const feedQuery = useFeed(activeVisibility);

 const handleReport = (postId: string) => {
  setReportPostId(postId);
 };

 return (
  <div className={cn("mx-auto", "w-full", "max-w-2xl", "px-4", "py-8")}>
   <h1 className={cn("mb-6", "bg-gradient-to-r", "from-purple-400", "to-pink-500", "bg-clip-text", "font-serif", "text-3xl", "font-bold", "text-transparent")}>
    {t("title")}
   </h1>
   <PostComposer currentVisibilityTab={activeVisibility} />
   <FeedVisibilityTabs activeVisibility={activeVisibility} labels={{ public: t("tabs.public"), private: t("tabs.private") }} onChange={setActiveVisibility} />
   {feedQuery.isLoading ? (
    <div className={cn("py-10", "text-center", "text-gray-500")}>{t("feed.loading")}</div>
   ) : (
    <FeedPostsSection activeVisibility={activeVisibility} data={feedQuery.data} hasNextPage={Boolean(feedQuery.hasNextPage)} isFetchingNextPage={feedQuery.isFetchingNextPage} labels={{ loadingMore: t("feed.loading_more"), loadMore: t("feed.load_more"), end: t("feed.end"), empty: t("feed.empty") }} onReport={handleReport} onLoadMore={() => void feedQuery.fetchNextPage()} />
   )}
   <PostReportModal postId={reportPostId} isOpen={Boolean(reportPostId)} onClose={() => setReportPostId(null)} />
  </div>
 );
}
