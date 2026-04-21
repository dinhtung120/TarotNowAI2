'use client';

import { useCallback, useMemo, useState, type FormEvent } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useAuthStore } from '@/store/authStore';
import { getReaderProfile, updateReaderProfile, updateReaderStatus } from '@/features/reader/public';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { normalizeReaderStatus, type ReaderStatus } from '@/features/reader/domain/readerStatus';
import { isReaderSpecialtyValue, type ReaderSpecialtyValue } from '@/features/reader/domain/readerSpecialties';
import { hasAtLeastOneSocialLink, normalizeOptionalSocialUrl } from '@/features/reader/domain/readerSocialLinks';

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

const NEUTRAL_TOAST_STYLE = {
 background: 'var(--bg-elevated)',
 color: 'var(--text-primary)',
 border: '1px solid var(--border-default)',
} as const;

const SUCCESS_TOAST_STYLE = {
 background: 'var(--success-bg)',
 color: 'var(--success)',
 border: '1px solid var(--success)',
} as const;

const DANGER_TOAST_STYLE = {
 background: 'var(--danger-bg)',
 color: 'var(--danger)',
 border: '1px solid var(--danger)',
} as const;

const DEFAULT_DIAMOND_PER_QUESTION = 100;
const DEFAULT_YEARS_OF_EXPERIENCE = 1;

function toDraft(profile: ReaderProfile | null | undefined): ReaderSettingsDraft {
 return {
  bioVi: profile?.bioVi || '',
  diamondPerQuestion: Math.max(50, profile?.diamondPerQuestion || DEFAULT_DIAMOND_PER_QUESTION),
  specialties: (profile?.specialties ?? []).filter(isReaderSpecialtyValue),
  yearsOfExperience: Math.max(1, profile?.yearsOfExperience || DEFAULT_YEARS_OF_EXPERIENCE),
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
 const [draft, setDraft] = useState<ReaderSettingsDraft | null>(null);

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
  () => draft ?? toDraft(profileQuery.data),
  [draft, profileQuery.data],
 );

 const updateDraft = useCallback((updater: (current: ReaderSettingsDraft) => ReaderSettingsDraft) => {
  setDraft((previous) => updater(previous ?? toDraft(profileQuery.data)));
 }, [profileQuery.data]);

 const setBioVi = useCallback((value: string) => {
  updateDraft((current) => ({ ...current, bioVi: value }));
 }, [updateDraft]);

 const setDiamondPerQuestion = useCallback((value: number) => {
  updateDraft((current) => ({ ...current, diamondPerQuestion: value }));
 }, [updateDraft]);

 const setSpecialties = useCallback((value: ReaderSpecialtyValue[]) => {
  updateDraft((current) => ({ ...current, specialties: value }));
 }, [updateDraft]);

 const setYearsOfExperience = useCallback((value: number) => {
  updateDraft((current) => ({ ...current, yearsOfExperience: value }));
 }, [updateDraft]);

 const setFacebookUrl = useCallback((value: string) => {
  updateDraft((current) => ({ ...current, facebookUrl: value }));
 }, [updateDraft]);

 const setInstagramUrl = useCallback((value: string) => {
  updateDraft((current) => ({ ...current, instagramUrl: value }));
 }, [updateDraft]);

 const setTikTokUrl = useCallback((value: string) => {
  updateDraft((current) => ({ ...current, tikTokUrl: value }));
 }, [updateDraft]);

 const handleSave = useCallback(async (event: FormEvent) => {
  event.preventDefault();
  if (!isTarotReader) {
   toast.error(t('reader.toast_not_found'), { style: NEUTRAL_TOAST_STYLE });
   return;
  }

  if (effectiveValues.specialties.length === 0) {
   toast.error(t('reader.toast_specialties_required'), { style: DANGER_TOAST_STYLE });
   return;
  }

  if (!hasAtLeastOneSocialLink(effectiveValues)) {
   toast.error(t('reader.toast_social_required'), { style: DANGER_TOAST_STYLE });
   return;
  }

  const result = await saveMutation.mutateAsync({
   bioVi: effectiveValues.bioVi,
   diamondPerQuestion: effectiveValues.diamondPerQuestion,
   specialties: effectiveValues.specialties,
   yearsOfExperience: effectiveValues.yearsOfExperience,
   facebookUrl: normalizeOptionalSocialUrl(effectiveValues.facebookUrl) || undefined,
   instagramUrl: normalizeOptionalSocialUrl(effectiveValues.instagramUrl) || undefined,
   tikTokUrl: normalizeOptionalSocialUrl(effectiveValues.tikTokUrl) || undefined,
  });

  if (!result.success) {
   toast.error(t('reader.toast_save_fail'), { style: DANGER_TOAST_STYLE });
   return;
  }

  toast.success(t('reader.toast_save_success'), { style: SUCCESS_TOAST_STYLE });
  setDraft(null);
  await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', user?.id] });
 }, [effectiveValues, isTarotReader, queryClient, saveMutation, t, user?.id]);

 const handleStatusChange = useCallback(async (newStatus: ReaderStatus) => {
  if (!isTarotReader) {
   return;
  }

  const previousStatus = effectiveValues.status;
  updateDraft((current) => ({ ...current, status: newStatus }));
  const result = await statusMutation.mutateAsync(newStatus);

  if (!result.success) {
   updateDraft((current) => ({ ...current, status: previousStatus }));
   toast.error(t('reader.toast_status_update_fail'), { style: DANGER_TOAST_STYLE });
   return;
  }

  await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', user?.id] });
  toast.success(t('reader.toast_status_updated'), { style: SUCCESS_TOAST_STYLE });
 }, [effectiveValues.status, isTarotReader, queryClient, statusMutation, t, updateDraft, user?.id]);

 return {
  loading: profileQuery.isLoading || profileQuery.isFetching,
  saving: saveMutation.isPending || statusMutation.isPending,
  bioVi: effectiveValues.bioVi,
  setBioVi,
  diamondPerQuestion: effectiveValues.diamondPerQuestion,
  setDiamondPerQuestion,
  specialties: effectiveValues.specialties,
  setSpecialties,
  yearsOfExperience: effectiveValues.yearsOfExperience,
  setYearsOfExperience,
  facebookUrl: effectiveValues.facebookUrl,
  setFacebookUrl,
  instagramUrl: effectiveValues.instagramUrl,
  setInstagramUrl,
  tikTokUrl: effectiveValues.tikTokUrl,
  setTikTokUrl,
  status: effectiveValues.status,
  handleSave,
  handleStatusChange,
 };
}
