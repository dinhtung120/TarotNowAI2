'use client';

import { Award, Trophy } from 'lucide-react';
import { cn } from '@/shared/utils/cn';

interface GamificationDetailHeaderProps {
  isQuest: boolean;
  isCompleted: boolean;
  title: string;
  completedLabel: string;
  pendingLabel: string;
}

export function GamificationDetailHeader({
  isQuest,
  isCompleted,
  title,
  completedLabel,
  pendingLabel,
}: GamificationDetailHeaderProps) {
  return (
    <div className="relative flex flex-col items-center text-center mb-8">
      <div
        className={cn(
          'w-20 h-20 rounded-3xl flex items-center justify-center mb-4 transition-transform duration-500 hover:scale-110 shadow-2xl',
          isQuest
            ? 'bg-gradient-to-br from-indigo-500 to-purple-600 shadow-indigo-500/20'
            : 'bg-gradient-to-br from-amber-400 to-orange-600 shadow-amber-500/20',
          !isCompleted && 'grayscale opacity-50',
        )}
      >
        {isQuest ? <Trophy className="w-10 h-10 text-white" /> : <Award className="w-10 h-10 text-white" />}
      </div>

      <h2 className="text-2xl font-black text-slate-100 mb-2 tracking-tight">{title}</h2>
      <div
        className={cn(
          'px-3 py-1 rounded-full text-[10px] font-bold uppercase tracking-widest border',
          isCompleted ? 'bg-green-500/10 text-green-400 border-green-500/20' : 'bg-slate-500/10 text-slate-400 border-slate-500/20',
        )}
      >
        {isCompleted ? completedLabel : pendingLabel}
      </div>
    </div>
  );
}
