import { Award, Coins, Diamond, Gift } from "lucide-react";
import type { QuestRewardItem } from "@/features/gamification/gamification.types";

interface QuestRewardBadgesProps {
 rewards: QuestRewardItem[];
}

export function QuestRewardBadges({ rewards }: QuestRewardBadgesProps) {
 return (
  <div className="absolute top-4 right-4 flex gap-2">
   {rewards.map((reward, index) => (
    <div key={`${reward.type}-${reward.amount}-${index}`} className="flex items-center gap-1.5 px-3 py-1 rounded-full bg-slate-900/50 border border-slate-700/50 shadow-inner">
     {reward.type.toLowerCase() === "gold" ? <Coins className="w-3.5 h-3.5 text-yellow-500" /> : reward.type.toLowerCase() === "diamond" ? <Diamond className="w-3.5 h-3.5 text-cyan-400" /> : reward.type.toLowerCase() === "title" ? <Award className="w-3.5 h-3.5 text-purple-400" /> : <Gift className="w-3.5 h-3.5 text-purple-400" />}
     <span className="text-xs font-bold text-slate-200">+{reward.amount}</span>
    </div>
   ))}
  </div>
 );
}
