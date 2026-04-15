'use client';

import { useEffect, useMemo, useState } from 'react';
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

export function useRareDropLottie(rewards: PullGachaReward[]) {
  const animationUrl = useMemo(() => {
    const rareItemRarity = rewards
      .filter((reward) => reward.kind === gachaRewardKinds.item)
      .map((reward) => normalizeRarity(reward.rarity))
      .filter((rarity) => rarity in rarityPriority)
      .sort((left, right) => rarityPriority[right] - rarityPriority[left])[0];

    return rareItemRarity ? rarityLottieMap[rareItemRarity] : '';
  }, [rewards]);

  const [animationData, setAnimationData] = useState<object | null>(null);

  useEffect(() => {
    let isMounted = true;
    if (!animationUrl) {
      setAnimationData(null);
      return () => {
        isMounted = false;
      };
    }

    fetch(animationUrl, { cache: 'no-store' })
      .then((response) => (response.ok ? response.json() : null))
      .then((payload) => {
        if (isMounted) {
          setAnimationData(payload);
        }
      })
      .catch(() => {
        if (isMounted) {
          setAnimationData(null);
        }
      });

    return () => {
      isMounted = false;
    };
  }, [animationUrl]);

  return { animationData };
}

