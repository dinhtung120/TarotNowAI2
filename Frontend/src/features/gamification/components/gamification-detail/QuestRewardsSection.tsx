import { Award, Coins, Diamond, Gift } from "lucide-react";
import type { QuestWithProgress } from "@/features/gamification/gamification.types";

interface QuestRewardsSectionProps {
 questData: QuestWithProgress;
 rewardsLabel: string;
 rewardTypeLabel: (rewardType: string) => string;
}

export function QuestRewardsSection({ questData, rewardsLabel, rewardTypeLabel }: QuestRewardsSectionProps) {
 if (questData.definition.rewards.length === 0) return null;

 return (
  <section>
   <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-4 flex items-center gap-2 px-1"><Gift className="w-3.5 h-3.5" />{rewardsLabel}</h3>
   <div className="grid grid-cols-2 gap-3">
    {questData.definition.rewards.map((reward, index) => <div key={`${reward.type}-${reward.amount}-${index}`} className="bg-slate-900/60 p-4 rounded-2xl border border-slate-700/50 flex flex-col items-center text-center group hover:border-indigo-500/30 transition-colors"><div className="w-10 h-10 rounded-xl bg-slate-800 flex items-center justify-center mb-2 group-hover:scale-110 transition-transform">{reward.type.toLowerCase() === "gold" ? <Coins className="w-5 h-5 text-yellow-500" /> : reward.type.toLowerCase() === "diamond" ? <Diamond className="w-5 h-5 text-cyan-400" /> : <Award className="w-5 h-5 text-purple-400" />}</div><span className="text-sm font-black text-slate-100">+{reward.amount}</span><span className="text-[10px] font-bold text-slate-500 uppercase">{rewardTypeLabel(reward.type)}</span></div>)}
   </div>
  </section>
 );
}
