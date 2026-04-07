'use client';

import { Crown, Medal, Trophy } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminGamificationTabsProps {
  activeTab: 'quests' | 'achievements' | 'titles';
  onChange: (tab: 'quests' | 'achievements' | 'titles') => void;
}

export function AdminGamificationTabs({ activeTab, onChange }: AdminGamificationTabsProps) {
  return (
    <nav className={cn("flex gap-2 p-1 bg-slate-900/50 rounded-2xl border border-slate-800 w-fit")}>
      <button
        type="button"
        onClick={() => onChange('quests')}
        className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all ${
          activeTab === 'quests' ? 'bg-indigo-600 text-white shadow-lg' : 'text-slate-400 hover:text-slate-200'
        }`}
      >
        <Trophy className={cn("w-4 h-4")} />
        Nhiệm Vụ
      </button>
      <button
        type="button"
        onClick={() => onChange('achievements')}
        className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all ${
          activeTab === 'achievements' ? 'bg-indigo-600 text-white shadow-lg' : 'text-slate-400 hover:text-slate-200'
        }`}
      >
        <Medal className={cn("w-4 h-4")} />
        Thành Tựu
      </button>
      <button
        type="button"
        onClick={() => onChange('titles')}
        className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all ${
          activeTab === 'titles' ? 'bg-indigo-600 text-white shadow-lg' : 'text-slate-400 hover:text-slate-200'
        }`}
      >
        <Crown className={cn("w-4 h-4")} />
        Danh Hiệu
      </button>
    </nav>
  );
}
