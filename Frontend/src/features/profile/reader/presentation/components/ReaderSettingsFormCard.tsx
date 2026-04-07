'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import type { FormEvent } from 'react';
import { BookOpen } from 'lucide-react';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
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

const readerSettingsFormCardSchema = z.object({
 bio: z.string().max(2000),
 specialties: z.string().max(500),
 price: z.number().min(50),
});

type ReaderSettingsFormCardFormValues = z.infer<typeof readerSettingsFormCardSchema>;

export function ReaderSettingsFormCard(props: ReaderSettingsFormCardProps) {
 const { onChangeBio, onChangePrice, onChangeSpecialties, onSubmit } = props;
 const { handleSubmit, setValue, control } = useForm<ReaderSettingsFormCardFormValues>({
  resolver: zodResolver(readerSettingsFormCardSchema),
  defaultValues: {
   bio: props.bioValue,
   specialties: props.specialtiesValue,
   price: props.priceValue,
  },
 });

 const watchedBio = useWatch({ control, name: 'bio' }) ?? '';
 const watchedSpecialties = useWatch({ control, name: 'specialties' }) ?? '';
 const watchedPrice = useWatch({ control, name: 'price' }) ?? 50;

 useEffect(() => {
  setValue('bio', props.bioValue, { shouldDirty: false, shouldValidate: false });
 }, [props.bioValue, setValue]);

 useEffect(() => {
  setValue('specialties', props.specialtiesValue, { shouldDirty: false, shouldValidate: false });
 }, [props.specialtiesValue, setValue]);

 useEffect(() => {
  setValue('price', props.priceValue, { shouldDirty: false, shouldValidate: false });
 }, [props.priceValue, setValue]);

 useEffect(() => {
  onChangeBio(watchedBio);
 }, [onChangeBio, watchedBio]);

 useEffect(() => {
  onChangeSpecialties(watchedSpecialties);
 }, [onChangeSpecialties, watchedSpecialties]);

 useEffect(() => {
  onChangePrice(watchedPrice);
 }, [onChangePrice, watchedPrice]);

 const submitWithValidation = handleSubmit(() => {
  onSubmit({
   preventDefault: () => undefined,
   stopPropagation: () => undefined,
  } as unknown as FormEvent<HTMLFormElement>);
 });

 return (
  <GlassCard className={cn('!p-8')}>
   <form onSubmit={submitWithValidation} className={cn('space-y-8')}>
    <h3 className={cn('text-lg font-black tn-text-primary italic tracking-tight mb-6 flex items-center gap-2.5')}>
     <BookOpen className={cn('w-5 h-5 tn-text-accent')} />
     {props.title}
    </h3>
    <div className={cn('space-y-6')}>
     <ReaderSettingsBioField label={props.bioLabel} value={watchedBio} onChange={(value) => setValue('bio', value, { shouldDirty: true, shouldValidate: true })} placeholder={props.bioPlaceholder} />
     <ReaderSettingsSpecialtiesField label={props.specialtiesLabel} value={watchedSpecialties} onChange={(value) => setValue('specialties', value, { shouldDirty: true, shouldValidate: true })} placeholder={props.specialtiesPlaceholder} />
     <ReaderSettingsPriceField label={props.priceLabel} value={watchedPrice} onChange={(value) => setValue('price', value, { shouldDirty: true, shouldValidate: true })} helpLabel={props.priceHelp} />
    </div>
    <div className={cn('pt-6 border-t tn-border mt-6')}>
     <ReaderSettingsSubmitButton disabled={props.saving} loadingLabel={props.savingLabel} saveLabel={props.saveLabel} />
    </div>
   </form>
  </GlassCard>
 );
}
