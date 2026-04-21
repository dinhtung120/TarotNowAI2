import { BriefcaseBusiness, Gem } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ReaderApplyExperiencePriceRowProps {
 yearsOfExperience: number;
 diamondPerQuestion: number;
 yearsError?: string;
 diamondError?: string;
 onChangeYears: (value: number) => void;
 onChangeDiamond: (value: number) => void;
}

export default function ReaderApplyExperiencePriceRow({
 yearsOfExperience,
 diamondPerQuestion,
 yearsError,
 diamondError,
 onChangeYears,
 onChangeDiamond,
}: ReaderApplyExperiencePriceRowProps) {
 const t = useTranslations('ReaderApply');

 return (
  <div className={cn('grid grid-cols-1 gap-4 sm:grid-cols-2')}>
   <div className={cn('space-y-2')}>
   <label className={cn('flex items-center gap-2 tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
     <BriefcaseBusiness className={cn('h-3 w-3')} />
     {t('form.years_experience_label')}
    </label>
    <input
     type="number"
     min={1}
     value={yearsOfExperience}
     onChange={(event) => onChangeYears(Number(event.target.value))}
     aria-invalid={Boolean(yearsError)}
     className={cn('w-full rounded-xl border bg-white/5 px-3 py-2 text-sm tn-text-primary', yearsError ? 'tn-border-danger' : 'tn-border-soft')}
    />
    {yearsError ? <p className={cn('tn-text-10 tn-text-danger')}>{yearsError}</p> : null}
   </div>

   <div className={cn('space-y-2')}>
    <label className={cn('flex items-center gap-2 tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
     <Gem className={cn('h-3 w-3')} />
     {t('form.diamond_per_question_label')}
    </label>
    <input
     type="number"
     min={50}
     value={diamondPerQuestion}
     onChange={(event) => onChangeDiamond(Number(event.target.value))}
     aria-invalid={Boolean(diamondError)}
     className={cn('w-full rounded-xl border bg-white/5 px-3 py-2 text-sm tn-text-primary', diamondError ? 'tn-border-danger' : 'tn-border-soft')}
    />
    {diamondError ? <p className={cn('tn-text-10 tn-text-danger')}>{diamondError}</p> : null}
   </div>
  </div>
 );
}
