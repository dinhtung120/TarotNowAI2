
import Image from "next/image";
import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";

interface PostCardContentProps {
 content: string;
}

export function PostCardContent({ content }: PostCardContentProps) {
 const t = useTranslations("Community");

 return (
  <div className={cn("mb-4", "whitespace-pre-wrap", "text-sm", "leading-relaxed", "text-gray-300")}>
   {content.split(/(?:!\[.*?\]\((.*?)\))/g).map((part, index) => {
   if (part.startsWith("http") || part.startsWith("/")) {
     return (
      <Image
       key={`${part}-${index}`}
       src={part}
       alt={t("post.post_media_alt")}
       width={1200}
       height={800}
       sizes="100vw"
       unoptimized
       className={cn("mt-3", "h-auto", "max-h-96", "w-full", "rounded-xl", "border", "border-slate-700", "object-cover")}
      />
     );
    }
    return <span key={`${part}-${index}`}>{part}</span>;
   })}
  </div>
 );
}
