import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { logger } from '@/shared/application/gateways/logger';

const HEARTBEAT_INTERVAL_MS = 5 * 60 * 1000;

export function startPresenceHeartbeat(hubConnection: HubConnection): NodeJS.Timeout {
 return setInterval(() => {
  if (hubConnection.state === HubConnectionState.Connected) {
   hubConnection.invoke('Heartbeat').catch((error) => {
    logger.error('[PresenceRealtimeSync] heartbeat error', error);
   });
  }
 }, HEARTBEAT_INTERVAL_MS);
}
