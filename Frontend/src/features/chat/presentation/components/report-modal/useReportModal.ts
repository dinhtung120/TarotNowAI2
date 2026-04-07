import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { sendReport } from '@/features/chat/application/actions';
import type { ReportTargetType } from '@/features/chat/presentation/components/report-modal/types';

interface UseReportModalResult {
  error: string;
  reason: string;
  submitting: boolean;
  success: boolean;
  targetType: ReportTargetType;
  setReason: (value: string) => void;
  setTargetType: (value: ReportTargetType) => void;
  submit: () => Promise<void>;
}

export function useReportModal(conversationId: string): UseReportModalResult {
  const t = useTranslations('Chat');
  const [targetType, setTargetType] = useState<ReportTargetType>('conversation');
  const [reason, setReason] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');

  const submit = async () => {
    if (reason.length < 10) {
      setError(t('report.validation_min'));
      return;
    }

    setSubmitting(true);
    setError('');
    const result = await sendReport({ targetType, targetId: conversationId, conversationRef: conversationId, reason });
    setSuccess(result.success);
    if (!result.success) setError(t('report.error_failed'));
    setSubmitting(false);
  };

  return { error, reason, submitting, success, targetType, setReason, setTargetType, submit };
}
