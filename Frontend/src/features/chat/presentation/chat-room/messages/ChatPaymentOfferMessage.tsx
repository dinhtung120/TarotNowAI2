import type { ChatMessageDto } from '@/features/chat/application/actions';
import { cn } from '@/lib/utils';

interface ChatPaymentOfferMessageProps {
  isMe: boolean;
  isUserRole: boolean | null;
  message: ChatMessageDto;
  processingOfferId: string | null;
  response?: 'accept' | 'reject';
  onAccept: (message: ChatMessageDto) => Promise<boolean>;
  onReject: (message: ChatMessageDto) => Promise<boolean>;
}

export default function ChatPaymentOfferMessage({
  isMe,
  isUserRole,
  message,
  processingOfferId,
  response,
  onAccept,
  onReject,
}: ChatPaymentOfferMessageProps) {
  const amount = message.paymentPayload?.amountDiamond ?? 0;

  return (
    <div className={cn('flex', isMe ? 'justify-end' : 'justify-start')}>
      <div
        className={cn(
          'max-w-[80%] space-y-2 rounded-xl border border-[var(--warning)]/30 bg-[var(--warning)]/10 p-3',
        )}
      >
        <p className={cn('text-xs font-bold text-[var(--warning)]')}>Yêu cầu cộng tiền</p>
        <p className={cn('text-sm text-white')}>{message.content}</p>
        <p className={cn('text-sm font-bold text-[var(--warning)]')}>{amount} 💎</p>

        {isUserRole === true ? (
          <div className={cn('flex gap-2')}>
            <button
              type="button"
              disabled={processingOfferId === message.id || Boolean(response)}
              onClick={() => void onAccept(message)}
              className={cn(
                'rounded border border-[var(--success)]/25 bg-[var(--success)]/20 px-2 py-1 text-xs text-[var(--success)] disabled:opacity-50',
              )}
            >
              Đồng ý
            </button>
            <button
              type="button"
              disabled={processingOfferId === message.id || Boolean(response)}
              onClick={() => void onReject(message)}
              className={cn(
                'rounded border border-[var(--danger)]/25 bg-[var(--danger)]/20 px-2 py-1 text-xs text-[var(--danger)] disabled:opacity-50',
              )}
            >
              Từ chối
            </button>
          </div>
        ) : null}

        {response ? (
          <p className={cn('text-[11px] text-[var(--text-secondary)]')}>
            {response === 'accept' ? 'Đã được chấp nhận' : 'Đã bị từ chối'}
          </p>
        ) : null}
      </div>
    </div>
  );
}
