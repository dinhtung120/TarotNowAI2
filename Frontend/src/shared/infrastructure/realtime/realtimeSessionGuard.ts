'use client';

import { getClientSessionSnapshot } from '@/shared/infrastructure/auth/clientSessionSnapshot';

/**
 * Guard SignalR startup with the latest cookie-backed session state.
 */
export async function ensureRealtimeSession(): Promise<boolean> {
 const sessionSnapshot = await getClientSessionSnapshot({
  maxAgeMs: 10_000,
  mode: 'lite',
 });
 // Không clear auth store tại đây để tránh làm UI "mất danh tính" tạm thời khi session endpoint lỗi tức thời.
 return sessionSnapshot.authenticated && !sessionSnapshot.terminalFailure;
}
