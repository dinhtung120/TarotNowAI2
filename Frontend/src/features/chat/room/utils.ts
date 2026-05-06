import { normalizeReaderStatus } from '@/features/reader/shared/readerStatus';
import type { ChatMessageDto } from '@/features/chat/shared/actions';

type OfferResponseMap = Record<string, 'accept' | 'reject'>;

export function parseOfferResponseMap(messages: ChatMessageDto[]): OfferResponseMap {
  const map: OfferResponseMap = {};
  for (const message of messages) {
    if (message.type !== 'payment_accept' && message.type !== 'payment_reject') continue;
    try {
      const payload = JSON.parse(message.content) as { offerMessageId?: string };
      if (!payload.offerMessageId) continue;
      map[payload.offerMessageId] = message.type === 'payment_accept' ? 'accept' : 'reject';
    } catch {
    }
  }
  return map;
}

export function parseStatusLabel(status?: string | null) {
  switch (normalizeReaderStatus(status)) {
    case 'online':
      return { text: 'Online', color: 'text-[var(--success)]' };
    case 'busy':
      return { text: 'Busy', color: 'text-[var(--warning)]' };
    case 'offline':
    default:
      return { text: 'Offline', color: 'text-[var(--danger)]' };
  }
}
