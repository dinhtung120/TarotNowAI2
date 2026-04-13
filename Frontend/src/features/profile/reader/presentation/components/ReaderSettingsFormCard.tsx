'use client';

import type { FormEvent } from 'react';
import { BookOpen } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { ReaderSettingsBioField } from './ReaderSettingsBioField';
import { ReaderSettingsPriceField } from './ReaderSettingsPriceField';
import { ReaderSettingsSpecialtiesField } from './ReaderSettingsSpecialtiesField';
import { ReaderSettingsSubmitButton } from './ReaderSettingsSubmitButton';
import { useReaderSettingsFormCard } from './useReaderSettingsFormCard';

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
  const vm = useReaderSettingsFormCard(props);

  return (
    <GlassCard className={cn('!p-8')}>
      <form onSubmit={vm.submitWithValidation} className={cn('space-y-8')}>
        <h3 className={cn('mb-6 flex items-center gap-2.5 text-lg font-black italic tracking-tight tn-text-primary')}>
          <BookOpen className={cn('h-5 w-5 tn-text-accent')} />
          {props.title}
        </h3>
        <div className={cn('space-y-6')}>
          <ReaderSettingsBioField
            label={props.bioLabel}
            value={vm.watchedBio}
            onChange={vm.setBio}
            placeholder={props.bioPlaceholder}
          />
          <ReaderSettingsSpecialtiesField
            label={props.specialtiesLabel}
            value={vm.watchedSpecialties}
            onChange={vm.setSpecialties}
            placeholder={props.specialtiesPlaceholder}
          />
          <ReaderSettingsPriceField
            label={props.priceLabel}
            value={vm.watchedPrice}
            onChange={vm.setPrice}
            helpLabel={props.priceHelp}
          />
        </div>
        <div className={cn('mt-6 border-t pt-6 tn-border')}>
          <ReaderSettingsSubmitButton
            disabled={props.saving}
            loadingLabel={props.savingLabel}
            saveLabel={props.saveLabel}
          />
        </div>
      </form>
    </GlassCard>
  );
}
