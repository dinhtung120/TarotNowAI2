
import Image from "next/image";
import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";
import { isRenderableImageUrl, parseMarkdownSegments } from "@/features/community/application/markdownImageParser";
import { shouldUseUnoptimizedImage } from "@/shared/infrastructure/http/assetUrl";

interface PostCardContentProps {
 content: string;
}

export function PostCardContent({ content }: PostCardContentProps) {
 const t = useTranslations("Community");

 return (
  <div className={cn("mb-4", "whitespace-pre-wrap", "text-sm", "leading-relaxed", "text-gray-300")}>
   {parseMarkdownSegments(content).map((segment, index) => {
   if (segment.kind === "image" && isRenderableImageUrl(segment.url)) {
     return (
      <Image
       key={`${segment.url}-${index}`}
       src={segment.url}
       alt={segment.alt || t("post.post_media_alt")}
       width={1200}
       height={800}
       sizes="100vw"
       unoptimized={shouldUseUnoptimizedImage(segment.url)}
       loading="lazy"
       className={cn("mt-3", "h-auto", "max-h-96", "w-full", "rounded-xl", "border", "border-slate-700", "object-cover")}
      />
     );
    }
    if (segment.kind === "image") {
      return <span key={`${segment.url}-${index}`} />;
    }
    return <span key={`${segment.value}-${index}`}>{segment.value}</span>;
   })}
  </div>
 );
}
