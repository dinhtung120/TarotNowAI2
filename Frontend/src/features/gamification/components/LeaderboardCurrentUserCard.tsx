"use client";

import LeaderboardCurrentUserEmpty from "@/features/gamification/components/leaderboard/LeaderboardCurrentUserEmpty";
import LeaderboardCurrentUserRankContent from "@/features/gamification/components/leaderboard/LeaderboardCurrentUserRankContent";
import type { LeaderboardEntry } from "../gamification.types";

interface LeaderboardCurrentUserCardProps {
  userRank: LeaderboardEntry | null;
  currency: "gold" | "diamond";
  yourRankLabel: string;
  notOnLeaderboardLabel: string;
}

export function LeaderboardCurrentUserCard({
  userRank,
  currency,
  yourRankLabel,
  notOnLeaderboardLabel,
}: LeaderboardCurrentUserCardProps) {
  return (
    <div className="mt-8 border-t border-slate-700/50 pt-8">
      <div className="mb-4 flex items-center justify-between px-2">
        <h3 className="text-[11px] font-black tracking-[0.2em] text-slate-500 uppercase">
          {yourRankLabel}
        </h3>
        {userRank && (
          <span className="flex items-center gap-1.5 text-[11px] font-bold text-emerald-500">
            <div className="h-1.5 w-1.5 animate-pulse rounded-full bg-emerald-500" />
            Live status
          </span>
        )}
      </div>

      {userRank ? (
        <LeaderboardCurrentUserRankContent
          currency={currency}
          userRank={userRank}
        />
      ) : (
        <LeaderboardCurrentUserEmpty
          notOnLeaderboardLabel={notOnLeaderboardLabel}
        />
      )}
    </div>
  );
}
