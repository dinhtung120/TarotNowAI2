import { BriefcaseBusiness, Gem, Star } from "lucide-react";
import type { FeaturedReaderMetaProps } from "@/features/home/presentation/components/featured-readers/types";
import ReaderSocialLinksInline from "@/features/reader/presentation/components/ReaderSocialLinksInline";
import { Badge } from "@/shared/components/ui";
import { cn } from "@/lib/utils";

export default function FeaturedReaderMeta({
  profileCta,
  experienceSuffix,
  reader,
  statusClassName,
}: FeaturedReaderMetaProps) {
  return (
    <div className={cn("pointer-events-none absolute inset-x-0 bottom-0 z-30 space-y-4 p-8")}>
      <div className={cn("flex items-center justify-between")}>
        <div className={cn("h-3 w-3 rounded-full", statusClassName)} />
        <Badge
          variant="default"
          size="sm"
          className={cn("tn-border tn-bg-glass")}
        >
          <Star
            className={cn(
              "h-3 w-3 tn-text-warning",
            )}
            fill="currentColor"
          />
          <span className={cn("tn-text-primary")}>
            {reader.avgRating.toFixed(1)}
          </span>
        </Badge>
      </div>
      <div>
        <h3
          className={cn(
            "truncate text-xl font-black tracking-tighter tn-text-ink uppercase italic",
          )}
        >
          {reader.displayName}
        </h3>
        <div
          className={cn(
            "mt-1 line-clamp-1 tn-text-9 font-black tn-tracking-02 tn-text-accent uppercase",
          )}
        >
          {reader.specialties.join(" • ")}
        </div>
      </div>
      <div
        className={cn(
          "flex items-center justify-between border-t tn-border-soft pt-4",
        )}
      >
        <div className={cn("flex flex-col gap-2")}>
          <div className={cn("flex items-center gap-1.5")}>
            <Gem className={cn("h-3.5 w-3.5 tn-text-warning")} />
            <span className={cn("text-xs font-black tn-text-ink")}>
              {reader.diamondPerQuestion} 💎
            </span>
          </div>
          <div className={cn("flex items-center gap-1.5")}>
            <BriefcaseBusiness className={cn("h-3.5 w-3.5 tn-text-secondary")} />
            <span className={cn("text-[10px] font-black uppercase tn-text-ink")}>
              {reader.yearsOfExperience}+ {experienceSuffix}
            </span>
          </div>
        </div>
        <ReaderSocialLinksInline
          className={cn("pointer-events-auto")}
          facebookUrl={reader.facebookUrl}
          instagramUrl={reader.instagramUrl}
          tikTokUrl={reader.tikTokUrl}
        />
        <div
          className={cn(
            "tn-text-10 font-black tracking-widest tn-text-accent uppercase tn-group-shift-x-2",
          )}
        >
          {profileCta}
        </div>
      </div>
    </div>
  );
}
