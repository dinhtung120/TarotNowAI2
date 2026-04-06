'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { BarChart3, TrendingUp, Medal, Coins, Diamond, Calendar, Clock, Globe } from 'lucide-react';
import { useLeaderboard } from '../useGamification';
import type { LeaderboardEntry } from '../gamification.types';
import Image from 'next/image';

export default function LeaderboardTable() {
  const t = useTranslations('Gamification');
  const [currency, setCurrency] = useState<'gold' | 'diamond'>('gold');
  const [period, setPeriod] = useState<'daily' | 'monthly' | 'all'>('daily');

  // Xác định track dựa trên currency và period
  const track = `spent_${currency}_${period}`;
  const { data, isLoading } = useLeaderboard(track);

  const currencyTabs = [
    { id: 'gold', label: t('Gold'), icon: Coins, color: 'emerald' },
    { id: 'diamond', label: t('Diamond'), icon: Diamond, color: 'indigo' },
  ];

  const periodTabs = [
    { id: 'daily', label: 'Ngày', icon: Clock },
    { id: 'monthly', label: 'Tháng', icon: Calendar },
    { id: 'all', label: 'Tất cả', icon: Globe },
  ];

  return (
    <div className="bg-slate-800/40 border border-slate-700/50 rounded-3xl p-6 backdrop-blur-xl shadow-2xl relative overflow-hidden">
      {/* Decorative background glow */}
      <div className={`absolute -top-24 -right-24 w-64 h-64 blur-[120px] opacity-20 pointer-events-none transition-colors duration-700 ${
        currency === 'gold' ? 'bg-emerald-500' : 'bg-indigo-500'
      }`} />

      <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 mb-8 relative z-10">
        <div className="flex items-center gap-3">
          <div className={`p-3 rounded-2xl shadow-lg transition-all duration-500 ${
            currency === 'gold' 
              ? 'bg-gradient-to-br from-emerald-400 to-teal-600 shadow-emerald-500/30' 
              : 'bg-gradient-to-br from-indigo-400 to-blue-600 shadow-indigo-500/30'
          }`}>
            <BarChart3 className="w-6 h-6 text-white" />
          </div>
          <div>
            <h2 className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-slate-100 to-slate-400">
              {t('Leaderboard')}
            </h2>
            <p className="text-sm text-slate-400">
              {currency === 'gold' ? 'Đại phú hào tích cực' : 'TarotNow VIP Rankings'}
            </p>
          </div>
        </div>

        {/* Currency Tabs */}
        <div className="flex bg-slate-900/60 p-1.5 rounded-2xl border border-slate-700/50 backdrop-blur-md">
          {currencyTabs.map((tab) => (
            <button
              key={tab.id}
              onClick={() => setCurrency(tab.id as any)}
              className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all duration-300 ${
                currency === tab.id
                  ? tab.id === 'gold' 
                    ? 'bg-emerald-500 text-white shadow-lg shadow-emerald-500/30' 
                    : 'bg-indigo-500 text-white shadow-lg shadow-indigo-500/30'
                  : 'text-slate-400 hover:text-slate-200 hover:bg-white/5'
              }`}
            >
              <tab.icon className="w-4 h-4" />
              {tab.label}
            </button>
          ))}
        </div>
      </div>

      {/* Period Selectors */}
      <div className="flex items-center gap-2 mb-8 p-1 bg-slate-900/40 rounded-xl w-fit">
        {periodTabs.map((p) => (
          <button
            key={p.id}
            onClick={() => setPeriod(p.id as any)}
            className={`flex items-center gap-2 px-4 py-1.5 rounded-lg text-xs font-semibold uppercase tracking-wider transition-all duration-200 ${
              period === p.id
                ? 'bg-slate-700 text-white shadow-sm'
                : 'text-slate-500 hover:text-slate-300'
            }`}
          >
            <p.icon className="w-3.5 h-3.5" />
            {p.label}
          </button>
        ))}
      </div>

      {isLoading ? (
        <div className="py-20 flex justify-center">
          <div className={`w-10 h-10 border-4 border-t-transparent rounded-full animate-spin ${
            currency === 'gold' ? 'border-emerald-500' : 'border-indigo-500'
          }`}></div>
        </div>
      ) : !data || data.entries.length === 0 ? (
        <div className="text-center py-20 bg-slate-900/30 rounded-3xl border border-slate-800/50 border-dashed">
          <TrendingUp className="w-16 h-16 text-slate-700 mx-auto mb-4 opacity-30" />
          <p className="text-slate-500 font-medium">{t('NoLeaderboardData')}</p>
        </div>
      ) : (
        <div className="space-y-3 min-h-[400px]">
          {data.entries.map((entry: LeaderboardEntry, index: number) => {
            const isTop3 = index < 3;
            return (
              <div
                key={entry.userId}
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
                    {entry.activeTitle || t('NoTitleYet')}
                  </p>
                </div>

                <div className="text-right shrink-0">
                  <div className={`text-xl font-black tabular-nums transition-all ${
                    index === 0 ? 'text-amber-400 scale-110' : isTop3 ? 'text-slate-200' : 'text-slate-500 group-hover:text-slate-300'
                  }`}>
                    {entry.score.toLocaleString()}
                  </div>
                  <div className="text-[10px] font-black text-slate-600 uppercase tracking-widest">
                    {currency === 'gold' ? 'VÀNG' : 'K.CƯƠNG'}
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      )}

      {/* Your Rank Section */}
      {!isLoading && data && (
        <div className="mt-8 pt-8 border-t border-slate-700/50">
          <div className="flex items-center justify-between mb-4 px-2">
            <h3 className="text-[11px] font-black text-slate-500 uppercase tracking-[0.2em]">
              {t('YourRank')}
            </h3>
            {data.userRank && (
              <span className="text-[11px] font-bold text-emerald-500 flex items-center gap-1.5">
                <div className="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse" />
                Live status
              </span>
            )}
          </div>
          
          {data.userRank ? (
            <div className={`flex items-center gap-4 p-5 rounded-2xl border-2 transition-all duration-500 ${
              currency === 'gold'
                ? 'bg-emerald-500/5 border-emerald-500/20 shadow-[0_0_20px_rgba(16,185,129,0.05)]'
                : 'bg-indigo-500/5 border-indigo-500/20 shadow-[0_0_20px_rgba(99,102,241,0.05)]'
            }`}>
              <div className="flex items-center justify-center w-8 h-8 shrink-0">
                <span className={`text-2xl font-black ${
                  currency === 'gold' ? 'text-emerald-400' : 'text-indigo-400'
                }`}>#{data.userRank.rank}</span>
              </div>
              
              <div className={`w-14 h-14 rounded-full overflow-hidden shrink-0 border-2 relative bg-slate-800 shadow-xl ${
                currency === 'gold' ? 'border-emerald-500/40' : 'border-indigo-500/40'
              }`}>
                {data.userRank.avatar ? (
                  <Image src={data.userRank.avatar} alt={data.userRank.displayName} fill sizes="56px" className="object-cover" />
                ) : (
                  <div className="w-full h-full flex items-center justify-center text-xl font-black text-slate-500">
                    {data.userRank.displayName.slice(0, 1).toUpperCase()}
                  </div>
                )}
              </div>

              <div className="flex-1 min-w-0">
                <h4 className="font-bold truncate text-lg text-white">
                  {data.userRank.displayName}
                </h4>
                <p className={`text-xs font-black uppercase tracking-wider truncate mb-0.5 ${
                  currency === 'gold' ? 'text-emerald-400' : 'text-indigo-400'
                }`}>
                  {data.userRank.activeTitle || 'TarotNow Traveler'}
                </p>
              </div>

              <div className="text-right shrink-0">
                <div className={`text-2xl font-black tabular-nums ${
                   currency === 'gold' ? 'text-emerald-400' : 'text-indigo-400'
                }`}>
                  {data.userRank.score.toLocaleString()}
                </div>
                <div className="text-[10px] font-black text-slate-500 uppercase tracking-widest text-right">
                  TIÊU THỤ
                </div>
              </div>
            </div>
          ) : (
            <div className="p-6 rounded-2xl bg-slate-900/40 border border-slate-800/50 border-dashed text-center">
              <p className="text-slate-500 text-sm font-medium">{t('NotOnLeaderboard')}</p>
              <p className="text-[10px] text-slate-600 mt-1 uppercase tracking-tight">Hãy bắt đầu hành trình của bạn để có tên trên bảng vàng!</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
}

