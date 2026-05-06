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

function canKeepOrderAfterReplace<T extends MessageWithId>(messages: T[], index: number, nextMessage: T): boolean {
 const previous = index > 0 ? messages[index - 1] : null;
 const following = index < messages.length - 1 ? messages[index + 1] : null;

 if (previous && compareMessageOrder(previous, nextMessage) > 0) {
  return false;
 }

 if (following && compareMessageOrder(nextMessage, following) > 0) {
  return false;
 }

 return true;
}

function canAppendWithoutSorting<T extends MessageWithId>(messages: T[], incoming: T): boolean {
 if (messages.length === 0) {
  return true;
 }

 const last = messages[messages.length - 1];
 return compareMessageOrder(last, incoming) <= 0;
}

export function appendUniqueMessage<T extends MessageWithId>(
 messages: T[],
 incoming: T
): T[] {
 const byIdIndex = messages.findIndex((item) => item.id === incoming.id);
 if (byIdIndex >= 0) {
  const next = [...messages];
  const merged = mergeDuplicateMessage(next[byIdIndex], incoming);
  next[byIdIndex] = merged;
  return canKeepOrderAfterReplace(next, byIdIndex, merged) ? next : sortMessages(next);
 }

 const incomingClientMessageId = normalizeClientMessageId(incoming.clientMessageId);
 if (incomingClientMessageId) {
  const byClientMessageIdIndex = messages.findIndex(
   (item) => normalizeClientMessageId(item.clientMessageId) === incomingClientMessageId
  );
  if (byClientMessageIdIndex >= 0) {
   const next = [...messages];
   const merged = mergeDuplicateMessage(next[byClientMessageIdIndex], incoming);
   next[byClientMessageIdIndex] = merged;
   return canKeepOrderAfterReplace(next, byClientMessageIdIndex, merged) ? next : sortMessages(next);
  }
 }

 if (canAppendWithoutSorting(messages, incoming)) {
  return [...messages, incoming];
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
