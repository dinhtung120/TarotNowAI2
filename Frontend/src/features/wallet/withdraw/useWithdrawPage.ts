'use client';

import { useCallback, useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { createWithdrawal, listMyWithdrawals, type WithdrawalResult } from '@/features/wallet/withdraw/actions';
import { fetchProfileDetail, profileDetailQueryKey } from '@/features/profile/overview/profileDetailQuery';
import { useWalletBalanceQuery } from '@/features/wallet/overview/useWalletBalanceQuery';
import { useRuntimePolicies } from '@/shared/hooks/useRuntimePolicies';
import { getWithdrawalStatusBadge } from '@/features/wallet/shared/withdrawalStatus';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';

const HISTORY_QUERY_KEY = userStateQueryKeys.wallet.withdrawalsMine();

export function useWithdrawPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const queryClient = useQueryClient();
 const balanceQuery = useWalletBalanceQuery();
 const runtimePoliciesQuery = useRuntimePolicies();

 const [amount, setAmount] = useState('');
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

 const profileQuery = useQuery({
  queryKey: profileDetailQueryKey,
  queryFn: fetchProfileDetail,
 });

 const withdrawalMutation = useMutation({ mutationFn: createWithdrawal });

 const walletPolicy = runtimePoliciesQuery.data?.wallet;
 const minWithdrawDiamond = walletPolicy?.minWithdrawDiamond ?? 0;
 const vndPerDiamond = walletPolicy?.vndPerDiamond ?? 0;
 const withdrawFeeRate = walletPolicy?.withdrawFeeRate ?? 0;
 const withdrawalPolicyReady = Boolean(walletPolicy && minWithdrawDiamond > 0 && vndPerDiamond > 0);
 const walletBalance = balanceQuery.data;

 const amountNum = useMemo(() => Number.parseInt(amount, 10) || 0, [amount]);
 const grossVnd = amountNum * vndPerDiamond;
 const feeVnd = Math.ceil(grossVnd * withdrawFeeRate);
 const netVnd = grossVnd - feeVnd;
 const withdrawFeePercent = Math.round(withdrawFeeRate * 10_000) / 100;

 const payoutInfo = useMemo(() => {
  const profile = profileQuery.data?.profile;
  if (!profile) {
   return null;
  }

  return {
   bankName: profile.payoutBankName?.trim() || '',
   bankBin: profile.payoutBankBin?.trim() || '',
   accountNumber: profile.payoutBankAccountNumber?.trim() || '',
   accountHolder: profile.payoutBankAccountHolder?.trim() || '',
  };
 }, [profileQuery.data?.profile]);

 const payoutConfigured = Boolean(
  payoutInfo?.bankName
  && payoutInfo.bankBin
  && payoutInfo.accountNumber
  && payoutInfo.accountHolder,
 );

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
  async (formData: { amount: string; userNote: string }) => {
   setError(null);
   setSuccess(false);
   const normalizedAmount = formData.amount.trim();
   const normalizedAmountNum = Number.parseInt(normalizedAmount, 10) || 0;
   const normalizedUserNote = formData.userNote.trim();

   if (!payoutConfigured) {
    setError(t('withdraw.error_bank_missing_profile'));
    return;
   }

   if (!withdrawalPolicyReady) {
    setError(t('withdraw.error_policy_unavailable'));
    return;
   }

   if (normalizedAmountNum < minWithdrawDiamond) {
   setError(t('withdraw.error_min_amount', { min: minWithdrawDiamond }));
   return;
  }

   if (balanceQuery.isError || typeof walletBalance?.diamondBalance !== 'number') {
    setError(t('withdraw.error_balance_unavailable'));
    return;
   }

   if (walletBalance.diamondBalance < normalizedAmountNum) {
    setError(t('withdraw.error_insufficient_balance'));
    return;
   }

   const result = await withdrawalMutation.mutateAsync({
    amountDiamond: normalizedAmountNum,
    userNote: normalizedUserNote || undefined,
    idempotencyKey: buildCreateWithdrawalIdempotencyKey(),
   });

   if (!result.success) {
    setError(result.error || t('withdraw.error_create_failed'));
    return;
   }

   setSuccess(true);
   setAmount('');
   setUserNote('');
   await Promise.all([
    queryClient.invalidateQueries({ queryKey: HISTORY_QUERY_KEY }),
    queryClient.invalidateQueries({ queryKey: userStateQueryKeys.wallet.balance() }),
   ]);
  },
  [balanceQuery.isError, minWithdrawDiamond, payoutConfigured, queryClient, t, walletBalance, withdrawalMutation, withdrawalPolicyReady],
 );

 return {
  t,
  locale,
  amount,
  setAmount,
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
  minWithdrawDiamond,
  withdrawFeePercent,
  withdrawalPolicyReady,
  payoutInfo,
  payoutConfigured,
  profilePath: '/profile',
  getStatusBadge,
  handleSubmit,
 };
}

function buildCreateWithdrawalIdempotencyKey(): string {
 const safeTimestamp = new Date().toISOString().replace(/[-:.TZ]/g, '');
 return `wd_create_${safeTimestamp}_${crypto.randomUUID()}`;
}
