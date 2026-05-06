import { cn } from '@/lib/utils';

interface ChatReadOnlyFooterProps {
  canStartNewSession: boolean;
  canSubmitReview: boolean;
  hasSubmittedReview: boolean;
  hint: string;
  isReadOnly: boolean;
  startingNewSession: boolean;
  onOpenReviewModal: () => void;
  onStartNewSession: () => Promise<void>;
}

export default function ChatReadOnlyFooter({
  canStartNewSession,
  canSubmitReview,
  hasSubmittedReview,
  hint,
  isReadOnly,
  startingNewSession,
  onOpenReviewModal,
  onStartNewSession,
}: ChatReadOnlyFooterProps) {
  return (
    <div className={cn('border-t border-white/10 p-3 text-xs tn-chat-readonly-footer')}>
      <div>{isReadOnly ? hint : 'Bạn chưa thể gửi tin nhắn ở trạng thái hiện tại.'}</div>
      {canSubmitReview ? (
        <button
          type="button"
          onClick={onOpenReviewModal}
          className={cn('mt-2 rounded-lg px-3 py-2 text-xs font-semibold tn-chat-readonly-action')}
        >
          Đánh giá Reader
        </button>
      ) : null}
      {hasSubmittedReview ? (
        <div className={cn('mt-2 text-[11px] text-emerald-300')}>Bạn đã gửi đánh giá cho phiên này.</div>
      ) : null}
      {canStartNewSession ? (
        <button
          type="button"
          onClick={() => void onStartNewSession()}
          disabled={startingNewSession}
          className={cn('mt-2 rounded-lg px-3 py-2 text-xs font-semibold tn-chat-readonly-action')}
        >
          {startingNewSession ? 'Đang tạo phiên...' : 'Bắt đầu phiên tư vấn mới'}
        </button>
      ) : null}
    </div>
  );
}
