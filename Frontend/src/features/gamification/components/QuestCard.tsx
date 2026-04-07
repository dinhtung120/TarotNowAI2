"use client";

import { cn } from "@/lib/utils";
import type { QuestWithProgress } from "@/features/gamification/gamification.types";
import { QuestClaimButton } from "@/features/gamification/components/quest-card/QuestClaimButton";
import { QuestRewardBadges } from "@/features/gamification/components/quest-card/QuestRewardBadges";

interface QuestCardProps {
 quest: QuestWithProgress;
 isClaimPending: boolean;
 localize: (vi: string, en: string) => string;
 t: (key: string) => string;
 onOpen: (quest: QuestWithProgress) => void;
 onClaim: (questCode: string, periodKey: string) => void;
}

export function QuestCard({ quest, isClaimPending, localize, t, onOpen, onClaim }: QuestCardProps) {
 const current = quest.progress?.currentProgress || 0;
 const target = quest.definition.target;
 const isCompleted = current >= target;
 const isClaimed = quest.progress?.isClaimed || false;
 const percent = Math.min((current / target) * 100, 100);
 const cardClassName = isClaimed ? "bg-slate-800/40 border-slate-700/50 opacity-70" : isCompleted ? "bg-gradient-to-br from-slate-800/80 to-indigo-900/40 border-indigo-500/50 shadow-lg shadow-indigo-500/10 hover:shadow-indigo-500/20" : "bg-slate-800/60 border-slate-700/80 hover:bg-slate-800/80";

 return (
  <div onClick={() => onOpen(quest)} className={`relative overflow-hidden p-5 rounded-2xl border transition-all duration-300 cursor-pointer ${cardClassName} backdrop-blur-xl group`}>
   <QuestRewardBadges rewards={quest.definition.rewards} />
   <div className="flex gap-4 items-start pr-16"><div className="min-w-0"><h3 className={`text-lg font-semibold truncate ${isCompleted && !isClaimed ? "text-indigo-300" : "text-slate-100"}`}>{localize(quest.definition.titleVi, quest.definition.titleEn)}</h3><p className="text-sm text-slate-400 mt-1 line-clamp-2">{localize(quest.definition.descriptionVi, quest.definition.descriptionEn)}</p></div></div>
   <div className="mt-5 flex items-center justify-between gap-4">
    <div className="flex-1"><div className="flex justify-between text-xs font-medium mb-1.5 px-1"><span className="text-slate-400">{isClaimed ? t("Claimed") : t("Progress")}</span><span className={isCompleted && !isClaimed ? "text-indigo-400" : "text-slate-300"}>{current} / {target}</span></div><div className="h-2 w-full bg-slate-900/50 rounded-full overflow-hidden border border-slate-800"><div className={cn("h-full rounded-full transition-all duration-1000 ease-out", isClaimed ? "bg-slate-600" : isCompleted ? "bg-gradient-to-r from-indigo-500 to-purple-500 shadow-[0_0_10px_rgba(99,102,241,0.5)]" : "bg-gradient-to-r from-blue-500 to-indigo-500")} style={{ width: `${percent}%` }} /></div></div>
    <div className="shrink-0"><QuestClaimButton isClaimed={isClaimed} isCompleted={isCompleted} isClaimPending={isClaimPending} claimedLabel={t("Claimed")} claimLabel={t("Claim")} pendingLabel={t("Pending")} onClaim={() => quest.progress?.periodKey && onClaim(quest.definition.code, quest.progress.periodKey)} /></div>
   </div>
  </div>
 );
}
