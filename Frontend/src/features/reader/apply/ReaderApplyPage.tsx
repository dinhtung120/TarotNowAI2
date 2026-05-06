'use client';
import { CheckCircle2 } from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import ReaderApplyFeedbackMessage from '@/features/reader/apply/components/ReaderApplyFeedbackMessage';
import ReaderApplyHeader from '@/features/reader/apply/components/ReaderApplyHeader';
import ReaderApplyIntroField from '@/features/reader/apply/components/ReaderApplyIntroField';
import { ReaderApplyLoadingState } from '@/features/reader/apply/components/ReaderApplyLoadingState';
import { ReaderApplyPendingPanel } from '@/features/reader/apply/components/ReaderApplyPendingPanel';
import ReaderApplyRejectedNotice from '@/features/reader/apply/components/ReaderApplyRejectedNotice';
import ReaderApplySocialLinksFields from '@/features/reader/apply/components/ReaderApplySocialLinksFields';
import ReaderApplySpecialtiesField from '@/features/reader/apply/components/ReaderApplySpecialtiesField';
import ReaderApplyStepsGrid from '@/features/reader/apply/components/ReaderApplyStepsGrid';
import { ReaderApplyStatusPanel } from '@/features/reader/apply/components/ReaderApplyStatusPanel';
import ReaderApplySubmitButton from '@/features/reader/apply/components/ReaderApplySubmitButton';
import ReaderApplyExperiencePriceRow from '@/features/reader/apply/components/ReaderApplyExperiencePriceRow';
import { READER_APPLY_EDIT_OPTIONS, useReaderApplyPageForm } from '@/features/reader/apply/useReaderApplyPageForm';
import { cn } from '@/lib/utils';

export default function ReaderApplyPage() {
 const t = useTranslations('ReaderApply');
 const locale = useLocale();
 const form = useReaderApplyPageForm(t);
 if (form.loading) return <ReaderApplyLoadingState label={t('loading')} />;
 if (form.existingRequest?.hasRequest && form.existingRequest.status === 'approved') return <ReaderApplyStatusPanel accent="success" icon={CheckCircle2} title={t('approved.title')} description={t('approved.desc')} />;
 if (form.existingRequest?.hasRequest && form.existingRequest.status === 'pending') return <ReaderApplyPendingPanel existingRequest={form.existingRequest} locale={locale} t={t} />;
 return (
  <div className={cn('mx-auto max-w-2xl animate-in fade-in slide-in-from-bottom-8 space-y-10 tn-page-x py-20 duration-1000')}>
   <ReaderApplyHeader />
   {form.existingRequest?.hasRequest && form.existingRequest.status === 'rejected' ? <ReaderApplyRejectedNotice adminNote={form.existingRequest.adminNote} /> : null}
   <form onSubmit={form.handleSubmit(form.onSubmit)} className={cn('space-y-8')}>
    <ReaderApplyStepsGrid />
    <ReaderApplySpecialtiesField value={form.specialties} onChange={(value) => form.setValue('specialties', value, READER_APPLY_EDIT_OPTIONS)} error={form.errors.specialties?.message} />
    <ReaderApplyExperiencePriceRow yearsOfExperience={form.yearsOfExperience} diamondPerQuestion={form.diamondPerQuestion} minYearsOfExperience={form.minYearsOfExperience} minDiamondPerQuestion={form.minDiamondPerQuestion} yearsError={form.errors.yearsOfExperience?.message} diamondError={form.errors.diamondPerQuestion?.message} onChangeYears={(value) => form.setValue('yearsOfExperience', Number.isFinite(value) ? value : form.minYearsOfExperience, READER_APPLY_EDIT_OPTIONS)} onChangeDiamond={(value) => form.setValue('diamondPerQuestion', Number.isFinite(value) ? value : form.minDiamondPerQuestion, READER_APPLY_EDIT_OPTIONS)} />
    <ReaderApplySocialLinksFields facebookUrl={form.facebookUrl} instagramUrl={form.instagramUrl} tikTokUrl={form.tikTokUrl} onChangeFacebook={(value) => form.setValue('facebookUrl', value, READER_APPLY_EDIT_OPTIONS)} onChangeInstagram={(value) => form.setValue('instagramUrl', value, READER_APPLY_EDIT_OPTIONS)} onChangeTikTok={(value) => form.setValue('tikTokUrl', value, READER_APPLY_EDIT_OPTIONS)} errors={{ facebookUrl: form.facebookFieldError, instagramUrl: form.errors.instagramUrl?.message, tikTokUrl: form.errors.tikTokUrl?.message, socialRequired: form.socialRequiredMessage }} />
    <ReaderApplyIntroField bio={form.bio} setBio={(value) => form.setValue('bio', value, READER_APPLY_EDIT_OPTIONS)} error={form.errors.bio?.message} />
    {form.feedback ? <ReaderApplyFeedbackMessage message={form.feedback.message} type={form.feedback.type} /> : null}
    <ReaderApplySubmitButton submitting={form.submitting} disabled={form.submitting || !form.isReaderPolicyReady} />
   </form>
  </div>
 );
}
