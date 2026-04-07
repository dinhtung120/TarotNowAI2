import { cn } from '@/lib/utils';

interface ChatCompleteResponseBarProps {
  awaitingCompleteResponse: boolean;
  processingAction: string | null;
  onRespondComplete: (accept: boolean) => Promise<void>;
}

export default function ChatCompleteResponseBar({
  awaitingCompleteResponse,
  processingAction,
  onRespondComplete,
}: ChatCompleteResponseBarProps) {
  if (!awaitingCompleteResponse) return null;

  return (
    <div className={cn('border-t border-white/10 bg-black/20 px-3 py-2')}>
      <div className={cn('mb-2 flex gap-2')}>
        <button
          type="button"
          onClick={() => void onRespondComplete(true)}
          disabled={processingAction !== null}
          className={cn('rounded-lg border border-[var(--success)]/25 bg-[var(--success)]/20 px-3 py-1.5 text-xs text-[var(--success)]')}
        >
          Đồng ý hoàn thành
        </button>
        <button
          type="button"
          onClick={() => void onRespondComplete(false)}
          disabled={processingAction !== null}
          className={cn('rounded-lg border border-[var(--danger)]/25 bg-[var(--danger)]/20 px-3 py-1.5 text-xs text-[var(--danger)]')}
        >
          Từ chối
        </button>
      </div>
    </div>
  );
}
