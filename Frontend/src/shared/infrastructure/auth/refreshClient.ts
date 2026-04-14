'use client';

import { AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { getOrCreateDeviceId } from '@/shared/infrastructure/auth/deviceId';

let refreshInFlight: Promise<boolean> | null = null;

export async function tryRefreshClientSide(): Promise<boolean> {
 if (refreshInFlight) {
  return refreshInFlight;
 }

 refreshInFlight = (async () => {
  try {
   const response = await fetch('/api/auth/refresh', {
    method: 'POST',
    credentials: 'include',
    cache: 'no-store',
    headers: {
     [AUTH_HEADER.IDEMPOTENCY_KEY]: crypto.randomUUID(),
     [AUTH_HEADER.DEVICE_ID]: getOrCreateDeviceId(),
    },
   });

   return response.ok;
  } catch {
   return false;
  } finally {
   refreshInFlight = null;
  }
 })();

 return refreshInFlight;
}
