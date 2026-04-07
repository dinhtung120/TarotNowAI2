'use client';

import { useState } from 'react';
import type { SpinGachaItemResult } from '../gacha.types';
import { GachaMultiResultLayout } from './result-card/GachaMultiResultLayout';
import { GachaRewardIcon } from './result-card/GachaRewardIcon';
import { GachaSingleResultLayout } from './result-card/GachaSingleResultLayout';
import { RARITY_CONFIG } from './result-card/rarityConfig';

interface GachaResultItemCardProps {
  item: SpinGachaItemResult;
  isSingle: boolean;
  isVi: boolean;
}

export function GachaResultItemCard({ item, isSingle, isVi }: GachaResultItemCardProps) {
  const [iconLoadFailed, setIconLoadFailed] = useState(false);
  const name = isVi ? item.displayNameVi : item.displayNameEn;
  const config = RARITY_CONFIG[item.rarity.toLowerCase()] || RARITY_CONFIG.common;

  const renderRewardIcon = (sizeClass: string) => {
   return (
    <GachaRewardIcon
     iconLoadFailed={iconLoadFailed}
     item={item}
     name={name}
     onError={() => setIconLoadFailed(true)}
     sizeClass={sizeClass}
     textClass={config.text}
    />
   );
  };

  if (isSingle) {
   return <GachaSingleResultLayout borderClass={config.border} glowClass={`${config.glow} ${config.bg}`} name={name} rarity={item.rarity} rarityStyle={config} renderRewardIcon={renderRewardIcon} />;
  }

  return <GachaMultiResultLayout borderClass={`${config.border} ${config.bg}`} glowClass={config.glow} name={name} rarityInitial={item.rarity[0]} rarityStyle={config} renderRewardIcon={renderRewardIcon} />;
}
