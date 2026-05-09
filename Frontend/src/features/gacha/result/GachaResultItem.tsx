'use client';

import { memo, useState } from 'react';
import Image from 'next/image';
import { cn } from '@/lib/utils';
import type { PullGachaReward } from '@/features/gacha/shared/gachaTypes';
import { shouldUseUnoptimizedImage } from '@/shared/http/assetUrl';
import {
 gachaResultRarityConfig,
 resolveGachaResultName,
 resolveGachaResultSummary,
 RewardFallbackIcon,
} from '@/features/gacha/result/gachaResultItemView';

interface GachaResultItemProps {
  reward: PullGachaReward;
  locale: string;
}

function GachaResultItemComponent({ reward, locale }: GachaResultItemProps) {
  const [imageError, setImageError] = useState(false);
  
  const normalizedRarity = String(reward.rarity).toLowerCase();
  const config = gachaResultRarityConfig[normalizedRarity] || { shadow: '', border: 'tn-border-soft', bg: 'bg-white/[0.02]', text: 'tn-text-primary' };
  const name = resolveGachaResultName(reward, locale);
  const summary = resolveGachaResultSummary(reward);
  const unoptimizedRewardImage = shouldUseUnoptimizedImage(reward.iconUrl);

  return (
    <article className={cn(
      'group relative flex items-center gap-4 rounded-3xl border p-4 transition-all duration-500 overflow-hidden',
      config.border,
      config.bg,
      config.shadow,
      'hover:bg-white/[0.05] hover:scale-[1.02]'
    )}>
      <div className={cn(
        'relative flex h-16 w-16 shrink-0 items-center justify-center rounded-2xl border tn-border-soft bg-black/20 overflow-hidden',
        config.border
      )}>
        {reward.iconUrl && !imageError ? (
          <Image
            src={reward.iconUrl}
            alt={name}
            fill
            className={cn("object-cover transition-transform duration-700 group-hover:scale-125")}
            onError={() => setImageError(true)}
            unoptimized={unoptimizedRewardImage}
            loading="lazy"
            sizes="64px"
          />
        ) : <RewardFallbackIcon reward={reward} />}
      </div>

      <div className={cn("min-w-0 flex-1 space-y-1")}>
        <h4 className={cn('truncate text-sm font-black tracking-tight tn-text-primary')}>
          {name}
        </h4>
        <div className={cn("flex items-center gap-2")}>
          <span className={cn('text-[10px] font-black uppercase tracking-widest', config.text)}>
            {reward.rarity}
          </span>
          <span className={cn("h-1 w-1 rounded-full bg-white/20")} />
          <span className={cn("tn-text-secondary text-[11px] font-bold")}>
            {summary}
          </span>
        </div>
      </div>
      <div className={cn("absolute inset-x-0 bottom-0 h-[1px] bg-gradient-to-r from-transparent via-current to-transparent opacity-0 transition-opacity duration-500 group-hover:opacity-30")} />
    </article>
  );
}

export const GachaResultItem = memo(GachaResultItemComponent);
