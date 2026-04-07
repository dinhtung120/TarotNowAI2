'use client';

import { useStreakStatus } from '@/features/checkin/application/hooks';
import { Flame } from 'lucide-react';

export default function StreakBadge() {
  
  const { data: streakData, isLoading } = useStreakStatus();

  
  if (isLoading || !streakData) {
    return null;
  }

  const streak = streakData.currentStreak;
  const isActive = streak > 0;

  return (
    <div
      className={`inline-flex items-center gap-1.5 px-2.5 py-1.5 rounded-xl
        border transition-all duration-300 cursor-default select-none min-h-9
        ${isActive
          ? 'bg-[var(--amber-50)] border-[var(--amber-accent)]/20 shadow-[0_0_12px_rgba(245,158,11,0.15)] hover:shadow-[0_0_18px_rgba(245,158,11,0.25)]'
          : 'bg-[var(--bg-surface-hover)] border-[var(--border-subtle)]'
        }`}
      title={isActive
        ? `Chuỗi ${streak} ngày rút bài liên tiếp`
        : 'Rút bài AI để bắt đầu chuỗi Streak!'
      }
    >
      {}
      <Flame
        className={`w-4 h-4 ${
          isActive
            ? 'text-[var(--amber-accent)] drop-shadow-[0_0_4px_rgba(245,158,11,0.6)]'
            : 'text-[var(--text-muted)]'
        } ${streak >= 7 ? 'animate-pulse' : ''}`}
        fill={isActive ? 'var(--amber-accent)' : 'var(--text-muted)'}
      />

      {}
      <span className={`text-[11px] font-black tracking-tight ${
        isActive ? 'text-[var(--amber-accent)]' : 'text-[var(--text-muted)]'
      }`}>
        {streak}
      </span>
    </div>
  );
}
