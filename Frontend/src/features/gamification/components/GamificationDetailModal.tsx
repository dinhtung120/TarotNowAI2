'use client';

/**
 * ===================================================================
 * COMPONENT: GamificationDetailModal
 * BỐI CẢNH (CONTEXT):
 *   Hiển thị chi tiết về một Nhiệm vụ hoặc Thành tựu khi người dùng click vào thẻ (Card).
 *   Bao gồm: Tên, Mô tả, Điều kiện hoàn thành, Danh hiệu đi kèm và Phần thưởng.
 * 
 * ĐẶC ĐIỂM NỔI BẬT:
 *   - Sử dụng Modal dùng chung của hệ thống.
 *   - Hiệu ứng Gradient sống động, Icon tương ứng với từng loại phần thưởng.
 *   - Nút "Nhận thưởng" tích hợp trực tiếp cho Nhiệm vụ.
 * ===================================================================
 */

import { useTranslations } from 'next-intl';
import Modal from '@/shared/components/ui/Modal';
import type { QuestWithProgress, AchievementDefinition, UserAchievement, TitleDefinition } from '../gamification.types';
import { cn } from '@/shared/utils/cn';
import { useLocalizedField } from '../useLocalizedField';
import { GamificationDetailHeader } from './GamificationDetailHeader';
import { GamificationDetailBody } from './GamificationDetailBody';
import { GamificationDetailActions } from './GamificationDetailActions';

interface GamificationDetailModalProps {
  isOpen: boolean;
  onClose: () => void;
  type: 'quest' | 'achievement' | 'title';
  questData?: QuestWithProgress;
  achievementData?: {
    definition: AchievementDefinition;
    unlockedInfo?: UserAchievement;
  };
  titleData?: {
    definition: TitleDefinition;
    isOwned: boolean;
    isActive: boolean;
    grantedAt?: string;
  };
  onClaim?: (questCode: string, periodKey: string) => void;
  onEquipTitle?: (titleCode: string) => void;
  isClaiming?: boolean;
}

export default function GamificationDetailModal({
  isOpen,
  onClose,
  type,
  questData,
  achievementData,
  titleData,
  onClaim,
  onEquipTitle,
  isClaiming,
}: GamificationDetailModalProps) {
  const t = useTranslations('Gamification');
  const { localize } = useLocalizedField();

  if (!isOpen) return null;

  const isQuest = type === 'quest' && !!questData;
  const isAchievement = type === 'achievement' && !!achievementData;
  const isTitle = type === 'title' && !!titleData;

  const title = isQuest 
    ? localize(questData.definition.titleVi, questData.definition.titleEn) 
    : isAchievement 
      ? localize(achievementData.definition.titleVi, achievementData.definition.titleEn) 
      : isTitle
        ? localize(titleData.definition.nameVi, titleData.definition.nameEn)
        : '';
      
  const description = isQuest 
    ? localize(questData.definition.descriptionVi, questData.definition.descriptionEn) 
    : isAchievement 
      ? localize(achievementData.definition.descriptionVi, achievementData.definition.descriptionEn) 
      : isTitle
        ? localize(titleData.definition.descriptionVi, titleData.definition.descriptionEn)
        : '';
  
  const questProgress = isQuest ? questData.progress : null;
  const current = questProgress?.currentProgress || 0;
  const target = isQuest ? questData.definition.target : 1;
  const isCompleted = isQuest ? current >= target : isAchievement ? !!achievementData?.unlockedInfo : !!titleData?.isOwned;
  const isClaimed = questProgress?.isClaimed || false;

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="md">
      <div className="relative overflow-hidden -m-6 p-8">
        {/* === Background Gradients === */}
        <div className={cn(
          "absolute top-0 left-0 w-full h-32 opacity-20",
          isQuest ? "bg-gradient-to-b from-indigo-500 to-transparent" : "bg-gradient-to-b from-amber-500 to-transparent"
        )} />

        <GamificationDetailHeader
          isQuest={isQuest}
          isCompleted={isCompleted}
          title={title}
          completedLabel={t('Completed')}
          pendingLabel={t('Pending')}
        />

        <GamificationDetailBody
          isQuest={isQuest}
          isAchievement={isAchievement}
          isTitle={isTitle}
          description={description}
          completionConditionsLabel={t('CompletionConditions')}
          descriptionLabel={t('SelectATitleToDisplay', { defaultMessage: 'Description' })}
          howToUnlockLabel={t('HowToUnlock')}
          progressLabel={t('Progress')}
          rewardsLabel={t('Rewards')}
          unlockedAtLabel={t('UnlockedAt') || 'Mở khoá vào'}
          current={current}
          target={target}
          isCompleted={isCompleted}
          questData={questData}
          unlockedInfo={achievementData?.unlockedInfo}
          titleGrantedAt={titleData?.grantedAt}
          rewardTypeLabel={(rewardType) => t(rewardType)}
        />

        <GamificationDetailActions
          showClaimButton={isQuest && isCompleted && !isClaimed}
          showEquipButton={isTitle && titleData.isOwned && !titleData.isActive}
          onClose={onClose}
          onClaim={() => {
            if (questData && questProgress) {
              onClaim?.(questData.definition.code, questProgress.periodKey);
            }
          }}
          onEquip={() => {
            if (titleData) {
              onEquipTitle?.(titleData.definition.code);
              onClose();
            }
          }}
          isClaiming={isClaiming}
          closeLabel={t('Close')}
          claimLabel={t('ClaimReward')}
          equipLabel={t('ActiveTitle', { defaultMessage: 'Sử dụng' })}
        />
      </div>
    </Modal>
  );
}
