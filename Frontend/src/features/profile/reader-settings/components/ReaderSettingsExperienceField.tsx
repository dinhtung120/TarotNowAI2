'use client';

import { BriefcaseBusiness } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderSettingsExperienceFieldProps {
 label: string;
 value: number;
 minValue: number;
 error?: string;
 onChange: (value: number) => void;
}

export function ReaderSettingsExperienceField({
 label,
 value,
 minValue,
 error,
 onChange,
}: ReaderSettingsExperienceFieldProps) {
 return (
  <div className={cn('space-y-2')}>
   <label className={cn('ml-1 flex items-center gap-2 tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
    <BriefcaseBusiness className={cn('h-3 w-3')} />
    {label}
   </label>
   <input
    type="number"
    min={minValue}
    value={value}
    aria-invalid={Boolean(error)}
    onChange={(event) => onChange(Number(event.target.value))}
    className={cn('w-full rounded-xl border bg-white/5 px-3 py-2 text-sm tn-text-primary', error ? 'tn-border-danger' : 'tn-border-soft')}
   />
   {error ? <p className={cn('tn-text-10 tn-text-danger')}>{error}</p> : null}
  </div>
 );
}
