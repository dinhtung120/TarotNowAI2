/* eslint-disable @next/next/no-img-element */
import { useTranslations } from "next-intl";

interface PostCardContentProps {
 content: string;
}

export function PostCardContent({ content }: PostCardContentProps) {
 const t = useTranslations("Community");

 return (
  <div className="text-gray-300 text-sm leading-relaxed mb-4 whitespace-pre-wrap">
   {content.split(/(?:!\[.*?\]\((.*?)\))/g).map((part, index) => {
   if (part.startsWith("http") || part.startsWith("/")) {
     return <img key={`${part}-${index}`} src={part} alt={t("post.post_media_alt")} className="rounded-xl mt-3 max-h-96 w-full object-cover border border-[#2a2b3d]" />;
    }
    return <span key={`${part}-${index}`}>{part}</span>;
   })}
  </div>
 );
}
