import type { HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { applyWalletDelta, createPresenceWalletRefreshScheduler } from '@/shared/application/hooks/usePresenceConnection.registration.wallet';
import { createPresenceInvalidationScheduler } from '@/shared/application/hooks/usePresenceConnection.registration.invalidationScheduler';
import { createPresenceRoleChangeLogoutCoordinator } from '@/shared/application/hooks/usePresenceConnection.registration.roleLogout';
import { createPresenceChatInvalidationSchedulers } from '@/shared/application/hooks/usePresenceConnection.registration.chatInvalidation';
import { registerPresenceDomainEventHandlers } from '@/shared/application/hooks/usePresenceConnection.registration.domainEvents';
import { startPresenceHeartbeat } from '@/shared/application/hooks/usePresenceConnection.registration.heartbeat';
import { createPresenceStatusObserverCoordinator } from '@/shared/application/hooks/usePresenceConnection.registration.statusObservers';

export function registerPresenceConnectionHandlers(
 hubConnection: HubConnection,
 queryClient: QueryClient,
) {
 const invalidationScheduler = createPresenceInvalidationScheduler(queryClient);
 const walletRefreshScheduler = createPresenceWalletRefreshScheduler();
 const roleLogoutCoordinator = createPresenceRoleChangeLogoutCoordinator(queryClient);
 const chatInvalidationSchedulers = createPresenceChatInvalidationSchedulers(queryClient);
 const statusObserverCoordinator = createPresenceStatusObserverCoordinator(hubConnection, queryClient);

 registerPresenceDomainEventHandlers({
  forceLogoutAfterRoleChange: roleLogoutCoordinator.handleNotificationType,
  hubConnection,
  invalidateInbox: chatInvalidationSchedulers.invalidateInbox,
  invalidateUnreadBadge: chatInvalidationSchedulers.invalidateUnreadBadge,
  queryClient,
  queueInvalidation: invalidationScheduler.queueInvalidation,
  scheduleWalletRefresh: walletRefreshScheduler.scheduleWalletRefresh,
  tryApplyWalletDelta: applyWalletDelta,
 });

 return {
  dispose: () => {
   statusObserverCoordinator.dispose();
   chatInvalidationSchedulers.dispose();
   invalidationScheduler.dispose();
   walletRefreshScheduler.dispose();
  },
  syncStatusObservers: () => statusObserverCoordinator.sync(),
  startHeartbeat: () => startPresenceHeartbeat(hubConnection),
 };
}
