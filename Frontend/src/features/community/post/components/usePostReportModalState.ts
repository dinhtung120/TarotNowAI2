import { useMemo, useState } from 'react';
import toast from 'react-hot-toast';
import { reportPostAction } from '@/features/community/shared/actions/communityActions';
import type { CommunityReportReasonCode } from '@/features/community/shared/types';

const REPORT_REASON_CODES: CommunityReportReasonCode[] = [
 'spam',
 'hate_speech',
 'harassment',
 'misinformation',
 'inappropriate',
 'other',
];

const MIN_DESCRIPTION_LENGTH = 10;

type Translator = (key: string, values?: Record<string, string | number>) => string;

interface UsePostReportModalStateOptions {
 postId: string | null;
 onClose: () => void;
 t: Translator;
}

export function usePostReportModalState({ postId, onClose, t }: UsePostReportModalStateOptions) {
 const [reasonCode, setReasonCode] = useState<CommunityReportReasonCode | ''>('');
 const [description, setDescription] = useState('');
 const [reasonError, setReasonError] = useState<string | null>(null);
 const [descriptionError, setDescriptionError] = useState<string | null>(null);
 const [isSubmitting, setIsSubmitting] = useState(false);

 const trimmedDescription = useMemo(() => description.trim(), [description]);

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

 const submit = async () => {
  if (!postId || isSubmitting || !validate()) {
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

 return {
  REPORT_REASON_CODES,
  MIN_DESCRIPTION_LENGTH,
  reasonCode,
  description,
  reasonError,
  descriptionError,
  isSubmitting,
  setReasonCode,
  setDescription,
  setReasonError,
  setDescriptionError,
  closeModal,
  submit,
 };
}
