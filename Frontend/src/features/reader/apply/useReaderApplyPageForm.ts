'use client';

import { useCallback, useMemo, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import { useReaderApplyPage } from '@/features/reader/apply/useReaderApplyPage';
import {
 createReaderApplyFormSchema,
 mapReaderRequestToFormValues,
 type ReaderApplyFormValues,
} from '@/features/reader/apply/readerApplyFormSchema';
import { resolveReaderApplyHydrationIdentity } from '@/features/reader/apply/readerApplyHydration';
import { normalizeOptionalSocialUrl } from '@/features/reader/shared/readerSocialLinks';
import { useHydrateFormOnce } from '@/shared/hooks/useHydrateFormOnce';
import { useRuntimePolicies } from '@/shared/hooks/useRuntimePolicies';

type FeedbackState = { type: 'success' | 'error'; message: string } | null;

export const READER_APPLY_EDIT_OPTIONS = { shouldDirty: true, shouldValidate: true } as const;
const EMPTY_SPECIALTIES: ReaderApplyFormValues['specialties'] = [];

type ReaderApplyTranslator = (key: string) => string;

export function useReaderApplyPageForm(t: ReaderApplyTranslator) {
 const { submitApplication, submitting, existingRequest, loading, defaultErrorMessage } = useReaderApplyPage(t);
 const runtimePoliciesQuery = useRuntimePolicies();
 const readerPolicy = runtimePoliciesQuery.data?.reader;
 const minYearsOfExperience = readerPolicy?.minYearsOfExperience ?? 0;
 const minDiamondPerQuestion = readerPolicy?.minDiamondPerQuestion ?? 0;
 const defaultDiamondPerQuestion = readerPolicy?.defaultDiamondPerQuestion ?? minDiamondPerQuestion;
 const isReaderPolicyReady = Boolean(readerPolicy);
 const [feedback, setFeedback] = useState<FeedbackState>(null);

 const schema = useMemo(() => createReaderApplyFormSchema(t, {
  minYearsOfExperience,
  minDiamondPerQuestion,
  defaultDiamondPerQuestion,
 }), [defaultDiamondPerQuestion, minDiamondPerQuestion, minYearsOfExperience, t]);
 const defaultValues = useMemo(() => mapReaderRequestToFormValues(existingRequest, {
  minYearsOfExperience,
  minDiamondPerQuestion,
  defaultDiamondPerQuestion,
 }), [defaultDiamondPerQuestion, existingRequest, minDiamondPerQuestion, minYearsOfExperience]);
 const { handleSubmit, setValue, control, reset, formState: { errors } } = useForm<ReaderApplyFormValues>({
  resolver: zodResolver(schema),
  defaultValues,
 });

 const bio = useWatch({ control, name: 'bio' }) ?? '';
 const specialties = useWatch({ control, name: 'specialties' }) ?? EMPTY_SPECIALTIES;
 const yearsOfExperience = useWatch({ control, name: 'yearsOfExperience' }) ?? minYearsOfExperience;
 const diamondPerQuestion = useWatch({ control, name: 'diamondPerQuestion' }) ?? minDiamondPerQuestion;
 const facebookUrl = useWatch({ control, name: 'facebookUrl' }) ?? '';
 const instagramUrl = useWatch({ control, name: 'instagramUrl' }) ?? '';
 const tikTokUrl = useWatch({ control, name: 'tikTokUrl' }) ?? '';

 useHydrateFormOnce({
  enabled: isReaderPolicyReady,
  identity: resolveReaderApplyHydrationIdentity(existingRequest),
  reset,
  values: defaultValues,
 });

 const socialRequiredMessage = errors.facebookUrl?.message === t('validation.social_required') ? errors.facebookUrl.message : undefined;
 const facebookFieldError = errors.facebookUrl?.message !== t('validation.social_required') ? errors.facebookUrl?.message : undefined;

 const onSubmit = useCallback(async (values: ReaderApplyFormValues) => {
  setFeedback(null);
  if (!isReaderPolicyReady) {
   setFeedback({ type: 'error', message: t('errors.policy_unavailable') });
   return;
  }

  const result = await submitApplication({
   bio: values.bio.trim(),
   specialties: values.specialties,
   yearsOfExperience: values.yearsOfExperience,
   diamondPerQuestion: values.diamondPerQuestion,
   facebookUrl: normalizeOptionalSocialUrl(values.facebookUrl) || null,
   instagramUrl: normalizeOptionalSocialUrl(values.instagramUrl) || null,
   tikTokUrl: normalizeOptionalSocialUrl(values.tikTokUrl) || null,
  });
  if (!result.success) {
   setFeedback({ type: 'error', message: result.error || defaultErrorMessage });
   return;
  }

  setFeedback({ type: 'success', message: result.data?.message ?? t('success.submitted') });
 }, [defaultErrorMessage, isReaderPolicyReady, submitApplication, t]);

 return {
  existingRequest,
  loading,
  submitting,
  minYearsOfExperience,
  minDiamondPerQuestion,
  isReaderPolicyReady,
  handleSubmit,
  setValue,
  errors,
  bio,
  specialties,
  yearsOfExperience,
  diamondPerQuestion,
  facebookUrl,
  instagramUrl,
  tikTokUrl,
  socialRequiredMessage,
  facebookFieldError,
  feedback,
  onSubmit,
 };
}
