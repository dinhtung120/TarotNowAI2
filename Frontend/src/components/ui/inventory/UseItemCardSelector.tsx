'use client';

import { cn } from '@/lib/utils';

export interface CardOption {
 id: number;
 name: string;
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
  <label className={cn('flex flex-col gap-2 text-sm font-medium text-slate-700 dark:text-slate-200')}>
   {label}
   <select
    value={value}
    onChange={(event) => onChange(event.target.value ? Number(event.target.value) : '')}
    className={cn('rounded-xl border border-slate-300 bg-white px-3 py-2 text-sm dark:border-slate-700 dark:bg-slate-900')}
   >
    <option value="">--</option>
    {cardOptions.map((card) => (
     <option key={card.id} value={card.id}>
      {card.name}
     </option>
    ))}
   </select>
  </label>
 );
}
