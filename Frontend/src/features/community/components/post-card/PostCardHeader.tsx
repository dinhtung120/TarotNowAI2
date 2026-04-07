
import { useTranslations } from "next-intl";
import type { CommunityPost } from "@/features/community/types";

interface PostCardHeaderProps {
 post: CommunityPost;
 dateText: string;
 timeText: string;
 onReportClick: () => void;
}

export function PostCardHeader({ post, dateText, timeText, onReportClick }: PostCardHeaderProps) {
 const t = useTranslations("Community");

 return (
  <div className="flex justify-between items-start mb-4">
   <div className="flex items-center gap-3">
    <div className="w-10 h-10 rounded-full bg-gradient-to-tr from-[#8a2be2] to-[#ff00ff] p-0.5"><div className="w-full h-full bg-[#0f0f16] rounded-full overflow-hidden flex items-center justify-center">{post.authorAvatarUrl ? <img src={post.authorAvatarUrl} alt={post.authorDisplayName} className="w-full h-full object-cover" /> : <span className="text-[#8a2be2] font-semibold text-sm">{post.authorDisplayName.charAt(0).toUpperCase()}</span>}</div></div>
    <div>
     <h4 className="text-gray-100 font-semibold">{post.authorDisplayName}</h4>
     <div className="text-xs text-gray-500 flex items-center gap-2"><span>{dateText} {t("post.at")} {timeText}</span><span>•</span><span className="capitalize">{post.visibility.replace("_", " ")}</span></div>
    </div>
   </div>
   <button type="button" onClick={onReportClick} className="text-gray-500 hover:text-red-400 transition-colors" title={t("post.report")}><svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor"><path fillRule="evenodd" d="M3 6a3 3 0 013-3h10a1 1 0 01.8 1.6L14.25 8l2.55 3.4A1 1 0 0116 13H6a1 1 0 00-1 1v3a1 1 0 11-2 0V6z" clipRule="evenodd" /></svg></button>
  </div>
 );
}
