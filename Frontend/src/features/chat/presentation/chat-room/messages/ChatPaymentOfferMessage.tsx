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
          'max-w-sm space-y-2 rounded-xl border tn-border-warning-30 tn-bg-warning-10 p-3',
        )}
      >
        <p className={cn('text-xs font-bold tn-text-warning')}>Yêu cầu cộng tiền</p>
        <p className={cn('text-sm text-white')}>{message.content}</p>
        <p className={cn('text-sm font-bold tn-text-warning')}>{amount} 💎</p>

        {isUserRole === true ? (
          <div className={cn('flex gap-2')}>
            <button
              type="button"
              disabled={processingOfferId === message.id || Boolean(response)}
              onClick={() => void onAccept(message)}
              className={cn(
                'rounded border tn-border-success-20 tn-bg-success-20 px-2 py-1 text-xs tn-text-success disabled:opacity-50',
              )}
            >
              Đồng ý
            </button>
            <button
              type="button"
              disabled={processingOfferId === message.id || Boolean(response)}
              onClick={() => void onReject(message)}
              className={cn(
                'rounded border tn-border-danger-50 tn-bg-danger-soft px-2 py-1 text-xs tn-text-danger disabled:opacity-50',
              )}
            >
              Từ chối
            </button>
          </div>
        ) : null}

        {response ? (
          <p className={cn('tn-text-11 tn-text-secondary')}>
            {response === 'accept' ? 'Đã được chấp nhận' : 'Đã bị từ chối'}
          </p>
        ) : null}
      </div>
    </div>
  );
}
