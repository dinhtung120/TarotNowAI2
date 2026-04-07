'use client';

import { useEffect } from 'react';
import type { FormEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { CheckCircle2, Clock } from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { useReaderApplyPage } from '@/features/reader/application/useReaderApplyPage';
import ReaderApplyFeedbackMessage from '@/features/reader/presentation/components/ReaderApplyFeedbackMessage';
import ReaderApplyHeader from '@/features/reader/presentation/components/ReaderApplyHeader';
import ReaderApplyIntroField from '@/features/reader/presentation/components/ReaderApplyIntroField';
import { ReaderApplyLoadingState } from '@/features/reader/presentation/components/ReaderApplyLoadingState';
import ReaderApplyRejectedNotice from '@/features/reader/presentation/components/ReaderApplyRejectedNotice';
import ReaderApplyStepsGrid from '@/features/reader/presentation/components/ReaderApplyStepsGrid';
import { ReaderApplyStatusPanel } from '@/features/reader/presentation/components/ReaderApplyStatusPanel';
import ReaderApplySubmitButton from '@/features/reader/presentation/components/ReaderApplySubmitButton';
import { cn } from '@/lib/utils';

const readerApplyFormSchema = z.object({
 introText: z.string().trim().min(20).max(2000),
});

type ReaderApplyFormValues = z.infer<typeof readerApplyFormSchema>;

export default function ReaderApplyPage() {
  const t = useTranslations('ReaderApply');
  const locale = useLocale();
  const { introText, setIntroText, submitting, message, messageType, existingRequest, loading, handleSubmit } = useReaderApplyPage(t);
  const { handleSubmit: handleValidatedSubmit, setValue, watch } = useForm<ReaderApplyFormValues>({
    resolver: zodResolver(readerApplyFormSchema),
    defaultValues: {
      introText,
    },
  });

  const watchedIntroText = watch('introText') ?? '';

  useEffect(() => {
    setValue('introText', introText, { shouldDirty: false, shouldValidate: false });
  }, [introText, setValue]);

  useEffect(() => {
    setIntroText(watchedIntroText);
  }, [setIntroText, watchedIntroText]);

  const submitWithValidation = handleValidatedSubmit(() => {
    handleSubmit({
      preventDefault: () => undefined,
      stopPropagation: () => undefined,
    } as unknown as FormEvent<HTMLFormElement>);
  });

  if (loading) return <ReaderApplyLoadingState label={t('loading')} />;

  if (existingRequest?.hasRequest && existingRequest.status === 'pending') {
    return (
      <ReaderApplyStatusPanel
        accent="warning"
        icon={Clock}
        title={t('pending.title')}
        description={t('pending.desc')}
        introLabel={t('pending.intro_label')}
        introText={existingRequest.introText}
        footerLabel={t('pending.sent_at', { date: new Date(existingRequest.createdAt || '').toLocaleString(locale) })}
      />
    );
  }

  if (existingRequest?.hasRequest && existingRequest.status === 'approved') {
    return <ReaderApplyStatusPanel accent="success" icon={CheckCircle2} title={t('approved.title')} description={t('approved.desc')} />;
  }

  const wasRejected = existingRequest?.hasRequest && existingRequest.status === 'rejected';

  return (
    <div className={cn('mx-auto max-w-2xl animate-in fade-in slide-in-from-bottom-8 space-y-10 px-4 py-20 duration-1000 sm:px-6')}>
      <ReaderApplyHeader />
      {wasRejected ? <ReaderApplyRejectedNotice adminNote={existingRequest?.adminNote} /> : null}
      <form onSubmit={submitWithValidation} className={cn('space-y-8')}>
        <ReaderApplyStepsGrid />
        <ReaderApplyIntroField introText={watchedIntroText} setIntroText={(value) => setValue('introText', value, { shouldDirty: true, shouldValidate: true })} />
        {message ? <ReaderApplyFeedbackMessage message={message} type={messageType} /> : null}
        <ReaderApplySubmitButton submitting={submitting} disabled={submitting || watchedIntroText.length < 20} />
      </form>
    </div>
  );
}
