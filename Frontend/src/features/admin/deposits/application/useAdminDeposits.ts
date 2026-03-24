'use client';

import { useCallback, useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import {
 listDeposits,
 processDeposit,
 type AdminDepositOrder,
} from '@/features/admin/application/actions';

interface ConfirmModalState {
 isOpen: boolean;
 type: 'approve' | 'reject';
 order: AdminDepositOrder | null;
}

export function useAdminDeposits() {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [orders, setOrders] = useState<AdminDepositOrder[]>([]);
 const [totalCount, setTotalCount] = useState(0);
 const [page, setPage] = useState(1);
 const [statusFilter, setStatusFilter] = useState('');
 const [loading, setLoading] = useState(true);
 const [processingId, setProcessingId] = useState<string | null>(null);
 const [confirmModal, setConfirmModal] = useState<ConfirmModalState>({
  isOpen: false,
  type: 'approve',
  order: null,
 });

 const fetchOrders = useCallback(async () => {
  setLoading(true);
  try {
   const result = await listDeposits(page, 10, statusFilter);
   if (!result.success || !result.data) return;

   setOrders(result.data.deposits);
   setTotalCount(result.data.totalCount);
  } finally {
   setLoading(false);
  }
 }, [page, statusFilter]);

 useEffect(() => {
  const timer = window.setTimeout(() => {
   void fetchOrders();
  }, 0);

  return () => {
   window.clearTimeout(timer);
  };
 }, [fetchOrders]);

 const handleAction = async () => {
  if (!confirmModal.order) return;

  const { id } = confirmModal.order;
  const action = confirmModal.type;
  setConfirmModal((prev) => ({ ...prev, isOpen: false }));
  setProcessingId(id);

  try {
   const result = await processDeposit(id, action);
   if (!result.success) {
    toast.error(t('deposits.toast.action_failed'));
    return;
   }

   toast.success(
    action === 'approve'
     ? t('deposits.toast.approve_success')
     : t('deposits.toast.reject_success')
   );
   await fetchOrders();
  } catch {
   toast.error(t('deposits.toast.network_error'));
  } finally {
   setProcessingId(null);
  }
 };

 return {
  t,
  locale,
  orders,
  totalCount,
  page,
  setPage,
  statusFilter,
  setStatusFilter,
  loading,
  processingId,
  confirmModal,
  setConfirmModal,
  handleAction,
 };
}
