'use client';

import { useEffect } from 'react';
import type { FormEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import type { WithdrawFormCardProps } from './WithdrawFormCard.types';

const withdrawFormCardSchema = z.object({
  amount: z
    .string()
    .trim()
    .min(1)
    .refine((value) => {
      const parsed = Number.parseInt(value, 10);
      return Number.isFinite(parsed) && parsed >= 50;
    }),
  bankName: z.string().trim().min(1).max(120),
  accountName: z.string().trim().min(1).max(120),
  accountNumber: z.string().trim().min(1).max(64),
});

type WithdrawFormCardFormValues = z.infer<typeof withdrawFormCardSchema>;

const syncOptions = { shouldDirty: false, shouldValidate: false } as const;
const editOptions = { shouldDirty: true, shouldValidate: true } as const;

export function useWithdrawFormCard(props: WithdrawFormCardProps) {
  const {
    amount,
    bankName,
    accountName,
    accountNumber,
    onAmountChange,
    onBankNameChange,
    onAccountNameChange,
    onAccountNumberChange,
    onSubmit,
  } = props;

  const { handleSubmit, setValue, control } = useForm<WithdrawFormCardFormValues>({
    resolver: zodResolver(withdrawFormCardSchema),
    defaultValues: {
      amount,
      bankName,
      accountName,
      accountNumber,
    },
  });

  const watchedAmount = useWatch({ control, name: 'amount' }) ?? '';
  const watchedBankName = useWatch({ control, name: 'bankName' }) ?? '';
  const watchedAccountName = useWatch({ control, name: 'accountName' }) ?? '';
  const watchedAccountNumber = useWatch({ control, name: 'accountNumber' }) ?? '';

  useEffect(() => {
    setValue('amount', amount, syncOptions);
  }, [amount, setValue]);

  useEffect(() => {
    setValue('bankName', bankName, syncOptions);
  }, [bankName, setValue]);

  useEffect(() => {
    setValue('accountName', accountName, syncOptions);
  }, [accountName, setValue]);

  useEffect(() => {
    setValue('accountNumber', accountNumber, syncOptions);
  }, [accountNumber, setValue]);

  useEffect(() => {
    onAmountChange(watchedAmount);
  }, [onAmountChange, watchedAmount]);

  useEffect(() => {
    onBankNameChange(watchedBankName);
  }, [onBankNameChange, watchedBankName]);

  useEffect(() => {
    onAccountNameChange(watchedAccountName);
  }, [onAccountNameChange, watchedAccountName]);

  useEffect(() => {
    onAccountNumberChange(watchedAccountNumber);
  }, [onAccountNumberChange, watchedAccountNumber]);

  const submitWithValidation = handleSubmit(() => {
    onSubmit({
      preventDefault: () => undefined,
      stopPropagation: () => undefined,
    } as unknown as FormEvent<HTMLFormElement>);
  });

  return {
    watchedAmount,
    watchedBankName,
    watchedAccountName,
    watchedAccountNumber,
    submitWithValidation,
    setAmount: (value: string) => setValue('amount', value, editOptions),
    setBankName: (value: string) => setValue('bankName', value, editOptions),
    setAccountName: (value: string) => setValue('accountName', value, editOptions),
    setAccountNumber: (value: string) => setValue('accountNumber', value, editOptions),
  };
}
