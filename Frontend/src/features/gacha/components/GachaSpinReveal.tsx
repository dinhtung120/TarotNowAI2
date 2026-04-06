"use client";

/*
 * ===================================================================
 * COMPONENT: GachaSpinReveal
 * ===================================================================
 * MỤC ĐÍCH: Hiệu ứng Mở Card/Phần thưởng (Gacha Animation Reveal).
 * Hỗ trợ x10 Pull hiển thị theo dạng Grid.
 */

import { useTranslations } from 'next-intl';
import type { SpinGachaResult } from '../gacha.types';
import Modal from '@/shared/components/ui/Modal';
import { useEffect, useState } from 'react';
import Button from '@/shared/components/ui/Button';
import { cn } from '@/shared/utils/cn';
import { GachaResultItemCard } from './GachaResultItemCard';

interface GachaSpinRevealProps {
  result: SpinGachaResult | null;
  isOpen: boolean;
  onClose: () => void;
}

export function GachaSpinReveal({ result, isOpen, onClose }: GachaSpinRevealProps) {
  const t = useTranslations('gacha');
  const [phase, setPhase] = useState<'closed' | 'firing' | 'revealed'>('closed');

  useEffect(() => {
    let phaseTimer: number | null = null;
    let revealTimer: number | null = null;

    if (isOpen && result) {
      phaseTimer = window.setTimeout(() => {
        setPhase('firing');
      }, 0);

      revealTimer = window.setTimeout(() => {
        setPhase('revealed');
      }, 2000); // 2s magic pulse then reveal
    } else {
      phaseTimer = window.setTimeout(() => {
        setPhase('closed');
      }, 0);
    }

    return () => {
      if (phaseTimer !== null) window.clearTimeout(phaseTimer);
      if (revealTimer !== null) window.clearTimeout(revealTimer);
    };
  }, [isOpen, result]);

  if (!result || !result.items || result.items.length === 0) return null;

  const isVi = t('lang') === 'vi';

  return (
    <Modal 
      isOpen={isOpen} 
      onClose={() => {
        if (phase === 'revealed') onClose();
      }}
      title={t('spinResult')}
      size="lg"
      showCloseButton={phase === 'revealed'}
    >
      <div className={cn(
        "flex flex-col items-center justify-center min-h-[400px]",
        result.items.length > 1 ? "w-full" : "max-w-2xl mx-auto"
      )}>
        {phase === 'firing' && (
          <div className="flex flex-col items-center justify-center animate-pulse">
            <div className="w-24 h-24 rounded-full border-4 border-indigo-500 border-t-transparent animate-spin mb-6" />
            <p className="text-xl font-black text-indigo-400 tracking-widest uppercase">{t('revealing')}...</p>
          </div>
        )}

        {phase === 'revealed' && (
          <div className="w-full h-full flex flex-col items-center">
            {result.wasPityTriggered && (
              <p className="text-amber-500 text-xs mt-2 mb-6 font-black bg-amber-500/10 px-4 py-2 rounded-full border border-amber-500/20 uppercase tracking-widest pulse">
                🌟 {t('pityTriggered')}
              </p>
            )}

            <div className={cn(
              "w-full",
              result.items.length > 1 
                ? "grid grid-cols-2 md:grid-cols-5 gap-4" 
                : "flex justify-center"
            )}>
              {result.items.map((item, idx) => (
                <GachaResultItemCard
                  key={`${idx}-${item.rewardType}-${item.rewardValue}-${item.displayIcon ?? 'no-icon'}`}
                  item={item}
                  isSingle={result.items.length === 1}
                  isVi={isVi}
                />
              ))}
            </div>


            <Button 
              variant="brand"
              size="lg"
              className="mt-10 min-w-xs"
              onClick={onClose}
            >
              {t('awesome')}
            </Button>
          </div>
        )}
      </div>
    </Modal>
  );
}
