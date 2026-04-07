'use client';

import { useCallback, useState } from 'react';
import { useStreakStatus } from '@/features/checkin/application/hooks';

export function useCheckinStreakCardState() {
  const { data: streakData, isLoading } = useStreakStatus();
  const [isFreezeModalOpen, setIsFreezeModalOpen] = useState(false);

  const openFreezeModal = useCallback(() => setIsFreezeModalOpen(true), []);
  const closeFreezeModal = useCallback(() => setIsFreezeModalOpen(false), []);

  return {
    streakData,
    isLoading,
    isFreezeModalOpen,
    openFreezeModal,
    closeFreezeModal,
  };
}
