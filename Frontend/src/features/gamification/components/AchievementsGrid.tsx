"use client";

import { useState } from "react";
import { useTranslations } from "next-intl";
import type { AchievementDefinition, UserAchievement } from "@/features/gamification/application/gamification.types";
import { useAchievements } from "@/features/gamification/application/useGamification";
import { useLocalizedField } from "@/features/gamification/presentation/useLocalizedField";
import GamificationDetailModal from "@/features/gamification/components/GamificationDetailModal";
import { AchievementCard } from "@/features/gamification/components/achievement/AchievementCard";
import { AchievementsHeader } from "@/features/gamification/components/achievement/AchievementsHeader";
import { cn } from "@/lib/utils";

export default function AchievementsGrid() {
 const t = useTranslations("Gamification");
 const { data, isLoading } = useAchievements();
 const { localize } = useLocalizedField();
 const [selectedAchievement, setSelectedAchievement] = useState<{ definition: AchievementDefinition; unlockedInfo?: UserAchievement } | null>(null);
 if (isLoading) return <div className={cn("flex", "items-center", "justify-center", "py-12")}><div className={cn("h-10", "w-10", "animate-spin", "rounded-full", "border-4", "border-amber-500", "border-t-transparent")} /></div>;
 if (!data) return null;

 return (
  <div className={cn("space-y-6")}>
   <AchievementsHeader title={t("Achievements")} subtitle={t("UnlockedAmount", { count: data.unlockedList.length, total: data.definitions.length })} />
   <div className={cn("grid", "grid-cols-1", "gap-4", "sm:grid-cols-2", "lg:grid-cols-3", "xl:grid-cols-4")}>
    {data.definitions.map((definition: AchievementDefinition) => {
     const unlockedInfo = data.unlockedList.find((item: UserAchievement) => item.achievementCode === definition.code);
     if (definition.isHidden && !unlockedInfo) return null;
     return <AchievementCard key={definition.code} definition={definition} unlockedInfo={unlockedInfo} hiddenLabel={t("HiddenAchievement")} localize={localize} onClick={() => setSelectedAchievement({ definition, unlockedInfo })} />;
    })}
   </div>
   <GamificationDetailModal isOpen={Boolean(selectedAchievement)} onClose={() => setSelectedAchievement(null)} type="achievement" achievementData={selectedAchievement || undefined} />
  </div>
 );
}
