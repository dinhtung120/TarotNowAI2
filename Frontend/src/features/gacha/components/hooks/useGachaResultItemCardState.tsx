'use client';

import { useCallback, useState } from 'react';
import type { SpinGachaItemResult } from '@/features/gacha/gacha.types';
import { GachaRewardIcon } from '@/features/gacha/components/result-card/GachaRewardIcon';
import { RARITY_CONFIG } from '@/features/gacha/components/result-card/rarityConfig';

interface UseGachaResultItemCardStateArgs {
  item: SpinGachaItemResult;
  isVi: boolean;
}

export function useGachaResultItemCardState({ item, isVi }: UseGachaResultItemCardStateArgs) {
  const [iconLoadFailed, setIconLoadFailed] = useState(false);
  const name = isVi ? item.displayNameVi : item.displayNameEn;
  const config = RARITY_CONFIG[item.rarity.toLowerCase()] || RARITY_CONFIG.common;

  const renderRewardIcon = useCallback(
    (sizeClass: string) => (
      <GachaRewardIcon
        iconLoadFailed={iconLoadFailed}
        item={item}
        name={name}
        onError={() => setIconLoadFailed(true)}
        sizeClass={sizeClass}
        textClass={config.text}
      />
    ),
    [config.text, iconLoadFailed, item, name]
  );

  return {
    name,
    config,
    renderRewardIcon,
  };
}
