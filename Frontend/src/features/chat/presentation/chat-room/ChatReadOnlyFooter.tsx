import { cn } from '@/lib/utils';

interface ChatReadOnlyFooterProps {
  canStartNewSession: boolean;
  hint: string;
  isReadOnly: boolean;
  startingNewSession: boolean;
  onStartNewSession: () => Promise<void>;
}

export default function ChatReadOnlyFooter({
  canStartNewSession,
  hint,
  isReadOnly,
  startingNewSession,
  onStartNewSession,
}: ChatReadOnlyFooterProps) {
  return (
    <div className={cn('border-t border-white/10 p-3 text-xs tn-chat-readonly-footer')}>
      <div>{isReadOnly ? hint : 'Bạn chưa thể gửi tin nhắn ở trạng thái hiện tại.'}</div>
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
