import { Award, Coins, Diamond, Gift } from "lucide-react";
import type { QuestRewardItem } from "@/features/gamification/gamification.types";
import { cn } from "@/lib/utils";

interface QuestRewardBadgesProps {
 rewards: QuestRewardItem[];
}

export function QuestRewardBadges({ rewards }: QuestRewardBadgesProps) {
 return (
  <div className={cn("absolute", "right-4", "top-4", "flex", "gap-2")}>
   {rewards.map((reward, index) => (
    <div key={`${reward.type}-${reward.amount}-${index}`} className={cn("flex", "items-center", "gap-1.5", "rounded-full", "border", "border-slate-700/50", "bg-slate-900/50", "px-3", "py-1", "shadow-inner")}>
     {reward.type.toLowerCase() === "gold" ? <Coins className={cn("h-4", "w-4", "text-yellow-500")} /> : reward.type.toLowerCase() === "diamond" ? <Diamond className={cn("h-4", "w-4", "text-cyan-400")} /> : reward.type.toLowerCase() === "title" ? <Award className={cn("h-4", "w-4", "text-purple-400")} /> : <Gift className={cn("h-4", "w-4", "text-purple-400")} />}
     <span className={cn("text-xs", "font-bold", "text-slate-200")}>+{reward.amount}</span>
    </div>
   ))}
  </div>
 );
}
