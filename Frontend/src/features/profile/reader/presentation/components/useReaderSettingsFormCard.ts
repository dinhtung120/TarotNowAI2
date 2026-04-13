'use client';

import { useEffect } from 'react';
import type { FormEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';

interface ReaderSettingsFormStateProps {
  bioValue: string;
  specialtiesValue: string;
  priceValue: number;
  onChangeBio: (value: string) => void;
  onChangePrice: (value: number) => void;
  onChangeSpecialties: (value: string) => void;
  onSubmit: (event: FormEvent<HTMLFormElement>) => void;
}

const readerSettingsFormCardSchema = z.object({
  bio: z.string().max(2000),
  specialties: z.string().max(500),
  price: z.number().min(50),
});

type ReaderSettingsFormCardFormValues = z.infer<typeof readerSettingsFormCardSchema>;

const syncOptions = { shouldDirty: false, shouldValidate: false } as const;
const editOptions = { shouldDirty: true, shouldValidate: true } as const;

export function useReaderSettingsFormCard(props: ReaderSettingsFormStateProps) {
  const {
    bioValue,
    specialtiesValue,
    priceValue,
    onChangeBio,
    onChangePrice,
    onChangeSpecialties,
    onSubmit,
  } = props;

  const { handleSubmit, setValue, control } = useForm<ReaderSettingsFormCardFormValues>({
    resolver: zodResolver(readerSettingsFormCardSchema),
    defaultValues: {
      bio: bioValue,
      specialties: specialtiesValue,
      price: priceValue,
    },
  });

  const watchedBio = useWatch({ control, name: 'bio' }) ?? '';
  const watchedSpecialties = useWatch({ control, name: 'specialties' }) ?? '';
  const watchedPrice = useWatch({ control, name: 'price' }) ?? 50;

  useEffect(() => {
    if (watchedBio !== bioValue) setValue('bio', bioValue, syncOptions);
  }, [bioValue, setValue, watchedBio]);

  useEffect(() => {
    if (watchedSpecialties !== specialtiesValue) {
      setValue('specialties', specialtiesValue, syncOptions);
    }
  }, [specialtiesValue, setValue, watchedSpecialties]);

  useEffect(() => {
    if (watchedPrice !== priceValue) setValue('price', priceValue, syncOptions);
  }, [priceValue, setValue, watchedPrice]);

  useEffect(() => {
    if (watchedBio !== bioValue) onChangeBio(watchedBio);
  }, [bioValue, onChangeBio, watchedBio]);

  useEffect(() => {
    if (watchedSpecialties !== specialtiesValue) onChangeSpecialties(watchedSpecialties);
  }, [onChangeSpecialties, specialtiesValue, watchedSpecialties]);

  useEffect(() => {
    if (watchedPrice !== priceValue) onChangePrice(watchedPrice);
  }, [onChangePrice, priceValue, watchedPrice]);

  const submitWithValidation = handleSubmit(() => {
    onSubmit({
      preventDefault: () => undefined,
      stopPropagation: () => undefined,
    } as unknown as FormEvent<HTMLFormElement>);
  });

  return {
    watchedBio,
    watchedSpecialties,
    watchedPrice,
    submitWithValidation,
    setBio: (value: string) => setValue('bio', value, editOptions),
    setSpecialties: (value: string) => setValue('specialties', value, editOptions),
    setPrice: (value: number) => setValue('price', value, editOptions),
  };
}
