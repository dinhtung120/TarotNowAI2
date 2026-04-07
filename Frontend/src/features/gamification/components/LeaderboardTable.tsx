"use client";

import { useState } from "react";
import { BarChart3 } from "lucide-react";
import { useTranslations } from "next-intl";
import { useLeaderboard } from "@/features/gamification/useGamification";
import { LeaderboardCurrentUserCard } from "@/features/gamification/components/LeaderboardCurrentUserCard";
import { LeaderboardBody } from "@/features/gamification/components/leaderboard/LeaderboardBody";
import { LeaderboardCurrencyTabs } from "@/features/gamification/components/leaderboard/LeaderboardCurrencyTabs";
import { LeaderboardPeriodTabs } from "@/features/gamification/components/leaderboard/LeaderboardPeriodTabs";
import { cn } from "@/lib/utils";

export default function LeaderboardTable() {
 const t = useTranslations("Gamification");
 const [currency, setCurrency] = useState<"gold" | "diamond">("gold");
 const [period, setPeriod] = useState<"daily" | "monthly" | "all">("daily");
 const track = `spent_${currency}_${period}`;
 const { data, isLoading } = useLeaderboard(track);
 const glowClassName = currency === "gold" ? "bg-emerald-500" : "bg-indigo-500";
 const cardClassName = currency === "gold" ? "bg-gradient-to-br from-emerald-400 to-teal-600 shadow-emerald-500/30" : "bg-gradient-to-br from-indigo-400 to-blue-600 shadow-indigo-500/30";

 return (
  <div className={cn("relative", "overflow-hidden", "rounded-3xl", "border", "border-slate-700/50", "bg-slate-800/40", "p-6", "shadow-2xl", "backdrop-blur-xl")}>
   <div className={cn("pointer-events-none", "absolute", "-right-24", "-top-24", "h-64", "w-64", "opacity-20", "blur-3xl", "transition-colors", "duration-700", glowClassName)} />
   <div className={cn("relative", "z-10", "mb-8", "flex", "flex-row", "items-center", "justify-between", "gap-6")}>
    <div className={cn("flex", "items-center", "gap-3")}>
     <div className={cn("rounded-2xl", "p-3", "shadow-lg", "transition-all", "duration-500", cardClassName)}>
      <BarChart3 className={cn("h-6", "w-6", "text-white")} />
     </div>
     <div>
      <h2 className={cn("bg-gradient-to-r", "from-slate-100", "to-slate-400", "bg-clip-text", "text-2xl", "font-bold", "text-transparent")}>{t("Leaderboard")}</h2>
      <p className={cn("text-sm", "text-slate-400")}>{currency === "gold" ? "Đại phú hào tích cực" : "TarotNow VIP Rankings"}</p>
     </div>
    </div>
    <LeaderboardCurrencyTabs currency={currency} goldLabel={t("Gold")} diamondLabel={t("Diamond")} onChange={setCurrency} />
   </div>
   <LeaderboardPeriodTabs period={period} onChange={setPeriod} />
   <LeaderboardBody currency={currency} data={data} isLoading={isLoading} noDataLabel={t("NoLeaderboardData")} noTitleLabel={t("NoTitleYet")} />
   {!isLoading && data ? <LeaderboardCurrentUserCard userRank={data.userRank} currency={currency} yourRankLabel={t("YourRank")} notOnLeaderboardLabel={t("NotOnLeaderboard")} /> : null}
  </div>
 );
}
