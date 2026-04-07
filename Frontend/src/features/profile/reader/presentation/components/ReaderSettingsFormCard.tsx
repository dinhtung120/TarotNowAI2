'use client';

import type { FormEvent } from 'react';
import { BookOpen } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { ReaderSettingsBioField } from './ReaderSettingsBioField';
import { ReaderSettingsPriceField } from './ReaderSettingsPriceField';
import { ReaderSettingsSpecialtiesField } from './ReaderSettingsSpecialtiesField';
import { ReaderSettingsSubmitButton } from './ReaderSettingsSubmitButton';

interface ReaderSettingsFormCardProps {
 bioLabel: string;
 bioPlaceholder: string;
 bioValue: string;
 onChangeBio: (value: string) => void;
 onChangePrice: (value: number) => void;
 onChangeSpecialties: (value: string) => void;
 onSubmit: (event: FormEvent<HTMLFormElement>) => void;
 priceHelp: string;
 priceLabel: string;
 priceValue: number;
 saveLabel: string;
 saving: boolean;
 savingLabel: string;
 specialtiesLabel: string;
 specialtiesPlaceholder: string;
 specialtiesValue: string;
 title: string;
}

export function ReaderSettingsFormCard(props: ReaderSettingsFormCardProps) {
 return (
  <GlassCard className={cn('!p-8')}>
   <form onSubmit={props.onSubmit} className={cn('space-y-8')}>
    <h3 className={cn('text-lg font-black tn-text-primary italic tracking-tight mb-6 flex items-center gap-2.5')}>
     <BookOpen className={cn('w-5 h-5 text-[var(--purple-accent)]')} />
     {props.title}
    </h3>
    <div className={cn('space-y-6')}>
     <ReaderSettingsBioField label={props.bioLabel} value={props.bioValue} onChange={props.onChangeBio} placeholder={props.bioPlaceholder} />
     <ReaderSettingsSpecialtiesField label={props.specialtiesLabel} value={props.specialtiesValue} onChange={props.onChangeSpecialties} placeholder={props.specialtiesPlaceholder} />
     <ReaderSettingsPriceField label={props.priceLabel} value={props.priceValue} onChange={props.onChangePrice} helpLabel={props.priceHelp} />
    </div>
    <div className={cn('pt-6 border-t tn-border mt-6')}>
     <ReaderSettingsSubmitButton disabled={props.saving} loadingLabel={props.savingLabel} saveLabel={props.saveLabel} />
    </div>
   </form>
  </GlassCard>
 );
}
