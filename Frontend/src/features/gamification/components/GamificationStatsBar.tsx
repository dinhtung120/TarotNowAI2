"use client";

import { Flame, Medal, Trophy, Zap } from "lucide-react";
import { useTranslations } from "next-intl";
import { useAuthStore } from "@/store/authStore";
import type { TitleDefinition } from "@/features/gamification/gamification.types";
import { useAchievements, useTitles } from "@/features/gamification/useGamification";
import { useLocalizedField } from "@/features/gamification/useLocalizedField";
import { StatsMetricCard } from "@/features/gamification/components/stats/StatsMetricCard";

export default function GamificationStatsBar() {
 const t = useTranslations("Gamification");
 const user = useAuthStore((state) => state.user);
 const { data: titlesData, isLoading: isLoadingTitles } = useTitles();
 const { data: achievementsData, isLoading: isLoadingAchievements } = useAchievements();
 const { localize } = useLocalizedField();
 const activeTitle = titlesData?.definitions?.find((title: TitleDefinition) => title.code === titlesData.activeTitleCode);
 const level = user?.level || 1;
 const exp = user?.exp || 0;
 const titlesOwned = titlesData?.unlockedList?.length || 0;
 const titlesTotal = titlesData?.definitions?.length || 0;
 const achievementsOwned = achievementsData?.unlockedList?.length || 0;
 const achievementsTotal = achievementsData?.definitions?.length || 0;

 return (
  <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8 relative z-10">
   <StatsMetricCard borderClassName="border-purple-500/30 shadow-purple-500/10" iconClassName="bg-purple-500/20 text-purple-400" icon={Zap} label={`${t("Level")} & EXP`} value={`Lv.${level}`} detail={`${exp} EXP`} progressPercent={exp % 100} />
   <StatsMetricCard borderClassName="border-blue-500/30 shadow-blue-500/10" iconClassName="bg-blue-500/20 text-blue-400" icon={Medal} label={t("TotalTitles")} value={isLoadingTitles ? "..." : `${titlesOwned} / ${titlesTotal}`} detail={`${t("ActiveTitle")}: ${activeTitle ? localize(activeTitle.nameVi, activeTitle.nameEn) : t("NoTitleYet")}`} progressPercent={titlesTotal > 0 ? (titlesOwned / titlesTotal) * 100 : 0} />
   <StatsMetricCard borderClassName="border-amber-500/30 shadow-amber-500/10" iconClassName="bg-amber-500/20 text-amber-500" icon={Trophy} label={t("TotalAchievements")} value={isLoadingAchievements ? "..." : `${achievementsOwned} / ${achievementsTotal}`} progressPercent={achievementsTotal > 0 ? (achievementsOwned / achievementsTotal) * 100 : 0} />
   <StatsMetricCard borderClassName="border-teal-500/30 shadow-teal-500/10" iconClassName="bg-teal-500/20 text-teal-400" icon={Flame} label={t("AchievementPoints", { defaultMessage: "Điểm Thành Tựu" })} value={isLoadingAchievements ? "..." : (achievementsOwned * 10).toLocaleString()} detail={t("KeepItUp", { defaultMessage: "Keep it up!" })} />
  </div>
 );
}
