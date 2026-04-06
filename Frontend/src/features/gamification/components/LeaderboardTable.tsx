'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { BarChart3, TrendingUp, Coins, Diamond, Calendar, Clock, Globe } from 'lucide-react';
import { useLeaderboard } from '../useGamification';
import type { LeaderboardEntry } from '../gamification.types';
import { LeaderboardEntryRow } from './LeaderboardEntryRow';
import { LeaderboardCurrentUserCard } from './LeaderboardCurrentUserCard';

export default function LeaderboardTable() {
  const t = useTranslations('Gamification');
  const [currency, setCurrency] = useState<'gold' | 'diamond'>('gold');
  const [period, setPeriod] = useState<'daily' | 'monthly' | 'all'>('daily');

  // Xác định track dựa trên currency và period
  const track = `spent_${currency}_${period}`;
  const { data, isLoading } = useLeaderboard(track);

  const currencyTabs = [
    { id: 'gold' as const, label: t('Gold'), icon: Coins },
    { id: 'diamond' as const, label: t('Diamond'), icon: Diamond },
  ];

  const periodTabs = [
    { id: 'daily' as const, label: 'Ngày', icon: Clock },
    { id: 'monthly' as const, label: 'Tháng', icon: Calendar },
    { id: 'all' as const, label: 'Tất cả', icon: Globe },
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
              onClick={() => setCurrency(tab.id)}
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
            onClick={() => setPeriod(p.id)}
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
            return (
              <LeaderboardEntryRow
                key={entry.userId}
                entry={entry}
                index={index}
                currency={currency}
                noTitleLabel={t('NoTitleYet')}
              />
            );
          })}
        </div>
      )}

      {/* Your Rank Section */}
      {!isLoading && data && (
        <LeaderboardCurrentUserCard
          userRank={data.userRank}
          currency={currency}
          yourRankLabel={t('YourRank')}
          notOnLeaderboardLabel={t('NotOnLeaderboard')}
        />
      )}
    </div>
  );
}
