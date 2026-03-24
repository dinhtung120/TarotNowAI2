'use client';

import { useEffect, useMemo, useState } from 'react';
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
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const user = useAuthStore((state) => state.user);

 const [profileData, setProfileData] = useState<ProfileDto | null>(null);
 const [loading, setLoading] = useState(true);
 const [successMsg, setSuccessMsg] = useState('');
 const [errorMsg, setErrorMsg] = useState('');
 const [readerRequest, setReaderRequest] = useState<MyReaderRequest | null>(null);
 const [readerRequestLoading, setReaderRequestLoading] = useState(false);

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

 useEffect(() => {
  if (!isAuthenticated) return;

  async function fetchProfile() {
   try {
   const result = await getProfileAction();

   if (!result.success) {
    setErrorMsg(result.error);
    return;
   }

	   if (result.data) {
	    const data = result.data;
	     setProfileData(data);

     setValue('displayName', data.displayName);
     setValue('avatarUrl', data.avatarUrl || '');

     if (data.dateOfBirth) {
      const dateObj = new Date(data.dateOfBirth);
      const yyyy = dateObj.getFullYear();
      const mm = String(dateObj.getMonth() + 1).padStart(2, '0');
      const dd = String(dateObj.getDate()).padStart(2, '0');
      setValue('dateOfBirth', `${yyyy}-${mm}-${dd}`);
     }
    }
   } catch {
    setErrorMsg(t('errorLoad'));
   } finally {
    setLoading(false);
   }
  }

  void fetchProfile();
 }, [isAuthenticated, setValue, t]);

 useEffect(() => {
  if (!isAuthenticated || !user) return;
  if (user.role === 'reader' || user.role === 'admin') return;

  setReaderRequestLoading(true);
  getMyReaderRequest().then((result) => {
   setReaderRequest(result.success ? result.data ?? null : null);
   setReaderRequestLoading(false);
  });
  // eslint-disable-next-line react-hooks/exhaustive-deps
 }, [isAuthenticated, user?.role]);

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

	   const profileResult = await getProfileAction();
	   if (profileResult.success && profileResult.data) {
	    setProfileData(profileResult.data);
	   }
  } catch {
   setErrorMsg(t('errorSave'));
  }
 };

 return {
  t,
  tCommon,
  router,
  user,
  profileData,
  loading,
  successMsg,
  errorMsg,
  readerRequest,
  readerRequestLoading,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 };
}
