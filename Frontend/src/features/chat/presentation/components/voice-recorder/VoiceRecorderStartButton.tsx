import { Loader2, Mic } from 'lucide-react';
import { cn } from '@/lib/utils';

interface VoiceRecorderStartButtonProps {
  disabled: boolean;
  isRequesting: boolean;
  onStart: () => void;
}

export default function VoiceRecorderStartButton({ disabled, isRequesting, onStart }: VoiceRecorderStartButtonProps) {
  return (
    <button
      type="button"
      onClick={onStart}
      disabled={disabled || isRequesting}
      className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl tn-chat-icon-btn')}
      title="Ghi âm tin nhắn thoại"
    >
      {isRequesting ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : <Mic className={cn('h-4 w-4')} />}
    </button>
  );
}
