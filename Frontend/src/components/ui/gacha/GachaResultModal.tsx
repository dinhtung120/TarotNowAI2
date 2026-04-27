'use client';

import { memo, useCallback, useEffect, useState } from 'react';
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';
import type { PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';
import { useRareDropLottie } from '@/shared/infrastructure/gacha/useRareDropLottie';
import { GachaResultModalRevealStage } from '@/components/ui/gacha/GachaResultModalRevealStage';
import { GachaResultModalResultStage } from '@/components/ui/gacha/GachaResultModalResultStage';

const REVEAL_DELAY_MS = 650;
const REDUCED_MOTION_REVEAL_DELAY_MS = 150;

interface GachaResultModalLabels {
  title: string;
  pityTriggered: string;
  close: string;
  rareDropAnimation: string;
}

interface GachaResultModalProps {
  isOpen: boolean;
  locale: string;
  result: PullGachaResult | null;
  labels: GachaResultModalLabels;
  onClose: () => void;
}

function GachaResultModalComponent({ isOpen, locale, result, labels, onClose }: GachaResultModalProps) {
  const [stage, setStage] = useState<'REVEALING' | 'SHOW_RESULT'>('REVEALING');
  const { animationData } = useRareDropLottie(result?.rewards ?? []);

  const handleClose = useCallback(() => {
    setStage('REVEALING');
    onClose();
  }, [onClose]);

  useEffect(() => {
    if (!isOpen || stage !== 'REVEALING') return;

    const revealDelay = window.matchMedia('(prefers-reduced-motion: reduce)').matches
      ? REDUCED_MOTION_REVEAL_DELAY_MS
      : REVEAL_DELAY_MS;

    const timer = window.setTimeout(() => setStage('SHOW_RESULT'), revealDelay);
    return () => window.clearTimeout(timer);
  }, [isOpen, stage]);

  if (!result) return null;

  const isUltraRare = result.rewards.some((reward) => {
    const rarity = String(reward.rarity).toLowerCase();
    return rarity.includes('5') || rarity.includes('legendary') || rarity.includes('mythic');
  });

  return (
    <Modal isOpen={isOpen} onClose={handleClose} title="" size={stage === 'REVEALING' ? 'sm' : 'lg'} className={cn('transition-all duration-700')}>
      <div className={cn('relative flex min-h-[300px] flex-col items-center justify-center py-6')}>
        {stage === 'REVEALING' ? (
          <GachaResultModalRevealStage
            animationData={animationData}
            isUltraRare={isUltraRare}
            revealLabel={labels.rareDropAnimation}
          />
        ) : (
          <GachaResultModalResultStage labels={labels} locale={locale} result={result} onClose={handleClose} />
        )}
      </div>
    </Modal>
  );
}

const GachaResultModal = memo(GachaResultModalComponent);
GachaResultModal.displayName = 'GachaResultModal';

export default GachaResultModal;
