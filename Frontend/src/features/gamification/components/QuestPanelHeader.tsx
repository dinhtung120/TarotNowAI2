'use client';

import { Trophy } from 'lucide-react';
import { cn } from '@/lib/utils';

interface QuestPanelHeaderProps {
  questType: 'daily' | 'weekly';
  setQuestType: (questType: 'daily' | 'weekly') => void;
  t: (key: string) => string;
}

export function QuestPanelHeader({ questType, setQuestType, t }: QuestPanelHeaderProps) {
  return (
    <div className={cn("tn-flex-col-row-sm tn-items-center-sm justify-between gap-4 mb-6")}>
      <div className={cn("flex items-center gap-3")}>
        <div className={cn("p-3 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-xl shadow-lg shadow-indigo-500/30")}>
          <Trophy className={cn("w-6 h-6 text-white")} />
        </div>
        <div>
          <h2 className={cn("text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-indigo-100 to-purple-200")}>
            {questType === 'daily' ? t('DailyQuests') : t('WeeklyQuests')}
          </h2>
          <p className={cn("text-sm text-indigo-200/70")}>{t('CompleteThemBeforeMidnight')}</p>
        </div>
      </div>

      <div className={cn("flex bg-slate-900/50 p-1 rounded-xl border border-slate-700/50")}>
        <button
          type="button"
          onClick={() => setQuestType('daily')}
          className={cn("px-4 py-2 rounded-lg text-sm font-bold transition-all", 
            questType === 'daily'
              ? 'bg-gradient-to-r from-indigo-500 to-purple-600 text-white shadow-md'
              : 'text-slate-400'
          )}
        >
          {t('DailyQuests')}
        </button>
        <button
          type="button"
          onClick={() => setQuestType('weekly')}
          className={cn("px-4 py-2 rounded-lg text-sm font-bold transition-all", 
            questType === 'weekly'
              ? 'bg-gradient-to-r from-indigo-500 to-purple-600 text-white shadow-md'
              : 'text-slate-400'
          )}
        >
          {t('WeeklyQuests')}
        </button>
      </div>
    </div>
  );
}
