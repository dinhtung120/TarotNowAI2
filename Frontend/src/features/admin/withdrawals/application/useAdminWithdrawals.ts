'use client';

import { useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import {
  listWithdrawalQueue,
  processWithdrawal,
  type WithdrawalResult,
} from '@/features/wallet/public';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

type PendingAction = { id: string; action: 'approve' | 'reject' } | null;

export function useAdminWithdrawals(t: TranslateFn, locale: string) {
  const queryClient = useQueryClient();
  const [processing, setProcessing] = useState<string | null>(null);
  const [notes, setNotes] = useState<Record<string, string>>({});
  const [showMfa, setShowMfa] = useState(false);
  const [pendingAction, setPendingAction] = useState<PendingAction>(null);

 const queueQueryKey = ['admin', 'withdrawals', 'queue'] as const;
 const { data, isLoading, isFetching } = useQuery<WithdrawalResult[]>({
  queryKey: queueQueryKey,
  queryFn: async () => {
   const result = await listWithdrawalQueue();
   return result.success && result.data ? result.data : [];
  },
 });

 const processMutation = useMutation({
  mutationFn: processWithdrawal,
 });

  const handleProcess = (id: string, action: 'approve' | 'reject') => {
    setPendingAction({ id, action });
    setShowMfa(true);
  };

  const handleMfaSuccess = async (mfaCode: string) => {
    if (!pendingAction) return;

    const { id, action } = pendingAction;
    setShowMfa(false);
    setProcessing(id);

    const res = await processMutation.mutateAsync({
      withdrawalId: id,
      action,
      adminNote: notes[id] || undefined,
      mfaCode,
    });

    if (res.success) {
      toast.success(t('withdrawals.toast.success'));
   queryClient.setQueryData<WithdrawalResult[]>(queueQueryKey, (prev) =>
    (prev ?? []).filter((item) => item.id !== id)
   );
    } else {
      toast.error(res.error);
    }

    setProcessing(null);
    setPendingAction(null);
  };

  const formatVnd = (value: number | null | undefined) => {
    if (value == null) return '-';
    return new Intl.NumberFormat(locale, {
      style: 'currency',
      currency: 'VND',
      maximumFractionDigits: 0,
    }).format(value);
  };

  return {
  queue: data ?? [],
  loading: isLoading || isFetching,
    processing,
    notes,
    setNotes,
    showMfa,
    setShowMfa,
    pendingAction,
    handleProcess,
    handleMfaSuccess,
    formatVnd,
  };
}
