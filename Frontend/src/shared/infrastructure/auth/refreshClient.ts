'use client';

import { AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { getOrCreateDeviceId } from '@/shared/infrastructure/auth/deviceId';
import { fetchWithTimeout } from '@/shared/infrastructure/http/clientFetch';

let refreshInFlight: Promise<boolean> | null = null;
const REFRESH_TIMEOUT_MS = 8_000;

function createRefreshIdempotencyKey(): string {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return crypto.randomUUID();
 }

 return `${Date.now()}_${Math.random().toString(16).slice(2)}`;
}

export async function tryRefreshClientSide(): Promise<boolean> {
 if (refreshInFlight) {
  return refreshInFlight;
 }

 refreshInFlight = (async () => {
  try {
   const response = await fetchWithTimeout(
    '/api/auth/refresh',
    {
     method: 'POST',
     credentials: 'include',
     cache: 'no-store',
     headers: {
      [AUTH_HEADER.IDEMPOTENCY_KEY]: createRefreshIdempotencyKey(),
      [AUTH_HEADER.DEVICE_ID]: getOrCreateDeviceId(),
     },
    },
    REFRESH_TIMEOUT_MS,
   );

   return response.ok;
  } catch {
   return false;
  } finally {
   refreshInFlight = null;
  }
 })();

 return refreshInFlight;
}
