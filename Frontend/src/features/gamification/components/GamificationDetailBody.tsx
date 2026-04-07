"use client";

import { Clock } from "lucide-react";
import type { QuestWithProgress, UserAchievement } from "@/features/gamification/gamification.types";
import { QuestProgressSection } from "@/features/gamification/components/gamification-detail/QuestProgressSection";
import { QuestRewardsSection } from "@/features/gamification/components/gamification-detail/QuestRewardsSection";
import { UnlockedInfoSection } from "@/features/gamification/components/gamification-detail/UnlockedInfoSection";
import { cn } from "@/lib/utils";

interface GamificationDetailBodyProps {
 isQuest: boolean;
 isAchievement: boolean;
 isTitle: boolean;
 description: string;
 completionConditionsLabel: string;
 descriptionLabel: string;
 howToUnlockLabel: string;
 progressLabel: string;
 rewardsLabel: string;
 unlockedAtLabel: string;
 current: number;
 target: number;
 isCompleted: boolean;
 questData?: QuestWithProgress;
 unlockedInfo?: UserAchievement;
 titleGrantedAt?: string;
 rewardTypeLabel: (rewardType: string) => string;
}

export function GamificationDetailBody({ isQuest, isAchievement, isTitle, description, completionConditionsLabel, descriptionLabel, howToUnlockLabel, progressLabel, rewardsLabel, unlockedAtLabel, current, target, isCompleted, questData, unlockedInfo, titleGrantedAt, rewardTypeLabel }: GamificationDetailBodyProps) {
 return (
  <div className={cn("relative", "z-10", "space-y-6")}>
   <section className={cn("rounded-2xl", "border", "border-slate-700/50", "bg-slate-900/40", "p-5")}>
    <h3 className={cn("mb-3", "flex", "items-center", "gap-2", "text-xs", "font-bold", "uppercase", "tracking-widest", "text-slate-400")}>
     <Clock className={cn("h-4", "w-4")} />
     {isQuest ? completionConditionsLabel : isTitle ? descriptionLabel : howToUnlockLabel}
    </h3>
    <p className={cn("text-sm", "font-medium", "leading-relaxed", "text-slate-200")}>{description}</p>
    {isQuest ? <QuestProgressSection current={current} target={target} isCompleted={isCompleted} progressLabel={progressLabel} /> : null}
   </section>
   {isQuest && questData ? <QuestRewardsSection questData={questData} rewardsLabel={rewardsLabel} rewardTypeLabel={rewardTypeLabel} /> : null}
   {isAchievement && unlockedInfo?.unlockedAt ? <UnlockedInfoSection unlockedAtLabel={unlockedAtLabel} unlockedAt={unlockedInfo.unlockedAt} /> : null}
   {isTitle && titleGrantedAt ? <UnlockedInfoSection unlockedAtLabel={unlockedAtLabel} unlockedAt={titleGrantedAt} /> : null}
  </div>
 );
}
