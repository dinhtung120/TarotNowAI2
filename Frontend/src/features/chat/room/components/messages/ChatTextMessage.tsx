import { Check, CheckCheck } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ChatTextMessageProps {
  content: string;
  createdAt: string;
  isMe: boolean;
  isRead: boolean;
  locale: string;
}

export default function ChatTextMessage({
  content,
  createdAt,
  isMe,
  isRead,
  locale,
}: ChatTextMessageProps) {
  return (
    <div className={cn('flex', isMe ? 'justify-end' : 'justify-start')}>
      <div
        className={cn(
          'max-w-[78%] rounded-2xl px-3 py-2',
          isMe
            ? 'rounded-br-md bg-[var(--purple-accent)] text-white'
            : 'rounded-bl-md border border-white/10 bg-white/6 text-white',
        )}
      >
        <p className={cn('break-words whitespace-pre-wrap text-sm')}>{content}</p>

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
