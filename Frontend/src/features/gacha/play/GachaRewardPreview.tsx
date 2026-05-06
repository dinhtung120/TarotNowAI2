'use client';

import { memo } from 'react';
import Image from 'next/image';
import { cn, formatCardStat } from '@/lib/utils';
import { gachaRewardKinds } from '@/features/gacha/shared/gachaConstants';
import type { GachaPoolRewardRate } from '@/features/gacha/shared/gachaTypes';
import { shouldUseUnoptimizedImage } from '@/shared/http/assetUrl';
import Badge from '@/shared/ui/Badge';

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
    return (
      <div className={cn('rounded-3xl border border-dashed tn-border-soft p-10 text-center')}>
        <p className={cn('tn-text-muted text-sm font-medium italic opacity-60')}>{emptyLabel}</p>
      </div>
    );
  }

  return (
    <div className={cn('grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3')}>
      {rewards.map((reward, index) => {
        const isRare = reward.rarity.toString().includes('5');
        const isEpic = reward.rarity.toString().includes('4');

        return (
          <article key={`${reward.kind}_${reward.rarity}_${reward.itemCode ?? reward.currency ?? index}`} className={cn('group relative flex items-center justify-between rounded-2xl border tn-border-soft bg-white/[0.02] p-4 transition-all duration-300 hover:border-white/10 hover:bg-white/[0.05] hover:shadow-[var(--shadow-card)]')}>
            <div className={cn('flex min-w-0 items-center gap-4')}>
              <div className={cn('relative h-12 w-12 shrink-0 overflow-hidden rounded-xl border tn-border-soft bg-slate-900/40 transition-transform duration-500 group-hover:scale-110', isRare ? 'border-amber-500/30' : isEpic ? 'border-purple-500/30' : '')}>
                {reward.iconUrl ? (
                  <Image
                    src={reward.iconUrl}
                    alt={localizeRewardName(reward, locale)}
                    fill
                    sizes="48px"
                    unoptimized={shouldUseUnoptimizedImage(reward.iconUrl)}
                    className={cn('object-cover p-1')}
                  />
                ) : (
                  <div className={cn('flex h-full w-full items-center justify-center text-xs opacity-20')}>✦</div>
                )}
              </div>

              <div className={cn('min-w-0')}>
                <p className={cn('mb-1 truncate text-sm font-black tracking-tight tn-text-primary')}>{localizeRewardName(reward, locale)}</p>
                <div className={cn('flex items-center gap-2')}>
                  <Badge variant={isRare ? 'amber' : isEpic ? 'purple' : 'default'} size="sm" className={cn('px-1.5 py-0')}>
                    {reward.rarity}★
                  </Badge>
                  <span className={cn('tn-text-muted text-[10px] font-bold uppercase tracking-widest opacity-60')}>
                    {rewardValueLabel(reward)}
                  </span>
                </div>
              </div>
            </div>

            <div className={cn('pl-4 text-right')}>
              <p className={cn('text-sm font-black tracking-tight', isRare ? 'tn-text-warning' : 'tn-text-primary')}>
                {formatCardStat(reward.probabilityPercent)}%
              </p>
              <p className={cn('tn-text-muted text-[9px] font-bold uppercase tracking-[0.1em] opacity-50')}>Xác suất</p>
            </div>
          </article>
        );
      })}
    </div>
  );
}

const GachaRewardPreview = memo(GachaRewardPreviewComponent);
GachaRewardPreview.displayName = 'GachaRewardPreview';

export default GachaRewardPreview;
