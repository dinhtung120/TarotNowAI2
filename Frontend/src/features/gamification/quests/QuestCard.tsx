"use client";

import { cn } from "@/lib/utils";
import type { QuestWithProgress } from "@/features/gamification/shared/gamification.types";
import { QuestClaimButton } from "@/features/gamification/quests/components/quest-card/QuestClaimButton";
import { QuestRewardBadges } from "@/features/gamification/quests/components/quest-card/QuestRewardBadges";

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
 const cardClassName = isClaimed
  ? cn("border-slate-700/50", "bg-slate-800/40", "opacity-70")
  : isCompleted
   ? cn("border-indigo-500/50", "bg-gradient-to-br", "from-slate-800/80", "to-indigo-900/40", "shadow-lg")
   : cn("border-slate-700/80", "bg-slate-800/60");

 return (
  <div
   onClick={() => onOpen(quest)}
   className={cn(
    "group",
    "relative",
    "cursor-pointer",
    "overflow-hidden",
    "rounded-2xl",
    "border",
    "p-5",
    "transition-all",
    "duration-300",
    "backdrop-blur-xl",
    cardClassName,
   )}
  >
   <QuestRewardBadges rewards={quest.definition.rewards} />
   <div className={cn("flex", "items-start", "gap-4", "pr-16")}>
    <div className={cn("min-w-0")}>
     <h3 className={cn("truncate", "text-lg", "font-semibold", isCompleted && !isClaimed ? "text-indigo-300" : "text-slate-100")}>
      {localize(quest.definition.titleVi, quest.definition.titleEn)}
     </h3>
     <p className={cn("mt-1", "line-clamp-2", "text-sm", "text-slate-400")}>{localize(quest.definition.descriptionVi, quest.definition.descriptionEn)}</p>
    </div>
   </div>
   <div className={cn("mt-5", "flex", "items-center", "justify-between", "gap-4")}>
    <div className={cn("flex-1")}>
     <div className={cn("mb-1.5", "flex", "justify-between", "px-1", "text-xs", "font-medium")}>
      <span className={cn("text-slate-400")}>{isClaimed ? t("Claimed") : t("Progress")}</span>
      <span className={cn(isCompleted && !isClaimed ? "text-indigo-400" : "text-slate-300")}>
       {current} / {target}
      </span>
     </div>
     <div className={cn("h-2", "w-full", "overflow-hidden", "rounded-full", "border", "border-slate-800", "bg-slate-900/50")}>
      <progress
       className={cn(
        "tn-progress",
        "tn-progress-sm",
        isClaimed ? "tn-progress-slate" : "tn-progress-indigo",
       )}
       max={100}
       value={percent}
      />
     </div>
    </div>
    <div className={cn("shrink-0")}>
     <QuestClaimButton
      isClaimed={isClaimed}
      isCompleted={isCompleted}
      isClaimPending={isClaimPending}
      claimedLabel={t("Claimed")}
      claimLabel={t("Claim")}
      pendingLabel={t("Pending")}
      onClaim={() => quest.progress?.periodKey && onClaim(quest.definition.code, quest.progress.periodKey)}
     />
    </div>
   </div>
  </div>
 );
}
