'use client';

import { useCallback, useMemo, useState, type FormEvent } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useAuthStore } from '@/store/authStore';
import {
 getReaderProfile,
 updateReaderProfile,
 updateReaderStatus,
} from '@/features/reader/public';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { normalizeReaderStatus, type ReaderStatus } from '@/features/reader/domain/readerStatus';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

interface ReaderSettingsDraft {
 bioVi: string;
 diamondPerQuestion: number;
 specialtiesStr: string;
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

function toDraft(profile: ReaderProfile | null | undefined): ReaderSettingsDraft {
 return {
  bioVi: profile?.bioVi || '',
  diamondPerQuestion: profile?.diamondPerQuestion || 100,
  specialtiesStr: profile?.specialties?.join(', ') || '',
  status: normalizeReaderStatus(profile?.status),
 };
}

function resolveSpecialties(input: string): string[] {
 return input
  .split(',')
  .map((item) => item.trim())
  .filter((item) => item.length > 0);
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

 const setSpecialtiesStr = useCallback((value: string) => {
  updateDraft((current) => ({ ...current, specialtiesStr: value }));
 }, [updateDraft]);

 const handleSave = useCallback(async (event: FormEvent) => {
  event.preventDefault();
  if (!isTarotReader) {
   toast.error(t('reader.toast_not_found'), { style: NEUTRAL_TOAST_STYLE });
   return;
  }

  const result = await saveMutation.mutateAsync({
   bioVi: effectiveValues.bioVi,
   diamondPerQuestion: effectiveValues.diamondPerQuestion,
   specialties: resolveSpecialties(effectiveValues.specialtiesStr),
  });

  if (!result.success) {
   toast.error(t('reader.toast_save_fail'), { style: DANGER_TOAST_STYLE });
   return;
  }

  toast.success(t('reader.toast_save_success'), { style: SUCCESS_TOAST_STYLE });
  setDraft(null);
  await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', user?.id] });
 }, [effectiveValues.bioVi, effectiveValues.diamondPerQuestion, effectiveValues.specialtiesStr, isTarotReader, queryClient, saveMutation, t, user?.id]);

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
  specialtiesStr: effectiveValues.specialtiesStr,
  setSpecialtiesStr,
  status: effectiveValues.status,
  handleSave,
  handleStatusChange,
 };
}
