'use client';
import { useCallback, useMemo, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { CheckCircle2 } from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { useForm, useWatch } from 'react-hook-form';
import { useReaderApplyPage } from '@/features/reader/application/useReaderApplyPage';
import ReaderApplyFeedbackMessage from '@/features/reader/presentation/components/ReaderApplyFeedbackMessage';
import ReaderApplyHeader from '@/features/reader/presentation/components/ReaderApplyHeader';
import ReaderApplyIntroField from '@/features/reader/presentation/components/ReaderApplyIntroField';
import { ReaderApplyLoadingState } from '@/features/reader/presentation/components/ReaderApplyLoadingState';
import { ReaderApplyPendingPanel } from '@/features/reader/presentation/components/ReaderApplyPendingPanel';
import ReaderApplyRejectedNotice from '@/features/reader/presentation/components/ReaderApplyRejectedNotice';
import ReaderApplySocialLinksFields from '@/features/reader/presentation/components/ReaderApplySocialLinksFields';
import ReaderApplySpecialtiesField from '@/features/reader/presentation/components/ReaderApplySpecialtiesField';
import ReaderApplyStepsGrid from '@/features/reader/presentation/components/ReaderApplyStepsGrid';
import { ReaderApplyStatusPanel } from '@/features/reader/presentation/components/ReaderApplyStatusPanel';
import ReaderApplySubmitButton from '@/features/reader/presentation/components/ReaderApplySubmitButton';
import ReaderApplyExperiencePriceRow from '@/features/reader/presentation/components/ReaderApplyExperiencePriceRow';
import {
 createReaderApplyFormSchema,
 mapReaderRequestToFormValues,
 type ReaderApplyFormValues,
} from '@/features/reader/presentation/readerApplyFormSchema';
import { resolveReaderApplyHydrationIdentity } from '@/features/reader/presentation/readerApplyHydration';
import { normalizeOptionalSocialUrl } from '@/features/reader/domain/readerSocialLinks';
import { useHydrateFormOnce } from '@/shared/application/hooks/useHydrateFormOnce';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { cn } from '@/lib/utils';

type FeedbackState = { type: 'success' | 'error'; message: string } | null;
const EDIT_OPTIONS = { shouldDirty: true, shouldValidate: true } as const;
const EMPTY_SPECIALTIES: ReaderApplyFormValues['specialties'] = [];
export default function ReaderApplyPage() {
 const t = useTranslations('ReaderApply');
 const locale = useLocale();
 const { submitApplication, submitting, existingRequest, loading, defaultErrorMessage } = useReaderApplyPage(t);
 const runtimePoliciesQuery = useRuntimePolicies();
 const readerPolicy = runtimePoliciesQuery.data?.reader;
 const minYearsOfExperience = readerPolicy?.minYearsOfExperience ?? 0;
 const minDiamondPerQuestion = readerPolicy?.minDiamondPerQuestion ?? 0;
 const defaultDiamondPerQuestion = readerPolicy?.defaultDiamondPerQuestion ?? minDiamondPerQuestion;
 const isReaderPolicyReady = Boolean(readerPolicy);

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
 const [feedback, setFeedback] = useState<FeedbackState>(null);
 const { handleSubmit, setValue, control, reset, formState: { errors } } = useForm<ReaderApplyFormValues>({
  resolver: zodResolver(schema),
  defaultValues,
 });
 const bio = useWatch({ control, name: 'bio' }) ?? '';
 const watchedSpecialties = useWatch({ control, name: 'specialties' });
 const specialties = watchedSpecialties ?? EMPTY_SPECIALTIES;
 const yearsOfExperience = useWatch({ control, name: 'yearsOfExperience' }) ?? minYearsOfExperience;
 const diamondPerQuestion = useWatch({ control, name: 'diamondPerQuestion' }) ?? minDiamondPerQuestion;
 const facebookUrl = useWatch({ control, name: 'facebookUrl' }) ?? '';
 const instagramUrl = useWatch({ control, name: 'instagramUrl' }) ?? '';
 const tikTokUrl = useWatch({ control, name: 'tikTokUrl' }) ?? '';
 useHydrateFormOnce({ enabled: isReaderPolicyReady, identity: resolveReaderApplyHydrationIdentity(existingRequest), reset, values: defaultValues });
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
 if (loading) return <ReaderApplyLoadingState label={t('loading')} />;
 if (existingRequest?.hasRequest && existingRequest.status === 'approved') return <ReaderApplyStatusPanel accent="success" icon={CheckCircle2} title={t('approved.title')} description={t('approved.desc')} />;
 if (existingRequest?.hasRequest && existingRequest.status === 'pending') return <ReaderApplyPendingPanel existingRequest={existingRequest} locale={locale} t={t} />;
 return (
  <div className={cn('mx-auto max-w-2xl animate-in fade-in slide-in-from-bottom-8 space-y-10 tn-page-x py-20 duration-1000')}>
   <ReaderApplyHeader />
   {existingRequest?.hasRequest && existingRequest.status === 'rejected' ? <ReaderApplyRejectedNotice adminNote={existingRequest.adminNote} /> : null}
   <form onSubmit={handleSubmit(onSubmit)} className={cn('space-y-8')}>
    <ReaderApplyStepsGrid />
    <ReaderApplySpecialtiesField value={specialties} onChange={(value) => setValue('specialties', value, EDIT_OPTIONS)} error={errors.specialties?.message} />
    <ReaderApplyExperiencePriceRow yearsOfExperience={yearsOfExperience} diamondPerQuestion={diamondPerQuestion} minYearsOfExperience={minYearsOfExperience} minDiamondPerQuestion={minDiamondPerQuestion} yearsError={errors.yearsOfExperience?.message} diamondError={errors.diamondPerQuestion?.message} onChangeYears={(value) => setValue('yearsOfExperience', Number.isFinite(value) ? value : minYearsOfExperience, EDIT_OPTIONS)} onChangeDiamond={(value) => setValue('diamondPerQuestion', Number.isFinite(value) ? value : minDiamondPerQuestion, EDIT_OPTIONS)} />
    <ReaderApplySocialLinksFields facebookUrl={facebookUrl} instagramUrl={instagramUrl} tikTokUrl={tikTokUrl} onChangeFacebook={(value) => setValue('facebookUrl', value, EDIT_OPTIONS)} onChangeInstagram={(value) => setValue('instagramUrl', value, EDIT_OPTIONS)} onChangeTikTok={(value) => setValue('tikTokUrl', value, EDIT_OPTIONS)} errors={{ facebookUrl: facebookFieldError, instagramUrl: errors.instagramUrl?.message, tikTokUrl: errors.tikTokUrl?.message, socialRequired: socialRequiredMessage }} />
    <ReaderApplyIntroField bio={bio} setBio={(value) => setValue('bio', value, EDIT_OPTIONS)} error={errors.bio?.message} />
    {feedback ? <ReaderApplyFeedbackMessage message={feedback.message} type={feedback.type} /> : null}
    <ReaderApplySubmitButton submitting={submitting} disabled={submitting || !isReaderPolicyReady} />
   </form>
  </div>
 );
}
