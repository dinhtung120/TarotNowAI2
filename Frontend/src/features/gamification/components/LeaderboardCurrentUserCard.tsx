'use client';

import Image from 'next/image';
import type { LeaderboardEntry } from '../gamification.types';

interface LeaderboardCurrentUserCardProps {
  userRank: LeaderboardEntry | null;
  currency: 'gold' | 'diamond';
  yourRankLabel: string;
  notOnLeaderboardLabel: string;
}

export function LeaderboardCurrentUserCard({
  userRank,
  currency,
  yourRankLabel,
  notOnLeaderboardLabel,
}: LeaderboardCurrentUserCardProps) {
  return (
    <div className="mt-8 pt-8 border-t border-slate-700/50">
      <div className="flex items-center justify-between mb-4 px-2">
        <h3 className="text-[11px] font-black text-slate-500 uppercase tracking-[0.2em]">{yourRankLabel}</h3>
        {userRank && (
          <span className="text-[11px] font-bold text-emerald-500 flex items-center gap-1.5">
            <div className="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse" />
            Live status
          </span>
        )}
      </div>

      {userRank ? (
        <div
          className={`flex items-center gap-4 p-5 rounded-2xl border-2 transition-all duration-500 ${
            currency === 'gold'
              ? 'bg-emerald-500/5 border-emerald-500/20 shadow-[0_0_20px_rgba(16,185,129,0.05)]'
              : 'bg-indigo-500/5 border-indigo-500/20 shadow-[0_0_20px_rgba(99,102,241,0.05)]'
          }`}
        >
          <div className="flex items-center justify-center w-8 h-8 shrink-0">
            <span className={`text-2xl font-black ${currency === 'gold' ? 'text-emerald-400' : 'text-indigo-400'}`}>#{userRank.rank}</span>
          </div>

          <div
            className={`w-14 h-14 rounded-full overflow-hidden shrink-0 border-2 relative bg-slate-800 shadow-xl ${
              currency === 'gold' ? 'border-emerald-500/40' : 'border-indigo-500/40'
            }`}
          >
            {userRank.avatar ? (
              <Image src={userRank.avatar} alt={userRank.displayName} fill sizes="56px" className="object-cover" />
            ) : (
              <div className="w-full h-full flex items-center justify-center text-xl font-black text-slate-500">
                {userRank.displayName.slice(0, 1).toUpperCase()}
              </div>
            )}
          </div>

          <div className="flex-1 min-w-0">
            <h4 className="font-bold truncate text-lg text-white">{userRank.displayName}</h4>
            <p className={`text-xs font-black uppercase tracking-wider truncate mb-0.5 ${currency === 'gold' ? 'text-emerald-400' : 'text-indigo-400'}`}>
              {userRank.activeTitle || 'TarotNow Traveler'}
            </p>
          </div>

          <div className="text-right shrink-0">
            <div className={`text-2xl font-black tabular-nums ${currency === 'gold' ? 'text-emerald-400' : 'text-indigo-400'}`}>
              {userRank.score.toLocaleString()}
            </div>
            <div className="text-[10px] font-black text-slate-500 uppercase tracking-widest text-right">TIÊU THỤ</div>
          </div>
        </div>
      ) : (
        <div className="p-6 rounded-2xl bg-slate-900/40 border border-slate-800/50 border-dashed text-center">
          <p className="text-slate-500 text-sm font-medium">{notOnLeaderboardLabel}</p>
          <p className="text-[10px] text-slate-600 mt-1 uppercase tracking-tight">Hãy bắt đầu hành trình của bạn để có tên trên bảng vàng!</p>
        </div>
      )}
    </div>
  );
}
