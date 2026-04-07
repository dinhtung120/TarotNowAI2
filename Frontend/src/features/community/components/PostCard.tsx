import type { CommunityPost } from "@/features/community/types";
import { PostCardActions } from "@/features/community/components/post-card/PostCardActions";
import { PostCardContent } from "@/features/community/components/post-card/PostCardContent";
import { PostCardHeader } from "@/features/community/components/post-card/PostCardHeader";
import { usePostCardState } from "@/features/community/components/post-card/usePostCardState";

interface PostCardProps {
 post: CommunityPost;
 currentVisibilityTab?: string;
 onReportClick: (postId: string) => void;
}

export function PostCard({ post, currentVisibilityTab, onReportClick }: PostCardProps) {
 const vm = usePostCardState({ post });

 return (
  <div className="bg-[#1a1b26] rounded-xl p-5 shadow-lg border border-[#2a2b3d] mb-4">
   <PostCardHeader post={post} dateText={vm.dateText} timeText={vm.timeText} onReportClick={() => onReportClick(post.id)} />
   <PostCardContent content={post.content} />
   <PostCardActions post={post} currentVisibilityTab={currentVisibilityTab} showComments={vm.showComments} onToggleComments={vm.toggleComments} />
  </div>
 );
}
