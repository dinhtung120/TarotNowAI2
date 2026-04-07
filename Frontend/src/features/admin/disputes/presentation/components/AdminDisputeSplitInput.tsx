'use client';

import { cn } from '@/lib/utils';

interface AdminDisputeSplitInputProps {
 label: string;
 onChange: (value: number) => void;
 splitPercent: number;
}

export function AdminDisputeSplitInput({
 label,
 onChange,
 splitPercent,
}: AdminDisputeSplitInputProps) {
 return (
  <div className={cn('flex items-center gap-2')}>
   <span className={cn('text-xs text-[var(--text-secondary)]')}>{label}</span>
   <input
    type="number"
    min={1}
    max={99}
    value={splitPercent}
    onChange={(event) => {
     const value = Number(event.target.value);
     if (Number.isFinite(value) == false) return;
     onChange(Math.max(1, Math.min(99, value)));
    }}
    className={cn('w-24 rounded-lg bg-white/5 border border-white/10 px-2 py-1 text-xs text-[var(--text-primary)]')}
   />
  </div>
 );
}
