'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';
import { useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import { getProfileAction, updateProfileAction, type ProfileDto } from '@/features/profile/application/actions';
import { getMyReaderRequest, type MyReaderRequest } from '@/features/reader/public';
import { useAuthGuard } from '@/shared/application/hooks/useAuthGuard';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';

interface ProfileFormValues {
 displayName: string;
 avatarUrl?: string;
 dateOfBirth: string;
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
 const profileQueryKey = ['profile', 'me'] as const;

 const profileSchema = useMemo(
  () =>
   z.object({
    displayName: z.string().min(2, t('validation.display_name_min')),
    avatarUrl: z.string().url(t('validation.avatar_url_invalid')).optional().or(z.literal('')),
    dateOfBirth: z.string().refine((date) => !isNaN(Date.parse(date)), {
     message: t('validation.date_of_birth_invalid'),
    }),
   }),
  [t]
 );

 const {
  register,
  handleSubmit,
  setValue,
  formState: { errors, isSubmitting },
 } = useForm<ProfileFormValues>({
  resolver: zodResolver(profileSchema),
 });

 useAuthGuard(isAuthenticated);

 const profileQuery = useQuery<{ profile: ProfileDto | null; error: string }>({
  queryKey: profileQueryKey,
  enabled: isAuthenticated,
  queryFn: async () => {
   const result = await getProfileAction();
   if (!result.success) {
    return { profile: null, error: result.error };
   }
   return { profile: result.data ?? null, error: '' };
  },
 });

 useEffect(() => {
  const payload = profileQuery.data;
  if (!payload) return;

  if (payload.error) return;
  if (!payload.profile) return;

  const data = payload.profile;
  setValue('displayName', data.displayName);
  setValue('avatarUrl', data.avatarUrl || '');

  if (data.dateOfBirth) {
   const dateObj = new Date(data.dateOfBirth);
   const yyyy = dateObj.getFullYear();
   const mm = String(dateObj.getMonth() + 1).padStart(2, '0');
   const dd = String(dateObj.getDate()).padStart(2, '0');
   setValue('dateOfBirth', `${yyyy}-${mm}-${dd}`);
  }
 }, [profileQuery.data, setValue]);

 const shouldLoadReaderRequest =
  isAuthenticated && !!user && user.role !== 'reader' && user.role !== 'admin';
 const readerRequestQuery = useQuery<MyReaderRequest | null>({
  queryKey: ['reader', 'my-request'],
  enabled: shouldLoadReaderRequest,
  queryFn: async () => {
   const result = await getMyReaderRequest();
   return result.success ? result.data ?? null : null;
  },
 });

 const onSubmit = async (data: ProfileFormValues) => {
  setSuccessMsg('');
  setErrorMsg('');
  try {
   const result = await updateProfileAction({
    displayName: data.displayName,
    avatarUrl: data.avatarUrl || null,
    dateOfBirth: new Date(data.dateOfBirth).toISOString(),
   });

   if (!result.success) {
    setErrorMsg(result.error);
    return;
   }

   setSuccessMsg(t('successMsg'));
   await queryClient.invalidateQueries({ queryKey: profileQueryKey });
  } catch {
   setErrorMsg(t('errorSave'));
  }
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
  readerRequest: readerRequestQuery.data ?? null,
  readerRequestLoading: readerRequestQuery.isLoading || readerRequestQuery.isFetching,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 };
}
