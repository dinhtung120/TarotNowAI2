import type { ChatMessageDto } from '@/actions/chatActions';

export function appendUniqueMessage(
 messages: ChatMessageDto[],
 incoming: ChatMessageDto
): ChatMessageDto[] {
 if (messages.some((item) => item.id === incoming.id)) {
  return messages;
 }

 return [...messages, incoming];
}

export function mergeHistoryWithRealtimeMessages(
 realtimeMessages: ChatMessageDto[],
 historyMessages: ChatMessageDto[]
): ChatMessageDto[] {
 const fetchedMessages = [...historyMessages].reverse();
 const fetchedIds = new Set(fetchedMessages.map((item) => item.id));
 const socketOnlyMessages = realtimeMessages.filter((item) => !fetchedIds.has(item.id));

 return [...fetchedMessages, ...socketOnlyMessages];
}
