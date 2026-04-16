'use client';

import { cn } from '@/lib/utils';

interface CardOptionStats {
  level: number;
  currentExp: number;
  expToNextLevel: number;
  totalAtk: number;
  totalDef: number;
}

/**
 * Interface cho các tùy chọn card mục tiêu.
 */
export interface CardOption {
  id: number;
  name: string;
  imageUrl?: string;
  stats?: CardOptionStats;
}

interface UseItemCardSelectorProps {
  label: string;
  value: number | '';
  cardOptions: CardOption[];
  onChange: (value: number | '') => void;
}

export default function UseItemCardSelector({
  label,
  value,
  cardOptions,
  onChange,
}: UseItemCardSelectorProps) {
  return (
    <div className={cn('flex flex-col gap-2.5')}>
      <span className={cn('tn-text-muted pl-1 text-[10px] font-black tracking-widest uppercase')}>
        {label}
      </span>
      <select
        value={value}
        onChange={(event) => onChange(event.target.value ? Number(event.target.value) : '')}
        className={cn(
          'tn-field tn-field-accent w-full cursor-pointer rounded-2xl px-4 py-3 text-sm font-bold transition-all',
          'hover:bg-white/[0.04]',
        )}
      >
        <option value="" className={cn('bg-[var(--bg-surface)]')}>
          --- Chọn lá bài mục tiêu ---
        </option>
        {cardOptions.map((card) => (
          <option key={card.id} value={card.id} className={cn('bg-[var(--bg-surface)]')}>
            {card.name}
          </option>
        ))}
      </select>
    </div>
  );
}
