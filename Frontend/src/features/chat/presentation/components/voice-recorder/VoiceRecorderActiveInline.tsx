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
      <button type="button" onClick={onCancel} className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl border border-[var(--danger)]/30 bg-[var(--danger)]/20 text-[var(--danger)] transition-colors hover:bg-[var(--danger)]/30')} title="Hủy ghi âm">
        <X className={cn('h-4 w-4')} />
      </button>

      <div className={cn('flex h-11 min-w-0 flex-1 items-center gap-2 rounded-xl border border-white/10 bg-white/5 px-3')}>
        <div className={cn('h-2.5 w-2.5 shrink-0 animate-pulse rounded-full bg-[var(--danger)]')} />
        <div className={cn('flex h-7 min-w-0 flex-1 items-center gap-[2px] overflow-hidden')}>
          {getWaveformLevels(audioLevels).map((level, index) => (
            <div
              key={index}
              className={cn('flex-1 rounded-full bg-[var(--purple-accent)] transition-all')}
              style={{ height: `${Math.max(3, Math.round(level * 28))}px`, opacity: Math.max(0.3, level), transitionDuration: '80ms' }}
            />
          ))}
        </div>
        <span className={cn('ml-1 shrink-0 font-mono text-xs tabular-nums text-[var(--text-secondary)]')}>{formatRecordingDuration(elapsedMs)}</span>
      </div>

      <button type="button" onClick={onSend} className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl bg-[var(--purple-accent)] text-white transition-all hover:brightness-110')} title="Gửi tin nhắn thoại">
        <Send className={cn('h-4 w-4')} />
      </button>
    </div>
  );
}
