'use client';

import { Award, Clock, Coins, Diamond, Gift } from 'lucide-react';
import { cn } from '@/shared/utils/cn';
import type { QuestWithProgress, UserAchievement } from '../gamification.types';

interface GamificationDetailBodyProps {
  isQuest: boolean;
  isAchievement: boolean;
  isTitle: boolean;
  description: string;
  completionConditionsLabel: string;
  descriptionLabel: string;
  howToUnlockLabel: string;
  progressLabel: string;
  rewardsLabel: string;
  unlockedAtLabel: string;
  current: number;
  target: number;
  isCompleted: boolean;
  questData?: QuestWithProgress;
  unlockedInfo?: UserAchievement;
  titleGrantedAt?: string;
  rewardTypeLabel: (rewardType: string) => string;
}

export function GamificationDetailBody({
  isQuest,
  isAchievement,
  isTitle,
  description,
  completionConditionsLabel,
  descriptionLabel,
  howToUnlockLabel,
  progressLabel,
  rewardsLabel,
  unlockedAtLabel,
  current,
  target,
  isCompleted,
  questData,
  unlockedInfo,
  titleGrantedAt,
  rewardTypeLabel,
}: GamificationDetailBodyProps) {
  return (
    <div className="space-y-6 relative z-10">
      <section className="bg-slate-900/40 rounded-2xl p-5 border border-slate-700/50">
        <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-3 flex items-center gap-2">
          <Clock className="w-3.5 h-3.5" />
          {isQuest ? completionConditionsLabel : isTitle ? descriptionLabel : howToUnlockLabel}
        </h3>
        <p className="text-slate-200 text-sm leading-relaxed font-medium">{description}</p>

        {isQuest && (
          <div className="mt-4 pt-4 border-t border-slate-700/30">
            <div className="flex justify-between items-end mb-2">
              <span className="text-xs text-slate-400 font-bold">{progressLabel}</span>
              <span className="text-sm font-black text-indigo-400">
                {current} / {target}
              </span>
            </div>
            <div className="h-2.5 w-full bg-slate-950/80 rounded-full overflow-hidden border border-slate-800">
              <div
                className={cn(
                  'h-full rounded-full transition-all duration-1000 ease-out',
                  isCompleted ? 'bg-gradient-to-r from-indigo-500 to-purple-500' : 'bg-indigo-500',
                )}
                style={{ width: `${Math.min((current / target) * 100, 100)}%` }}
              />
            </div>
          </div>
        )}
      </section>

      {isQuest && questData && questData.definition.rewards.length > 0 && (
        <section>
          <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-4 flex items-center gap-2 px-1">
            <Gift className="w-3.5 h-3.5" />
            {rewardsLabel}
          </h3>
          <div className="grid grid-cols-2 gap-3">
            {questData.definition.rewards.map((reward, index) => (
              <div
                key={index}
                className="bg-slate-900/60 p-4 rounded-2xl border border-slate-700/50 flex flex-col items-center text-center group hover:border-indigo-500/30 transition-colors"
              >
                <div className="w-10 h-10 rounded-xl bg-slate-800 flex items-center justify-center mb-2 group-hover:scale-110 transition-transform">
                  {reward.type.toLowerCase() === 'gold' ? (
                    <Coins className="w-5 h-5 text-yellow-500" />
                  ) : reward.type.toLowerCase() === 'diamond' ? (
                    <Diamond className="w-5 h-5 text-cyan-400" />
                  ) : (
                    <Award className="w-5 h-5 text-purple-400" />
                  )}
                </div>
                <span className="text-sm font-black text-slate-100">+{reward.amount}</span>
                <span className="text-[10px] font-bold text-slate-500 uppercase">{rewardTypeLabel(reward.type)}</span>
              </div>
            ))}
          </div>
        </section>
      )}

      {isAchievement && unlockedInfo && (
        <section className="bg-amber-500/5 rounded-2xl p-4 border border-amber-500/20 flex items-center gap-3">
          <Award className="w-8 h-8 text-amber-500/50" />
          <div className="text-xs">
            <p className="text-amber-200/50 font-bold uppercase tracking-wider">{unlockedAtLabel}</p>
            <p className="text-amber-200 font-black text-sm">{new Date(unlockedInfo.unlockedAt).toLocaleDateString()}</p>
          </div>
        </section>
      )}

      {isTitle && titleGrantedAt && (
        <section className="bg-amber-500/5 rounded-2xl p-4 border border-amber-500/20 flex items-center gap-3">
          <Award className="w-8 h-8 text-amber-500/50" />
          <div className="text-xs">
            <p className="text-amber-200/50 font-bold uppercase tracking-wider">{unlockedAtLabel}</p>
            <p className="text-amber-200 font-black text-sm">{new Date(titleGrantedAt).toLocaleDateString()}</p>
          </div>
        </section>
      )}
    </div>
  );
}
