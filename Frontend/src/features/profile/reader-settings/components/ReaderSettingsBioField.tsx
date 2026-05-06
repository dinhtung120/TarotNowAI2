'use client';

import { cn } from '@/lib/utils';

interface ReaderSettingsBioFieldProps {
 error?: string;
 label: string;
 onChange: (value: string) => void;
 placeholder: string;
 value: string;
}

export function ReaderSettingsBioField({
 error,
 label,
 onChange,
 placeholder,
 value,
}: ReaderSettingsBioFieldProps) {
 return (
  <div className={cn('space-y-2')}>
   <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary ml-1')}>{label}</label>
   <textarea
    value={value}
    onChange={(event) => onChange(event.target.value)}
    rows={4}
    placeholder={placeholder}
    aria-invalid={Boolean(error)}
    className={cn('w-full tn-field rounded-xl px-4 py-3.5 text-sm tn-text-primary tn-field-accent transition-all shadow-inner resize-none', error ? 'tn-border-danger' : '')}
   />
   {error ? <p className={cn('tn-text-10 tn-text-danger')}>{error}</p> : null}
  </div>
 );
}
