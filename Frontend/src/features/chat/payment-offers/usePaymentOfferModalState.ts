'use client';

import { useCallback, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

function createPaymentOfferModalSchema(maxNoteLength: number) {
  return z.object({
    amount: z.number().int().min(1),
    note: z.string().trim().min(1).max(maxNoteLength),
  });
}

interface PaymentOfferModalFormValues {
  amount: number;
  note: string;
}

interface UsePaymentOfferModalStateArgs {
  onClose: () => void;
  onSubmit: (amount: number, note: string) => Promise<void>;
}

export function usePaymentOfferModalState({ onClose, onSubmit }: UsePaymentOfferModalStateArgs) {
  const t = useTranslations('Chat');
  const runtimePoliciesQuery = useRuntimePolicies();
  const defaultAmount = runtimePoliciesQuery.data?.chat.paymentOffer.defaultAmount
    ?? RUNTIME_POLICY_FALLBACKS.chat.paymentOffer.defaultAmount;
  const maxNoteLength = runtimePoliciesQuery.data?.chat.paymentOffer.maxNoteLength
    ?? RUNTIME_POLICY_FALLBACKS.chat.paymentOffer.maxNoteLength;
  const paymentOfferModalSchema = createPaymentOfferModalSchema(maxNoteLength);
  const [submitting, setSubmitting] = useState(false);
  const { handleSubmit, setValue, watch } = useForm<PaymentOfferModalFormValues>({
    resolver: zodResolver(paymentOfferModalSchema),
    defaultValues: {
      amount: defaultAmount,
      note: '',
    },
  });

  const amount = watch('amount') ?? defaultAmount;
  const note = watch('note') ?? '';
  const isSubmitDisabled = submitting || amount <= 0 || !note.trim() || note.trim().length > maxNoteLength;

  const submitWithValidation = handleSubmit(async (values) => {
    if (submitting) return;
    setSubmitting(true);
    try {
      await onSubmit(values.amount, values.note.trim());
      onClose();
    } finally {
      setSubmitting(false);
    }
  });

  const onAmountChange = useCallback(
    (value: number) => {
      setValue('amount', value, { shouldDirty: true, shouldValidate: true });
    },
    [setValue]
  );

  const onNoteChange = useCallback(
    (value: string) => {
      setValue('note', value, { shouldDirty: true, shouldValidate: true });
    },
    [setValue]
  );

  return {
    t,
    amount,
    note,
    submitting,
    isSubmitDisabled,
    submitWithValidation,
    onAmountChange,
    onNoteChange,
  };
}
