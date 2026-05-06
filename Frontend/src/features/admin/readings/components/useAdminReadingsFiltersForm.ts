'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import type { AdminReadingsFilters } from '@/features/admin/readings/hooks/useAdminReadings';

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
 initialValues: AdminReadingsFilters;
 onSubmit: (values: AdminReadingsFilters) => void;
}

function normalizeFormValues(values: AdminReadingsFiltersFormValues): AdminReadingsFilters {
 return {
  username: values.username.trim(),
  spreadType: values.spreadType.trim(),
  startDate: values.startDate.trim(),
  endDate: values.endDate.trim(),
 };
}

export function useAdminReadingsFiltersForm(options: UseAdminReadingsFiltersFormOptions) {
 const { handleSubmit, setValue, control } = useForm<AdminReadingsFiltersFormValues>({
  resolver: zodResolver(adminReadingsFiltersSchema),
  defaultValues: options.initialValues,
 });

 const watchedUsername = useWatch({ control, name: 'username' }) ?? '';
 const watchedSpreadType = useWatch({ control, name: 'spreadType' }) ?? '';
 const watchedStartDate = useWatch({ control, name: 'startDate' }) ?? '';
 const watchedEndDate = useWatch({ control, name: 'endDate' }) ?? '';

 useEffect(() => {
  setValue('username', options.initialValues.username, { shouldDirty: false, shouldValidate: false });
  setValue('spreadType', options.initialValues.spreadType, { shouldDirty: false, shouldValidate: false });
  setValue('startDate', options.initialValues.startDate, { shouldDirty: false, shouldValidate: false });
  setValue('endDate', options.initialValues.endDate, { shouldDirty: false, shouldValidate: false });
 }, [options.initialValues.endDate, options.initialValues.spreadType, options.initialValues.startDate, options.initialValues.username, setValue]);

 return {
  setValue,
  submitWithValidation: handleSubmit((values) => options.onSubmit(normalizeFormValues(values))),
  watchedUsername,
  watchedSpreadType,
  watchedStartDate,
  watchedEndDate,
 };
}
