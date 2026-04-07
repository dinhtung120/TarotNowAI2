import type { ChatMessageDto } from '@/features/chat/application/actions';
import type { CallLogPayload } from '@/features/chat/presentation/chat-room/messages/types';

const SYSTEM_TYPES = new Set([
  'system',
  'system_refund',
  'system_release',
  'system_dispute',
]);

export function isSystemMessage(message: ChatMessageDto) {
  return SYSTEM_TYPES.has(message.type);
}

export function parseCallLogPayload(message: ChatMessageDto): CallLogPayload {
  try {
    const parsed = JSON.parse(message.content) as unknown;
    if (parsed && typeof parsed === 'object') {
      return parsed as CallLogPayload;
    }
  } catch {
  }

  return {};
}
