"use client";

import { useTranslations } from "next-intl";
import Modal from "@/shared/components/ui/Modal";
import type { AchievementDefinition, QuestWithProgress, TitleDefinition, UserAchievement } from "@/features/gamification/application/gamification.types";
import { cn } from "@/lib/utils";
import { useLocalizedField } from "@/features/gamification/presentation/useLocalizedField";
import { GamificationDetailActions } from "@/features/gamification/components/GamificationDetailActions";
import { GamificationDetailBody } from "@/features/gamification/components/GamificationDetailBody";
import { GamificationDetailHeader } from "@/features/gamification/components/GamificationDetailHeader";
import { getDetailModalView } from "@/features/gamification/components/gamification-detail/getDetailModalView";

interface GamificationDetailModalProps {
 isOpen: boolean;
 onClose: () => void;
 type: "quest" | "achievement" | "title";
 questData?: QuestWithProgress;
 achievementData?: { definition: AchievementDefinition; unlockedInfo?: UserAchievement };
 titleData?: { definition: TitleDefinition; isOwned: boolean; isActive: boolean; grantedAt?: string };
 onClaim?: (questCode: string, periodKey: string) => void;
 onEquipTitle?: (titleCode: string) => void;
 isClaiming?: boolean;
}

export default function GamificationDetailModal({ isOpen, onClose, type, questData, achievementData, titleData, onClaim, onEquipTitle, isClaiming }: GamificationDetailModalProps) {
 const t = useTranslations("Gamification");
 const { localize } = useLocalizedField();
 if (!isOpen) return null;
 const view = getDetailModalView({ type, questData, achievementData, titleData, localize });

 return (
  <Modal isOpen={isOpen} onClose={onClose} size="md">
   <div className={cn("relative", "overflow-hidden", "-m-6", "p-8")}>
    <div className={cn("absolute top-0 left-0 w-full h-32 opacity-20", view.isQuest ? "bg-gradient-to-b from-indigo-500 to-transparent" : "bg-gradient-to-b from-amber-500 to-transparent")} />
    <GamificationDetailHeader isQuest={view.isQuest} isCompleted={view.isCompleted} title={view.title} completedLabel={t("Completed")} pendingLabel={t("Pending")} />
    <GamificationDetailBody isQuest={view.isQuest} isAchievement={view.isAchievement} isTitle={view.isTitle} description={view.description} completionConditionsLabel={t("CompletionConditions")} descriptionLabel={t("SelectATitleToDisplay", { defaultMessage: "Description" })} howToUnlockLabel={t("HowToUnlock")} progressLabel={t("Progress")} rewardsLabel={t("Rewards")} unlockedAtLabel={t("UnlockedAt") || "Mở khoá vào"} current={view.current} target={view.target} isCompleted={view.isCompleted} questData={questData} unlockedInfo={achievementData?.unlockedInfo} titleGrantedAt={titleData?.grantedAt} rewardTypeLabel={(rewardType) => t(rewardType)} />
    <GamificationDetailActions showClaimButton={view.isQuest && view.isCompleted && !view.isClaimed} showEquipButton={Boolean(view.isTitle && titleData?.isOwned && !titleData?.isActive)} onClose={onClose} onClaim={() => questData?.progress && onClaim?.(questData.definition.code, questData.progress.periodKey)} onEquip={() => { if (!titleData) return; onEquipTitle?.(titleData.definition.code); onClose(); }} isClaiming={isClaiming} closeLabel={t("Close")} claimLabel={t("ClaimReward")} equipLabel={t("ActiveTitle", { defaultMessage: "Sử dụng" })} />
   </div>
  </Modal>
 );
}
