'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { Medal } from 'lucide-react';
import { toast } from 'react-hot-toast';
import { useTitles, useSetActiveTitle } from '../useGamification';
import type { TitleDefinition, UserTitle } from '../gamification.types';
import { useLocalizedField } from '../useLocalizedField';
import GamificationDetailModal from './GamificationDetailModal';
import { NoTitleCard } from './NoTitleCard';
import { TitleCard } from './TitleCard';

export default function TitleSelector() {
  const t = useTranslations('Gamification');
  const { data, isLoading } = useTitles();
  const setMutation = useSetActiveTitle();
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
        <NoTitleCard
          isNoTitleActive={!data.activeTitleCode}
          noTitleLabel={t('NoTitle')}
          hintLabel={t('EarnTitleHint')}
          onClick={() => handleEquipTitle('')}
        />

        {/* Danh sách các danh hiệu đã sở hữu */}
        {definitions.map((title: TitleDefinition) => {
          const unlockedInfo = unlockedList.find((u: UserTitle) => u.titleCode === title.code);
          const isOwned = !!unlockedInfo;
          const isActive = isOwned && title.code === activeTitleCode;

          return (
            <TitleCard
              key={title.code}
              title={title}
              isOwned={isOwned}
              isActive={isActive}
              localize={localize}
              hiddenLabel={t('HiddenAchievement', { defaultMessage: 'Chưa mở khoá' })}
              onClick={() => setSelectedTitle({ definition: title, isOwned, isActive, grantedAt: unlockedInfo?.grantedAt })}
            />
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
