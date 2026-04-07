import { useState } from "react";
import type { CommunityPost } from "@/features/community/types";
import { PostCardActions } from "@/features/community/components/post-card/PostCardActions";
import { PostCardContent } from "@/features/community/components/post-card/PostCardContent";
import { PostCardHeader } from "@/features/community/components/post-card/PostCardHeader";

interface PostCardProps {
 post: CommunityPost;
 currentVisibilityTab?: string;
 onReportClick: (postId: string) => void;
}

export function PostCard({ post, currentVisibilityTab, onReportClick }: PostCardProps) {
 const [showComments, setShowComments] = useState(false);
 const dateText = new Date(post.createdAt).toLocaleDateString();
 const timeText = new Date(post.createdAt).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });

 return (
  <div className="bg-[#1a1b26] rounded-xl p-5 shadow-lg border border-[#2a2b3d] mb-4">
   <PostCardHeader post={post} dateText={dateText} timeText={timeText} onReportClick={() => onReportClick(post.id)} />
   <PostCardContent content={post.content} />
   <PostCardActions post={post} currentVisibilityTab={currentVisibilityTab} showComments={showComments} onToggleComments={() => setShowComments((prev) => !prev)} />
  </div>
 );
}
