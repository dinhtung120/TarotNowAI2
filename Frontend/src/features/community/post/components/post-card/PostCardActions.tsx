import { MessageCircle } from "lucide-react";
import type { CommunityPost } from "@/features/community/shared/types";
import { CommentSection } from "@/features/community/post/components/CommentSection";
import { ReactionBar } from "@/features/community/post/components/ReactionBar";
import { cn } from "@/lib/utils";

interface PostCardActionsProps {
 post: CommunityPost;
 currentVisibilityTab?: string;
 showComments: boolean;
 onToggleComments: () => void;
}

export function PostCardActions({ post, currentVisibilityTab, showComments, onToggleComments }: PostCardActionsProps) {
 return (
  <>
   <div className={cn("flex", "items-center", "gap-6")}>
    <ReactionBar postId={post.id} reactionsCount={post.reactionsCount} viewerReaction={post.viewerReaction} currentVisibilityTab={currentVisibilityTab} />
    <button
     type="button"
     onClick={onToggleComments}
     className={cn("flex", "items-center", "gap-1.5", "transition-colors", showComments ? "text-fuchsia-400" : "text-gray-400")}
    >
     <MessageCircle className={cn("h-5", "w-5")} />
     <span className={cn("text-sm", "font-medium")}>{post.commentsCount || 0}</span>
    </button>
   </div>
   {showComments ? <CommentSection postId={post.id} /> : null}
  </>
 );
}
