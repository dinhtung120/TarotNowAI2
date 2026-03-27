'use client';

import { useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import {
 listAdminDisputes,
 resolveAdminDispute,
 type AdminDisputeItemDto,
} from '@/features/chat/application/actions';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useAdminDisputes(t: TranslateFn) {
 const queryClient = useQueryClient();
 const [noteById, setNoteById] = useState<Record<string, string>>({});
 const [splitPercentById, setSplitPercentById] = useState<Record<string, number>>({});

 const disputesQuery = useQuery({
  queryKey: ['admin', 'disputes', 'list'],
  queryFn: async () => {
   const result = await listAdminDisputes(1, 100);
   if (result.success && result.data) {
    return result.data.items;
   }
   return [] as AdminDisputeItemDto[];
  },
 });

 const resolveMutation = useMutation({
  mutationFn: async (payload: { itemId: string; action: 'release' | 'refund' | 'split' }) => {
   return resolveAdminDispute(payload.itemId, {
    action: payload.action,
    splitPercentToReader:
     payload.action === 'split'
      ? Math.max(1, Math.min(99, splitPercentById[payload.itemId] ?? 50))
      : undefined,
    adminNote: noteById[payload.itemId] || undefined,
   });
  },
  onSuccess: (result, variables) => {
   if (result.success) {
    const message =
     variables.action === 'release'
      ? t('disputes.toast.release_success')
      : variables.action === 'refund'
       ? t('disputes.toast.refund_success')
       : t('disputes.toast.release_success');
    toast.success(message);
    void queryClient.invalidateQueries({ queryKey: ['admin', 'disputes', 'list'] });
   } else {
    toast.error(result.error || t('disputes.toast.failed'));
   }
  },
  onError: () => {
   toast.error(t('disputes.toast.failed'));
  },
 });

 const processingId = resolveMutation.isPending ? resolveMutation.variables?.itemId ?? null : null;

 const sortedItems = useMemo(
  () => [...(disputesQuery.data ?? [])].sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()),
  [disputesQuery.data]
 );

 return {
  disputes: sortedItems,
  loading: disputesQuery.isLoading,
  fetching: disputesQuery.isFetching,
  processingId,
  noteById,
  setNoteById,
  splitPercentById,
  setSplitPercentById,
  resolveDispute: (itemId: string, action: 'release' | 'refund' | 'split') =>
   resolveMutation.mutate({ itemId, action }),
 };
}
