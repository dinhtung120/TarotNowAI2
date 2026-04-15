'use client';

import { memo } from 'react';
import { Coins, Gem, Package } from 'lucide-react';
import Image from 'next/image';
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

function RewardIcon({ reward, locale }: { reward: PullGachaReward; locale: string }) {
  if (reward.iconUrl) {
    return (
      <Image
        src={reward.iconUrl}
        alt={localizeRewardName(reward, locale)}
        width={48}
        height={48}
        unoptimized
        className={cn('h-12 w-12 rounded-lg object-cover')}
      />
    );
  }

  if (reward.currency?.toLowerCase() === 'gold') {
    return <Coins className={cn('h-6 w-6 text-amber-500')} aria-hidden="true" />;
  }

  if (reward.currency?.toLowerCase() === 'diamond') {
    return <Gem className={cn('h-6 w-6 text-cyan-500')} aria-hidden="true" />;
  }

  return <Package className={cn('h-6 w-6 text-violet-500')} aria-hidden="true" />;
}

function GachaResultModalComponent({ isOpen, locale, result, labels, onClose }: GachaResultModalProps) {
  const { animationData, hasRareDrop } = useRareDropLottie(result?.rewards ?? []);
  if (!result) return null;

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
        {hasRareDrop && !animationData ? (
          <div className={cn('rounded-xl border border-fuchsia-300/50 bg-fuchsia-50/60 p-4 dark:border-fuchsia-700/60 dark:bg-fuchsia-950/30')}>
            <p className={cn('text-center text-xs font-semibold uppercase tracking-wide text-fuchsia-700 dark:text-fuchsia-200')}>
              {labels.rareDropAnimation}
            </p>
            <div className={cn('mx-auto mt-3 h-3 w-32 rounded-full bg-gradient-to-r from-fuchsia-400 via-amber-300 to-cyan-400 animate-pulse')} />
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
              <div className={cn('flex items-start gap-3')}>
                <div className={cn('flex h-12 w-12 shrink-0 items-center justify-center rounded-lg bg-slate-100 dark:bg-slate-800')}>
                  <RewardIcon reward={reward} locale={locale} />
                </div>
                <div className={cn('min-w-0')}>
                  <p className={cn('truncate text-sm font-semibold text-slate-900 dark:text-slate-100')}>{localizeRewardName(reward, locale)}</p>
                  <p className={cn('mt-1 text-xs uppercase text-slate-500 dark:text-slate-300')}>{reward.rarity}</p>
                  <p className={cn('mt-2 text-sm text-slate-700 dark:text-slate-200')}>{rewardSummary(reward)}</p>
                </div>
              </div>
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
