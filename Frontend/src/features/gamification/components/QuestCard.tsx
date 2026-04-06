'use client';

import { Check, Clock, Coins, Diamond, Gift } from 'lucide-react';
import type { QuestRewardItem, QuestWithProgress } from '../gamification.types';

interface QuestCardProps {
  quest: QuestWithProgress;
  isClaimPending: boolean;
  localize: (vi: string, en: string) => string;
  t: (key: string) => string;
  onOpen: (quest: QuestWithProgress) => void;
  onClaim: (questCode: string, periodKey: string) => void;
}

export function QuestCard({
  quest,
  isClaimPending,
  localize,
  t,
  onOpen,
  onClaim,
}: QuestCardProps) {
  const def = quest.definition;
  const prog = quest.progress;
  const current = prog?.currentProgress || 0;
  const target = def.target;
  const isCompleted = current >= target;
  const isClaimed = prog?.isClaimed || false;
  const percent = Math.min((current / target) * 100, 100);

  return (
    <div
      onClick={() => onOpen(quest)}
      className={`relative overflow-hidden p-5 rounded-2xl border transition-all duration-300 cursor-pointer ${
        isClaimed
          ? 'bg-slate-800/40 border-slate-700/50 opacity-70'
          : isCompleted
            ? 'bg-gradient-to-br from-slate-800/80 to-indigo-900/40 border-indigo-500/50 shadow-lg shadow-indigo-500/10 hover:shadow-indigo-500/20'
            : 'bg-slate-800/60 border-slate-700/80 hover:bg-slate-800/80'
      } backdrop-blur-xl group`}
    >
      <div className="absolute top-4 right-4 flex gap-2">
        {def.rewards.map((r: QuestRewardItem, idx: number) => (
          <div
            key={idx}
            className="flex items-center gap-1.5 px-3 py-1 rounded-full bg-slate-900/50 border border-slate-700/50 shadow-inner"
          >
            {r.type.toLowerCase() === 'gold' ? (
              <Coins className="w-3.5 h-3.5 text-yellow-500" />
            ) : r.type.toLowerCase() === 'diamond' ? (
              <Diamond className="w-3.5 h-3.5 text-cyan-400" />
            ) : (
              <Gift className="w-3.5 h-3.5 text-purple-400" />
            )}
            <span className="text-xs font-bold text-slate-200">+{r.amount}</span>
          </div>
        ))}
      </div>

      <div className="flex gap-4 items-start pr-16">
        <div className="min-w-0">
          <h3 className={`text-lg font-semibold truncate ${isCompleted && !isClaimed ? 'text-indigo-300' : 'text-slate-100'}`}>
            {localize(def.titleVi, def.titleEn)}
          </h3>
          <p className="text-sm text-slate-400 mt-1 line-clamp-2">{localize(def.descriptionVi, def.descriptionEn)}</p>
        </div>
      </div>

      <div className="mt-5 flex items-center justify-between gap-4">
        <div className="flex-1">
          <div className="flex justify-between text-xs font-medium mb-1.5 px-1">
            <span className="text-slate-400">{isClaimed ? t('Claimed') : t('Progress')}</span>
            <span className={isCompleted && !isClaimed ? 'text-indigo-400' : 'text-slate-300'}>
              {current} / {target}
            </span>
          </div>
          <div className="h-2 w-full bg-slate-900/50 rounded-full overflow-hidden border border-slate-800">
            <div
              className={`h-full rounded-full transition-all duration-1000 ease-out ${
                isClaimed
                  ? 'bg-slate-600'
                  : isCompleted
                    ? 'bg-gradient-to-r from-indigo-500 to-purple-500 shadow-[0_0_10px_rgba(99,102,241,0.5)]'
                    : 'bg-gradient-to-r from-blue-500 to-indigo-500'
              }`}
              style={{ width: `${percent}%` }}
            />
          </div>
        </div>

        <div className="shrink-0">
          {isClaimed ? (
            <button
              disabled
              className="px-4 py-2 rounded-xl bg-slate-800/80 text-slate-500 border border-slate-700/50 font-medium flex items-center gap-2 cursor-not-allowed"
            >
              <Check className="w-4 h-4" />
              {t('Claimed')}
            </button>
          ) : isCompleted ? (
            <button
              onClick={(e) => {
                e.stopPropagation();
                onClaim(def.code, prog!.periodKey);
              }}
              disabled={isClaimPending}
              className="relative overflow-hidden px-5 py-2 rounded-xl bg-gradient-to-r from-indigo-500 to-purple-600 text-white font-medium hover:from-indigo-400 hover:to-purple-500 focus:ring-2 focus:ring-purple-500/50 focus:outline-none transition-all duration-200 transform hover:scale-105 active:scale-95 disabled:opacity-70 disabled:hover:scale-100 flex items-center group/btn shadow-[0_0_15px_rgba(99,102,241,0.4)]"
            >
              <span className="absolute inset-0 w-full h-full bg-white/20 -translate-x-full group-hover/btn:animate-[shimmer_1s_infinite] skew-x-[-20deg]" />
              {isClaimPending ? (
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
              ) : (
                t('Claim')
              )}
            </button>
          ) : (
            <button
              disabled
              className="px-4 py-2 rounded-xl bg-slate-800/50 text-slate-400 border border-slate-700 font-medium flex items-center gap-2 transition"
            >
              <Clock className="w-4 h-4 opacity-50" />
              {t('Pending')}
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
