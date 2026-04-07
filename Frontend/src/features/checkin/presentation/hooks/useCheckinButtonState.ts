'use client';

import { useCallback, useState } from 'react';
import { useDailyCheckIn } from '@/features/checkin/application/hooks';

const AnimationDurationMs = 1500;

interface UseCheckinButtonStateArgs {
  isCheckedIn: boolean;
}

export function useCheckinButtonState({ isCheckedIn }: UseCheckinButtonStateArgs) {
  const { mutate: performCheckIn, isPending } = useDailyCheckIn();
  const [animating, setAnimating] = useState(false);

  const handleClick = useCallback(() => {
    if (isCheckedIn || isPending) return;

    setAnimating(true);
    window.setTimeout(() => setAnimating(false), AnimationDurationMs);

    performCheckIn(undefined, {
      onError: (err: unknown) => console.error('Điểm danh lỗi tẹo:', err),
    });
  }, [isCheckedIn, isPending, performCheckIn]);

  return {
    animating,
    isPending,
    handleClick,
  };
}
