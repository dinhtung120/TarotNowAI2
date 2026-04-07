'use client';

import { useCallback, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import { z } from 'zod';

const paymentOfferModalSchema = z.object({
  amount: z.number().int().min(1),
  note: z.string().trim().min(1).max(100),
});

type PaymentOfferModalFormValues = z.infer<typeof paymentOfferModalSchema>;

interface UsePaymentOfferModalStateArgs {
  onClose: () => void;
  onSubmit: (amount: number, note: string) => Promise<void>;
}

export function usePaymentOfferModalState({ onClose, onSubmit }: UsePaymentOfferModalStateArgs) {
  const t = useTranslations('Chat');
  const [submitting, setSubmitting] = useState(false);
  const { handleSubmit, setValue, watch } = useForm<PaymentOfferModalFormValues>({
    resolver: zodResolver(paymentOfferModalSchema),
    defaultValues: {
      amount: 10,
      note: '',
    },
  });

  const amount = watch('amount') ?? 10;
  const note = watch('note') ?? '';
  const isSubmitDisabled = submitting || amount <= 0 || !note.trim() || note.trim().length > 100;

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
