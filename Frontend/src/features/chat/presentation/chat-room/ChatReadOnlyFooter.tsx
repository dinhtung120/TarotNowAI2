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
    <div className={cn('border-t border-white/10 p-3 text-xs text-[var(--text-secondary)]')}>
      <div>{isReadOnly ? hint : 'Bạn chưa thể gửi tin nhắn ở trạng thái hiện tại.'}</div>
      {canStartNewSession ? (
        <button
          type="button"
          onClick={() => void onStartNewSession()}
          disabled={startingNewSession}
          className={cn('mt-2 rounded-lg border border-[var(--purple-accent)]/30 bg-[var(--purple-accent)]/20 px-3 py-2 text-xs font-semibold text-[var(--purple-accent)] disabled:opacity-60')}
        >
          {startingNewSession ? 'Đang tạo phiên...' : 'Bắt đầu phiên tư vấn mới'}
        </button>
      ) : null}
    </div>
  );
}
