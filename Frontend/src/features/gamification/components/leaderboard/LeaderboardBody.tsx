import { TrendingUp } from "lucide-react";
import type { LeaderboardResult } from "@/features/gamification/gamification.types";
import { LeaderboardEntryRow } from "@/features/gamification/components/LeaderboardEntryRow";

interface LeaderboardBodyProps {
 currency: "gold" | "diamond";
 data: LeaderboardResult | undefined;
 isLoading: boolean;
 noDataLabel: string;
 noTitleLabel: string;
}

export function LeaderboardBody({ currency, data, isLoading, noDataLabel, noTitleLabel }: LeaderboardBodyProps) {
 if (isLoading) return <div className="py-20 flex justify-center"><div className={`w-10 h-10 border-4 border-t-transparent rounded-full animate-spin ${currency === "gold" ? "border-emerald-500" : "border-indigo-500"}`} /></div>;
 if (!data || data.entries.length === 0) return <div className="text-center py-20 bg-slate-900/30 rounded-3xl border border-slate-800/50 border-dashed"><TrendingUp className="w-16 h-16 text-slate-700 mx-auto mb-4 opacity-30" /><p className="text-slate-500 font-medium">{noDataLabel}</p></div>;

 return (
  <div className="space-y-3 min-h-[400px]">
   {data.entries.map((entry, index) => <LeaderboardEntryRow key={entry.userId} entry={entry} index={index} currency={currency} noTitleLabel={noTitleLabel} />)}
  </div>
 );
}
