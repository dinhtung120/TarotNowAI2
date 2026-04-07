'use client';

import type { SpinGachaItemResult } from '../gacha.types';
import { GachaMultiResultLayout } from './result-card/GachaMultiResultLayout';
import { GachaSingleResultLayout } from './result-card/GachaSingleResultLayout';
import { useGachaResultItemCardState } from '@/features/gacha/components/hooks/useGachaResultItemCardState';

interface GachaResultItemCardProps {
  item: SpinGachaItemResult;
  isSingle: boolean;
  isVi: boolean;
}

export function GachaResultItemCard({ item, isSingle, isVi }: GachaResultItemCardProps) {
  const vm = useGachaResultItemCardState({ item, isVi });

  if (isSingle) {
   return <GachaSingleResultLayout borderClass={vm.config.border} glowClass={`${vm.config.glow} ${vm.config.bg}`} name={vm.name} rarity={item.rarity} rarityStyle={vm.config} renderRewardIcon={vm.renderRewardIcon} />;
  }

  return <GachaMultiResultLayout borderClass={`${vm.config.border} ${vm.config.bg}`} glowClass={vm.config.glow} name={vm.name} rarityInitial={item.rarity[0]} rarityStyle={vm.config} renderRewardIcon={vm.renderRewardIcon} />;
}
