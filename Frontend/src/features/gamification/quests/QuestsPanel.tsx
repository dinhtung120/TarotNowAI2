"use client";

import { useState } from "react";
import { useTranslations } from "next-intl";
import type { QuestWithProgress } from "@/features/gamification/shared/gamification.types";
import { useQuests } from "@/features/gamification/shared/useGamification";
import { useLocalizedField } from "@/features/gamification/shared/useLocalizedField";
import GamificationDetailModal from "@/features/gamification/hub/GamificationDetailModal";
import { QuestCard } from "@/features/gamification/quests/QuestCard";
import { QuestPanelHeader } from "@/features/gamification/quests/QuestPanelHeader";
import { useQuestClaimHandlers } from "@/features/gamification/quests/components/quest-panel/useQuestClaimHandlers";
import { RUNTIME_POLICY_FALLBACKS } from "@/shared/config/runtimePolicyFallbacks";
import { cn } from "@/lib/utils";

export default function QuestsPanel() {
 const t = useTranslations("Gamification");
 const defaultQuestType = RUNTIME_POLICY_FALLBACKS.gamification.defaultQuestType;
 const [selectedQuestType, setSelectedQuestType] = useState<"daily" | "weekly" | null>(null);
 const questType = selectedQuestType ?? defaultQuestType;
 const { data: quests, isLoading, isError } = useQuests(questType ?? undefined);
 const { localize } = useLocalizedField();
 const [selectedQuest, setSelectedQuest] = useState<QuestWithProgress | null>(null);
 const { claimMutation, handleClaim } = useQuestClaimHandlers({ t });

 if (isLoading) return <div className={cn("flex", "items-center", "justify-center", "py-12")}><div className={cn("h-10", "w-10", "animate-spin", "rounded-full", "border-4", "border-indigo-500", "border-t-transparent")} /></div>;
 if (isError || !quests) return <div className={cn("rounded-2xl", "border", "border-red-500/20", "bg-red-500/10", "p-6", "text-center", "text-red-400", "backdrop-blur-md")}><p>{t("FailedToLoadQuests")}</p></div>;

 return (
  <div className={cn("space-y-6")}>
   <QuestPanelHeader questType={questType} setQuestType={setSelectedQuestType} t={t} />
   <div className={cn("grid", "grid-cols-1", "gap-4", "md:grid-cols-2")}>{quests.map((quest: QuestWithProgress) => <QuestCard key={quest.definition.code} quest={quest} isClaimPending={claimMutation.isPending} localize={localize} t={t} onOpen={setSelectedQuest} onClaim={handleClaim} />)}</div>
   <GamificationDetailModal isOpen={Boolean(selectedQuest)} onClose={() => setSelectedQuest(null)} type="quest" questData={selectedQuest || undefined} onClaim={handleClaim} isClaiming={claimMutation.isPending} />
  </div>
 );
}
