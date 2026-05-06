'use client';

import { useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import { listDeposits, processDeposit, type AdminDepositOrder } from '@/features/admin/deposits/actions/deposits';
import { ADMIN_QUERY_POLICY } from '@/features/admin/shared/adminQueryPolicy';
import { adminQueryKeys } from '@/features/admin/shared/adminQueryKeys';
import { queryFnOrThrow } from '@/shared/utils/queryPolicy';

interface ConfirmModalState {
 isOpen: boolean;
 type: 'approve' | 'reject';
 order: AdminDepositOrder | null;
}

export function useAdminDeposits() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const queryClient = useQueryClient();

 const [page, setPage] = useState(1);
 const [statusFilter, setStatusFilter] = useState('');
 const [processingId, setProcessingId] = useState<string | null>(null);
 const [confirmModal, setConfirmModal] = useState<ConfirmModalState>({
  isOpen: false,
  type: 'approve',
  order: null,
 });

 const { data, isLoading, isFetching, error } = useQuery({
  queryKey: adminQueryKeys.deposits(page, statusFilter),
 queryFn: async () => {
  const result = await listDeposits(page, 10, statusFilter);
  return queryFnOrThrow(result, 'Failed to load deposits');
 },
  ...ADMIN_QUERY_POLICY.list,
 });

 const processMutation = useMutation({
  mutationFn: async (payload: { id: string; action: 'approve' | 'reject' }) =>
   processDeposit(payload.id, payload.action),
 });

 const handleAction = async () => {
  if (!confirmModal.order) return;

  const { id } = confirmModal.order;
  const action = confirmModal.type;
  setConfirmModal((prev) => ({ ...prev, isOpen: false }));
  setProcessingId(id);

  try {
   const result = await processMutation.mutateAsync({ id, action });
   if (!result.success) {
    toast.error(t('deposits.toast.action_failed'));
    return;
   }

   toast.success(
    action === 'approve'
     ? t('deposits.toast.approve_success')
     : t('deposits.toast.reject_success')
   );
   await queryClient.invalidateQueries({
    queryKey: adminQueryKeys.deposits(page, statusFilter),
    exact: true,
   });
  } catch {
   toast.error(t('deposits.toast.network_error'));
  } finally {
   setProcessingId(null);
  }
 };

 return {
  t,
  locale,
  orders: data?.deposits ?? [],
  totalCount: data?.totalCount ?? 0,
  page,
  setPage,
  statusFilter,
  setStatusFilter,
  loading: isLoading || isFetching,
  listError: error instanceof Error ? error.message : '',
  processingId,
  confirmModal,
  setConfirmModal,
  handleAction,
 };
}
