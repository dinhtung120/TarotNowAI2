'use client';

import { READER_SPECIALTY_VALUES, type ReaderSpecialtyValue } from '@/features/reader/shared/readerSpecialties';
import { cn } from '@/lib/utils';

interface ReaderSettingsSpecialtiesFieldProps {
 label: string;
 value: ReaderSpecialtyValue[];
 error?: string;
 onChange: (value: ReaderSpecialtyValue[]) => void;
 renderSpecialtyLabel: (value: ReaderSpecialtyValue) => string;
}

export function ReaderSettingsSpecialtiesField({
 label,
 value,
 error,
 onChange,
 renderSpecialtyLabel,
}: ReaderSettingsSpecialtiesFieldProps) {
 return (
  <div className={cn('space-y-2')}>
   <label className={cn('ml-1 tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>{label}</label>
   <div className={cn('grid grid-cols-1 gap-2 sm:grid-cols-2')}>
    {READER_SPECIALTY_VALUES.map((specialty) => {
     const checked = value.includes(specialty);
     return (
      <label
       key={specialty}
       className={cn(
        'flex items-center gap-2 rounded-xl border px-3 py-2 text-xs tn-text-secondary',
        checked ? 'tn-border-accent-30 tn-bg-accent-10' : 'tn-border-soft',
       )}
      >
       <input
        type="checkbox"
        checked={checked}
        aria-invalid={Boolean(error)}
        onChange={(event) => {
         const next = event.target.checked
          ? [...value, specialty]
          : value.filter((item) => item !== specialty);
         onChange(next);
        }}
       />
       {renderSpecialtyLabel(specialty)}
      </label>
     );
    })}
   </div>
   {error ? <p className={cn('tn-text-10 tn-text-danger')}>{error}</p> : null}
  </div>
 );
}
