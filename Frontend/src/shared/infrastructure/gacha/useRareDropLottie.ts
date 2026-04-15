'use client';

import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { PullGachaReward } from '@/shared/infrastructure/gacha/gachaTypes';
import { gachaRewardKinds } from '@/shared/infrastructure/gacha/gachaConstants';

const rarityPriority: Record<string, number> = {
  mythic: 3,
  legendary: 2,
  epic: 1,
};

const rarityLottieMap: Record<string, string> = {
  mythic: '/lottie/mythic-drop.json',
  legendary: '/lottie/legendary-drop.json',
  epic: '/lottie/epic-drop.json',
};

function normalizeRarity(rarity: string): string {
  return rarity.trim().toLowerCase();
}

function normalizeKind(kind: string): string {
  return kind.trim().toLowerCase();
}

export function useRareDropLottie(rewards: PullGachaReward[]) {
  const animationUrl = useMemo(() => {
    const rareItemRarity = rewards
      .filter((reward) => normalizeKind(reward.kind) === gachaRewardKinds.item)
      .map((reward) => normalizeRarity(reward.rarity))
      .filter((rarity) => rarity in rarityPriority)
      .sort((left, right) => rarityPriority[right] - rarityPriority[left])[0];

    return rareItemRarity ? rarityLottieMap[rareItemRarity] : '';
  }, [rewards]);

  const animationQuery = useQuery({
    queryKey: ['gacha', 'lottie', animationUrl],
    queryFn: async () => {
      const response = await fetch(animationUrl, { cache: 'no-store' });
      if (!response.ok) {
        return null;
      }

      return (await response.json()) as object;
    },
    enabled: Boolean(animationUrl),
    retry: false,
    staleTime: 300_000,
  });

  return {
    animationData: animationQuery.data ?? null,
    hasRareDrop: Boolean(animationUrl),
  };
}
