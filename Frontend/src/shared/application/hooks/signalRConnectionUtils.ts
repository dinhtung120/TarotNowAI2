import { HubConnectionState, type HubConnection } from '@microsoft/signalr';

export function shouldStopConnection(connection: HubConnection | null): connection is HubConnection {
 return !!connection && (
  connection.state === HubConnectionState.Connected
  || connection.state === HubConnectionState.Reconnecting
  || connection.state === HubConnectionState.Disconnecting
 );
}

export function isUnauthorizedNegotiationError(error: unknown): boolean {
 if (!error) {
  return false;
 }

 const text = typeof error === 'string'
  ? error
  : error instanceof Error
   ? error.message
   : JSON.stringify(error);

 return text.includes('401') || /unauthorized/i.test(text);
}

function createTimeoutError(label: string, timeoutMs: number): Error {
 return new Error(`${label} negotiation timeout after ${timeoutMs}ms.`);
}

export async function startConnectionWithTimeout(
 connection: HubConnection,
 timeoutMs: number,
 label = 'SignalR',
): Promise<void> {
 let timeoutId: NodeJS.Timeout | null = null;
 try {
  await Promise.race([
   connection.start(),
   new Promise<never>((_, reject) => {
    timeoutId = setTimeout(() => {
     reject(createTimeoutError(label, timeoutMs));
    }, timeoutMs);
   }),
  ]);
 } finally {
  if (timeoutId) {
   clearTimeout(timeoutId);
  }
 }
}

export async function stopConnectionSafely(connection: HubConnection): Promise<void> {
 try {
  await connection.stop();
 } catch {
  return;
 }
}

export function hasSameNumberArray(left: readonly number[], right: readonly number[]): boolean {
 if (left.length !== right.length) {
  return false;
 }

 for (let index = 0; index < left.length; index += 1) {
  if (left[index] !== right[index]) {
   return false;
  }
 }

 return true;
}
