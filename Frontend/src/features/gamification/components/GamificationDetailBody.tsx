"use client";

import { Clock } from "lucide-react";
import type { QuestWithProgress, UserAchievement } from "@/features/gamification/gamification.types";
import { QuestProgressSection } from "@/features/gamification/components/gamification-detail/QuestProgressSection";
import { QuestRewardsSection } from "@/features/gamification/components/gamification-detail/QuestRewardsSection";
import { UnlockedInfoSection } from "@/features/gamification/components/gamification-detail/UnlockedInfoSection";

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
  <div className="space-y-6 relative z-10">
   <section className="bg-slate-900/40 rounded-2xl p-5 border border-slate-700/50">
    <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-3 flex items-center gap-2"><Clock className="w-3.5 h-3.5" />{isQuest ? completionConditionsLabel : isTitle ? descriptionLabel : howToUnlockLabel}</h3>
    <p className="text-slate-200 text-sm leading-relaxed font-medium">{description}</p>
    {isQuest ? <QuestProgressSection current={current} target={target} isCompleted={isCompleted} progressLabel={progressLabel} /> : null}
   </section>
   {isQuest && questData ? <QuestRewardsSection questData={questData} rewardsLabel={rewardsLabel} rewardTypeLabel={rewardTypeLabel} /> : null}
   {isAchievement && unlockedInfo?.unlockedAt ? <UnlockedInfoSection unlockedAtLabel={unlockedAtLabel} unlockedAt={unlockedInfo.unlockedAt} /> : null}
   {isTitle && titleGrantedAt ? <UnlockedInfoSection unlockedAtLabel={unlockedAtLabel} unlockedAt={titleGrantedAt} /> : null}
  </div>
 );
}
