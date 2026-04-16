'use client';

import { useCallback, useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { createWithdrawal, listMyWithdrawals, type WithdrawalResult } from '@/features/wallet/application/actions/withdrawal';
import { useLocale, useTranslations } from 'next-intl';
import { getWithdrawalStatusBadge } from '@/features/wallet/domain/withdrawalStatus';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

const HISTORY_QUERY_KEY = userStateQueryKeys.wallet.withdrawalsMine();

export function useWithdrawPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const queryClient = useQueryClient();

 const [amount, setAmount] = useState('');
 const [bankName, setBankName] = useState('');
 const [accountName, setAccountName] = useState('');
 const [accountNumber, setAccountNumber] = useState('');
 const [success, setSuccess] = useState(false);
 const [error, setError] = useState<string | null>(null);
 const [showMfa, setShowMfa] = useState(false);

 const { data: historyData, isLoading, isFetching } = useQuery<WithdrawalResult[]>({
  queryKey: HISTORY_QUERY_KEY,
  queryFn: async () => {
   const result = await listMyWithdrawals();
   return result.success && result.data ? result.data : [];
  },
 });

 const withdrawalMutation = useMutation({
  mutationFn: createWithdrawal,
 });

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
  setError(null);

  const result = await withdrawalMutation.mutateAsync({
   amountDiamond: amountNum,
   bankName: bankName.trim(),
    bankAccountName: accountName.trim(),
    bankAccountNumber: accountNumber.trim(),
    mfaCode,
   });

   if (result.success) {
    setSuccess(true);
    setAmount('');
    await queryClient.invalidateQueries({ queryKey: HISTORY_QUERY_KEY });
  } else {
   setError(result.error);
  }
  },
  [accountName, accountNumber, amountNum, bankName, queryClient, withdrawalMutation]
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
  submitting: withdrawalMutation.isPending,
  success,
  error,
  showMfa,
  setShowMfa,
  history: historyData ?? [],
  loadingHistory: isLoading || isFetching,
  amountNum,
  grossVnd,
  feeVnd,
  netVnd,
  getStatusBadge,
  handleSubmit,
  handleMfaSuccess,
 };
}
