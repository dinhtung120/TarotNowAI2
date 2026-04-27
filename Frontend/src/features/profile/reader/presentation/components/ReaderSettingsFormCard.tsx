'use client';

import { BookOpen } from 'lucide-react';
import type { ReaderSpecialtyValue } from '@/features/reader/domain/readerSpecialties';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { ReaderSettingsBioField } from './ReaderSettingsBioField';
import { ReaderSettingsExperienceField } from './ReaderSettingsExperienceField';
import { ReaderSettingsPriceField } from './ReaderSettingsPriceField';
import { ReaderSettingsSocialLinksFields } from './ReaderSettingsSocialLinksFields';
import { ReaderSettingsSpecialtiesField } from './ReaderSettingsSpecialtiesField';
import { ReaderSettingsSubmitButton } from './ReaderSettingsSubmitButton';
import { useReaderSettingsFormCard } from './useReaderSettingsFormCard';
import type { ReaderSettingsFormCardFormValues } from './useReaderSettingsFormCard';

interface ReaderSettingsValidationLabels {
 bioMax: string;
 specialtiesMin: string;
 yearsMin: string;
 priceMin: string;
 socialRequired: string;
 socialTooLong: string;
 facebookInvalid: string;
 instagramInvalid: string;
 tikTokInvalid: string;
}

interface ReaderSettingsFormCardProps {
 title: string;
 bioLabel: string;
 bioPlaceholder: string;
 bioValue: string;
 specialtiesLabel: string;
 specialtiesValue: ReaderSpecialtyValue[];
 renderSpecialtyLabel: (value: ReaderSpecialtyValue) => string;
 yearsLabel: string;
 yearsValue: number;
 minYearsValue: number;
 socialLinksLabel: string;
 socialLinksHint: string;
 facebookPlaceholder: string;
 instagramPlaceholder: string;
 tikTokPlaceholder: string;
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
 priceLabel: string;
 priceHelp: string;
 priceValue: number;
 minPriceValue: number;
 saveLabel: string;
 savingLabel: string;
 saving: boolean;
 validation: ReaderSettingsValidationLabels;
 onSubmit: (values: ReaderSettingsFormCardFormValues) => void;
}

export function ReaderSettingsFormCard(props: ReaderSettingsFormCardProps) {
 const vm = useReaderSettingsFormCard(props);
 const socialRequiredMessage = vm.errors.facebookUrl?.message === props.validation.socialRequired ? vm.errors.facebookUrl.message : undefined;
 const facebookFieldError = vm.errors.facebookUrl?.message !== props.validation.socialRequired ? vm.errors.facebookUrl?.message : undefined;

 return (
  <GlassCard className={cn('!p-8')}>
   <form onSubmit={vm.submitWithValidation} className={cn('space-y-8')}>
    <h3 className={cn('mb-6 flex items-center gap-2.5 text-lg font-black italic tracking-tight tn-text-primary')}>
     <BookOpen className={cn('h-5 w-5 tn-text-accent')} />
     {props.title}
    </h3>
    <div className={cn('space-y-6')}>
     <ReaderSettingsBioField label={props.bioLabel} value={vm.watchedBio} onChange={vm.setBio} placeholder={props.bioPlaceholder} error={vm.errors.bio?.message} />
     <ReaderSettingsSpecialtiesField label={props.specialtiesLabel} value={vm.watchedSpecialties} onChange={vm.setSpecialties} renderSpecialtyLabel={props.renderSpecialtyLabel} error={vm.errors.specialties?.message} />
     <ReaderSettingsExperienceField label={props.yearsLabel} value={vm.watchedYears} minValue={props.minYearsValue} onChange={vm.setYears} error={vm.errors.years?.message} />
     <ReaderSettingsSocialLinksFields label={props.socialLinksLabel} hintLabel={props.socialLinksHint} facebookPlaceholder={props.facebookPlaceholder} instagramPlaceholder={props.instagramPlaceholder} tikTokPlaceholder={props.tikTokPlaceholder} facebookUrl={vm.watchedFacebookUrl} instagramUrl={vm.watchedInstagramUrl} tikTokUrl={vm.watchedTikTokUrl} onChangeFacebook={vm.setFacebookUrl} onChangeInstagram={vm.setInstagramUrl} onChangeTikTok={vm.setTikTokUrl} errors={{ socialRequired: socialRequiredMessage, facebookUrl: facebookFieldError, instagramUrl: vm.errors.instagramUrl?.message, tikTokUrl: vm.errors.tikTokUrl?.message }} />
     <ReaderSettingsPriceField label={props.priceLabel} value={vm.watchedPrice} minValue={props.minPriceValue} onChange={vm.setPrice} helpLabel={props.priceHelp} error={vm.errors.price?.message} />
    </div>
    <div className={cn('mt-6 border-t pt-6 tn-border')}>
     <ReaderSettingsSubmitButton disabled={props.saving} loadingLabel={props.savingLabel} saveLabel={props.saveLabel} />
    </div>
   </form>
  </GlassCard>
 );
}
