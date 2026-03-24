type MessageWithId = {
 id: string;
};

export function appendUniqueMessage<T extends MessageWithId>(
 messages: T[],
 incoming: T
): T[] {
 if (messages.some((item) => item.id === incoming.id)) {
  return messages;
 }

 return [...messages, incoming];
}

export function mergeHistoryWithRealtimeMessages<T extends MessageWithId>(
 realtimeMessages: T[],
 historyMessages: T[]
): T[] {
 const fetchedMessages = [...historyMessages].reverse();
 const fetchedIds = new Set(fetchedMessages.map((item) => item.id));
 const socketOnlyMessages = realtimeMessages.filter((item) => !fetchedIds.has(item.id));

 return [...fetchedMessages, ...socketOnlyMessages];
}
