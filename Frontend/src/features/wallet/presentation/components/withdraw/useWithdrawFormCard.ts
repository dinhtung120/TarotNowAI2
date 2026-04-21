'use client';

import { useEffect } from 'react';
import type { FormEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { MIN_WITHDRAW_DIAMOND } from '@/features/wallet/domain/constants';
import type { WithdrawFormCardProps } from './WithdrawFormCard.types';

const withdrawFormCardSchema = z.object({
  amount: z
    .string()
    .trim()
    .min(1)
    .refine((value) => {
      const parsed = Number.parseInt(value, 10);
      return Number.isFinite(parsed) && parsed >= MIN_WITHDRAW_DIAMOND;
    }),
  userNote: z.string().max(1_000),
});

type WithdrawFormCardFormValues = z.infer<typeof withdrawFormCardSchema>;

const syncOptions = { shouldDirty: false, shouldValidate: false } as const;
const editOptions = { shouldDirty: true, shouldValidate: true } as const;

export function useWithdrawFormCard(props: WithdrawFormCardProps) {
  const { amount, onAmountChange, onSubmit, onUserNoteChange, userNote } = props;

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

  const submitWithValidation = handleSubmit(() => {
    onSubmit({
      preventDefault: () => undefined,
      stopPropagation: () => undefined,
    } as unknown as FormEvent<HTMLFormElement>);
  });

  return {
    watchedAmount,
    watchedUserNote,
    submitWithValidation,
    setAmount: (value: string) => setValue('amount', value, editOptions),
    setUserNote: (value: string) => setValue('userNote', value, editOptions),
  };
}
