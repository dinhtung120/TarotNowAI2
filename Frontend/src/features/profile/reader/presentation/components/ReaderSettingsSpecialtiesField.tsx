'use client';

import { cn } from '@/lib/utils';

interface ReaderSettingsSpecialtiesFieldProps {
 label: string;
 onChange: (value: string) => void;
 placeholder: string;
 value: string;
}

export function ReaderSettingsSpecialtiesField({
 label,
 onChange,
 placeholder,
 value,
}: ReaderSettingsSpecialtiesFieldProps) {
 return (
  <div className={cn('space-y-2')}>
   <label className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] ml-1')}>{label}</label>
   <input
    type="text"
    value={value}
    onChange={(event) => onChange(event.target.value)}
    placeholder={placeholder}
    className={cn('w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary tn-field-accent transition-all shadow-inner')}
   />
  </div>
 );
}
