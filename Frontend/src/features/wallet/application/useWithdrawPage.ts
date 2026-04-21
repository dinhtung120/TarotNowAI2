'use client';

import { useCallback, useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { createWithdrawal, listMyWithdrawals, type WithdrawalResult } from '@/features/wallet/application/actions/withdrawal';
import { EXCHANGE_RATE_VND_PER_DIAMOND, MIN_WITHDRAW_DIAMOND, WITHDRAW_FEE_RATE } from '@/features/wallet/domain/constants';
import { getWithdrawalStatusBadge } from '@/features/wallet/domain/withdrawalStatus';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { useWalletStore } from '@/store/walletStore';

const HISTORY_QUERY_KEY = userStateQueryKeys.wallet.withdrawalsMine();

export function useWithdrawPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const queryClient = useQueryClient();
 const balance = useWalletStore((state) => state.balance);

 const [amount, setAmount] = useState('');
 const [bankName, setBankName] = useState('');
 const [accountName, setAccountName] = useState('');
 const [accountNumber, setAccountNumber] = useState('');
 const [userNote, setUserNote] = useState('');
 const [success, setSuccess] = useState(false);
 const [error, setError] = useState<string | null>(null);

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

 const amountNum = useMemo(() => Number.parseInt(amount, 10) || 0, [amount]);
 const grossVnd = amountNum * EXCHANGE_RATE_VND_PER_DIAMOND;
 const feeVnd = Math.ceil(grossVnd * WITHDRAW_FEE_RATE);
 const netVnd = grossVnd - feeVnd;

 const getStatusBadge = useCallback(
  (status: string) =>
   getWithdrawalStatusBadge(status, {
    pending: t('withdraw.status_pending'),
    approved: t('withdraw.status_approved'),
    rejected: t('withdraw.status_rejected'),
    paid: t('withdraw.status_paid'),
   }),
  [t],
 );

 const handleSubmit = useCallback(
  async (event: React.FormEvent<HTMLFormElement>) => {
   event.preventDefault();
   setError(null);
   setSuccess(false);

   if (amountNum < MIN_WITHDRAW_DIAMOND) {
    setError(t('withdraw.error_min_amount'));
    return;
   }

   if ((balance?.diamondBalance ?? 0) < amountNum) {
    setError(t('withdraw.error_insufficient_balance'));
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

   const result = await withdrawalMutation.mutateAsync({
    amountDiamond: amountNum,
    bankName: bankName.trim(),
    bankAccountName: accountName.trim(),
    bankAccountNumber: accountNumber.trim(),
    userNote: userNote.trim() || undefined,
    idempotencyKey: buildCreateWithdrawalIdempotencyKey(),
   });

   if (!result.success) {
    setError(result.error);
    return;
   }

   setSuccess(true);
   setAmount('');
   setUserNote('');
   await queryClient.invalidateQueries({ queryKey: HISTORY_QUERY_KEY });
  },
  [accountName, accountNumber, amountNum, balance?.diamondBalance, bankName, queryClient, t, userNote, withdrawalMutation],
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
  userNote,
  setUserNote,
  submitting: withdrawalMutation.isPending,
  success,
  error,
  history: historyData ?? [],
  loadingHistory: isLoading || isFetching,
  amountNum,
  grossVnd,
  feeVnd,
  netVnd,
  getStatusBadge,
  handleSubmit,
 };
}

function buildCreateWithdrawalIdempotencyKey(): string {
 const safeTimestamp = new Date().toISOString().replace(/[-:.TZ]/g, '');
 return `wd_create_${safeTimestamp}_${crypto.randomUUID()}`;
}
