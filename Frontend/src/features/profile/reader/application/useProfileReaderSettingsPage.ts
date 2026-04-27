'use client';

import { useCallback, useMemo } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useAuthStore } from '@/store/authStore';
import { getReaderProfile, updateReaderProfile, updateReaderStatus } from '@/features/reader/public';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { normalizeReaderStatus, type ReaderStatus } from '@/features/reader/domain/readerStatus';
import { isReaderSpecialtyValue, type ReaderSpecialtyValue } from '@/features/reader/domain/readerSpecialties';
import { hasAtLeastOneSocialLink, normalizeOptionalSocialUrl } from '@/features/reader/domain/readerSocialLinks';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

interface ReaderSettingsDraft {
 bioVi: string;
 diamondPerQuestion: number;
 specialties: ReaderSpecialtyValue[];
 yearsOfExperience: number;
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
 status: ReaderStatus;
}

interface ReaderSettingsSubmitPayload {
 bio: string;
 specialties: ReaderSpecialtyValue[];
 years: number;
 price: number;
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
}

function toDraft(
 profile: ReaderProfile | null | undefined,
 minYearsOfExperience: number,
 minDiamondPerQuestion: number,
 defaultDiamondPerQuestion: number,
): ReaderSettingsDraft {
 return {
  bioVi: profile?.bioVi || '',
  diamondPerQuestion: Math.max(minDiamondPerQuestion, profile?.diamondPerQuestion || defaultDiamondPerQuestion),
  specialties: (profile?.specialties ?? []).filter(isReaderSpecialtyValue),
  yearsOfExperience: Math.max(minYearsOfExperience, profile?.yearsOfExperience || minYearsOfExperience),
  facebookUrl: normalizeOptionalSocialUrl(profile?.facebookUrl),
  instagramUrl: normalizeOptionalSocialUrl(profile?.instagramUrl),
  tikTokUrl: normalizeOptionalSocialUrl(profile?.tikTokUrl),
  status: normalizeReaderStatus(profile?.status),
 };
}

export function useProfileReaderSettingsPage(t: TranslateFn) {
 const queryClient = useQueryClient();
 const user = useAuthStore((state) => state.user);
 const isTarotReader = user?.role === 'tarot_reader';
 const runtimePoliciesQuery = useRuntimePolicies();
 const readerPolicy = runtimePoliciesQuery.data?.reader;
 const minYearsOfExperience = readerPolicy?.minYearsOfExperience ?? 0;
 const minDiamondPerQuestion = readerPolicy?.minDiamondPerQuestion ?? 0;
 const defaultDiamondPerQuestion = readerPolicy?.defaultDiamondPerQuestion ?? minDiamondPerQuestion;
 const readerPolicyReady = Boolean(readerPolicy);

 const profileQuery = useQuery({
  queryKey: ['reader-profile-settings', user?.id],
  enabled: Boolean(user?.id && isTarotReader),
  queryFn: async () => {
   if (!user?.id) {
    return null;
   }

   const result = await getReaderProfile(user.id);
   return result.success ? result.data ?? null : null;
  },
 });

 const saveMutation = useMutation({ mutationFn: updateReaderProfile });
 const statusMutation = useMutation({ mutationFn: updateReaderStatus });

 const effectiveValues = useMemo(
  () => toDraft(profileQuery.data, minYearsOfExperience, minDiamondPerQuestion, defaultDiamondPerQuestion),
  [defaultDiamondPerQuestion, minDiamondPerQuestion, minYearsOfExperience, profileQuery.data],
 );

 const handleSave = useCallback(async (values: ReaderSettingsSubmitPayload) => {
  if (!isTarotReader) {
   toast.error(t('reader.toast_not_found'));
   return;
  }

  if (!readerPolicyReady) {
   toast.error(t('reader.toast_policy_unavailable'));
   return;
  }

  if (values.specialties.length === 0) {
   toast.error(t('reader.toast_specialties_required'));
   return;
  }

  if (!hasAtLeastOneSocialLink(values)) {
   toast.error(t('reader.toast_social_required'));
   return;
  }

  const result = await saveMutation.mutateAsync({
   bioVi: values.bio,
   diamondPerQuestion: values.price,
   specialties: values.specialties,
   yearsOfExperience: values.years,
   facebookUrl: normalizeOptionalSocialUrl(values.facebookUrl) || undefined,
   instagramUrl: normalizeOptionalSocialUrl(values.instagramUrl) || undefined,
   tikTokUrl: normalizeOptionalSocialUrl(values.tikTokUrl) || undefined,
  });

  if (!result.success) {
   toast.error(t('reader.toast_save_fail'));
   return;
  }

  toast.success(t('reader.toast_save_success'));
  await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', user?.id] });
 }, [isTarotReader, queryClient, readerPolicyReady, saveMutation, t, user?.id]);

 const handleStatusChange = useCallback(async (newStatus: ReaderStatus) => {
  if (!isTarotReader) {
   return;
  }

  const result = await statusMutation.mutateAsync(newStatus);

  if (!result.success) {
   toast.error(t('reader.toast_status_update_fail'));
   return;
  }

  await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', user?.id] });
  toast.success(t('reader.toast_status_updated'));
 }, [isTarotReader, queryClient, statusMutation, t, user?.id]);

 return {
  loading: profileQuery.isLoading || profileQuery.isFetching,
  saving: saveMutation.isPending || statusMutation.isPending,
  bioVi: effectiveValues.bioVi,
  diamondPerQuestion: effectiveValues.diamondPerQuestion,
  specialties: effectiveValues.specialties,
  yearsOfExperience: effectiveValues.yearsOfExperience,
  minYearsOfExperience,
  facebookUrl: effectiveValues.facebookUrl,
  instagramUrl: effectiveValues.instagramUrl,
  tikTokUrl: effectiveValues.tikTokUrl,
  minDiamondPerQuestion,
  readerPolicyReady,
  status: effectiveValues.status,
  handleSave,
  handleStatusChange,
 };
}
