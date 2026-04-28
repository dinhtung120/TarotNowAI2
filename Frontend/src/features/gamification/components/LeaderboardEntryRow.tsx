"use client";

import LeaderboardAvatar from "@/features/gamification/components/leaderboard/LeaderboardAvatar";
import LeaderboardRankBadge from "@/features/gamification/components/leaderboard/LeaderboardRankBadge";
import LeaderboardScore from "@/features/gamification/components/leaderboard/LeaderboardScore";
import { leaderboardRowClass } from "@/features/gamification/components/leaderboard/leaderboardRowClass";
import type { LeaderboardEntry } from "@/features/gamification/application/gamification.types";
import { cn } from "@/lib/utils";

interface LeaderboardEntryRowProps {
  entry: LeaderboardEntry;
  index: number;
  currency: "gold" | "diamond";
  noTitleLabel: string;
}

export function LeaderboardEntryRow({
  entry,
  index,
  currency,
  noTitleLabel,
}: LeaderboardEntryRowProps) {
  const isTop3 = index < 3;

  return (
    <div
      className={cn(
        "group flex items-center gap-4 rounded-2xl p-4 transition-all duration-300",
        leaderboardRowClass(index),
      )}
    >
      <div className={cn("relative", "flex", "h-8", "w-8", "shrink-0", "items-center", "justify-center")}>
        <LeaderboardRankBadge index={index} />
      </div>
      <LeaderboardAvatar
        avatar={entry.avatar}
        displayName={entry.displayName}
      />
      <div className={cn("min-w-0", "flex-1")}>
        <h4
          className={cn(
            "truncate text-base font-bold",
            isTop3 ? "text-white" : "text-slate-300 group-hover:text-white",
          )}
        >
          {entry.displayName}
        </h4>
        <p className={cn("truncate", "text-xs", "font-bold", "uppercase", "tracking-wider", "text-blue-400/80")}>
          {entry.activeTitle || noTitleLabel}
        </p>
      </div>
      <LeaderboardScore currency={currency} index={index} score={entry.score} />
    </div>
  );
}
