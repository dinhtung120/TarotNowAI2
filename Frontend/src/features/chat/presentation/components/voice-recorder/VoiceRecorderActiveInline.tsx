import { Send, X } from 'lucide-react';
import { cn } from '@/lib/utils';
import { formatRecordingDuration, getWaveformLevels } from '@/features/chat/presentation/components/voice-recorder/voiceRecorderUtils';

interface VoiceRecorderActiveInlineProps {
  audioLevels: number[];
  elapsedMs: number;
  onCancel: () => void;
  onSend: () => void;
}

export default function VoiceRecorderActiveInline({ audioLevels, elapsedMs, onCancel, onSend }: VoiceRecorderActiveInlineProps) {
  return (
    <div className={cn('flex min-w-0 flex-1 items-center gap-2')}>
      <button type="button" onClick={onCancel} className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl border tn-border-danger tn-bg-danger-soft tn-text-danger transition-colors tn-voice-recorder-cancel-hover')} title="Hủy ghi âm">
        <X className={cn('h-4 w-4')} />
      </button>

      <div className={cn('flex h-11 min-w-0 flex-1 items-center gap-2 rounded-xl border border-white/10 bg-white/5 px-3')}>
        <div className={cn('h-2.5 w-2.5 shrink-0 animate-pulse rounded-full tn-bg-danger')} />
        <div className={cn('tn-gap-0_5 flex h-7 min-w-0 flex-1 items-center overflow-hidden')}>
          {getWaveformLevels(audioLevels).map((level, index) => (
            <div
              key={index}
              className={cn('flex-1 rounded-full tn-bg-accent transition-all')}
              style={{ height: `${Math.max(3, Math.round(level * 28))}px`, opacity: Math.max(0.3, level), transitionDuration: '80ms' }}
            />
          ))}
        </div>
        <span className={cn('ml-1 shrink-0 font-mono text-xs tabular-nums tn-text-secondary')}>{formatRecordingDuration(elapsedMs)}</span>
      </div>

      <button type="button" onClick={onSend} className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl tn-bg-accent text-white transition-all tn-hover-brightness-110')} title="Gửi tin nhắn thoại">
        <Send className={cn('h-4 w-4')} />
      </button>
    </div>
  );
}
