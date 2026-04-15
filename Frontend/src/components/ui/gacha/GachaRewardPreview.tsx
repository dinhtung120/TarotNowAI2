'use client';

import { memo } from 'react';
import Image from 'next/image';
import { cn } from '@/lib/utils';
import { gachaRewardKinds } from '@/shared/infrastructure/gacha/gachaConstants';
import type { GachaPoolRewardRate } from '@/shared/infrastructure/gacha/gachaTypes';

interface GachaRewardPreviewProps {
 rewards: GachaPoolRewardRate[];
 locale: string;
 emptyLabel: string;
}

function localizeRewardName(reward: GachaPoolRewardRate, locale: string): string {
 if (locale === 'en') return reward.nameEn;
 if (locale === 'zh') return reward.nameZh;
 return reward.nameVi;
}

function rewardValueLabel(reward: GachaPoolRewardRate): string {
 if (reward.kind === gachaRewardKinds.currency) {
  return `${reward.amount ?? 0} ${reward.currency?.toUpperCase() ?? ''}`.trim();
 }

 return `x${reward.quantityGranted}`;
}

function GachaRewardPreviewComponent({ rewards, locale, emptyLabel }: GachaRewardPreviewProps) {
 if (!rewards.length) {
  return <div className={cn('rounded-2xl border border-dashed border-slate-300 p-6 text-sm text-slate-600 dark:border-slate-700 dark:text-slate-300')}>{emptyLabel}</div>;
 }

 return (
  <div className={cn('grid grid-cols-1 gap-3 md:grid-cols-2')}>
   {rewards.map((reward, index) => (
    <article
     key={`${reward.kind}_${reward.rarity}_${reward.itemCode ?? reward.currency ?? index}`}
     className={cn('flex items-center justify-between rounded-xl border border-slate-200 bg-white px-3 py-2 dark:border-slate-700 dark:bg-slate-900')}
    >
     <div className={cn('flex min-w-0 items-center gap-3')}>
      <div className={cn('relative h-9 w-9 overflow-hidden rounded-lg bg-slate-100 dark:bg-slate-800')}>
       {reward.iconUrl ? (
        <Image
         src={reward.iconUrl}
         alt={localizeRewardName(reward, locale)}
         fill
         sizes="36px"
         className={cn('object-cover')}
        />
       ) : null}
      </div>
      <div className={cn('min-w-0')}>
       <p className={cn('truncate text-sm font-medium text-slate-900 dark:text-slate-100')}>{localizeRewardName(reward, locale)}</p>
       <p className={cn('text-xs uppercase tracking-wide text-slate-500 dark:text-slate-300')}>{reward.rarity}</p>
      </div>
     </div>
     <div className={cn('text-right')}>
      <p className={cn('text-xs font-semibold text-slate-800 dark:text-slate-100')}>{rewardValueLabel(reward)}</p>
      <p className={cn('text-xs text-slate-500 dark:text-slate-300')}>{reward.probabilityPercent.toFixed(2)}%</p>
     </div>
    </article>
   ))}
  </div>
 );
}

const GachaRewardPreview = memo(GachaRewardPreviewComponent);
GachaRewardPreview.displayName = 'GachaRewardPreview';

export default GachaRewardPreview;
