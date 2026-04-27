'use client';

import { useMemo, useState, type FormEvent } from 'react';
import { useTranslations } from 'next-intl';
import toast from 'react-hot-toast';
import { cn } from '@/lib/utils';
import Button from '@/shared/components/ui/Button';
import Modal from '@/shared/components/ui/Modal';
import { reportPostAction } from '@/features/community/application/actions/communityActions';
import type { CommunityReportReasonCode } from '@/features/community/types';

interface PostReportModalProps {
  postId: string | null;
  isOpen: boolean;
  onClose: () => void;
}

const REPORT_REASON_CODES: CommunityReportReasonCode[] = [
  'spam',
  'hate_speech',
  'harassment',
  'misinformation',
  'inappropriate',
  'other',
];

const MIN_DESCRIPTION_LENGTH = 10;

export function PostReportModal({ postId, isOpen, onClose }: PostReportModalProps) {
  const t = useTranslations('Community');
  const [reasonCode, setReasonCode] = useState<CommunityReportReasonCode | ''>('');
  const [description, setDescription] = useState('');
  const [reasonError, setReasonError] = useState<string | null>(null);
  const [descriptionError, setDescriptionError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const resetForm = () => {
    setReasonCode('');
    setDescription('');
    setReasonError(null);
    setDescriptionError(null);
    setIsSubmitting(false);
  };

  const closeModal = () => {
    resetForm();
    onClose();
  };

  const trimmedDescription = useMemo(() => description.trim(), [description]);

  if (!isOpen || !postId) {
    return null;
  }

  const validate = (): boolean => {
    let valid = true;

    if (!reasonCode) {
      setReasonError(t('report_modal.validation.reason_required'));
      valid = false;
    } else {
      setReasonError(null);
    }

    if (trimmedDescription.length < MIN_DESCRIPTION_LENGTH) {
      setDescriptionError(
        t('report_modal.validation.description_min', { min: MIN_DESCRIPTION_LENGTH }),
      );
      valid = false;
    } else {
      setDescriptionError(null);
    }

    return valid;
  };

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (isSubmitting) {
      return;
    }

    if (!validate()) {
      return;
    }

    setIsSubmitting(true);
    const result = await reportPostAction(postId, {
      reasonCode: reasonCode as CommunityReportReasonCode,
      description: trimmedDescription,
    });

    if (!result.success || !result.data?.success) {
      toast.error(result.error || t('report_modal.toast.failure'));
      setIsSubmitting(false);
      return;
    }

    toast.success(t('report_modal.toast.success'));
    closeModal();
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={closeModal}
      title={t('report_modal.title')}
      description={t('report_modal.description')}
      size="sm"
    >
      <form onSubmit={(event) => void handleSubmit(event)} className={cn('space-y-4')}>
        <div className={cn('space-y-2')}>
          <label htmlFor="community-report-reason" className={cn('text-xs font-black uppercase tracking-wider tn-text-muted')}>
            {t('report_modal.reason_label')}
          </label>
          <select
            id="community-report-reason"
            value={reasonCode}
            disabled={isSubmitting}
            onChange={(event) => {
              setReasonCode(event.target.value as CommunityReportReasonCode);
              setReasonError(null);
            }}
            className={cn('tn-input tn-field-accent w-full rounded-xl px-3 py-2 text-sm')}
          >
            <option value="">{t('report_modal.reason_placeholder')}</option>
            {REPORT_REASON_CODES.map((code) => (
              <option key={code} value={code}>
                {t(`report_modal.reasons.${code}`)}
              </option>
            ))}
          </select>
          {reasonError ? <p className={cn('text-xs font-medium tn-text-danger')}>{reasonError}</p> : null}
        </div>

        <div className={cn('space-y-2')}>
          <label htmlFor="community-report-description" className={cn('text-xs font-black uppercase tracking-wider tn-text-muted')}>
            {t('report_modal.description_label')}
          </label>
          <textarea
            id="community-report-description"
            value={description}
            disabled={isSubmitting}
            onChange={(event) => {
              setDescription(event.target.value);
              setDescriptionError(null);
            }}
            rows={4}
            maxLength={500}
            placeholder={t('report_modal.description_placeholder', { min: MIN_DESCRIPTION_LENGTH })}
            className={cn('tn-input tn-field-accent custom-scrollbar min-h-28 w-full resize-y rounded-xl px-3 py-2 text-sm')}
          />
          <div className={cn('flex items-center justify-between gap-2')}>
            {descriptionError ? <p className={cn('text-xs font-medium tn-text-danger')}>{descriptionError}</p> : <span />}
            <span className={cn('text-xs tn-text-muted')}>{description.length}/500</span>
          </div>
        </div>

        <div className={cn('flex justify-end gap-3 pt-1')}>
          <Button type="button" variant="secondary" onClick={closeModal} disabled={isSubmitting}>
            {t('report_modal.cancel')}
          </Button>
          <Button type="submit" variant="danger" isLoading={isSubmitting}>
            {isSubmitting ? t('report_modal.submitting') : t('report_modal.submit')}
          </Button>
        </div>
      </form>
    </Modal>
  );
}
