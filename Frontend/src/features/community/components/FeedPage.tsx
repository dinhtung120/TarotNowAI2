"use client";

import { useState } from "react";
import { useTranslations } from "next-intl";
import { useFeed } from "@/features/community/hooks/useFeed";
import { FeedPostsSection } from "./FeedPostsSection";
import { FeedVisibilityTabs } from "./FeedVisibilityTabs";
import { PostComposer } from "./PostComposer";

export function FeedPage() {
 const t = useTranslations("Community");
 const [activeVisibility, setActiveVisibility] = useState<"public" | "private">("public");
 const feedQuery = useFeed(activeVisibility);

 const handleReport = (postId: string) => {
  alert(t("feed.report_placeholder", { postId }));
 };

 return (
  <div className="w-full max-w-2xl mx-auto py-8 px-4">
   <h1 className="text-3xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-purple-400 to-pink-500 mb-6 font-serif">{t("title")}</h1>
   <PostComposer currentVisibilityTab={activeVisibility} />
   <FeedVisibilityTabs activeVisibility={activeVisibility} labels={{ public: t("tabs.public"), private: t("tabs.private") }} onChange={setActiveVisibility} />
   {feedQuery.isLoading ? (
    <div className="text-center text-gray-500 py-10">{t("feed.loading")}</div>
   ) : (
    <FeedPostsSection activeVisibility={activeVisibility} data={feedQuery.data} hasNextPage={Boolean(feedQuery.hasNextPage)} isFetchingNextPage={feedQuery.isFetchingNextPage} labels={{ loadingMore: t("feed.loading_more"), loadMore: t("feed.load_more"), end: t("feed.end"), empty: t("feed.empty") }} onReport={handleReport} onLoadMore={() => void feedQuery.fetchNextPage()} />
   )}
  </div>
 );
}
