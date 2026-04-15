'use client';

import { memo } from 'react';
import Lottie from 'lottie-react';
import Modal from '@/shared/components/ui/Modal';
import Button from '@/shared/components/ui/Button';
import { cn } from '@/lib/utils';
import type { PullGachaResult, PullGachaReward } from '@/shared/infrastructure/gacha/gachaTypes';
import { useRareDropLottie } from '@/shared/infrastructure/gacha/useRareDropLottie';

interface GachaResultModalLabels {
  title: string;
  pityTriggered: string;
  close: string;
  rareDropAnimation: string;
}

interface GachaResultModalProps {
  isOpen: boolean;
  locale: string;
  result: PullGachaResult | null;
  labels: GachaResultModalLabels;
  onClose: () => void;
}

function localizeRewardName(reward: PullGachaReward, locale: string): string {
  if (locale === 'en') return reward.nameEn;
  if (locale === 'zh') return reward.nameZh;
  return reward.nameVi;
}

function rewardSummary(reward: PullGachaReward): string {
  if (reward.currency) {
    return `${reward.amount ?? 0} ${reward.currency.toUpperCase()}`;
  }

  return `x${reward.quantityGranted}`;
}

function GachaResultModalComponent({ isOpen, locale, result, labels, onClose }: GachaResultModalProps) {
  if (!result) return null;
  const { animationData } = useRareDropLottie(result.rewards);

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={labels.title} description={result.poolCode}>
      <div className={cn('space-y-4')}>
        {animationData ? (
          <div className={cn('rounded-xl border border-fuchsia-300/50 bg-fuchsia-50/60 p-2 dark:border-fuchsia-700/60 dark:bg-fuchsia-950/30')}>
            <p className={cn('mb-2 text-center text-xs font-semibold uppercase tracking-wide text-fuchsia-700 dark:text-fuchsia-200')}>
              {labels.rareDropAnimation}
            </p>
            <Lottie animationData={animationData} loop autoplay className={cn('mx-auto h-40 w-40')} />
          </div>
        ) : null}
        {result.wasPityTriggered ? (
          <p className={cn('rounded-lg bg-amber-100 px-3 py-2 text-xs font-semibold text-amber-800 dark:bg-amber-900/40 dark:text-amber-200')}>{labels.pityTriggered}</p>
        ) : null}
        <div className={cn('grid grid-cols-1 gap-3 sm:grid-cols-2')}>
          {result.rewards.map((reward, index) => (
            <article
              key={`${reward.kind}_${reward.itemCode ?? reward.currency ?? index}`}
              className={cn('rounded-xl border border-slate-200 bg-white p-3 dark:border-slate-700 dark:bg-slate-900')}
            >
              <p className={cn('text-sm font-semibold text-slate-900 dark:text-slate-100')}>{localizeRewardName(reward, locale)}</p>
              <p className={cn('mt-1 text-xs uppercase text-slate-500 dark:text-slate-300')}>{reward.rarity}</p>
              <p className={cn('mt-2 text-sm text-slate-700 dark:text-slate-200')}>{rewardSummary(reward)}</p>
            </article>
          ))}
        </div>
        <Button className={cn('w-full')} onClick={onClose}>
          {labels.close}
        </Button>
      </div>
    </Modal>
  );
}

const GachaResultModal = memo(GachaResultModalComponent);
GachaResultModal.displayName = 'GachaResultModal';

export default GachaResultModal;
