import { Gem, Star } from "lucide-react";
import type { ReaderProfile } from "@/features/reader/application/actions";
import type { ReaderDirectoryCardLabels } from "@/features/reader/presentation/components/readers-directory/types";
import { cn } from "@/lib/utils";

interface ReaderDirectoryCardStatsProps {
  labels: ReaderDirectoryCardLabels;
  reader: ReaderProfile;
}

export default function ReaderDirectoryCardStats({
  labels,
  reader,
}: ReaderDirectoryCardStatsProps) {
  return (
    <div className={cn("flex items-center justify-between")}>
      <div
        className={cn(
          "tn-border-soft tn-surface flex items-center gap-1.5 rounded-lg border px-2.5 py-1.5",
        )}
      >
        <Star
          className={cn("h-3.5 w-3.5 text-[var(--warning)]")}
          fill="currentColor"
        />
        <span className={cn("tn-text-primary text-xs font-black")}>
          {reader.avgRating > 0 ? reader.avgRating.toFixed(1) : "--"}
        </span>
        <span
          className={cn("text-[10px] font-bold text-[var(--text-tertiary)]")}
        >
          ({reader.totalReviews})
        </span>
      </div>

      <div
        className={cn(
          "flex items-center gap-1.5 rounded-lg border border-[var(--purple-accent)]/20 bg-[var(--purple-accent)]/10 px-2.5 py-1.5",
        )}
      >
        <Gem className={cn("h-3.5 w-3.5 text-[var(--purple-accent)]")} />
        <span
          className={cn(
            "text-[10px] font-black tracking-widest text-[var(--purple-accent)] uppercase",
          )}
        >
          {reader.diamondPerQuestion} {labels.perQuestionSuffix}
        </span>
      </div>
    </div>
  );
}
