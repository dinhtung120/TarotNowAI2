'use client';

import { useState } from 'react';
import toast from 'react-hot-toast';
import { resolveDispute } from '@/actions/escrowActions';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useAdminDisputes(t: TranslateFn) {
  const [processing, setProcessing] = useState<string | null>(null);
  const [resolved, setResolved] = useState<Set<string>>(new Set());
  const [itemId, setItemId] = useState('');
  const [note, setNote] = useState('');

  const handleResolve = async (targetItemId: string, action: 'release' | 'refund') => {
    setProcessing(targetItemId);
    const ok = await resolveDispute({
      itemId: targetItemId,
      action,
      adminNote: note || undefined,
    });

    if (ok) {
      toast.success(
        action === 'release'
          ? t('disputes.toast.release_success')
          : t('disputes.toast.refund_success')
      );
      setResolved((prev) => new Set(prev).add(targetItemId));
    } else {
      toast.error(t('disputes.toast.failed'));
    }

    setProcessing(null);
    setNote('');
  };

  const resolveByCurrentItem = (action: 'release' | 'refund') => {
    const trimmed = itemId.trim();
    if (!trimmed) {
      toast.error(t('disputes.toast.missing_item_id'));
      return;
    }

    void handleResolve(trimmed, action);
  };

  return {
    processing,
    resolved,
    itemId,
    setItemId,
    note,
    setNote,
    resolveByCurrentItem,
  };
}
