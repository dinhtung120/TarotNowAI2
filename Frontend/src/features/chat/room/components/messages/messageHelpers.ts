import type { ChatMessageDto } from '@/features/chat/shared/actions';

const SYSTEM_TYPES = new Set([
  'system',
  'system_refund',
  'system_release',
  'system_dispute',
]);

export function isSystemMessage(message: ChatMessageDto) {
  return SYSTEM_TYPES.has(message.type);
}
