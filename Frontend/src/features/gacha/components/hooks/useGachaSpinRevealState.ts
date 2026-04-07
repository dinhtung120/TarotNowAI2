'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import type { SpinGachaResult } from '@/features/gacha/gacha.types';

type RevealPhase = 'closed' | 'firing' | 'revealed';

const FireDelayMs = 0;
const RevealDelayMs = 2000;

interface UseGachaSpinRevealStateArgs {
  isOpen: boolean;
  result: SpinGachaResult | null;
}

export function useGachaSpinRevealState({ isOpen, result }: UseGachaSpinRevealStateArgs) {
  const t = useTranslations('gacha');
  const [phase, setPhase] = useState<RevealPhase>('closed');

  useEffect(() => {
    let phaseTimer: number | null = null;
    let revealTimer: number | null = null;

    if (isOpen && result) {
      phaseTimer = window.setTimeout(() => setPhase('firing'), FireDelayMs);
      revealTimer = window.setTimeout(() => setPhase('revealed'), RevealDelayMs);
    } else {
      phaseTimer = window.setTimeout(() => setPhase('closed'), FireDelayMs);
    }

    return () => {
      if (phaseTimer !== null) window.clearTimeout(phaseTimer);
      if (revealTimer !== null) window.clearTimeout(revealTimer);
    };
  }, [isOpen, result]);

  return {
    t,
    phase,
    isVi: t('lang') === 'vi',
  };
}
