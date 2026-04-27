import { Send, X } from 'lucide-react';
import { cn } from '@/lib/utils';
import { formatRecordingDuration, getWaveformLevels } from '@/features/chat/presentation/components/voice-recorder/voiceRecorderUtils';

interface VoiceRecorderActiveInlineProps {
  audioLevels: number[];
  elapsedMs: number;
  onCancel: () => void;
  onSend: () => void;
}

function resolveWaveHeightClass(level: number): string {
  const height = Math.max(3, Math.round(level * 28));
  if (height >= 28) return 'tn-wave-h-28';
  if (height >= 25) return 'tn-wave-h-25';
  if (height >= 23) return 'tn-wave-h-23';
  if (height >= 21) return 'tn-wave-h-21';
  if (height >= 19) return 'tn-wave-h-19';
  if (height >= 17) return 'tn-wave-h-17';
  if (height >= 15) return 'tn-wave-h-15';
  if (height >= 13) return 'tn-wave-h-13';
  if (height >= 11) return 'tn-wave-h-11';
  if (height >= 9) return 'tn-wave-h-9';
  if (height >= 7) return 'tn-wave-h-7';
  if (height >= 5) return 'tn-wave-h-5';
  return 'tn-wave-h-3';
}

function resolveWaveOpacityClass(level: number): string {
  const opacity = Math.max(0.3, level);
  if (opacity >= 1) return 'tn-opacity-100';
  if (opacity >= 0.9) return 'tn-opacity-90';
  if (opacity >= 0.8) return 'tn-opacity-80';
  if (opacity >= 0.7) return 'tn-opacity-70';
  if (opacity >= 0.6) return 'tn-opacity-60';
  if (opacity >= 0.5) return 'tn-opacity-50';
  if (opacity >= 0.4) return 'tn-opacity-40';
  return 'tn-opacity-30';
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
              className={cn(
                'flex-1 rounded-full tn-bg-accent transition-all duration-75',
                resolveWaveHeightClass(level),
                resolveWaveOpacityClass(level),
              )}
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
