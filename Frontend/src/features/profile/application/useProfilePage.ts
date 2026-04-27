'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import * as z from 'zod';
import {
 getPayoutBanksAction,
 getProfileAction,
 updateProfileAction,
 type PayoutBankOption,
 type ProfileDto,
} from '@/features/profile/application/actions';
import { uploadProfileAvatar } from '@/features/profile/application/uploadProfileAvatar';
import { getMyReaderRequest, type MyReaderRequest } from '@/features/reader/public';
import { useRouter } from '@/i18n/routing';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { useAuthStore } from '@/store/authStore';
import { toast } from 'react-hot-toast';

interface ProfileFormValues {
  dateOfBirth: string;
  displayName: string;
  payoutBankBin: string;
  payoutBankAccountNumber: string;
  payoutBankAccountHolder: string;
}

export function useProfilePage() {
  const t = useTranslations('Profile');
  const tCommon = useTranslations('Common');
  const router = useRouter();
  const queryClient = useQueryClient();
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const user = useAuthStore((state) => state.user);
  const [successMsg, setSuccessMsg] = useState('');
  const [errorMsg, setErrorMsg] = useState('');
  const [avatarPreview, setAvatarPreview] = useState<string | null>(null);
  const [avatarUploadProgress, setAvatarUploadProgress] = useState(0);
  const [avatarUploading, setAvatarUploading] = useState(false);
  const profileQueryKey = userStateQueryKeys.profile.me();
  const payoutBanksQueryKey = userStateQueryKeys.profile.payoutBanks();
  const isAdmin = user?.role === 'admin';
  const isTarotReader = user?.role === 'tarot_reader';
  const MIN_ACCOUNT_NUMBER_LENGTH = 6;
  const MAX_ACCOUNT_NUMBER_LENGTH = 32;

  const profileSchema = useMemo(() => z.object({
    displayName: z.string().min(2, t('validation.display_name_min')),
    dateOfBirth: z.string().refine((date) => !isNaN(Date.parse(date)), { message: t('validation.date_of_birth_invalid') }),
    payoutBankBin: z.string(),
    payoutBankAccountNumber: z.string().max(MAX_ACCOUNT_NUMBER_LENGTH, t('validation.account_number_invalid')),
    payoutBankAccountHolder: z.string().max(120, t('validation.account_holder_invalid')),
  }).superRefine((value, context) => {
    if (!isTarotReader) {
      return;
    }

    const hasAnyPayoutField = Boolean(
      value.payoutBankBin.trim()
      || value.payoutBankAccountNumber.trim()
      || value.payoutBankAccountHolder.trim(),
    );

    if (!hasAnyPayoutField) {
      return;
    }

    if (!value.payoutBankBin.trim()) {
      context.addIssue({
        code: z.ZodIssueCode.custom,
        message: t('validation.bank_required'),
        path: ['payoutBankBin'],
      });
    }

    const accountNumber = value.payoutBankAccountNumber.trim();
    if (!accountNumber) {
      context.addIssue({
        code: z.ZodIssueCode.custom,
        message: t('validation.account_number_required'),
        path: ['payoutBankAccountNumber'],
      });
    } else if (!/^\d+$/.test(accountNumber)
      || accountNumber.length < MIN_ACCOUNT_NUMBER_LENGTH
      || accountNumber.length > MAX_ACCOUNT_NUMBER_LENGTH) {
      context.addIssue({
        code: z.ZodIssueCode.custom,
        message: t('validation.account_number_invalid'),
        path: ['payoutBankAccountNumber'],
      });
    }

    const accountHolder = value.payoutBankAccountHolder.trim();
    if (!accountHolder) {
      context.addIssue({
        code: z.ZodIssueCode.custom,
        message: t('validation.account_holder_required'),
        path: ['payoutBankAccountHolder'],
      });
    } else if (!isUppercaseNoAccent(accountHolder)) {
      context.addIssue({
        code: z.ZodIssueCode.custom,
        message: t('validation.account_holder_invalid'),
        path: ['payoutBankAccountHolder'],
      });
    }
  }), [MAX_ACCOUNT_NUMBER_LENGTH, MIN_ACCOUNT_NUMBER_LENGTH, isTarotReader, t]);

  const { register, handleSubmit, setValue, formState: { errors, isSubmitting } } = useForm<ProfileFormValues>({
    resolver: zodResolver(profileSchema),
  });

  const profileQuery = useQuery<{ profile: ProfileDto | null; error: string }>({
    queryKey: profileQueryKey,
    enabled: isAuthenticated,
    queryFn: async () => {
      const result = await getProfileAction();
      return result.success ? { profile: result.data ?? null, error: '' } : { profile: null, error: result.error };
    },
  });

  const payoutBanksQuery = useQuery<{ options: PayoutBankOption[]; error: string }>({
    queryKey: payoutBanksQueryKey,
    enabled: isAuthenticated && isTarotReader,
    queryFn: async () => {
      const result = await getPayoutBanksAction();
      return result.success ? { options: result.data ?? [], error: '' } : { options: [], error: result.error };
    },
  });

  const payoutBankOptions = payoutBanksQuery.data?.options ?? [];

  useEffect(() => {
    const payload = profileQuery.data;
    if (!payload || payload.error || !payload.profile) return;

    setValue('displayName', payload.profile.displayName);
    setValue('payoutBankBin', payload.profile.payoutBankBin ?? '');
    setValue('payoutBankAccountNumber', payload.profile.payoutBankAccountNumber ?? '');
    setValue('payoutBankAccountHolder', payload.profile.payoutBankAccountHolder ?? '');
    setAvatarPreview(payload.profile.avatarUrl || null);
    if (!payload.profile.dateOfBirth) return;

    const normalizedDate = toDateInputValue(payload.profile.dateOfBirth);
    if (normalizedDate) {
      setValue('dateOfBirth', normalizedDate);
    }
  }, [profileQuery.data, setValue]);

  const readerRequestQuery = useQuery<MyReaderRequest | null>({
    queryKey: userStateQueryKeys.reader.myRequest(),
    enabled: isAuthenticated && !!user && !isTarotReader && !isAdmin,
    queryFn: async () => {
      const result = await getMyReaderRequest();
      return result.success ? result.data ?? null : null;
    },
  });

  const onSubmit = async (data: ProfileFormValues) => {
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
        queryClient.invalidateQueries({ queryKey: profileQueryKey }),
        queryClient.invalidateQueries({ queryKey: payoutBanksQueryKey }),
      ]);
    } catch {
      setErrorMsg(t('errorSave'));
    }
  };

  const handleAvatarSelect = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    event.target.value = '';
    if (!file) return;

    const previousAvatarPreview = avatarPreview;
    const optimisticPreview = URL.createObjectURL(file);

    setAvatarPreview(optimisticPreview);
    setAvatarUploadProgress(0);
    setAvatarUploading(true);
    setErrorMsg('');
    try {
      const uploadResult = await uploadProfileAvatar({
        file,
        profileQueryKey,
        queryClient,
        t,
        onProgress: setAvatarUploadProgress,
      });

      if (!uploadResult.success) {
        const err = uploadResult.error || t('avatar_upload_error') || 'Không thể tải ảnh lên';
        setAvatarPreview(previousAvatarPreview);
        setErrorMsg(err);
        toast.error(err);
        revokeObjectUrlIfNeeded(optimisticPreview);
        return;
      }

      setAvatarPreview(uploadResult.avatarUrl);
      revokeObjectUrlIfNeeded(optimisticPreview);
      setSuccessMsg(uploadResult.message);
      toast.success(uploadResult.message);
      useAuthStore.getState().updateUser({ avatarUrl: uploadResult.avatarUrl });
    } catch {
      setAvatarPreview(previousAvatarPreview);
      const err = t('avatar_upload_error') || 'Không thể tải ảnh lên';
      setErrorMsg(err);
      toast.error(err);
      revokeObjectUrlIfNeeded(optimisticPreview);
    } finally {
      setAvatarUploading(false);
      setAvatarUploadProgress(0);
    }
  };

  return {
    t, tCommon, router, user,
    profileData: profileQuery.data?.profile ?? null,
    loading: profileQuery.isLoading || profileQuery.isFetching,
    successMsg,
    errorMsg: errorMsg || profileQuery.data?.error || '',
    payoutBanksError: payoutBanksQuery.data?.error || '',
    payoutBankOptions,
    isTarotReader,
    readerRequest: readerRequestQuery.data ?? null,
    readerRequestLoading: readerRequestQuery.isLoading || readerRequestQuery.isFetching,
    register, handleSubmit, errors, isSubmitting, onSubmit,
    avatarPreview, avatarUploadProgress, avatarUploading, handleAvatarSelect,
  };
}

