import { Phone, PhoneOff, Video, VideoOff } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ChatCallLogMessageProps {
  callType: string;
  durationSeconds: number;
  isMe: boolean;
  reason: string;
}

export default function ChatCallLogMessage({
  callType,
  durationSeconds,
  isMe,
  reason,
}: ChatCallLogMessageProps) {
  const isMissed = durationSeconds === 0;
  const icon = callType === 'video'
    ? isMissed
      ? <VideoOff className={cn('h-4 w-4')} />
      : <Video className={cn('h-4 w-4')} />
    : isMissed
      ? <PhoneOff className={cn('h-4 w-4')} />
      : <Phone className={cn('h-4 w-4')} />;

  const bubbleClass = cn(
    'flex max-w-[75%] items-center gap-3 rounded-2xl border px-4 py-2.5 shadow-sm transition-all hover:brightness-110',
    isMe
      ? 'rounded-br-md border-white/5 bg-[var(--purple-accent)] text-white'
      : 'rounded-bl-md border-white/10 bg-white/6 text-white',
  );

  const iconClass = cn(
    'rounded-full p-2',
    isMissed ? 'bg-white/20 text-red-300' : 'bg-white/20 text-emerald-300',
  );

  const metaText = isMissed
    ? (reason === 'cancelled' || reason === 'timeout' ? 'Chưa trả lời' : 'Bị nhỡ')
    : `${Math.floor(durationSeconds / 60).toString().padStart(2, '0')}:${(durationSeconds % 60).toString().padStart(2, '0')}`;

  return (
    <div className={cn('flex py-2', isMe ? 'justify-end' : 'justify-start')}>
      <div className={bubbleClass}>
        <div className={iconClass}>{icon}</div>

        <div className={cn('flex flex-col')}>
          <span className={cn('text-sm font-medium text-white/90')}>
            {callType === 'video' ? 'Cuộc gọi video' : 'Cuộc gọi thoại'}
          </span>
          <span
            className={cn(
              'text-[11px] font-mono',
              isMissed ? 'text-red-200' : 'text-white/75',
            )}
          >
            {metaText}
          </span>
        </div>
      </div>
    </div>
  );
}
