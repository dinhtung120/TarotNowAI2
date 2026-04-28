import { Award, Coins, Diamond, Gift } from "lucide-react";
import type { QuestWithProgress } from "@/features/gamification/application/gamification.types";
import { cn } from "@/lib/utils";

interface QuestRewardsSectionProps {
 questData: QuestWithProgress;
 rewardsLabel: string;
 rewardTypeLabel: (rewardType: string) => string;
}

export function QuestRewardsSection({ questData, rewardsLabel, rewardTypeLabel }: QuestRewardsSectionProps) {
 if (questData.definition.rewards.length === 0) return null;

 return (
  <section>
   <h3 className={cn("mb-4", "flex", "items-center", "gap-2", "px-1", "text-xs", "font-bold", "uppercase", "tracking-widest", "text-slate-400")}>
    <Gift className={cn("h-4", "w-4")} />
    {rewardsLabel}
   </h3>
   <div className={cn("grid", "grid-cols-1", "gap-3", "sm:grid-cols-2")}>
    {questData.definition.rewards.map((reward, index) => (
     <div
      key={`${reward.type}-${reward.amount}-${index}`}
      className={cn(
       "group",
       "flex",
       "flex-col",
       "items-center",
       "rounded-2xl",
       "border",
       "border-slate-700/50",
       "bg-slate-900/60",
       "p-4",
       "text-center",
       "transition-colors",
      )}
     >
      <div className={cn("mb-2", "flex", "h-10", "w-10", "items-center", "justify-center", "rounded-xl", "bg-slate-800", "transition-transform")}>
       {reward.type.toLowerCase() === "gold" ? (
        <Coins className={cn("h-5", "w-5", "text-yellow-500")} />
       ) : reward.type.toLowerCase() === "diamond" ? (
        <Diamond className={cn("h-5", "w-5", "text-cyan-400")} />
       ) : (
        <Award className={cn("h-5", "w-5", "text-purple-400")} />
       )}
      </div>
      <span className={cn("text-sm", "font-black", "text-slate-100")}>+{reward.amount}</span>
      <span className={cn("text-xs", "font-bold", "uppercase", "text-slate-500")}>{rewardTypeLabel(reward.type)}</span>
     </div>
    ))}
   </div>
  </section>
 );
}
