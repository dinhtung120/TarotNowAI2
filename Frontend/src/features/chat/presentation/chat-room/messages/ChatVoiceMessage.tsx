import type { ComponentType } from 'react';
import { Check, CheckCheck } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ChatVoiceMessageProps {
  audioUrl: string;
  createdAt: string;
  durationMs?: number | null;
  isMe: boolean;
  isRead: boolean;
  locale: string;
  VoiceMessageBubble: ComponentType<{
    audioUrl: string;
    durationMs?: number | null;
    isMe: boolean;
  }>;
}

export default function ChatVoiceMessage({
  audioUrl,
  createdAt,
  durationMs,
  isMe,
  isRead,
  locale,
  VoiceMessageBubble,
}: ChatVoiceMessageProps) {
  return (
    <div className={cn('flex', isMe ? 'justify-end' : 'justify-start')}>
      <div
        className={cn(
          'max-w-[80%] rounded-2xl px-3 py-2.5',
          isMe
            ? 'rounded-br-md bg-[var(--purple-accent)]'
            : 'rounded-bl-md border border-white/10 bg-white/6',
        )}
      >
        <VoiceMessageBubble
          audioUrl={audioUrl}
          durationMs={durationMs}
          isMe={isMe}
        />

        <div
          className={cn(
            'mt-1 flex items-center gap-1 text-[10px]',
            isMe ? 'justify-end text-white/75' : 'text-[var(--text-secondary)]',
          )}
        >
          <span>
            {new Date(createdAt).toLocaleTimeString(locale, {
              hour: '2-digit',
              minute: '2-digit',
            })}
          </span>
          {isMe ? (
            isRead ? <CheckCheck className={cn('h-3 w-3 text-sky-300')} /> : <Check className={cn('h-3 w-3')} />
          ) : null}
        </div>
      </div>
    </div>
  );
}
