'use client';

import { getClientSessionSnapshot } from '@/shared/infrastructure/auth/clientSessionSnapshot';
import { useAuthStore } from '@/store/authStore';

/**
 * Guard SignalR startup with the latest cookie-backed session state.
 */
export async function ensureRealtimeSession(): Promise<boolean> {
 const sessionSnapshot = await getClientSessionSnapshot({
  maxAgeMs: 10_000,
  mode: 'lite',
 });
 if (sessionSnapshot.terminalFailure) {
  useAuthStore.getState().clearAuth();
 }

 return sessionSnapshot.authenticated;
}
