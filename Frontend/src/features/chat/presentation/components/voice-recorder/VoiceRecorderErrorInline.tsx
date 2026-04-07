import { X } from 'lucide-react';
import { cn } from '@/lib/utils';

interface VoiceRecorderErrorInlineProps {
  errorMessage: string | null;
  onDismiss: () => void;
}

export default function VoiceRecorderErrorInline({ errorMessage, onDismiss }: VoiceRecorderErrorInlineProps) {
  return (
    <div className={cn('flex items-center gap-2')}>
      <span className={cn('max-w-[200px] truncate text-[11px] text-[var(--danger)]')} title={errorMessage ?? ''}>
        {errorMessage}
      </span>
      <button
        type="button"
        onClick={onDismiss}
        className={cn('flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-[var(--danger)]/30 bg-[var(--danger)]/20 text-[var(--danger)]')}
        title="Đóng lỗi"
      >
        <X className={cn('h-4 w-4')} />
      </button>
    </div>
  );
}
