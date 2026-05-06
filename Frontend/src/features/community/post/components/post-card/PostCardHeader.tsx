
import Image from "next/image";
import { useTranslations } from "next-intl";
import type { CommunityPost } from "@/features/community/shared/types";
import { cn } from "@/lib/utils";
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from "@/shared/infrastructure/http/assetUrl";

interface PostCardHeaderProps {
 post: CommunityPost;
 dateText: string;
 timeText: string;
 onReportClick: () => void;
}

export function PostCardHeader({ post, dateText, timeText, onReportClick }: PostCardHeaderProps) {
 const t = useTranslations("Community");
 const avatarSrc = resolveAvatarUrl(post.authorAvatarUrl);
 const unoptimizedAvatar = shouldUseUnoptimizedImage(avatarSrc);

 return (
  <div className={cn("mb-4", "flex", "items-start", "justify-between")}>
   <div className={cn("flex", "items-center", "gap-3")}>
    <div className={cn("h-10", "w-10", "rounded-full", "bg-gradient-to-tr", "from-violet-600", "to-fuchsia-500", "p-0.5")}>
     <div className={cn("relative", "flex", "h-full", "w-full", "items-center", "justify-center", "overflow-hidden", "rounded-full", "bg-zinc-950")}>
      {avatarSrc ? (
       <Image
        src={avatarSrc}
        alt={post.authorDisplayName}
        fill
        sizes="40px"
        unoptimized={unoptimizedAvatar}
        loading="lazy"
        className={cn("object-cover")}
       />
      ) : (
       <span className={cn("text-sm", "font-semibold", "text-violet-500")}>{post.authorDisplayName.charAt(0).toUpperCase()}</span>
      )}
     </div>
    </div>
    <div>
     <h4 className={cn("font-semibold", "text-gray-100")}>{post.authorDisplayName}</h4>
     <div className={cn("flex", "items-center", "gap-2", "text-xs", "text-gray-500")}>
      <span>{dateText} {t("post.at")} {timeText}</span>
      <span>•</span>
      <span className={cn("capitalize")}>{post.visibility.replace("_", " ")}</span>
     </div>
    </div>
   </div>
   <button type="button" onClick={onReportClick} className={cn("text-gray-500", "transition-colors")} title={t("post.report")}>
    <svg xmlns="http://www.w3.org/2000/svg" className={cn("h-5", "w-5")} viewBox="0 0 20 20" fill="currentColor">
     <path fillRule="evenodd" d="M3 6a3 3 0 013-3h10a1 1 0 01.8 1.6L14.25 8l2.55 3.4A1 1 0 0116 13H6a1 1 0 00-1 1v3a1 1 0 11-2 0V6z" clipRule="evenodd" />
    </svg>
   </button>
  </div>
 );
}
