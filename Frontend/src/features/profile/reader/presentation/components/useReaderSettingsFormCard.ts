'use client';

import { useEffect, useMemo } from 'react';
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
 onSubmit: (values: ReaderSettingsFormCardFormValues) => void;
}

const specialtySchema = z.enum(READER_SPECIALTY_VALUES);
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
  onSubmit,
 } = props;

 const schema = useMemo(
  () =>
   z
    .object({
     bio: z.string().max(MAX_BIO_LENGTH, validation.bioMax),
     specialties: z.array(specialtySchema).min(1, validation.specialtiesMin),
     years: z.number().int().min(minYearsValue, validation.yearsMin),
     price: z.number().int().min(minPriceValue, validation.priceMin),
     facebookUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, validation.socialTooLong),
     instagramUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, validation.socialTooLong),
     tikTokUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, validation.socialTooLong),
    })
    .superRefine((values, context) => {
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
    }),
  [minPriceValue, minYearsValue, validation],
 );

 const initialValues = useMemo<ReaderSettingsFormCardFormValues>(
  () => ({
   bio: bioValue,
   specialties: specialtiesValue,
   years: yearsValue,
   price: priceValue,
   facebookUrl,
   instagramUrl,
   tikTokUrl,
  }),
  [bioValue, facebookUrl, instagramUrl, priceValue, specialtiesValue, tikTokUrl, yearsValue],
 );

 const {
  handleSubmit,
  setValue,
  control,
  reset,
  formState: { errors, isDirty },
 } = useForm<ReaderSettingsFormCardFormValues>({
  resolver: zodResolver(schema),
  defaultValues: initialValues,
 });

 useEffect(() => {
  if (isDirty) {
   return;
  }
  reset(initialValues);
 }, [initialValues, isDirty, reset]);

 const watchedBio = useWatch({ control, name: 'bio' }) ?? '';
 const watchedSpecialties = useWatch({ control, name: 'specialties' }) ?? EMPTY_SPECIALTIES;
 const watchedYears = useWatch({ control, name: 'years' }) ?? minYearsValue;
 const watchedPrice = useWatch({ control, name: 'price' }) ?? minPriceValue;
 const watchedFacebookUrl = useWatch({ control, name: 'facebookUrl' }) ?? '';
 const watchedInstagramUrl = useWatch({ control, name: 'instagramUrl' }) ?? '';
 const watchedTikTokUrl = useWatch({ control, name: 'tikTokUrl' }) ?? '';

 const submitWithValidation = handleSubmit((values) => {
  onSubmit(values);
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

export type { ReaderSettingsFormCardFormValues };
