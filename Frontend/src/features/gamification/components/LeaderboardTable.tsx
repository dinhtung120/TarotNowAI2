"use client";

import { useState } from "react";
import { BarChart3 } from "lucide-react";
import { useTranslations } from "next-intl";
import { useLeaderboard } from "@/features/gamification/useGamification";
import { LeaderboardCurrentUserCard } from "@/features/gamification/components/LeaderboardCurrentUserCard";
import { LeaderboardBody } from "@/features/gamification/components/leaderboard/LeaderboardBody";
import { LeaderboardCurrencyTabs } from "@/features/gamification/components/leaderboard/LeaderboardCurrencyTabs";
import { LeaderboardPeriodTabs } from "@/features/gamification/components/leaderboard/LeaderboardPeriodTabs";

export default function LeaderboardTable() {
 const t = useTranslations("Gamification");
 const [currency, setCurrency] = useState<"gold" | "diamond">("gold");
 const [period, setPeriod] = useState<"daily" | "monthly" | "all">("daily");
 const track = `spent_${currency}_${period}`;
 const { data, isLoading } = useLeaderboard(track);
 const glowClassName = currency === "gold" ? "bg-emerald-500" : "bg-indigo-500";
 const cardClassName = currency === "gold" ? "bg-gradient-to-br from-emerald-400 to-teal-600 shadow-emerald-500/30" : "bg-gradient-to-br from-indigo-400 to-blue-600 shadow-indigo-500/30";

 return (
  <div className="bg-slate-800/40 border border-slate-700/50 rounded-3xl p-6 backdrop-blur-xl shadow-2xl relative overflow-hidden">
   <div className={`absolute -top-24 -right-24 w-64 h-64 blur-[120px] opacity-20 pointer-events-none transition-colors duration-700 ${glowClassName}`} />
   <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 mb-8 relative z-10"><div className="flex items-center gap-3"><div className={`p-3 rounded-2xl shadow-lg transition-all duration-500 ${cardClassName}`}><BarChart3 className="w-6 h-6 text-white" /></div><div><h2 className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-slate-100 to-slate-400">{t("Leaderboard")}</h2><p className="text-sm text-slate-400">{currency === "gold" ? "Đại phú hào tích cực" : "TarotNow VIP Rankings"}</p></div></div><LeaderboardCurrencyTabs currency={currency} goldLabel={t("Gold")} diamondLabel={t("Diamond")} onChange={setCurrency} /></div>
   <LeaderboardPeriodTabs period={period} onChange={setPeriod} />
   <LeaderboardBody currency={currency} data={data} isLoading={isLoading} noDataLabel={t("NoLeaderboardData")} noTitleLabel={t("NoTitleYet")} />
   {!isLoading && data ? <LeaderboardCurrentUserCard userRank={data.userRank} currency={currency} yourRankLabel={t("YourRank")} notOnLeaderboardLabel={t("NotOnLeaderboard")} /> : null}
  </div>
 );
}
