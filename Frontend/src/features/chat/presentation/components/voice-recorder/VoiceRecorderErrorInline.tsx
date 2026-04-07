import { X } from 'lucide-react';
import { cn } from '@/lib/utils';

interface VoiceRecorderErrorInlineProps {
  errorMessage: string | null;
  onDismiss: () => void;
}

export default function VoiceRecorderErrorInline({ errorMessage, onDismiss }: VoiceRecorderErrorInlineProps) {
  return (
    <div className={cn('flex items-center gap-2')}>
      <span className={cn('tn-maxw-200 truncate tn-text-11 tn-text-danger')} title={errorMessage ?? ''}>
        {errorMessage}
      </span>
      <button
        type="button"
        onClick={onDismiss}
        className={cn('flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border tn-voice-error-btn')}
        title="Đóng lỗi"
      >
        <X className={cn('h-4 w-4')} />
      </button>
    </div>
  );
}
