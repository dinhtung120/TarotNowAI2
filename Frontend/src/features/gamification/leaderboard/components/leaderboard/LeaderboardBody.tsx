import { TrendingUp } from "lucide-react";
import type { LeaderboardResult } from "@/features/gamification/shared/gamification.types";
import { LeaderboardEntryRow } from "@/features/gamification/leaderboard/LeaderboardEntryRow";
import { cn } from "@/lib/utils";

interface LeaderboardBodyProps {
 currency: "gold" | "diamond";
 data: LeaderboardResult | undefined;
 isLoading: boolean;
 noDataLabel: string;
 noTitleLabel: string;
}

export function LeaderboardBody({ currency, data, isLoading, noDataLabel, noTitleLabel }: LeaderboardBodyProps) {
 if (isLoading) {
  return (
   <div className={cn("flex", "justify-center", "py-20")}>
    <div className={cn("h-10", "w-10", "animate-spin", "rounded-full", "border-4", "border-t-transparent", currency === "gold" ? "border-emerald-500" : "border-indigo-500")} />
   </div>
  );
 }

 if (!data || data.entries.length === 0) {
  return (
   <div className={cn("rounded-3xl", "border", "border-dashed", "border-slate-800/50", "bg-slate-900/30", "py-20", "text-center")}>
    <TrendingUp className={cn("mx-auto", "mb-4", "h-16", "w-16", "text-slate-700", "opacity-30")} />
    <p className={cn("font-medium", "text-slate-500")}>{noDataLabel}</p>
   </div>
  );
 }

 return (
  <div className={cn("min-h-96", "space-y-3")}>
   {data.entries.map((entry, index) => <LeaderboardEntryRow key={entry.userId} entry={entry} index={index} currency={currency} noTitleLabel={noTitleLabel} />)}
  </div>
 );
}
