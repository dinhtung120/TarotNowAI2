import type { CommunityPost } from "@/features/community/shared/types";
import { cn } from "@/lib/utils";
import { PostCardActions } from "@/features/community/post/components/post-card/PostCardActions";
import { PostCardContent } from "@/features/community/post/components/post-card/PostCardContent";
import { PostCardHeader } from "@/features/community/post/components/post-card/PostCardHeader";
import { usePostCardState } from "@/features/community/post/components/post-card/usePostCardState";

interface PostCardProps {
 post: CommunityPost;
 currentVisibilityTab?: string;
 onReportClick: (postId: string) => void;
}

export function PostCard({ post, currentVisibilityTab, onReportClick }: PostCardProps) {
 const vm = usePostCardState({ post });

 return (
  <div className={cn("tn-community-card", "rounded-xl", "border", "p-5", "shadow-lg", "mb-4")}>
   <PostCardHeader post={post} dateText={vm.dateText} timeText={vm.timeText} onReportClick={() => onReportClick(post.id)} />
   <PostCardContent content={post.content} />
   <PostCardActions post={post} currentVisibilityTab={currentVisibilityTab} showComments={vm.showComments} onToggleComments={vm.toggleComments} />
  </div>
 );
}
