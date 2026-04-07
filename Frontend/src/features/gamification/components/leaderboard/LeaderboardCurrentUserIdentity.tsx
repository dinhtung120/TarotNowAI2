import Image from "next/image";
import type { LeaderboardEntry } from "@/features/gamification/gamification.types";
import { cn } from "@/lib/utils";

interface LeaderboardCurrentUserIdentityProps {
  userRank: LeaderboardEntry;
  currency: "gold" | "diamond";
}

export default function LeaderboardCurrentUserIdentity({
  userRank,
  currency,
}: LeaderboardCurrentUserIdentityProps) {
  return (
    <>
      <div
        className={cn(
          "relative h-14 w-14 shrink-0 overflow-hidden rounded-full border-2 bg-slate-800 shadow-xl",
          currency === "gold"
            ? "border-emerald-500/40"
            : "border-indigo-500/40",
        )}
      >
        {userRank.avatar ? (
          <Image
            alt={userRank.displayName}
            className="object-cover"
            fill
            sizes="56px"
            src={userRank.avatar}
          />
        ) : (
          <div className="flex h-full w-full items-center justify-center text-xl font-black text-slate-500">
            {userRank.displayName.slice(0, 1).toUpperCase()}
          </div>
        )}
      </div>
      <div className="min-w-0 flex-1">
        <h4 className="truncate text-lg font-bold text-white">
          {userRank.displayName}
        </h4>
        <p
          className={cn(
            "mb-0.5 truncate text-xs font-black tracking-wider uppercase",
            currency === "gold" ? "text-emerald-400" : "text-indigo-400",
          )}
        >
          {userRank.activeTitle || "TarotNow Traveler"}
        </p>
      </div>
    </>
  );
}
