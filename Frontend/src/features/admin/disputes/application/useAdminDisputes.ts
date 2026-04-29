'use client';

import { useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { ADMIN_QUERY_POLICY } from '@/features/admin/application/adminQueryPolicy';
import { adminQueryKeys } from '@/features/admin/application/adminQueryKeys';
import {
 listAdminDisputes,
 resolveAdminDispute,
} from '@/features/chat/application/actions';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { queryFnOrThrow } from '@/shared/application/utils/queryPolicy';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useAdminDisputes(t: TranslateFn) {
 const queryClient = useQueryClient();
 const runtimePoliciesQuery = useRuntimePolicies();
 const defaultSplitPercentToReader =
  runtimePoliciesQuery.data?.adminDispute.defaultSplitPercentToReader
  ?? RUNTIME_POLICY_FALLBACKS.adminDispute.defaultSplitPercentToReader;
 const [noteById, setNoteById] = useState<Record<string, string>>({});
 const [splitPercentById, setSplitPercentById] = useState<Record<string, number>>({});

 const disputesQuery = useQuery({
  queryKey: adminQueryKeys.disputesList(),
 queryFn: async () => {
  const result = await listAdminDisputes(1, 100);
  return queryFnOrThrow(result, 'Failed to load disputes').items;
 },
  ...ADMIN_QUERY_POLICY.heavyList,
 });

 const resolveMutation = useMutation({
  mutationFn: async (payload: { itemId: string; action: 'release' | 'refund' | 'split' }) => {
   return resolveAdminDispute(payload.itemId, {
    action: payload.action,
    splitPercentToReader:
     payload.action === 'split'
      ? Math.max(1, Math.min(99, splitPercentById[payload.itemId] ?? defaultSplitPercentToReader))
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
       : t('disputes.toast.split_success');
    toast.success(message);
    void queryClient.invalidateQueries({ queryKey: adminQueryKeys.disputesList() });
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
  error: disputesQuery.error instanceof Error ? disputesQuery.error.message : '',
  fetching: disputesQuery.isFetching,
  processingId,
  noteById,
  setNoteById,
  splitPercentById,
  setSplitPercentById,
  defaultSplitPercentToReader,
  resolveDispute: (itemId: string, action: 'release' | 'refund' | 'split') =>
   resolveMutation.mutate({ itemId, action }),
 };
}
