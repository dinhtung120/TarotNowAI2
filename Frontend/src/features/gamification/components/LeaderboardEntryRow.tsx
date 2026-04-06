'use client';

import { Medal } from 'lucide-react';
import Image from 'next/image';
import type { LeaderboardEntry } from '../gamification.types';

interface LeaderboardEntryRowProps {
  entry: LeaderboardEntry;
  index: number;
  currency: 'gold' | 'diamond';
  noTitleLabel: string;
}

export function LeaderboardEntryRow({ entry, index, currency, noTitleLabel }: LeaderboardEntryRowProps) {
  const isTop3 = index < 3;

  return (
    <div
      className={`flex items-center gap-4 p-4 rounded-2xl transition-all duration-300 group ${
        index === 0
          ? 'bg-gradient-to-r from-amber-500/15 to-transparent border border-amber-500/20 shadow-[0_4px_30px_rgba(245,158,11,0.05)]'
          : index === 1
            ? 'bg-gradient-to-r from-slate-300/10 to-transparent border border-slate-300/20'
            : index === 2
              ? 'bg-gradient-to-r from-amber-700/10 to-transparent border border-amber-700/20'
              : 'bg-slate-900/40 border border-slate-800/60 hover:bg-slate-800/60 hover:translate-x-1'
      }`}
    >
      <div className="flex items-center justify-center w-8 h-8 shrink-0 relative">
        {index === 0 ? (
          <Medal className="w-9 h-9 text-amber-400 drop-shadow-[0_0_10px_rgba(251,191,36,0.6)]" />
        ) : index === 1 ? (
          <Medal className="w-8 h-8 text-slate-300 drop-shadow-[0_0_8px_rgba(203,213,225,0.6)]" />
        ) : index === 2 ? (
          <Medal className="w-8 h-8 text-amber-700 drop-shadow-[0_0_8px_rgba(180,83,9,0.6)]" />
        ) : (
          <span className="text-lg font-black text-slate-600 group-hover:text-slate-400">#{index + 1}</span>
        )}
      </div>

      <div className="w-12 h-12 rounded-full overflow-hidden shrink-0 border-2 border-slate-700 relative bg-slate-800 shadow-inner group-hover:border-slate-500 transition-colors">
        {entry.avatar ? (
          <Image src={entry.avatar} alt={entry.displayName} fill sizes="48px" className="object-cover" />
        ) : (
          <div className="w-full h-full flex items-center justify-center text-lg font-bold text-slate-500">
            {entry.displayName.slice(0, 1).toUpperCase()}
          </div>
        )}
      </div>

      <div className="flex-1 min-w-0">
        <h4 className={`font-bold truncate text-base ${isTop3 ? 'text-white' : 'text-slate-300 group-hover:text-white'}`}>
          {entry.displayName}
        </h4>
        <p className="text-[11px] text-blue-400/80 font-bold uppercase tracking-wider truncate">
          {entry.activeTitle || noTitleLabel}
        </p>
      </div>

      <div className="text-right shrink-0">
        <div
          className={`text-xl font-black tabular-nums transition-all ${
            index === 0 ? 'text-amber-400 scale-110' : isTop3 ? 'text-slate-200' : 'text-slate-500 group-hover:text-slate-300'
          }`}
        >
          {entry.score.toLocaleString()}
        </div>
        <div className="text-[10px] font-black text-slate-600 uppercase tracking-widest">
          {currency === 'gold' ? 'VÀNG' : 'K.CƯƠNG'}
        </div>
      </div>
    </div>
  );
}
