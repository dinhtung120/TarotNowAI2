'use client';

import { useCallback, useMemo, useState } from 'react';
import { useTranslations } from 'next-intl';
import type { GachaBannerDto } from '@/features/gacha/gacha.types';

interface UseGachaBannerCardStateArgs {
  banner: GachaBannerDto;
  currentPity: number;
  hardPityCount: number;
}

export function useGachaBannerCardState({
  banner,
  currentPity,
  hardPityCount,
}: UseGachaBannerCardStateArgs) {
  const t = useTranslations('gacha');
  const [isOddsOpen, setIsOddsOpen] = useState(false);

  const isVi = t('lang') === 'vi';
  const name = (isVi ? banner.nameVi : banner.nameEn) || '';
  const description = (isVi ? banner.descriptionVi : banner.descriptionEn) || '';

  const pityPercentage = useMemo(() => {
    return Math.min(Math.max((currentPity / hardPityCount) * 100, 2), 100);
  }, [currentPity, hardPityCount]);

  const openOdds = useCallback(() => setIsOddsOpen(true), []);
  const closeOdds = useCallback(() => setIsOddsOpen(false), []);

  return {
    t,
    name,
    description,
    pityPercentage,
    isOddsOpen,
    openOdds,
    closeOdds,
  };
}
