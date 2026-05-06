'use client';

import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import Modal from '@/shared/ui/Modal';
import { PostReportActions } from '@/features/community/post/components/PostReportActions';
import { PostReportFields } from '@/features/community/post/components/PostReportFields';
import { usePostReportModalState } from '@/features/community/post/components/usePostReportModalState';

interface PostReportModalProps {
 postId: string | null;
 isOpen: boolean;
 onClose: () => void;
}

export function PostReportModal({ postId, isOpen, onClose }: PostReportModalProps) {
 const t = useTranslations('Community');
 const vm = usePostReportModalState({ postId, onClose, t });

 if (!isOpen || !postId) {
  return null;
 }

 return (
  <Modal isOpen={isOpen} onClose={vm.closeModal} title={t('report_modal.title')} description={t('report_modal.description')} size="sm">
   <form
    onSubmit={(event) => {
     event.preventDefault();
     void vm.submit();
    }}
    className={cn('space-y-4')}
   >
    <PostReportFields
     t={t}
     reasonCode={vm.reasonCode}
     description={vm.description}
     reasonError={vm.reasonError}
     descriptionError={vm.descriptionError}
     isSubmitting={vm.isSubmitting}
     minDescriptionLength={vm.MIN_DESCRIPTION_LENGTH}
     reasonCodes={vm.REPORT_REASON_CODES}
     onReasonCodeChange={(value) => {
      vm.setReasonCode(value);
      vm.setReasonError(null);
     }}
     onDescriptionChange={(value) => {
      vm.setDescription(value);
      vm.setDescriptionError(null);
     }}
    />
    <PostReportActions t={t} isSubmitting={vm.isSubmitting} onCancel={vm.closeModal} />
   </form>
  </Modal>
 );
}
