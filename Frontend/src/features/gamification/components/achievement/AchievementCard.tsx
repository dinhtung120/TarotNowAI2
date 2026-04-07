import { Award, Lock } from "lucide-react";
import type { AchievementDefinition, UserAchievement } from "@/features/gamification/gamification.types";

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
  <div onClick={onClick} className={`group relative overflow-hidden rounded-2xl border p-5 flex flex-col items-center text-center transition-all duration-500 cursor-pointer ${isUnlocked ? "bg-gradient-to-b from-slate-800/80 to-amber-900/20 border-amber-500/40 shadow-lg hover:shadow-amber-500/20 hover:-translate-y-1" : "bg-slate-900/60 border-slate-800 opacity-60 grayscale hover:grayscale-0 transition-all"} backdrop-blur-xl`}>
   <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_top,_var(--tw-gradient-stops))] from-white/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity" />
   <div className="relative mb-4"><div className={`w-16 h-16 rounded-full flex items-center justify-center border-2 ${isUnlocked ? "border-amber-400/50 bg-amber-400/10 shadow-[0_0_20px_rgba(251,191,36,0.3)]" : "border-slate-700 bg-slate-800"}`}>{isUnlocked ? <Award className="w-8 h-8 text-amber-400 drop-shadow-[0_0_8px_rgba(251,191,36,0.8)]" /> : <Lock className="w-6 h-6 text-slate-500" />}</div></div>
   <h3 className={`text-sm font-bold mb-1 ${isUnlocked ? "text-amber-200" : "text-slate-400"}`}>{localize(definition.titleVi, definition.titleEn)}</h3>
   <p className="text-xs text-slate-400 line-clamp-3 leading-relaxed mt-1">{isUnlocked ? localize(definition.descriptionVi, definition.descriptionEn) : hiddenLabel}</p>
   {isUnlocked && unlockedInfo?.unlockedAt ? <div className="mt-auto pt-4 text-[10px] uppercase font-semibold tracking-wider text-amber-500/70">{new Date(unlockedInfo.unlockedAt).toLocaleDateString()}</div> : null}
  </div>
 );
}
