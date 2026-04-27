'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import type { WithdrawFormCardProps } from './WithdrawFormCard.types';

type WithdrawFormCardFormValues = {
  amount: string;
  userNote: string;
};

const syncOptions = { shouldDirty: false, shouldValidate: false } as const;
const editOptions = { shouldDirty: true, shouldValidate: true } as const;

export function useWithdrawFormCard(props: WithdrawFormCardProps) {
  const {
    amount,
    minWithdrawDiamond,
    onAmountChange,
    onSubmit,
    onUserNoteChange,
    userNote,
  } = props;

  const withdrawFormCardSchema = z.object({
    amount: z
      .string()
      .trim()
      .min(1)
      .refine((value) => {
        const parsed = Number.parseInt(value, 10);
        return Number.isFinite(parsed) && parsed >= minWithdrawDiamond;
      }),
    userNote: z.string().max(1_000),
  });

  const { handleSubmit, setValue, control } = useForm<WithdrawFormCardFormValues>({
    resolver: zodResolver(withdrawFormCardSchema),
    defaultValues: { amount, userNote },
  });

  const watchedAmount = useWatch({ control, name: 'amount' }) ?? '';
  const watchedUserNote = useWatch({ control, name: 'userNote' }) ?? '';

  useEffect(() => {
    setValue('amount', amount, syncOptions);
  }, [amount, setValue]);

  useEffect(() => {
    setValue('userNote', userNote, syncOptions);
  }, [setValue, userNote]);

  useEffect(() => {
    onAmountChange(watchedAmount);
  }, [onAmountChange, watchedAmount]);

  useEffect(() => {
    onUserNoteChange(watchedUserNote);
  }, [onUserNoteChange, watchedUserNote]);

  const submitWithValidation = handleSubmit(async (values) => {
    await onSubmit({
      amount: values.amount,
      userNote: values.userNote,
    });
  });

  return {
    watchedAmount,
    watchedUserNote,
    submitWithValidation,
    setAmount: (value: string) => setValue('amount', value, editOptions),
    setUserNote: (value: string) => setValue('userNote', value, editOptions),
  };
}
