import type { AchievementDefinition, QuestWithProgress, TitleDefinition, UserAchievement } from "@/features/gamification/gamification.types";

interface DetailModalInput {
 type: "quest" | "achievement" | "title";
 questData?: QuestWithProgress;
 achievementData?: {
  definition: AchievementDefinition;
  unlockedInfo?: UserAchievement;
 };
 titleData?: {
  definition: TitleDefinition;
  isOwned: boolean;
  isActive: boolean;
  grantedAt?: string;
 };
 localize: (vi: string, en: string) => string;
}

export function getDetailModalView({ type, questData, achievementData, titleData, localize }: DetailModalInput) {
 const isQuest = type === "quest" && Boolean(questData);
 const isAchievement = type === "achievement" && Boolean(achievementData);
 const isTitle = type === "title" && Boolean(titleData);
 const title = isQuest && questData ? localize(questData.definition.titleVi, questData.definition.titleEn) : isAchievement && achievementData ? localize(achievementData.definition.titleVi, achievementData.definition.titleEn) : isTitle && titleData ? localize(titleData.definition.nameVi, titleData.definition.nameEn) : "";
 const description = isQuest && questData ? localize(questData.definition.descriptionVi, questData.definition.descriptionEn) : isAchievement && achievementData ? localize(achievementData.definition.descriptionVi, achievementData.definition.descriptionEn) : isTitle && titleData ? localize(titleData.definition.descriptionVi, titleData.definition.descriptionEn) : "";
 const current = questData?.progress?.currentProgress || 0;
 const target = isQuest && questData ? questData.definition.target : 1;
 const isCompleted = isQuest ? current >= target : isAchievement ? Boolean(achievementData?.unlockedInfo) : Boolean(titleData?.isOwned);
 const isClaimed = questData?.progress?.isClaimed || false;
 return { isQuest, isAchievement, isTitle, title, description, current, target, isCompleted, isClaimed };
}
