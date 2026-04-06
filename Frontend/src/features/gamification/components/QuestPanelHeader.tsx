'use client';

import { Trophy } from 'lucide-react';

interface QuestPanelHeaderProps {
  questType: 'daily' | 'weekly';
  setQuestType: (questType: 'daily' | 'weekly') => void;
  t: (key: string) => string;
}

export function QuestPanelHeader({ questType, setQuestType, t }: QuestPanelHeaderProps) {
  return (
    <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6">
      <div className="flex items-center gap-3">
        <div className="p-3 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-xl shadow-lg shadow-indigo-500/30">
          <Trophy className="w-6 h-6 text-white" />
        </div>
        <div>
          <h2 className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-indigo-100 to-purple-200">
            {questType === 'daily' ? t('DailyQuests') : t('WeeklyQuests')}
          </h2>
          <p className="text-sm text-indigo-200/70">{t('CompleteThemBeforeMidnight')}</p>
        </div>
      </div>

      <div className="flex bg-slate-900/50 p-1 rounded-xl border border-slate-700/50">
        <button
          onClick={() => setQuestType('daily')}
          className={`px-4 py-2 rounded-lg text-sm font-bold transition-all ${
            questType === 'daily'
              ? 'bg-gradient-to-r from-indigo-500 to-purple-600 text-white shadow-md'
              : 'text-slate-400 hover:text-slate-200 hover:bg-slate-800/50'
          }`}
        >
          {t('DailyQuests')}
        </button>
        <button
          onClick={() => setQuestType('weekly')}
          className={`px-4 py-2 rounded-lg text-sm font-bold transition-all ${
            questType === 'weekly'
              ? 'bg-gradient-to-r from-indigo-500 to-purple-600 text-white shadow-md'
              : 'text-slate-400 hover:text-slate-200 hover:bg-slate-800/50'
          }`}
        >
          {t('WeeklyQuests')}
        </button>
      </div>
    </div>
  );
}
