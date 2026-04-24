"use client";

import { useMemo, useState } from "react";
import { BarChart3 } from "lucide-react";
import { useTranslations } from "next-intl";
import { useLeaderboard } from "@/features/gamification/useGamification";
import { LeaderboardCurrentUserCard } from "@/features/gamification/components/LeaderboardCurrentUserCard";
import { LeaderboardBody } from "@/features/gamification/components/leaderboard/LeaderboardBody";
import { LeaderboardCurrencyTabs } from "@/features/gamification/components/leaderboard/LeaderboardCurrencyTabs";
import { LeaderboardPeriodTabs } from "@/features/gamification/components/leaderboard/LeaderboardPeriodTabs";
import { useRuntimePolicies } from "@/shared/application/hooks/useRuntimePolicies";
import { cn } from "@/lib/utils";

type LeaderboardCurrency = "gold" | "diamond";
type LeaderboardPeriod = "daily" | "monthly" | "all";

interface LeaderboardSelection {
 currency: LeaderboardCurrency;
 period: LeaderboardPeriod;
}

function parseLeaderboardTrack(track: string): LeaderboardSelection | null {
 const match = /^spent_(gold|diamond)_(daily|monthly|all)$/.exec(track);
 if (!match) {
  return null;
 }

 return {
  currency: match[1] as LeaderboardCurrency,
  period: match[2] as LeaderboardPeriod,
 };
}

export default function LeaderboardTable() {
 const t = useTranslations("Gamification");
 const runtimePoliciesQuery = useRuntimePolicies();
 const defaultSelection = useMemo<LeaderboardSelection | null>(() => {
  const defaultTrack = runtimePoliciesQuery.data?.gamification.defaultLeaderboardTrack;
  if (!defaultTrack) {
   return null;
  }

  return parseLeaderboardTrack(defaultTrack);
 }, [runtimePoliciesQuery.data?.gamification.defaultLeaderboardTrack]);

 const [selectedCurrency, setSelectedCurrency] = useState<LeaderboardCurrency | null>(null);
 const [selectedPeriod, setSelectedPeriod] = useState<LeaderboardPeriod | null>(null);

 const currency = selectedCurrency ?? defaultSelection?.currency ?? null;
 const period = selectedPeriod ?? defaultSelection?.period ?? null;
 const track = currency && period ? `spent_${currency}_${period}` : undefined;
 const { data, isLoading } = useLeaderboard(track);

 if (runtimePoliciesQuery.isLoading || !currency || !period) {
  return <div className={cn("flex", "items-center", "justify-center", "py-12")}><div className={cn("h-10", "w-10", "animate-spin", "rounded-full", "border-4", "border-indigo-500", "border-t-transparent")} /></div>;
 }

 if (runtimePoliciesQuery.isError || !runtimePoliciesQuery.data || !defaultSelection) {
  return <div className={cn("rounded-2xl", "border", "border-red-500/20", "bg-red-500/10", "p-6", "text-center", "text-red-400", "backdrop-blur-md")}><p>{t("NoLeaderboardData")}</p></div>;
 }

 const glowClassName = currency === "gold" ? "bg-emerald-500" : "bg-indigo-500";
 const cardClassName = currency === "gold" ? "bg-gradient-to-br from-emerald-400 to-teal-600 shadow-emerald-500/30" : "bg-gradient-to-br from-indigo-400 to-blue-600 shadow-indigo-500/30";

 return (
  <div className={cn("relative", "overflow-hidden", "rounded-3xl", "border", "border-slate-700/50", "bg-slate-800/40", "p-4", "shadow-2xl", "backdrop-blur-xl", "sm:p-6")}>
   <div className={cn("pointer-events-none", "absolute", "-right-24", "-top-24", "h-64", "w-64", "opacity-20", "blur-3xl", "transition-colors", "duration-700", glowClassName)} />
   <div className={cn("relative", "z-10", "mb-6", "flex", "flex-col", "items-start", "gap-4", "sm:mb-8", "sm:flex-row", "sm:items-center", "sm:justify-between", "sm:gap-6")}>
    <div className={cn("flex", "items-center", "gap-3")}>
     <div className={cn("rounded-2xl", "p-3", "shadow-lg", "transition-all", "duration-500", cardClassName)}>
      <BarChart3 className={cn("h-6", "w-6", "text-white")} />
     </div>
     <div>
      <h2 className={cn("bg-gradient-to-r", "from-slate-100", "to-slate-400", "bg-clip-text", "text-2xl", "font-bold", "text-transparent")}>{t("Leaderboard")}</h2>
      <p className={cn("text-sm", "text-slate-400")}>{currency === "gold" ? "Đại phú hào tích cực" : "TarotNow VIP Rankings"}</p>
     </div>
    </div>
    <div className={cn("w-full sm:w-auto")}>
     <LeaderboardCurrencyTabs currency={currency} goldLabel={t("Gold")} diamondLabel={t("Diamond")} onChange={setSelectedCurrency} />
    </div>
   </div>
   <LeaderboardPeriodTabs period={period} onChange={setSelectedPeriod} />
   <LeaderboardBody currency={currency} data={data} isLoading={isLoading} noDataLabel={t("NoLeaderboardData")} noTitleLabel={t("NoTitleYet")} />
   {!isLoading && data ? <LeaderboardCurrentUserCard userRank={data.userRank} currency={currency} yourRankLabel={t("YourRank")} notOnLeaderboardLabel={t("NotOnLeaderboard")} /> : null}
  </div>
 );
}
