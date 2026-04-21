'use client';

import { useCallback, useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { getProfileAction, type ProfileDto } from '@/features/profile/application/actions';
import { createWithdrawal, listMyWithdrawals, type WithdrawalResult } from '@/features/wallet/application/actions/withdrawal';
import { EXCHANGE_RATE_VND_PER_DIAMOND, MIN_WITHDRAW_DIAMOND, WITHDRAW_FEE_RATE } from '@/features/wallet/domain/constants';
import { getWithdrawalStatusBadge } from '@/features/wallet/domain/withdrawalStatus';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { useWalletStore } from '@/store/walletStore';

const HISTORY_QUERY_KEY = userStateQueryKeys.wallet.withdrawalsMine();
const PROFILE_QUERY_KEY = userStateQueryKeys.profile.me();

export function useWithdrawPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const queryClient = useQueryClient();
 const balance = useWalletStore((state) => state.balance);

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

 const profileQuery = useQuery<{ profile: ProfileDto | null; error: string }>({
  queryKey: PROFILE_QUERY_KEY,
  queryFn: async () => {
   const result = await getProfileAction();
   return result.success ? { profile: result.data ?? null, error: '' } : { profile: null, error: result.error };
  },
 });

 const withdrawalMutation = useMutation({ mutationFn: createWithdrawal });

 const amountNum = useMemo(() => Number.parseInt(amount, 10) || 0, [amount]);
 const grossVnd = amountNum * EXCHANGE_RATE_VND_PER_DIAMOND;
 const feeVnd = Math.ceil(grossVnd * WITHDRAW_FEE_RATE);
 const netVnd = grossVnd - feeVnd;

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
  async (event: React.FormEvent<HTMLFormElement>) => {
   event.preventDefault();
   setError(null);
   setSuccess(false);

   if (!payoutConfigured) {
    setError(t('withdraw.error_bank_missing_profile'));
    return;
   }

   if (amountNum < MIN_WITHDRAW_DIAMOND) {
    setError(t('withdraw.error_min_amount'));
    return;
   }

   if ((balance?.diamondBalance ?? 0) < amountNum) {
    setError(t('withdraw.error_insufficient_balance'));
    return;
   }

   const result = await withdrawalMutation.mutateAsync({
    amountDiamond: amountNum,
    userNote: userNote.trim() || undefined,
    idempotencyKey: buildCreateWithdrawalIdempotencyKey(),
   });

   if (!result.success) {
    setError(result.error || t('withdraw.error_create_failed'));
    return;
   }

   setSuccess(true);
   setAmount('');
   setUserNote('');
   await queryClient.invalidateQueries({ queryKey: HISTORY_QUERY_KEY });
  },
  [amountNum, balance?.diamondBalance, payoutConfigured, queryClient, t, userNote, withdrawalMutation],
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
