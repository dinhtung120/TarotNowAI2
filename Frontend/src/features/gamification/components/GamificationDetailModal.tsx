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
import { Trophy, Gift, Coins, Diamond, Target, Award, Check, Clock, X } from 'lucide-react';
import Modal from '@/shared/components/ui/Modal';
import type { QuestWithProgress, AchievementDefinition, UserAchievement, TitleDefinition } from '../gamification.types';
import { cn } from '@/shared/utils/cn';
import { useLocalizedField } from '../useLocalizedField';

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

  const isQuest = type === 'quest' && questData;
  const isAchievement = type === 'achievement' && achievementData;
  const isTitle = type === 'title' && titleData;

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
        
        {/* === Header Icon === */}
        <div className="relative flex flex-col items-center text-center mb-8">
          <div className={cn(
            "w-20 h-20 rounded-3xl flex items-center justify-center mb-4 transition-transform duration-500 hover:scale-110 shadow-2xl",
            isQuest 
              ? "bg-gradient-to-br from-indigo-500 to-purple-600 shadow-indigo-500/20" 
              : "bg-gradient-to-br from-amber-400 to-orange-600 shadow-amber-500/20",
            !isCompleted && "grayscale opacity-50"
          )}>
            {isQuest ? (
              <Trophy className="w-10 h-10 text-white" />
            ) : (
              <Award className="w-10 h-10 text-white" />
            )}
          </div>
          
          <h2 className="text-2xl font-black text-slate-100 mb-2 tracking-tight">
            {title}
          </h2>
          <div className={cn(
            "px-3 py-1 rounded-full text-[10px] font-bold uppercase tracking-widest border",
            isCompleted 
              ? "bg-green-500/10 text-green-400 border-green-500/20" 
              : "bg-slate-500/10 text-slate-400 border-slate-500/20"
          )}>
            {isCompleted ? t('Completed') : t('Pending')}
          </div>
        </div>

        {/* === Description Section === */}
        <div className="space-y-6 relative z-10">
          <section className="bg-slate-900/40 rounded-2xl p-5 border border-slate-700/50">
            <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-3 flex items-center gap-2">
              <Clock className="w-3.5 h-3.5" />
              {isQuest ? t('CompletionConditions') : isTitle ? t('SelectATitleToDisplay', { defaultMessage: 'Description' }) : t('HowToUnlock')}
            </h3>
            <p className="text-slate-200 text-sm leading-relaxed font-medium">
              {description}
            </p>
            
            {isQuest && (
               <div className="mt-4 pt-4 border-t border-slate-700/30">
                  <div className="flex justify-between items-end mb-2">
                    <span className="text-xs text-slate-400 font-bold">{t('Progress')}</span>
                    <span className="text-sm font-black text-indigo-400">{current} / {target}</span>
                  </div>
                  <div className="h-2.5 w-full bg-slate-950/80 rounded-full overflow-hidden border border-slate-800">
                    <div 
                      className={cn(
                        "h-full rounded-full transition-all duration-1000 ease-out",
                        isCompleted ? "bg-gradient-to-r from-indigo-500 to-purple-500" : "bg-indigo-500"
                      )}
                      style={{ width: `${Math.min((current/target)*100, 100)}%` }}
                    />
                  </div>
               </div>
            )}
          </section>

          {/* === Rewards Section === */}
          {isQuest && questData.definition.rewards.length > 0 && (
            <section>
              <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-4 flex items-center gap-2 px-1">
                <Gift className="w-3.5 h-3.5" />
                {t('Rewards')}
              </h3>
              <div className="grid grid-cols-2 gap-3">
                {questData.definition.rewards.map((reward, index) => (
                  <div 
                    key={index}
                    className="bg-slate-900/60 p-4 rounded-2xl border border-slate-700/50 flex flex-col items-center text-center group hover:border-indigo-500/30 transition-colors"
                  >
                    <div className="w-10 h-10 rounded-xl bg-slate-800 flex items-center justify-center mb-2 group-hover:scale-110 transition-transform">
                      {reward.type.toLowerCase() === 'gold' ? (
                        <Coins className="w-5 h-5 text-yellow-500" />
                      ) : reward.type.toLowerCase() === 'diamond' ? (
                        <Diamond className="w-5 h-5 text-cyan-400" />
                      ) : (
                        <Award className="w-5 h-5 text-purple-400" />
                      )}
                    </div>
                    <span className="text-sm font-black text-slate-100">+{reward.amount}</span>
                    <span className="text-[10px] font-bold text-slate-500 uppercase">{t(reward.type)}</span>
                  </div>
                ))}
              </div>
            </section>
          )}

          {/* === Unlock Info for Achievements === */}
          {isAchievement && achievementData.unlockedInfo && (
            <section className="bg-amber-500/5 rounded-2xl p-4 border border-amber-500/20 flex items-center gap-3">
              <Award className="w-8 h-8 text-amber-500/50" />
              <div className="text-xs">
                <p className="text-amber-200/50 font-bold uppercase tracking-wider">{t('UnlockedAt') || 'Mở khoá vào'}</p>
                <p className="text-amber-200 font-black text-sm">{new Date(achievementData.unlockedInfo.unlockedAt).toLocaleDateString()}</p>
              </div>
            </section>
          )}

          {/* === Unlock Info for Titles === */}
          {isTitle && titleData.grantedAt && (
            <section className="bg-amber-500/5 rounded-2xl p-4 border border-amber-500/20 flex items-center gap-3">
              <Award className="w-8 h-8 text-amber-500/50" />
              <div className="text-xs">
                <p className="text-amber-200/50 font-bold uppercase tracking-wider">{t('UnlockedAt') || 'Mở khoá vào'}</p>
                <p className="text-amber-200 font-black text-sm">{new Date(titleData.grantedAt).toLocaleDateString()}</p>
              </div>
            </section>
          )}
        </div>

        {/* === Footer Actions === */}
        <div className="mt-10 flex gap-3">
          <button
            onClick={onClose}
            className="flex-1 py-3.5 rounded-2xl bg-slate-800 text-slate-300 font-bold hover:bg-slate-700 transition-colors border border-slate-700/50"
          >
            {t('Close')}
          </button>
          
          {isQuest && isCompleted && !isClaimed && (
            <button
              onClick={() => onClaim?.(questData.definition.code, questProgress!.periodKey)}
              disabled={isClaiming}
              className="flex-[2] py-3.5 rounded-2xl bg-gradient-to-r from-indigo-500 to-purple-600 text-white font-black hover:scale-[1.02] active:scale-95 transition-all shadow-lg shadow-indigo-500/25 flex items-center justify-center gap-2"
            >
              {isClaiming ? (
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
              ) : (
                <>
                  <Check className="w-5 h-5" />
                  {t('ClaimReward')}
                </>
              )}
            </button>
          )}
          
          {isTitle && titleData.isOwned && !titleData.isActive && (
            <button
              onClick={() => {
                if (titleData.definition.code) {
                  // @ts-ignore
                  onEquipTitle?.(titleData.definition.code);
                  onClose();
                }
              }}
              className="flex-[2] py-3.5 rounded-2xl bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-black hover:scale-[1.02] active:scale-95 transition-all shadow-lg shadow-blue-500/25 flex items-center justify-center gap-2"
            >
              <Check className="w-5 h-5" />
              {t('ActiveTitle', { defaultMessage: 'Sử dụng' })}
            </button>
          )}
        </div>
      </div>
    </Modal>
  );
}
