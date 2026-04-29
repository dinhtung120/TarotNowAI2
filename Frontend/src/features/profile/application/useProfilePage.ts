'use client';

import { useMemo, useState, type ChangeEvent } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import {
 getPayoutBanksAction,
 type PayoutBankOption,
} from '@/features/profile/application/actions';
import { fetchProfileDetail, profileDetailQueryKey } from '@/features/profile/application/profileDetailQuery';
import { handleProfileAvatarSelect } from '@/features/profile/application/useProfilePage.avatar';
import { buildProfileFormValues } from '@/features/profile/application/useProfilePage.helpers';
import { submitProfileUpdate } from '@/features/profile/application/useProfilePage.submit';
import type { ProfileFormValues } from '@/features/profile/application/useProfilePage.types';
import { createProfileSchema } from '@/features/profile/application/useProfilePage.validation';
import { getMyReaderRequest, type MyReaderRequest } from '@/features/reader/public';
import { useRouter } from '@/i18n/routing';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { useHydrateFormOnce } from '@/shared/application/hooks/useHydrateFormOnce';
import { useAuthStore } from '@/store/authStore';

export function useProfilePage() {
 const t = useTranslations('Profile');
 const tCommon = useTranslations('Common');
 const router = useRouter();
 const queryClient = useQueryClient();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const user = useAuthStore((state) => state.user);
 const [successMsg, setSuccessMsg] = useState('');
 const [errorMsg, setErrorMsg] = useState('');
 const [avatarPreviewOverride, setAvatarPreviewOverride] = useState<{
  profileId: string | null;
  value: string | null;
 }>({ profileId: null, value: null });
 const [avatarUploadProgress, setAvatarUploadProgress] = useState(0);
 const [avatarUploading, setAvatarUploading] = useState(false);
 const payoutBanksQueryKey = userStateQueryKeys.profile.payoutBanks();
 const isAdmin = user?.role === 'admin';
 const isTarotReader = user?.role === 'tarot_reader';

 const profileSchema = useMemo(() => createProfileSchema({ isTarotReader, t }), [isTarotReader, t]);
 const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<ProfileFormValues>({
  resolver: zodResolver(profileSchema),
 });

 const profileQuery = useQuery({
  queryKey: profileDetailQueryKey,
  enabled: isAuthenticated,
  queryFn: fetchProfileDetail,
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
 const activeProfileId = profileQuery.data?.profile?.id ?? null;
 const profileAvatar = profileQuery.data?.profile && !profileQuery.data.error
  ? profileQuery.data.profile.avatarUrl || null
  : null;
 const avatarPreview = avatarPreviewOverride.profileId === activeProfileId
  ? (avatarPreviewOverride.value ?? profileAvatar)
  : profileAvatar;
 const setAvatarPreviewForActiveProfile = (value: string | null) => {
  setAvatarPreviewOverride({ profileId: activeProfileId, value });
 };
 const profileFormValues = useMemo(
  () => buildProfileFormValues(profileQuery.data?.profile, profileQuery.data?.error),
  [profileQuery.data?.error, profileQuery.data?.profile],
 );

 useHydrateFormOnce({
  identity: profileQuery.data?.profile?.id ?? null,
  reset,
  values: profileFormValues,
 });

 const readerRequestQuery = useQuery<MyReaderRequest | null>({
  queryKey: userStateQueryKeys.reader.myRequest(),
  enabled: isAuthenticated && !!user && !isTarotReader && !isAdmin,
  queryFn: async () => {
   const result = await getMyReaderRequest();
   return result.success ? result.data ?? null : null;
  },
 });

 const onSubmit = async (data: ProfileFormValues) => {
  await submitProfileUpdate({
   avatarPreview,
   data,
   isTarotReader,
   payoutBankOptions,
   payoutBanksQueryKey,
   queryClient,
   setErrorMsg,
   setSuccessMsg,
   t,
  });
 };

 const handleAvatarSelect = async (event: ChangeEvent<HTMLInputElement>) => {
  await handleProfileAvatarSelect({
   avatarPreview,
   event,
   queryClient,
   setAvatarPreview: setAvatarPreviewForActiveProfile,
   setAvatarUploadProgress,
   setAvatarUploading,
   setErrorMsg,
   setSuccessMsg,
   t,
  });
 };

 return {
  t,
  tCommon,
  router,
  user,
  profileData: profileQuery.data?.profile ?? null,
  loading: profileQuery.isLoading || profileQuery.isFetching,
  successMsg,
  errorMsg: errorMsg || profileQuery.data?.error || '',
  payoutBanksError: payoutBanksQuery.data?.error || '',
  payoutBankOptions,
  isTarotReader,
  readerRequest: readerRequestQuery.data ?? null,
  readerRequestLoading: readerRequestQuery.isLoading || readerRequestQuery.isFetching,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
  avatarPreview,
  avatarUploadProgress,
  avatarUploading,
  handleAvatarSelect,
 };
}
