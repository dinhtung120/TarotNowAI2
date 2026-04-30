type MessageWithId = {
 id: string;
 createdAt?: string;
 clientMessageId?: string | null;
 localStatus?: 'sending' | 'sent' | 'failed';
};

function normalizeClientMessageId(value: string | null | undefined): string | null {
 if (!value) return null;
 const trimmed = value.trim();
 return trimmed ? trimmed : null;
}

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

function mergeDuplicateMessage<T extends MessageWithId>(existing: T, incoming: T): T {
 const merged: T = { ...existing, ...incoming };
 if (incoming.localStatus) {
  return merged;
 }

 if (incoming.id.startsWith('tmp:')) {
  merged.localStatus = existing.localStatus;
  return merged;
 }

 merged.localStatus = undefined;
 return merged;
}

export function appendUniqueMessage<T extends MessageWithId>(
 messages: T[],
 incoming: T
): T[] {
 const byIdIndex = messages.findIndex((item) => item.id === incoming.id);
 if (byIdIndex >= 0) {
  const next = [...messages];
  next[byIdIndex] = mergeDuplicateMessage(next[byIdIndex], incoming);
  return sortMessages(next);
 }

 const incomingClientMessageId = normalizeClientMessageId(incoming.clientMessageId);
 if (incomingClientMessageId) {
  const byClientMessageIdIndex = messages.findIndex(
   (item) => normalizeClientMessageId(item.clientMessageId) === incomingClientMessageId
  );
  if (byClientMessageIdIndex >= 0) {
   const next = [...messages];
   next[byClientMessageIdIndex] = mergeDuplicateMessage(next[byClientMessageIdIndex], incoming);
   return sortMessages(next);
  }
 }

 return sortMessages([...messages, incoming]);
}

export function mergeHistoryWithRealtimeMessages<T extends MessageWithId>(
 realtimeMessages: T[],
 historyMessages: T[]
): T[] {
 let merged = [...historyMessages].reverse();
 for (const realtimeMessage of realtimeMessages) {
  merged = appendUniqueMessage(merged, realtimeMessage);
 }

 return sortMessages(merged);
}
