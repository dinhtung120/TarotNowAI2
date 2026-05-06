'use client';

import { useMemo } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/features/auth/public';
import { getReaderProfile } from '@/features/reader/public';
import type { ReaderProfile } from '@/features/reader/shared';
import { normalizeReaderStatus } from '@/features/reader/shared/readerStatus';
import { isReaderSpecialtyValue } from '@/features/reader/shared/readerSpecialties';
import { normalizeOptionalSocialUrl } from '@/features/reader/shared/readerSocialLinks';
import { useProfileReaderSettingsHandlers } from '@/features/profile/reader-settings/useProfileReaderSettings.handlers';
import type {
 ReaderSettingsDraft,
 TranslateFn,
} from '@/features/profile/reader-settings/useProfileReaderSettings.types';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';

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
 const { handleSave, handleStatusChange, saving } = useProfileReaderSettingsHandlers({
  t,
  queryClient,
  isTarotReader,
  readerPolicyReady,
  userId: user?.id,
 });

 const effectiveValues = useMemo(
  () => toDraft(profileQuery.data, minYearsOfExperience, minDiamondPerQuestion, defaultDiamondPerQuestion),
  [defaultDiamondPerQuestion, minDiamondPerQuestion, minYearsOfExperience, profileQuery.data],
 );

 return {
  loading: profileQuery.isLoading || profileQuery.isFetching,
  saving,
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
