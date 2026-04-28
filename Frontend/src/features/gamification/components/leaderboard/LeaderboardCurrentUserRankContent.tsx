import type { LeaderboardEntry } from "@/features/gamification/application/gamification.types";
import LeaderboardCurrentUserIdentity from "@/features/gamification/components/leaderboard/LeaderboardCurrentUserIdentity";
import { cn } from "@/lib/utils";

interface LeaderboardCurrentUserRankContentProps {
  userRank: LeaderboardEntry;
  currency: "gold" | "diamond";
}

export default function LeaderboardCurrentUserRankContent({
  userRank,
  currency,
}: LeaderboardCurrentUserRankContentProps) {
  return (
    <div
      className={cn(
        "flex items-center gap-4 rounded-2xl border-2 p-5 transition-all duration-500",
        currency === "gold"
          ? "border-emerald-500/20 bg-emerald-500/5 shadow-[0_0_20px_rgba(16,185,129,0.05)]"
          : "border-indigo-500/20 bg-indigo-500/5 shadow-[0_0_20px_rgba(99,102,241,0.05)]",
      )}
    >
      <div className={cn("flex", "h-8", "w-8", "shrink-0", "items-center", "justify-center")}>
        <span
          className={cn(
            "text-2xl font-black",
            currency === "gold" ? "text-emerald-400" : "text-indigo-400",
          )}
        >
          #{userRank.rank}
        </span>
      </div>

      <LeaderboardCurrentUserIdentity currency={currency} userRank={userRank} />

      <div className={cn("shrink-0", "text-right")}>
        <div
          className={cn(
            "text-2xl font-black tabular-nums",
            currency === "gold" ? "text-emerald-400" : "text-indigo-400",
          )}
        >
          {userRank.score.toLocaleString('en-US')}
        </div>
        <div className={cn("text-right", "text-xs", "font-black", "uppercase", "tracking-widest", "text-slate-500")}>
          TIÊU THỤ
        </div>
      </div>
    </div>
  );
}
