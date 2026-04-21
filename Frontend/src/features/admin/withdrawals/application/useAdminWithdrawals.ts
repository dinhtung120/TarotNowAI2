'use client';

import { useCallback, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { getWithdrawalDetail, listWithdrawalQueue, processWithdrawal, type WithdrawalDetailResult, type WithdrawalResult } from '@/features/wallet/public';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;
type ProcessAction = 'approve' | 'reject';
const queueQueryKey = ['admin', 'withdrawals', 'queue'] as const;

export function useAdminWithdrawals(t: TranslateFn, locale: string) {
 const queryClient = useQueryClient();
 const [processing, setProcessing] = useState<string | null>(null);
 const [notes, setNotes] = useState<Record<string, string>>({});
 const [selectedWithdrawalId, setSelectedWithdrawalId] = useState<string | null>(null);

 const { data, isLoading, isFetching } = useQuery<WithdrawalResult[]>({
  queryKey: queueQueryKey,
  queryFn: async () => {
   const result = await listWithdrawalQueue();
   return result.success && result.data ? result.data : [];
  },
 });

 const detailQuery = useQuery<WithdrawalDetailResult | null>({
  queryKey: ['admin', 'withdrawals', 'detail', selectedWithdrawalId] as const,
  enabled: Boolean(selectedWithdrawalId),
  queryFn: async () => {
   if (!selectedWithdrawalId) {
    return null;
   }

   const result = await getWithdrawalDetail(selectedWithdrawalId);
   if (!result.success || !result.data) {
    throw new Error(result.error || 'Failed to get withdrawal detail');
   }

   return result.data;
  },
 });

 const processMutation = useMutation({ mutationFn: processWithdrawal });

 const handleProcess = useCallback(
  async (id: string, action: ProcessAction) => {
   const adminNote = notes[id]?.trim();
   if (action === 'reject' && !adminNote) {
    toast.error(t('withdrawals.toast.reject_note_required'));
    return;
   }

   setProcessing(id);
   try {
    const result = await processMutation.mutateAsync({
     withdrawalId: id,
     action,
     adminNote: adminNote || undefined,
     idempotencyKey: buildProcessIdempotencyKey(id, action),
    });

    if (!result.success) {
     toast.error(result.error);
     return;
    }

    toast.success(t('withdrawals.toast.success'));
    queryClient.setQueryData<WithdrawalResult[]>(queueQueryKey, (previous) =>
     (previous ?? []).filter((item) => item.id !== id),
    );
    if (selectedWithdrawalId === id) {
      setSelectedWithdrawalId(null);
    }
   } catch {
    toast.error(t('withdrawals.toast.failed'));
   } finally {
    setProcessing(null);
   }
  },
  [notes, processMutation, queryClient, selectedWithdrawalId, t],
 );

 const openDetail = useCallback((withdrawalId: string) => {
  setSelectedWithdrawalId(withdrawalId);
 }, []);

 const closeDetail = useCallback(() => {
  setSelectedWithdrawalId(null);
 }, []);

 const formatVnd = useCallback((value: number | null | undefined) => {
  if (value == null) return '-';
  return new Intl.NumberFormat(locale, {
   style: 'currency',
   currency: 'VND',
   maximumFractionDigits: 0,
  }).format(value);
 }, [locale]);

 return {
  queue: data ?? [],
  loading: isLoading || isFetching,
  processing,
  notes,
  setNotes,
  handleProcess,
  formatVnd,
  selectedWithdrawalId,
  detail: detailQuery.data ?? null,
  loadingDetail: detailQuery.isLoading || detailQuery.isFetching,
  detailError: detailQuery.error ? t('withdrawals.toast.detail_failed') : '',
  openDetail,
  closeDetail,
 };
}

function buildProcessIdempotencyKey(withdrawalId: string, action: ProcessAction): string {
 const safeTimestamp = new Date().toISOString().replace(/[-:.TZ]/g, '');
 return `wd_process_${action}_${withdrawalId}_${safeTimestamp}_${crypto.randomUUID()}`;
}
