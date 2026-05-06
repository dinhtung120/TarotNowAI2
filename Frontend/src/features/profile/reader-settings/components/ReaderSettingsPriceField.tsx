'use client';

import { Gem } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderSettingsPriceFieldProps {
 helpLabel: string;
 label: string;
 minValue: number;
 onChange: (value: number) => void;
 value: number;
 error?: string;
}

export function ReaderSettingsPriceField({
 helpLabel,
 label,
 minValue,
 onChange,
 value,
 error,
}: ReaderSettingsPriceFieldProps) {
 return (
  <div className={cn('space-y-2')}>
   <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary ml-1 flex justify-between items-center')}>
    <span>{label}</span>
    <span className={cn('tn-text-warning font-bold flex items-center gap-1')}><Gem className={cn('w-3 h-3')} /> {value}</span>
   </label>
   <div className={cn('relative')}>
    <Gem className={cn('absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 tn-text-warning')} />
    <input type="number" value={value} onChange={(event) => onChange(Number(event.target.value))} min={minValue} aria-invalid={Boolean(error)} className={cn('w-full pl-12 pr-4 py-3.5 tn-field rounded-xl text-sm tn-text-primary tn-field-warning transition-all font-bold shadow-inner', error ? 'tn-border-danger' : '')} />
   </div>
   <p className={cn('tn-text-10 tn-text-tertiary italic pl-1')}>{helpLabel}</p>
   {error ? <p className={cn('tn-text-10 tn-text-danger pl-1')}>{error}</p> : null}
  </div>
 );
}
