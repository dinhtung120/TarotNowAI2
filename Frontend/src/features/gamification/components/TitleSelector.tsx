'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { Medal, CheckCircle2, Wand2, Lock } from 'lucide-react';
import { toast } from 'react-hot-toast';
import { useTitles, useSetActiveTitle, useGrantSandboxTitles } from '../useGamification';
import type { TitleDefinition, UserTitle } from '../gamification.types';
import { useLocalizedField } from '../useLocalizedField';
import GamificationDetailModal from './GamificationDetailModal';

export default function TitleSelector() {
  const t = useTranslations('Gamification');
  const { data, isLoading } = useTitles();
  const setMutation = useSetActiveTitle();
  const grantMutation = useGrantSandboxTitles();
  const { localize } = useLocalizedField();
  const [selectedTitle, setSelectedTitle] = useState<{
    definition: TitleDefinition;
    isOwned: boolean;
    isActive: boolean;
    grantedAt?: string;
  } | null>(null);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-6">
        <div className="w-8 h-8 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"></div>
      </div>
    );
  }

  if (!data) return null;

  const { definitions, unlockedList, activeTitleCode } = data;

  if (definitions.length === 0) {
    return (
      <div className="flex justify-center items-center py-6 text-slate-500 text-sm">
        Không có dữ liệu danh hiệu.
      </div>
    );
  }

  const handleEquipTitle = (code: string) => {
    if (code === activeTitleCode) return;
    setMutation.mutate(code, {
      onSuccess: () => {
        toast.success(t('TitleUpdated', { defaultMessage: 'Cập nhật danh hiệu thành công' }), {
          style: {
            background: 'rgba(30, 41, 59, 0.9)',
            color: '#fff',
            backdropFilter: 'blur(10px)',
          },
        });
      },
    });
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-3 mb-6 relative z-10">
        <div className="p-3 bg-gradient-to-br from-blue-400 to-indigo-600 rounded-xl shadow-lg shadow-blue-500/30">
          <Medal className="w-6 h-6 text-white" />
        </div>
        <div>
          <h2 className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-blue-100 to-indigo-200">
            {t('YourTitles', { defaultMessage: 'Danh hiệu' })}
          </h2>
          <p className="text-sm text-blue-200/70">
            {t('UnlockedAmount', { count: unlockedList.length, total: definitions.length })}
          </p>
        </div>
        

      </div>

      <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 gap-4 pb-6">
        {/* Card gỡ danh hiệu - No Title */}
        <div
          onClick={() => handleEquipTitle('')}
          className={`group relative overflow-hidden rounded-2xl border p-5 flex flex-col items-center text-center transition-all duration-500 cursor-pointer ${
            !data.activeTitleCode
              ? 'bg-gradient-to-b from-slate-800/80 to-blue-900/20 border-blue-500/40 shadow-lg scale-[1.02] ring-1 ring-blue-500/30'
              : 'bg-slate-900/60 border-slate-800 hover:-translate-y-1 hover:border-slate-700'
          } backdrop-blur-xl`}
        >
          <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_top,_var(--tw-gradient-stops))] from-white/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity" />
          
          <div className="relative mb-4">
            <div
              className={`w-16 h-16 rounded-full flex items-center justify-center border-2 ${
                !data.activeTitleCode
                  ? 'border-blue-400/50 bg-blue-400/10 shadow-[0_0_20px_rgba(59,130,246,0.3)]'
                  : 'border-slate-700 bg-slate-800'
              }`}
            >
              {!data.activeTitleCode ? (
                <CheckCircle2 className="w-8 h-8 text-blue-400 drop-shadow-[0_0_8px_rgba(59,130,246,0.8)]" />
              ) : (
                <Medal className="w-6 h-6 text-slate-600" />
              )}
            </div>
          </div>

          <h3
            className={`text-sm font-bold mb-1 ${
              !data.activeTitleCode ? 'text-blue-200' : 'text-slate-400'
            }`}
          >
            {t('NoTitle')}
          </h3>
          
          <p className="text-xs text-slate-500 line-clamp-3 leading-relaxed mt-1">
            {t('EarnTitleHint')}
          </p>
        </div>

        {/* Danh sách các danh hiệu đã sở hữu */}
        {definitions.map((title: TitleDefinition) => {
          const unlockedInfo = unlockedList.find((u: UserTitle) => u.titleCode === title.code);
          const isOwned = !!unlockedInfo;
          const isActive = isOwned && title.code === activeTitleCode;
          
          const rarityColors: Record<string, { bg: string, ring: string, iconBg: string, iconText: string, text: string, textTitle: string }> = {
            'Common': { bg: 'to-slate-600/20', ring: 'border-slate-500/40', iconBg: 'bg-slate-500/10 border-slate-500/50 shadow-[0_0_20px_rgba(100,116,139,0.3)]', iconText: 'text-slate-400 drop-shadow-[0_0_8px_rgba(100,116,139,0.8)]', text: 'text-slate-300', textTitle: 'text-slate-200' },
            'Rare': { bg: 'to-blue-600/20', ring: 'border-blue-500/40', iconBg: 'bg-blue-500/10 border-blue-500/50 shadow-[0_0_20px_rgba(59,130,246,0.3)]', iconText: 'text-blue-400 drop-shadow-[0_0_8px_rgba(59,130,246,0.8)]', text: 'text-blue-300', textTitle: 'text-blue-200' },
            'Epic': { bg: 'to-purple-600/20', ring: 'border-purple-500/40', iconBg: 'bg-purple-500/10 border-purple-500/50 shadow-[0_0_20px_rgba(168,85,247,0.3)]', iconText: 'text-purple-400 drop-shadow-[0_0_8px_rgba(168,85,247,0.8)]', text: 'text-purple-300', textTitle: 'text-purple-200' },
            'Legendary': { bg: 'to-amber-500/20', ring: 'border-amber-500/40', iconBg: 'bg-amber-500/10 border-amber-500/50 shadow-[0_0_20px_rgba(245,158,11,0.3)]', iconText: 'text-amber-400 drop-shadow-[0_0_8px_rgba(245,158,11,0.8)]', text: 'text-amber-300', textTitle: 'text-amber-200' },
          };
          
          const style = rarityColors[title.rarity] || rarityColors['Common'];

          return (
            <div
              key={title.code}
              onClick={() => setSelectedTitle({ definition: title, isOwned, isActive, grantedAt: unlockedInfo?.grantedAt })}
              className={`group relative overflow-hidden rounded-2xl border p-5 flex flex-col items-center text-center transition-all duration-500 cursor-pointer ${
                isActive
                  ? `bg-gradient-to-b from-slate-800/80 ${style.bg} ${style.ring} shadow-lg scale-[1.02] ring-1 ${style.ring.replace('border-', 'ring-').replace('/40', '/30')}`
                  : isOwned 
                    ? 'bg-slate-900/60 border-slate-700 hover:-translate-y-1 hover:border-slate-600'
                    : 'bg-slate-900/40 border-slate-800 opacity-60 grayscale hover:grayscale-0 transition-all'
              } backdrop-blur-xl`}
            >
              <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_top,_var(--tw-gradient-stops))] from-white/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity" />
              
              <div className="relative mb-4">
                <div
                  className={`w-16 h-16 rounded-full flex items-center justify-center border-2 transition-all duration-500 ${
                    isActive
                      ? style.iconBg
                      : isOwned
                        ? 'border-slate-700 bg-slate-800 group-hover:border-slate-600'
                        : 'border-slate-800 bg-slate-900'
                  }`}
                >
                  {isActive ? (
                    <CheckCircle2 className={`w-8 h-8 ${style.iconText}`} />
                  ) : isOwned ? (
                    <Medal className="w-6 h-6 text-slate-500 group-hover:text-slate-400 transition-colors" />
                  ) : (
                    <Lock className="w-6 h-6 text-slate-600" />
                  )}
                </div>
              </div>

              <h3
                className={`text-sm font-bold mb-1 ${
                  isActive ? style.textTitle : isOwned ? 'text-slate-300 group-hover:text-slate-200' : 'text-slate-500'
                }`}
              >
                {localize(title.nameVi, title.nameEn)}
              </h3>
              
              <p className={`text-xs ${isActive ? style.text : 'text-slate-400'} line-clamp-3 leading-relaxed mt-1 opacity-80`}>
                {isOwned ? localize(title.descriptionVi, title.descriptionEn) : t('HiddenAchievement', { defaultMessage: 'Chưa mở khoá' })}
              </p>

              <div className="mt-auto pt-4 text-[10px] uppercase font-semibold tracking-wider text-slate-500">
                {title.rarity}
              </div>
            </div>
          );
        })}
      </div>

      <GamificationDetailModal 
        isOpen={!!selectedTitle}
        onClose={() => setSelectedTitle(null)}
        type="title"
        titleData={selectedTitle || undefined}
        onEquipTitle={handleEquipTitle}
      />
    </div>
  );
}
