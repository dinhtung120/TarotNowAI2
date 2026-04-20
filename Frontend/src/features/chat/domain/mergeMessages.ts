type MessageWithId = {
 id: string;
 createdAt?: string;
};

function toUnixMillis(value: string | undefined): number {
 if (!value) {
  return Number.NaN;
 }

 const parsed = Date.parse(value);
 return Number.isFinite(parsed) ? parsed : Number.NaN;
}

function compareMessageOrder<T extends MessageWithId>(left: T, right: T): number {
 const leftTime = toUnixMillis(left.createdAt);
 const rightTime = toUnixMillis(right.createdAt);
 if (Number.isFinite(leftTime) && Number.isFinite(rightTime) && leftTime !== rightTime) {
  return leftTime - rightTime;
 }

 return left.id.localeCompare(right.id);
}

function sortMessages<T extends MessageWithId>(messages: T[]): T[] {
 return [...messages].sort(compareMessageOrder);
}

export function appendUniqueMessage<T extends MessageWithId>(
 messages: T[],
 incoming: T
): T[] {
 if (messages.some((item) => item.id === incoming.id)) {
  return messages;
 }

 return sortMessages([...messages, incoming]);
}

export function mergeHistoryWithRealtimeMessages<T extends MessageWithId>(
 realtimeMessages: T[],
 historyMessages: T[]
): T[] {
 const fetchedMessages = [...historyMessages].reverse();
 const fetchedIds = new Set(fetchedMessages.map((item) => item.id));
 const socketOnlyMessages = realtimeMessages.filter((item) => !fetchedIds.has(item.id));

 return sortMessages([...fetchedMessages, ...socketOnlyMessages]);
}