function revokeObjectUrlIfNeeded(value: string | null) {
  if (!value || !value.startsWith('blob:')) {
    return;
  }

  URL.revokeObjectURL(value);
}

function normalizeOptional(value: string): string | null {
  const normalized = value.trim();
  return normalized ? normalized : null;
}

function resolveBankName(options: PayoutBankOption[], bankBin: string): string | null {
  const normalizedBankBin = bankBin.trim();
  if (!normalizedBankBin) {
    return null;
  }

  return options.find((item) => item.bankBin === normalizedBankBin)?.bankName ?? null;
}

function isUppercaseNoAccent(value: string): boolean {
  const normalized = value.trim();
  if (!normalized || normalized !== normalized.toUpperCase()) {
    return false;
  }

  if (/[\u0300-\u036f]/.test(normalized.normalize('NFD'))) {
    return false;
  }

  return /^[A-Z ]+$/.test(normalized);
}

function toDateInputValue(rawDate: string): string | null {
 const dateOnlyMatch = /^(\d{4})-(\d{2})-(\d{2})/.exec(rawDate.trim());
 if (dateOnlyMatch) {
  return `${dateOnlyMatch[1]}-${dateOnlyMatch[2]}-${dateOnlyMatch[3]}`;
 }

 const parsed = new Date(rawDate);
 if (Number.isNaN(parsed.getTime())) {
  return null;
 }

 const yyyy = parsed.getUTCFullYear();
 const mm = String(parsed.getUTCMonth() + 1).padStart(2, '0');
 const dd = String(parsed.getUTCDate()).padStart(2, '0');
 return `${yyyy}-${mm}-${dd}`;
}
