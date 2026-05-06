'use client';

import { useCallback } from 'react';
import { useMutation, type QueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { updateReaderProfile, updateReaderStatus } from '@/features/reader/public';
import type { ReaderStatus } from '@/features/reader/shared/readerStatus';
import { hasAtLeastOneSocialLink, normalizeOptionalSocialUrl } from '@/features/reader/shared/readerSocialLinks';
import type {
 ReaderSettingsSubmitPayload,
 TranslateFn,
} from '@/features/profile/reader-settings/useProfileReaderSettings.types';

interface UseProfileReaderSettingsHandlersOptions {
 t: TranslateFn;
 queryClient: QueryClient;
 isTarotReader: boolean;
 readerPolicyReady: boolean;
 userId: string | undefined;
}

export function useProfileReaderSettingsHandlers(options: UseProfileReaderSettingsHandlersOptions) {
 const { t, queryClient, isTarotReader, readerPolicyReady, userId } = options;
 const saveMutation = useMutation({ mutationFn: updateReaderProfile });
 const statusMutation = useMutation({ mutationFn: updateReaderStatus });

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
  await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', userId] });
 }, [isTarotReader, queryClient, readerPolicyReady, saveMutation, t, userId]);

 const handleStatusChange = useCallback(async (newStatus: ReaderStatus) => {
  if (!isTarotReader) {
   return;
  }

  const result = await statusMutation.mutateAsync(newStatus);
  if (!result.success) {
   toast.error(t('reader.toast_status_update_fail'));
   return;
  }

  await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', userId] });
  toast.success(t('reader.toast_status_updated'));
 }, [isTarotReader, queryClient, statusMutation, t, userId]);

 return {
  handleSave,
  handleStatusChange,
  saving: saveMutation.isPending || statusMutation.isPending,
 };
}
