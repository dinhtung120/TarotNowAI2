import { cn } from '@/lib/utils';

interface ReaderApplyFeedbackMessageProps {
  message: string;
  type: 'success' | 'error';
}

export default function ReaderApplyFeedbackMessage({ message, type }: ReaderApplyFeedbackMessageProps) {
  return (
    <div
      className={cn(
        'rounded-xl border p-4 text-sm',
        type === 'success'
          ? 'border-[var(--success)]/20 bg-[var(--success)]/10 text-[var(--success)]'
          : 'border-[var(--danger)]/20 bg-[var(--danger)]/10 text-[var(--danger)]',
      )}
    >
      {message}
    </div>
  );
}
