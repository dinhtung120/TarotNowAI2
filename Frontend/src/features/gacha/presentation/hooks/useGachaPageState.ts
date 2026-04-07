'use client';

import { useCallback, useState } from 'react';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import type { SpinGachaResult } from '@/features/gacha/gacha.types';
import { useGachaBanners, useSpinGacha } from '@/features/gacha/hooks/useGacha';

export function useGachaPageState() {
  const t = useTranslations('gacha');
  const tCommon = useTranslations('common');
  const { data: banners, isLoading, isError, error } = useGachaBanners();
  const spinMutation = useSpinGacha();
  const [spinResult, setSpinResult] = useState<SpinGachaResult | null>(null);
  const [isRevealOpen, setIsRevealOpen] = useState(false);
  const [isHistoryOpen, setIsHistoryOpen] = useState(false);
  const [optimisticPity, setOptimisticPity] = useState<Record<string, number>>({});

  const handleSpin = useCallback(
    async (bannerCode: string, count: number) => {
      try {
        const result = await spinMutation.mutateAsync({ bannerCode, count });
        if (!result) return;
        setSpinResult(result as SpinGachaResult);
        setIsRevealOpen(true);
        setOptimisticPity((prev) => ({ ...prev, [bannerCode]: result.currentPityCount }));
      } catch (spinError: unknown) {
        toast.error(spinError instanceof Error ? spinError.message : tCommon('error_unknown'));
      }
    },
    [spinMutation, tCommon]
  );

  const openHistory = useCallback(() => setIsHistoryOpen(true), []);
  const closeHistory = useCallback(() => setIsHistoryOpen(false), []);
  const closeReveal = useCallback(() => setIsRevealOpen(false), []);
  const retryLoad = useCallback(() => window.location.reload(), []);

  return {
    t,
    banners,
    isLoading,
    isError,
    error,
    spinMutation,
    spinResult,
    isRevealOpen,
    isHistoryOpen,
    optimisticPity,
    handleSpin,
    openHistory,
    closeHistory,
    closeReveal,
    retryLoad,
  };
}
