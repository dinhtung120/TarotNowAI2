'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { Gift } from 'lucide-react';
import { toast } from 'react-hot-toast';
import { useQueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { useQuests, useClaimQuestReward } from '../useGamification';
import type { QuestWithProgress } from '../gamification.types';
import GamificationDetailModal from './GamificationDetailModal';
import { useLocalizedField } from '../useLocalizedField';
import { QuestCard } from './QuestCard';
import { QuestPanelHeader } from './QuestPanelHeader';

export default function QuestsPanel() {
  const t = useTranslations('Gamification');
  const queryClient = useQueryClient();
  const [questType, setQuestType] = useState<'daily' | 'weekly'>('daily');
  const { data: quests, isLoading, isError } = useQuests(questType);
  const claimMutation = useClaimQuestReward();
  const { localize } = useLocalizedField();
  
  const [selectedQuest, setSelectedQuest] = useState<QuestWithProgress | null>(null);
  const getClaimErrorMessage = (error: unknown): string => {
    if (error instanceof Error && error.message) {
      return error.message;
    }

    if (typeof error === 'object' && error !== null) {
      const maybeHttpError = error as {
        response?: {
          data?: {
            message?: string;
          };
        };
      };

      if (maybeHttpError.response?.data?.message) {
        return maybeHttpError.response.data.message;
      }
    }

    return t('ClaimFailed');
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-12">
        <div className="w-10 h-10 border-4 border-indigo-500 border-t-transparent rounded-full animate-spin"></div>
      </div>
    );
  }

  if (isError || !quests) {
    return (
      <div className="text-center text-red-400 p-6 bg-red-500/10 rounded-2xl border border-red-500/20 backdrop-blur-md">
        <p>{t('FailedToLoadQuests')}</p>
      </div>
    );
  }

  const handleClaim = (questCode: string, periodKey: string) => {
    claimMutation.mutate(
      { questCode, periodKey },
      {
        onSuccess: (res) => {
          if (res.success) {
            // Invalidate queries to refresh balance and quest status immediately
            queryClient.invalidateQueries({ queryKey: ['wallet', 'balance'] });
            queryClient.invalidateQueries({ queryKey: ['gamification', 'quests'] });
            
            // Re-fetch wallet store balance for real-time UI update (Zustand)
            useWalletStore.getState().fetchBalance();
            
            toast.success(
              <div className="flex items-center gap-2">
                <Gift className="text-yellow-400 w-5 h-5" />
                <span>
                  {t('ClaimedSuccessfully', { amount: res.rewardAmount, type: res.rewardType })}
                </span>
              </div>,
              {
                style: {
                  borderRadius: '16px',
                  background: 'rgba(30, 41, 59, 0.9)',
                  color: '#fff',
                  border: '1px solid rgba(255, 255, 255, 0.1)',
                  backdropFilter: 'blur(10px)',
                },
              }
            );
          }
        },
        onError: (err: unknown) => {
          toast.error(getClaimErrorMessage(err));
        },
      }
    );
  };

  return (
    <div className="space-y-6">
      <QuestPanelHeader questType={questType} setQuestType={setQuestType} t={t} />

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {quests.map((q: QuestWithProgress) => {
          return (
            <QuestCard
              key={q.definition.code}
              quest={q}
              isClaimPending={claimMutation.isPending}
              localize={localize}
              t={t}
              onOpen={setSelectedQuest}
              onClaim={handleClaim}
            />
          );
        })}
      </div>

      <GamificationDetailModal 
        isOpen={!!selectedQuest}
        onClose={() => setSelectedQuest(null)}
        type="quest"
        questData={selectedQuest || undefined}
        onClaim={handleClaim}
        isClaiming={claimMutation.isPending}
      />
    </div>
  );
}
