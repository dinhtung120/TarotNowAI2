'use client';

import { useCallback, useEffect, useMemo, useState } from 'react';
import { createWithdrawal, listMyWithdrawals, type WithdrawalResult } from '@/features/wallet/application/actions/withdrawal';
import { useLocale, useTranslations } from 'next-intl';
import { getWithdrawalStatusBadge } from '@/features/wallet/domain/withdrawalStatus';

export function useWithdrawPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();

 const [amount, setAmount] = useState('');
 const [bankName, setBankName] = useState('');
 const [accountName, setAccountName] = useState('');
 const [accountNumber, setAccountNumber] = useState('');
 const [submitting, setSubmitting] = useState(false);
 const [success, setSuccess] = useState(false);
 const [error, setError] = useState<string | null>(null);
 const [showMfa, setShowMfa] = useState(false);
 const [history, setHistory] = useState<WithdrawalResult[]>([]);
 const [loadingHistory, setLoadingHistory] = useState(true);

 const loadHistory = useCallback(async () => {
  const result = await listMyWithdrawals();
  setHistory(result.success && result.data ? result.data : []);
  setLoadingHistory(false);
 }, []);

 useEffect(() => {
  const initialFetchTimer = window.setTimeout(() => {
   void loadHistory();
  }, 0);

  return () => {
   window.clearTimeout(initialFetchTimer);
  };
 }, [loadHistory]);

 const amountNum = useMemo(() => parseInt(amount, 10) || 0, [amount]);
 const grossVnd = amountNum * 1000;
 const feeVnd = Math.ceil(grossVnd * 0.1);
 const netVnd = grossVnd - feeVnd;

 const getStatusBadge = useCallback(
  (status: string) =>
   getWithdrawalStatusBadge(status, {
    pending: t('withdraw.status_pending'),
    approved: t('withdraw.status_approved'),
    rejected: t('withdraw.status_rejected'),
    paid: t('withdraw.status_paid'),
   }),
  [t]
 );

 const handleSubmit = useCallback(
  (e: React.FormEvent) => {
   e.preventDefault();
   setError(null);
   setSuccess(false);

   if (amountNum < 50) {
    setError(t('withdraw.error_min_amount'));
    return;
   }
   if (!bankName.trim()) {
    setError(t('withdraw.error_enter_bank'));
    return;
   }
   if (!accountName.trim()) {
    setError(t('withdraw.error_enter_account_name'));
    return;
   }
   if (!accountNumber.trim()) {
    setError(t('withdraw.error_enter_account_number'));
    return;
   }

   setShowMfa(true);
  },
  [accountName, accountNumber, amountNum, bankName, t]
 );

 const handleMfaSuccess = useCallback(
  async (mfaCode: string) => {
   setShowMfa(false);
   setSubmitting(true);
   setError(null);

   const result = await createWithdrawal({
    amountDiamond: amountNum,
    bankName: bankName.trim(),
    bankAccountName: accountName.trim(),
    bankAccountNumber: accountNumber.trim(),
    mfaCode,
   });

   if (result.success) {
    setSuccess(true);
    setAmount('');
    await loadHistory();
   } else {
    setError(result.error);
   }

   setSubmitting(false);
  },
  [accountName, accountNumber, amountNum, bankName, loadHistory]
 );

 return {
  t,
  locale,
  amount,
  setAmount,
  bankName,
  setBankName,
  accountName,
  setAccountName,
  accountNumber,
  setAccountNumber,
  submitting,
  success,
  error,
  showMfa,
  setShowMfa,
  history,
  loadingHistory,
  amountNum,
  grossVnd,
  feeVnd,
  netVnd,
  getStatusBadge,
  handleSubmit,
  handleMfaSuccess,
 };
}
