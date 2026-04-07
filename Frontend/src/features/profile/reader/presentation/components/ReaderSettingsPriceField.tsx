'use client';

import { Gem } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderSettingsPriceFieldProps {
 helpLabel: string;
 label: string;
 onChange: (value: number) => void;
 value: number;
}

export function ReaderSettingsPriceField({
 helpLabel,
 label,
 onChange,
 value,
}: ReaderSettingsPriceFieldProps) {
 return (
  <div className={cn('space-y-2')}>
   <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary ml-1 flex justify-between items-center')}>
    <span>{label}</span>
    <span className={cn('tn-text-warning font-bold flex items-center gap-1')}><Gem className={cn('w-3 h-3')} /> {value}</span>
   </label>
   <div className={cn('relative')}>
    <Gem className={cn('absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 tn-text-warning')} />
    <input type="number" value={value} onChange={(event) => onChange(Number(event.target.value))} min={50} className={cn('w-full pl-12 pr-4 py-3.5 tn-field rounded-xl text-sm tn-text-primary tn-field-warning transition-all font-bold shadow-inner')} />
   </div>
   <p className={cn('tn-text-10 tn-text-tertiary italic pl-1')}>{helpLabel}</p>
  </div>
 );
}
