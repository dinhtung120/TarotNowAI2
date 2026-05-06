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
          className={cn('rounded-lg border tn-border-success-20 tn-bg-success-20 px-3 py-1.5 text-xs tn-text-success')}
        >
          Đồng ý hoàn thành
        </button>
        <button
          type="button"
          onClick={() => void onRespondComplete(false)}
          disabled={processingAction !== null}
          className={cn('rounded-lg border tn-border-danger-50 tn-bg-danger-soft px-3 py-1.5 text-xs tn-text-danger')}
        >
          Từ chối
        </button>
      </div>
    </div>
  );
}
