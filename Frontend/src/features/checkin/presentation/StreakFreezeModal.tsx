'use client';

import { useState } from 'react';
import { v4 as uuidv4 } from 'uuid';
import { usePurchaseFreeze } from '@/features/checkin/application/hooks';
import StreakFreezeModalCard from '@/features/checkin/presentation/streak-freeze/StreakFreezeModalCard';
import { getErrorDescription, getRemainingTimeParts } from '@/features/checkin/presentation/streak-freeze/streakFreezeUtils';
import { cn } from '@/lib/utils';

interface StreakFreezeModalProps {
  freezePrice: number;
  isOpen: boolean;
  preBreakStreak: number;
  remainingSeconds: number;
  onClose: () => void;
}

export const StreakFreezeModal = ({ freezePrice, isOpen, preBreakStreak, remainingSeconds, onClose }: StreakFreezeModalProps) => {
  const { mutate: purchaseFreeze, isPending } = usePurchaseFreeze();
  const [errorDesc, setErrorDesc] = useState<string | null>(null);
  const { hours, minutes } = getRemainingTimeParts(remainingSeconds);
  if (!isOpen) return null;

  const handleBuy = () => {
    setErrorDesc(null);
    purchaseFreeze({ idempotencyKey: uuidv4() }, { onSuccess: onClose, onError: (error: unknown) => setErrorDesc(getErrorDescription(error)) });
  };

  return (
    <div className={cn('fixed inset-0 z-50 flex items-center justify-center bg-black/60 p-4 backdrop-blur-sm animate-in fade-in duration-200')}>
      <StreakFreezeModalCard
        errorDesc={errorDesc}
        freezePrice={freezePrice}
        hours={hours}
        isPending={isPending}
        minutes={minutes}
        preBreakStreak={preBreakStreak}
        onClose={onClose}
        onBuy={handleBuy}
      />
    </div>
  );
};
