import { Gem, Star } from "lucide-react";
import type { FeaturedReaderMetaProps } from "@/features/home/presentation/components/featured-readers/types";
import { Badge } from "@/shared/components/ui";
import { cn } from "@/lib/utils";

export default function FeaturedReaderMeta({
  profileCta,
  reader,
  statusClassName,
}: FeaturedReaderMetaProps) {
  return (
    <div className={cn("absolute inset-x-0 bottom-0 z-20 space-y-4 p-8")}>
      <div className={cn("flex items-center justify-between")}>
        <div className={cn("h-3 w-3 rounded-full", statusClassName)} />
        <Badge
          variant="default"
          size="sm"
          className={cn("border-[var(--border-default)] bg-[var(--bg-glass)]")}
        >
          <Star
            className={cn(
              "h-3 w-3 fill-[var(--amber-accent)] text-[var(--amber-accent)]",
            )}
          />
          <span className={cn("tn-text-primary")}>
            {reader.avgRating.toFixed(1)}
          </span>
        </Badge>
      </div>
      <div>
        <h3
          className={cn(
            "truncate text-xl font-black tracking-tighter text-[var(--text-ink)] uppercase italic",
          )}
        >
          {reader.displayName}
        </h3>
        <div
          className={cn(
            "mt-1 line-clamp-1 text-[9px] font-black tracking-[0.2em] text-[var(--purple-accent)] uppercase",
          )}
        >
          {reader.specialties.join(" • ")}
        </div>
      </div>
      <div
        className={cn(
          "flex items-center justify-between border-t border-[var(--border-subtle)] pt-4",
        )}
      >
        <div className={cn("flex items-center gap-1.5")}>
          <Gem className={cn("h-3.5 w-3.5 text-[var(--amber-accent)]")} />
          <span className={cn("text-xs font-black text-[var(--text-ink)]")}>
            {reader.diamondPerQuestion} 💎
          </span>
        </div>
        <div
          className={cn(
            "text-[10px] font-black tracking-widest text-[var(--purple-accent)] uppercase transition-transform group-hover:translate-x-2",
          )}
        >
          {profileCta}
        </div>
      </div>
    </div>
  );
}
