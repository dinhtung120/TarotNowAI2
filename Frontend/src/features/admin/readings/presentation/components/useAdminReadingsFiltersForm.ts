'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';

const adminReadingsFiltersSchema = z
 .object({
  username: z.string().trim().max(100),
  spreadType: z.string().max(50),
  startDate: z.string().max(50),
  endDate: z.string().max(50),
 })
 .refine(
  (values) => !values.startDate || !values.endDate || values.endDate >= values.startDate,
  {
   path: ['endDate'],
   message: 'End date must be greater than or equal to start date',
  },
 );

type AdminReadingsFiltersFormValues = z.infer<typeof adminReadingsFiltersSchema>;

interface UseAdminReadingsFiltersFormOptions {
 username: string;
 spreadType: string;
 startDate: string;
 endDate: string;
 onUsernameChange: (value: string) => void;
 onSpreadTypeChange: (value: string) => void;
 onStartDateChange: (value: string) => void;
 onEndDateChange: (value: string) => void;
 onSubmit: () => void;
}

export function useAdminReadingsFiltersForm(options: UseAdminReadingsFiltersFormOptions) {
 const { handleSubmit, setValue, control } = useForm<AdminReadingsFiltersFormValues>({
  resolver: zodResolver(adminReadingsFiltersSchema),
  defaultValues: {
   username: options.username,
   spreadType: options.spreadType,
   startDate: options.startDate,
   endDate: options.endDate,
  },
 });

 const watchedUsername = useWatch({ control, name: 'username' }) ?? '';
 const watchedSpreadType = useWatch({ control, name: 'spreadType' }) ?? '';
 const watchedStartDate = useWatch({ control, name: 'startDate' }) ?? '';
 const watchedEndDate = useWatch({ control, name: 'endDate' }) ?? '';

 useEffect(() => {
  setValue('username', options.username, { shouldDirty: false, shouldValidate: false });
  setValue('spreadType', options.spreadType, { shouldDirty: false, shouldValidate: false });
  setValue('startDate', options.startDate, { shouldDirty: false, shouldValidate: false });
  setValue('endDate', options.endDate, { shouldDirty: false, shouldValidate: false });
 }, [options.endDate, options.spreadType, options.startDate, options.username, setValue]);

 useEffect(() => {
  options.onUsernameChange(watchedUsername);
  options.onSpreadTypeChange(watchedSpreadType);
  options.onStartDateChange(watchedStartDate);
  options.onEndDateChange(watchedEndDate);
 }, [
  options,
  watchedEndDate,
  watchedSpreadType,
  watchedStartDate,
  watchedUsername,
 ]);

 return {
  setValue,
  submitWithValidation: handleSubmit(options.onSubmit),
  watchedUsername,
  watchedSpreadType,
  watchedStartDate,
  watchedEndDate,
 };
}
