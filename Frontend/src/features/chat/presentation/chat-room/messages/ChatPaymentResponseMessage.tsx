import { Check, CheckCheck } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ChatPaymentResponseMessageProps {
  content: string;
  createdAt: string;
  isMe: boolean;
  isRead: boolean;
  locale: string;
  type: 'payment_accept' | 'payment_reject';
}

interface PaymentResponsePayload {
  note?: string;
}

function tryParsePayload(content: string): PaymentResponsePayload | null {
  try {
    return JSON.parse(content) as PaymentResponsePayload;
  } catch {
    return null;
  }
}

function resolveTitle(type: 'payment_accept' | 'payment_reject', isMe: boolean): string {
  if (type === 'payment_accept') {
    return isMe
      ? 'Bạn đã chấp nhận đề xuất cộng tiền.'
      : 'Đề xuất cộng tiền đã được chấp nhận.';
  }

  return isMe
    ? 'Bạn đã từ chối đề xuất cộng tiền.'
    : 'Đề xuất cộng tiền đã bị từ chối.';
}

export default function ChatPaymentResponseMessage({
  content,
  createdAt,
  isMe,
  isRead,
  locale,
  type,
}: ChatPaymentResponseMessageProps) {
  const payload = tryParsePayload(content);
  const note = payload?.note?.trim();

  return (
    <div className={cn('flex', isMe ? 'justify-end' : 'justify-start')}>
      <div
        className={cn(
          'max-w-[78%] rounded-2xl px-3 py-2',
          type === 'payment_accept'
            ? isMe
              ? 'rounded-br-md bg-emerald-600 text-white'
              : 'rounded-bl-md border border-emerald-400/30 bg-emerald-500/10 text-emerald-100'
            : isMe
              ? 'rounded-br-md bg-amber-600 text-white'
              : 'rounded-bl-md border border-amber-400/30 bg-amber-500/10 text-amber-100',
        )}
      >
        <p className={cn('break-words whitespace-pre-wrap text-sm')}>{resolveTitle(type, isMe)}</p>
        {note ? <p className={cn('mt-1 break-words whitespace-pre-wrap text-xs opacity-85')}>{note}</p> : null}

        <div
          className={cn(
            'mt-1 flex items-center gap-1 text-[10px]',
            isMe ? 'justify-end text-white/80' : 'text-[var(--text-secondary)]',
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
