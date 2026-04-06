'use client';

import { useTranslations } from 'next-intl';
import { Medal, Trophy, Flame, Zap } from 'lucide-react';
import { useAuthStore } from '@/store/authStore';
import { useTitles, useAchievements } from '../useGamification';
import { useLocalizedField } from '../useLocalizedField';
import type { TitleDefinition } from '../gamification.types';

export default function GamificationStatsBar() {
  const t = useTranslations('Gamification');
  const user = useAuthStore((state) => state.user);
  const { data: titlesData, isLoading: isLoadingTitles } = useTitles();
  const { data: achievementsData, isLoading: isLoadingAchievements } = useAchievements();
  const { localize } = useLocalizedField();

  // Derived values
  const ownedTitlesCount = titlesData?.unlockedList?.length || 0;
  const totalTitlesCount = titlesData?.definitions?.length || 0;
  const activeTitle = titlesData?.definitions?.find(
    (title: TitleDefinition) => title.code === titlesData.activeTitleCode,
  );
  const activeTitleName = activeTitle ? localize(activeTitle.nameVi, activeTitle.nameEn) : null;
  
  const unlockedAchievementsCount = achievementsData?.unlockedList?.length || 0;
  const totalAchievementsCount = achievementsData?.definitions?.length || 0;
  
  const level = user?.level || 1;
  const exp = user?.exp || 0;

  return (
    <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8 relative z-10">
      {/* Level & EXP */}
      <div className="bg-slate-900/60 border border-purple-500/30 rounded-2xl p-4 md:p-5 backdrop-blur-md flex flex-col justify-center shadow-lg shadow-purple-500/10">
        <div className="flex items-center gap-2 mb-2">
          <div className="p-2 bg-purple-500/20 rounded-lg text-purple-400">
            <Zap className="w-4 h-4" />
          </div>
          <span className="text-xs font-bold text-slate-400 uppercase tracking-widest">{t('Level')} & EXP</span>
        </div>
        <div className="flex items-end justify-between mb-2">
          <span className="text-2xl font-black text-white">Lv.{level}</span>
          <span className="text-sm font-bold text-slate-400">{exp} EXP</span>
        </div>
        <div className="h-1.5 w-full bg-slate-800 rounded-full overflow-hidden">
          <div 
            className="h-full bg-gradient-to-r from-purple-500 to-indigo-500 rounded-full" 
            style={{ width: `${Math.min((exp % 100), 100)}%` }} 
          />
        </div>
      </div>

      {/* Titles */}
      <div className="bg-slate-900/60 border border-blue-500/30 rounded-2xl p-4 md:p-5 backdrop-blur-md flex flex-col justify-center shadow-lg shadow-blue-500/10">
        <div className="flex items-center gap-2 mb-2">
          <div className="p-2 bg-blue-500/20 rounded-lg text-blue-400">
            <Medal className="w-4 h-4" />
          </div>
          <span className="text-xs font-bold text-slate-400 uppercase tracking-widest">{t('TotalTitles')}</span>
        </div>
        <div className="text-2xl font-black text-white flex items-baseline gap-1">
          {isLoadingTitles ? '...' : ownedTitlesCount}
          <span className="text-base font-semibold text-slate-500">/ {totalTitlesCount}</span>
        </div>
        <div className="h-1.5 w-full bg-slate-800 rounded-full overflow-hidden mt-3">
          <div 
            className="h-full bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full" 
            style={{ width: totalTitlesCount > 0 ? `${(ownedTitlesCount / totalTitlesCount) * 100}%` : '0%' }} 
          />
        </div>
        <div className="text-xs text-blue-400 truncate mt-2">
          <span className="text-slate-500">{t('ActiveTitle')}: </span>
          <span className="font-semibold">{activeTitleName || t('NoTitleYet')}</span>
        </div>
      </div>

      {/* Achievements */}
      <div className="bg-slate-900/60 border border-amber-500/30 rounded-2xl p-4 md:p-5 backdrop-blur-md flex flex-col justify-center shadow-lg shadow-amber-500/10">
        <div className="flex items-center gap-2 mb-2">
          <div className="p-2 bg-amber-500/20 rounded-lg text-amber-500">
            <Trophy className="w-4 h-4" />
          </div>
          <span className="text-xs font-bold text-slate-400 uppercase tracking-widest">{t('TotalAchievements')}</span>
        </div>
        <div className="text-2xl font-black text-white flex items-baseline gap-1">
          {isLoadingAchievements ? '...' : unlockedAchievementsCount}
          <span className="text-base font-semibold text-slate-500">/ {totalAchievementsCount}</span>
        </div>
        <div className="h-1.5 w-full bg-slate-800 rounded-full overflow-hidden mt-3">
          <div 
            className="h-full bg-gradient-to-r from-amber-500 to-orange-500 rounded-full" 
            style={{ width: totalAchievementsCount > 0 ? `${(unlockedAchievementsCount / totalAchievementsCount) * 100}%` : '0%' }} 
          />
        </div>
      </div>

      {/* Điểm Thành Tựu */}
      <div className="bg-slate-900/60 border border-teal-500/30 rounded-2xl p-4 md:p-5 backdrop-blur-md flex flex-col justify-center shadow-lg shadow-teal-500/10">
        <div className="flex items-center gap-2 mb-2">
          <div className="p-2 bg-teal-500/20 rounded-lg text-teal-400">
            <Flame className="w-4 h-4" />
          </div>
          <span className="text-xs font-bold text-slate-400 uppercase tracking-widest">{t('AchievementPoints', { defaultMessage: 'Điểm Thành Tựu' })}</span>
        </div>
        <div className="text-2xl font-black text-white mb-1">
          {isLoadingAchievements ? '...' : (unlockedAchievementsCount * 10).toLocaleString()}
        </div>
        <p className="text-xs text-teal-400 truncate">
          {t('KeepItUp', { defaultMessage: 'Keep it up!' })}
        </p>
      </div>
    </div>
  );
}
