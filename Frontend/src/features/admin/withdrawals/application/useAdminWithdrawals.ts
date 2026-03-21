'use client';

import { useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import {
  listWithdrawalQueue,
  processWithdrawal,
  type WithdrawalResult,
} from '@/actions/withdrawalActions';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

type PendingAction = { id: string; action: 'approve' | 'reject' } | null;

export function useAdminWithdrawals(t: TranslateFn, locale: string) {
  const [queue, setQueue] = useState<WithdrawalResult[]>([]);
  const [loading, setLoading] = useState(true);
  const [processing, setProcessing] = useState<string | null>(null);
  const [notes, setNotes] = useState<Record<string, string>>({});
  const [showMfa, setShowMfa] = useState(false);
  const [pendingAction, setPendingAction] = useState<PendingAction>(null);

  useEffect(() => {
    const loadQueue = async () => {
      const data = await listWithdrawalQueue();
      setQueue(data);
      setLoading(false);
    };

    const initialFetchTimer = window.setTimeout(() => {
      void loadQueue();
    }, 0);

    return () => {
      window.clearTimeout(initialFetchTimer);
    };
  }, []);

  const handleProcess = (id: string, action: 'approve' | 'reject') => {
    setPendingAction({ id, action });
    setShowMfa(true);
  };

  const handleMfaSuccess = async (mfaCode: string) => {
    if (!pendingAction) return;

    const { id, action } = pendingAction;
    setShowMfa(false);
    setProcessing(id);

    const res = await processWithdrawal({
      withdrawalId: id,
      action,
      adminNote: notes[id] || undefined,
      mfaCode,
    });

    if (res.success) {
      toast.success(t('withdrawals.toast.success'));
      setQueue((prev) => prev.filter((item) => item.id !== id));
    } else {
      toast.error(res.error || t('withdrawals.toast.failed'));
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
    queue,
    loading,
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
