'use client';
import { useEffect, useMemo, useRef, useState, type FormEvent } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import {
 getReaderProfile,
 updateReaderProfile,
 updateReaderStatus,
} from '@/features/reader/public';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { normalizeReaderStatus, type ReaderStatus } from '@/features/reader/domain/readerStatus';
import toast from 'react-hot-toast';
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
export function useProfileReaderSettingsPage(t: TranslateFn) {
 const router = useRouter();
 const queryClient = useQueryClient();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const user = useAuthStore((state) => state.user);
 const redirectShownRef = useRef(false);
 const isTarotReader = user?.role === 'tarot_reader';
 const [draft, setDraft] = useState<ReaderSettingsDraft | null>(null);
 const profileQuery = useQuery({
  queryKey: ['reader-profile-settings', user?.id],
  enabled: isAuthenticated && !!user && isTarotReader,
  queryFn: async () => {
   if (!user) return null;
   const result = await getReaderProfile(user.id);
   return result.success ? result.data ?? null : null;
  },
 });
 const saveMutation = useMutation({
  mutationFn: updateReaderProfile,
 });
 const statusMutation = useMutation({
  mutationFn: updateReaderStatus,
 });
 useEffect(() => {
  if (!isAuthenticated || !user) {
   router.push('/login');
   return;
  }
  if (!isTarotReader && !redirectShownRef.current) {
   redirectShownRef.current = true;
   toast.error(t('reader.toast_not_found'), { style: NEUTRAL_TOAST_STYLE });
   router.push('/profile');
  }
 }, [isAuthenticated, isTarotReader, router, t, user]);
 useEffect(() => {
  if (!isAuthenticated || !user || !isTarotReader) return;
  if (profileQuery.isLoading) return;
  const profile = profileQuery.data;
  if (!profile && !redirectShownRef.current) {
   redirectShownRef.current = true;
   toast.error(t('reader.toast_not_found'), { style: NEUTRAL_TOAST_STYLE });
   router.push('/profile');
  }
 }, [isAuthenticated, isTarotReader, profileQuery.data, profileQuery.isLoading, router, t, user]);
 const effectiveValues = useMemo(
  () => draft ?? toDraft(profileQuery.data),
  [draft, profileQuery.data]
 );
 const updateDraft = (updater: (current: ReaderSettingsDraft) => ReaderSettingsDraft) => {
  setDraft((prev) => updater(prev ?? toDraft(profileQuery.data)));
 };
 const setBioVi = (value: string) => {
  updateDraft((current) => ({ ...current, bioVi: value }));
 };
 const setDiamondPerQuestion = (value: number) => {
  updateDraft((current) => ({ ...current, diamondPerQuestion: value }));
 };
 const setSpecialtiesStr = (value: string) => {
  updateDraft((current) => ({ ...current, specialtiesStr: value }));
 };
 const handleSave = async (event: FormEvent) => {
  event.preventDefault();
  const specArray = effectiveValues.specialtiesStr
   .split(',')
   .map((item) => item.trim())
   .filter((item) => item.length > 0);
  const result = await saveMutation.mutateAsync({
   bioVi: effectiveValues.bioVi,
   diamondPerQuestion: effectiveValues.diamondPerQuestion,
   specialties: specArray,
  });
  if (result.success) {
   toast.success(t('reader.toast_save_success'), { style: SUCCESS_TOAST_STYLE });
   setDraft(null);
  } else {
   toast.error(t('reader.toast_save_fail'), { style: DANGER_TOAST_STYLE });
  }
 };
 const handleStatusChange = async (newStatus: ReaderStatus) => {
  const previousStatus = effectiveValues.status;
  updateDraft((current) => ({ ...current, status: newStatus }));
  const result = await statusMutation.mutateAsync(newStatus);
  if (result.success) {
   await queryClient.invalidateQueries({ queryKey: ['reader-profile-settings', user?.id] });
   toast.success(t('reader.toast_status_updated'), { style: SUCCESS_TOAST_STYLE });
  } else {
   updateDraft((current) => ({ ...current, status: previousStatus }));
   toast.error(t('reader.toast_status_update_fail'), { style: DANGER_TOAST_STYLE });
  }
 };
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
