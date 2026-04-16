'use client';

import { useAuthStore } from '@/store/authStore';

interface SessionPayload {
 authenticated?: boolean;
}

/**
 * Guard SignalR startup with the latest cookie-backed session state.
 */
export async function ensureRealtimeSession(): Promise<boolean> {
 try {
  const response = await fetch('/api/auth/session', {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  });

  if (!response.ok) {
   if (response.status === 401 || response.status === 403) {
    useAuthStore.getState().clearAuth();
    return false;
   }
   return true;
  }

  const payload = (await response.json()) as SessionPayload;
  if (payload.authenticated) {
   return true;
  }

  useAuthStore.getState().clearAuth();
  return false;
 } catch {
  // Network failures should not hard-stop realtime forever.
  return true;
 }
}

