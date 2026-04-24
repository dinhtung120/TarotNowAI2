'use client';

import { useEffect, useMemo } from 'react';
import type { FormEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { READER_SPECIALTY_VALUES, type ReaderSpecialtyValue } from '@/features/reader/domain/readerSpecialties';
import {
 hasAtLeastOneSocialLink,
 isValidFacebookUrl,
 isValidInstagramUrl,
 isValidTikTokUrl,
} from '@/features/reader/domain/readerSocialLinks';

interface ReaderSettingsFormValidationLabels {
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

interface ReaderSettingsFormStateProps {
 bioValue: string;
 specialtiesValue: ReaderSpecialtyValue[];
 yearsValue: number;
 priceValue: number;
 minYearsValue: number;
 minPriceValue: number;
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
 validation: ReaderSettingsFormValidationLabels;
 onChangeBio: (value: string) => void;
 onChangeSpecialties: (value: ReaderSpecialtyValue[]) => void;
 onChangeYears: (value: number) => void;
 onChangePrice: (value: number) => void;
 onChangeFacebookUrl: (value: string) => void;
 onChangeInstagramUrl: (value: string) => void;
 onChangeTikTokUrl: (value: string) => void;
 onSubmit: (event: FormEvent<HTMLFormElement>) => void;
}

const specialtySchema = z.enum(READER_SPECIALTY_VALUES);
const syncOptions = { shouldDirty: false, shouldValidate: false } as const;
const editOptions = { shouldDirty: true, shouldValidate: true } as const;
const MAX_BIO_LENGTH = 4_000;
const MAX_SOCIAL_URL_LENGTH = 500;
const EMPTY_SPECIALTIES: ReaderSpecialtyValue[] = [];

export function useReaderSettingsFormCard(props: ReaderSettingsFormStateProps) {
 const {
 bioValue,
 specialtiesValue,
 yearsValue,
 priceValue,
 minYearsValue,
 minPriceValue,
 facebookUrl,
 instagramUrl,
 tikTokUrl,
 validation,
  onChangeBio,
  onChangeSpecialties,
  onChangeYears,
  onChangePrice,
  onChangeFacebookUrl,
  onChangeInstagramUrl,
  onChangeTikTokUrl,
  onSubmit,
 } = props;

 const schema = useMemo(() => z.object({
  bio: z.string().max(MAX_BIO_LENGTH, validation.bioMax),
  specialties: z.array(specialtySchema).min(1, validation.specialtiesMin),
  years: z.number().int().min(minYearsValue, validation.yearsMin),
  price: z.number().int().min(minPriceValue, validation.priceMin),
  facebookUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, validation.socialTooLong),
  instagramUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, validation.socialTooLong),
  tikTokUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, validation.socialTooLong),
 }).superRefine((values, context) => {
  if (!hasAtLeastOneSocialLink(values)) {
   context.addIssue({ code: z.ZodIssueCode.custom, path: ['facebookUrl'], message: validation.socialRequired });
  }
  if (!isValidFacebookUrl(values.facebookUrl)) {
   context.addIssue({ code: z.ZodIssueCode.custom, path: ['facebookUrl'], message: validation.facebookInvalid });
  }
  if (!isValidInstagramUrl(values.instagramUrl)) {
   context.addIssue({ code: z.ZodIssueCode.custom, path: ['instagramUrl'], message: validation.instagramInvalid });
  }
  if (!isValidTikTokUrl(values.tikTokUrl)) {
   context.addIssue({ code: z.ZodIssueCode.custom, path: ['tikTokUrl'], message: validation.tikTokInvalid });
  }
 }), [minPriceValue, validation, minYearsValue]);

 const { handleSubmit, setValue, control, formState: { errors } } = useForm<ReaderSettingsFormCardFormValues>({
  resolver: zodResolver(schema),
  defaultValues: {
   bio: bioValue,
   specialties: specialtiesValue,
   years: yearsValue,
   price: priceValue,
   facebookUrl,
   instagramUrl,
   tikTokUrl,
  },
 });

 const watchedBio = useWatch({ control, name: 'bio' }) ?? '';
 const watchedSpecialties = useWatch({ control, name: 'specialties' }) ?? EMPTY_SPECIALTIES;
 const watchedYears = useWatch({ control, name: 'years' }) ?? minYearsValue;
 const watchedPrice = useWatch({ control, name: 'price' }) ?? minPriceValue;
 const watchedFacebookUrl = useWatch({ control, name: 'facebookUrl' }) ?? '';
 const watchedInstagramUrl = useWatch({ control, name: 'instagramUrl' }) ?? '';
 const watchedTikTokUrl = useWatch({ control, name: 'tikTokUrl' }) ?? '';

 useEffect(() => { if (watchedBio !== bioValue) setValue('bio', bioValue, syncOptions); }, [bioValue, setValue, watchedBio]);
 useEffect(() => { if (!areArrayValuesEqual(watchedSpecialties, specialtiesValue)) setValue('specialties', specialtiesValue, syncOptions); }, [setValue, specialtiesValue, watchedSpecialties]);
 useEffect(() => { if (watchedYears !== yearsValue) setValue('years', yearsValue, syncOptions); }, [setValue, watchedYears, yearsValue]);
 useEffect(() => { if (watchedPrice !== priceValue) setValue('price', priceValue, syncOptions); }, [priceValue, setValue, watchedPrice]);
 useEffect(() => { if (watchedFacebookUrl !== facebookUrl) setValue('facebookUrl', facebookUrl, syncOptions); }, [facebookUrl, setValue, watchedFacebookUrl]);
 useEffect(() => { if (watchedInstagramUrl !== instagramUrl) setValue('instagramUrl', instagramUrl, syncOptions); }, [instagramUrl, setValue, watchedInstagramUrl]);
 useEffect(() => { if (watchedTikTokUrl !== tikTokUrl) setValue('tikTokUrl', tikTokUrl, syncOptions); }, [setValue, tikTokUrl, watchedTikTokUrl]);

 useEffect(() => { if (watchedBio !== bioValue) onChangeBio(watchedBio); }, [bioValue, onChangeBio, watchedBio]);
 useEffect(() => { if (!areArrayValuesEqual(watchedSpecialties, specialtiesValue)) onChangeSpecialties(watchedSpecialties); }, [onChangeSpecialties, specialtiesValue, watchedSpecialties]);
 useEffect(() => { if (watchedYears !== yearsValue) onChangeYears(watchedYears); }, [onChangeYears, watchedYears, yearsValue]);
 useEffect(() => { if (watchedPrice !== priceValue) onChangePrice(watchedPrice); }, [onChangePrice, priceValue, watchedPrice]);
 useEffect(() => { if (watchedFacebookUrl !== facebookUrl) onChangeFacebookUrl(watchedFacebookUrl); }, [facebookUrl, onChangeFacebookUrl, watchedFacebookUrl]);
 useEffect(() => { if (watchedInstagramUrl !== instagramUrl) onChangeInstagramUrl(watchedInstagramUrl); }, [instagramUrl, onChangeInstagramUrl, watchedInstagramUrl]);
 useEffect(() => { if (watchedTikTokUrl !== tikTokUrl) onChangeTikTokUrl(watchedTikTokUrl); }, [onChangeTikTokUrl, tikTokUrl, watchedTikTokUrl]);

 const submitWithValidation = handleSubmit(() => {
  onSubmit({
   preventDefault: () => undefined,
   stopPropagation: () => undefined,
  } as unknown as FormEvent<HTMLFormElement>);
 });

 return {
  errors,
  watchedBio,
  watchedSpecialties,
  watchedYears,
  watchedPrice,
  watchedFacebookUrl,
  watchedInstagramUrl,
  watchedTikTokUrl,
  submitWithValidation,
  setBio: (value: string) => setValue('bio', value, editOptions),
  setSpecialties: (value: ReaderSpecialtyValue[]) => setValue('specialties', value, editOptions),
  setYears: (value: number) => setValue('years', value, editOptions),
  setPrice: (value: number) => setValue('price', value, editOptions),
  setFacebookUrl: (value: string) => setValue('facebookUrl', value, editOptions),
  setInstagramUrl: (value: string) => setValue('instagramUrl', value, editOptions),
  setTikTokUrl: (value: string) => setValue('tikTokUrl', value, editOptions),
 };
}

type ReaderSettingsFormCardFormValues = {
 bio: string;
 specialties: ReaderSpecialtyValue[];
 years: number;
 price: number;
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
};

function areArrayValuesEqual(left: ReaderSpecialtyValue[], right: ReaderSpecialtyValue[]): boolean {
 if (left.length !== right.length) {
  return false;
 }
 return left.every((value, index) => value === right[index]);
}
