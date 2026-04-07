import { MessageCircle } from "lucide-react";
import type { CommunityPost } from "@/features/community/types";
import { CommentSection } from "@/features/community/components/CommentSection";
import { ReactionBar } from "@/features/community/components/ReactionBar";

interface PostCardActionsProps {
 post: CommunityPost;
 currentVisibilityTab?: string;
 showComments: boolean;
 onToggleComments: () => void;
}

export function PostCardActions({ post, currentVisibilityTab, showComments, onToggleComments }: PostCardActionsProps) {
 return (
  <>
   <div className="flex items-center gap-6">
    <ReactionBar postId={post.id} reactionsCount={post.reactionsCount} viewerReaction={post.viewerReaction} currentVisibilityTab={currentVisibilityTab} />
    <button onClick={onToggleComments} className={`flex items-center gap-1.5 transition-colors ${showComments ? "text-[#ff00ff]" : "text-gray-400 hover:text-gray-200"}`}>
     <MessageCircle className="w-5 h-5" />
     <span className="text-sm font-medium">{post.commentsCount || 0}</span>
    </button>
   </div>
   {showComments ? <CommentSection postId={post.id} /> : null}
  </>
 );
}
