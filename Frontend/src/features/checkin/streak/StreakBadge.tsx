'use client';

import { useStreakStatus } from '@/features/checkin/streak/hooks';
import { Flame } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

export default function StreakBadge() {
  const t = useTranslations('Common');
  const { data: streakData, isLoading } = useStreakStatus();

  
  if (isLoading || !streakData) {
    return null;
  }

  const streak = streakData.currentStreak;
  const isActive = streak > 0;

  return (
    <div
      className={cn(
        "inline-flex",
        "min-h-9",
        "cursor-default",
        "select-none",
        "items-center",
        "gap-1.5",
        "rounded-xl",
        "border",
        "px-2.5",
        "py-1.5",
        "transition-all",
        "duration-300",
        isActive
          ? "border-amber-500/20 bg-amber-50 shadow-md"
          : "border-slate-700 bg-slate-800/70",
      )}
      title={isActive
        ? t('streak_active_title', { streak })
        : t('streak_inactive_title')
      }
    >
      <Flame
        className={cn(
          "h-4",
          "w-4",
          isActive ? "text-amber-500" : "text-slate-400",
          streak >= 7 ? "animate-pulse" : null,
        )}
        fill={isActive ? 'var(--amber-accent)' : 'var(--text-muted)'}
      />
      <span className={cn("text-xs", "font-black", "tracking-tight", isActive ? "text-amber-500" : "text-slate-400")}>
        {streak}
      </span>
    </div>
  );
}
