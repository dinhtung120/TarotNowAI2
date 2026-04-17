'use client';

import { getClientSessionSnapshot } from '@/shared/infrastructure/auth/clientSessionSnapshot';
import { useAuthStore } from '@/store/authStore';

/**
 * Guard SignalR startup with the latest cookie-backed session state.
 */
export async function ensureRealtimeSession(): Promise<boolean> {
 const sessionSnapshot = await getClientSessionSnapshot({ maxAgeMs: 2_000 });
 if (sessionSnapshot.authenticated) {
  return true;
 }

 if (sessionSnapshot.terminalFailure) {
  useAuthStore.getState().clearAuth();
  return false;
 }

 // Network/transient failures should not hard-stop realtime forever.
 return true;
}
