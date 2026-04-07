"use client";

import { useState } from "react";
import { useTranslations } from "next-intl";
import type { QuestWithProgress } from "@/features/gamification/gamification.types";
import { useQuests } from "@/features/gamification/useGamification";
import { useLocalizedField } from "@/features/gamification/useLocalizedField";
import GamificationDetailModal from "@/features/gamification/components/GamificationDetailModal";
import { QuestCard } from "@/features/gamification/components/QuestCard";
import { QuestPanelHeader } from "@/features/gamification/components/QuestPanelHeader";
import { useQuestClaimHandlers } from "@/features/gamification/components/quest-panel/useQuestClaimHandlers";

export default function QuestsPanel() {
 const t = useTranslations("Gamification");
 const [questType, setQuestType] = useState<"daily" | "weekly">("daily");
 const { data: quests, isLoading, isError } = useQuests(questType);
 const { localize } = useLocalizedField();
 const [selectedQuest, setSelectedQuest] = useState<QuestWithProgress | null>(null);
 const { claimMutation, handleClaim } = useQuestClaimHandlers({ t });
 if (isLoading) return <div className="flex justify-center items-center py-12"><div className="w-10 h-10 border-4 border-indigo-500 border-t-transparent rounded-full animate-spin" /></div>;
 if (isError || !quests) return <div className="text-center text-red-400 p-6 bg-red-500/10 rounded-2xl border border-red-500/20 backdrop-blur-md"><p>{t("FailedToLoadQuests")}</p></div>;

 return (
  <div className="space-y-6">
   <QuestPanelHeader questType={questType} setQuestType={setQuestType} t={t} />
   <div className="grid grid-cols-1 md:grid-cols-2 gap-4">{quests.map((quest: QuestWithProgress) => <QuestCard key={quest.definition.code} quest={quest} isClaimPending={claimMutation.isPending} localize={localize} t={t} onOpen={setSelectedQuest} onClaim={handleClaim} />)}</div>
   <GamificationDetailModal isOpen={Boolean(selectedQuest)} onClose={() => setSelectedQuest(null)} type="quest" questData={selectedQuest || undefined} onClaim={handleClaim} isClaiming={claimMutation.isPending} />
  </div>
 );
}
