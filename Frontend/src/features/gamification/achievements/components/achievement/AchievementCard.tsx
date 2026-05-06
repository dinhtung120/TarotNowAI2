import { Award, Lock } from "lucide-react";
import type { AchievementDefinition, UserAchievement } from "@/features/gamification/shared/gamification.types";
import { cn } from "@/lib/utils";

interface AchievementCardProps {
 definition: AchievementDefinition;
 unlockedInfo?: UserAchievement;
 hiddenLabel: string;
 localize: (vi: string, en: string) => string;
 onClick: () => void;
}

export function AchievementCard({ definition, unlockedInfo, hiddenLabel, localize, onClick }: AchievementCardProps) {
 const isUnlocked = Boolean(unlockedInfo);

 return (
  <div
   onClick={onClick}
   className={cn(
    "group",
    "relative",
    "cursor-pointer",
    "overflow-hidden",
    "rounded-2xl",
    "border",
    "p-5",
    "text-center",
    "transition-all",
    "duration-500",
    "backdrop-blur-xl",
    "flex",
    "flex-col",
    "items-center",
    isUnlocked
     ? cn("border-amber-500/40", "bg-gradient-to-b", "from-slate-800/80", "to-amber-900/20", "shadow-lg")
     : cn("border-slate-800", "bg-slate-900/60", "grayscale", "opacity-60"),
   )}
  >
   <div className={cn("absolute", "inset-0", "bg-gradient-to-b", "from-white/5", "to-transparent", "opacity-0", "transition-opacity")} />
   <div className={cn("relative", "mb-4")}>
    <div className={cn("flex", "h-16", "w-16", "items-center", "justify-center", "rounded-full", "border-2", isUnlocked ? "border-amber-400/50 bg-amber-400/10 shadow-lg" : "border-slate-700 bg-slate-800")}>
     {isUnlocked ? <Award className={cn("h-8", "w-8", "text-amber-400")} /> : <Lock className={cn("h-6", "w-6", "text-slate-500")} />}
    </div>
   </div>
   <h3 className={cn("mb-1", "text-sm", "font-bold", isUnlocked ? "text-amber-200" : "text-slate-400")}>{localize(definition.titleVi, definition.titleEn)}</h3>
   <p className={cn("mt-1", "line-clamp-3", "text-xs", "leading-relaxed", "text-slate-400")}>
    {isUnlocked ? localize(definition.descriptionVi, definition.descriptionEn) : hiddenLabel}
   </p>
   {isUnlocked && unlockedInfo?.unlockedAt ? (
    <div className={cn("mt-auto", "pt-4", "text-xs", "font-semibold", "uppercase", "tracking-wider", "text-amber-500/70")}>
     {new Date(unlockedInfo.unlockedAt).toLocaleDateString()}
    </div>
   ) : null}
  </div>
 );
}
