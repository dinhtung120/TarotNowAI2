'use client';

import ReportModalFormView from '@/features/chat/presentation/components/report-modal/ReportModalFormView';
import ReportModalSuccessView from '@/features/chat/presentation/components/report-modal/ReportModalSuccessView';
import { useReportModal } from '@/features/chat/presentation/components/report-modal/useReportModal';

interface ReportModalProps {
  conversationId: string;
  onClose: () => void;
}

export default function ReportModal({ conversationId, onClose }: ReportModalProps) {
  const { error, reason, submitting, success, targetType, setReason, setTargetType, submit } = useReportModal(conversationId);

  if (success) return <ReportModalSuccessView onClose={onClose} />;

  return (
    <ReportModalFormView
      error={error}
      reason={reason}
      submitting={submitting}
      targetType={targetType}
      onClose={onClose}
      onSubmit={() => void submit()}
      setReason={setReason}
      setTargetType={setTargetType}
    />
  );
}
