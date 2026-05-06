'use client';

import type { QueryClient } from '@tanstack/react-query';
import { updateProfileAction, type PayoutBankOption } from '@/features/profile/overview/actions/actions';
import { profileDetailQueryKey } from '@/features/profile/overview/profileDetailQuery';
import { normalizeOptional, resolveBankName } from '@/features/profile/overview/useProfilePage.helpers';
import type { ProfileFormValues } from '@/features/profile/overview/useProfilePage.types';
import { useAuthStore } from '@/features/auth/public';

interface SubmitProfileUpdateParams {
 avatarPreview: string | null;
 data: ProfileFormValues;
 isTarotReader: boolean;
 payoutBankOptions: PayoutBankOption[];
 payoutBanksQueryKey: readonly unknown[];
 queryClient: QueryClient;
 setErrorMsg: (value: string) => void;
 setSuccessMsg: (value: string) => void;
 t: (key: string) => string;
}

export async function submitProfileUpdate(params: SubmitProfileUpdateParams): Promise<void> {
 const {
  avatarPreview,
  data,
  isTarotReader,
  payoutBankOptions,
  payoutBanksQueryKey,
  queryClient,
  setErrorMsg,
  setSuccessMsg,
  t,
 } = params;

 setSuccessMsg('');
 setErrorMsg('');

 try {
  const payoutBankName = isTarotReader ? resolveBankName(payoutBankOptions, data.payoutBankBin) : undefined;
  if (isTarotReader && normalizeOptional(data.payoutBankBin) && !payoutBankName) {
   setErrorMsg(t('validation.bank_required'));
   return;
  }

  const result = await updateProfileAction({
   displayName: data.displayName.trim(),
   dateOfBirth: data.dateOfBirth.trim(),
   payoutBankBin: isTarotReader ? normalizeOptional(data.payoutBankBin) : undefined,
   payoutBankName,
   payoutBankAccountNumber: isTarotReader ? normalizeOptional(data.payoutBankAccountNumber) : undefined,
   payoutBankAccountHolder: isTarotReader ? normalizeOptional(data.payoutBankAccountHolder) : undefined,
  });
  if (!result.success) {
   setErrorMsg(result.error);
   return;
  }

  setSuccessMsg(t('successMsg'));
  useAuthStore.getState().updateUser({ displayName: data.displayName, avatarUrl: avatarPreview || null });
  await Promise.all([
   queryClient.invalidateQueries({ queryKey: profileDetailQueryKey }),
   queryClient.invalidateQueries({ queryKey: payoutBanksQueryKey }),
  ]);
 } catch {
  setErrorMsg(t('errorSave'));
 }
}
