'use client';

import type { UserProfile } from '@/features/auth/domain/types';
import { isTerminalAuthError } from '@/shared/domain/authErrors';

interface SessionPayload {
 authenticated?: boolean;
 user?: UserProfile;
 error?: string;
}

export interface ClientSessionSnapshot {
 authenticated: boolean;
 user: UserProfile | null;
 terminalFailure: boolean;
}

interface SessionCacheState {
 expiresAt: number;
 snapshot: ClientSessionSnapshot;
}

interface GetClientSessionSnapshotOptions {
 force?: boolean;
 maxAgeMs?: number;
}

const DEFAULT_SNAPSHOT_TTL_MS = 2_000;

let sessionCache: SessionCacheState | null = null;
let sessionInFlight: Promise<ClientSessionSnapshot> | null = null;

function resolveTerminalFailure(response: Response, payload: SessionPayload | null): boolean {
 if (response.status === 401 || response.status === 403) {
  return true;
 }

 return isTerminalAuthError(payload?.error);
}

async function fetchClientSessionSnapshot(): Promise<ClientSessionSnapshot> {
 try {
  const response = await fetch('/api/auth/session', {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  });

  let payload: SessionPayload | null = null;
  try {
   payload = (await response.json()) as SessionPayload;
  } catch {
   payload = null;
  }

  if (!response.ok) {
   return {
    authenticated: false,
    user: null,
    terminalFailure: resolveTerminalFailure(response, payload),
   };
  }

  return {
   authenticated: Boolean(payload?.authenticated),
   user: payload?.user ?? null,
   terminalFailure: false,
  };
 } catch {
  return {
   authenticated: false,
   user: null,
   terminalFailure: false,
  };
 }
}

function normalizeTtl(maxAgeMs: number | undefined): number {
 if (typeof maxAgeMs !== 'number' || !Number.isFinite(maxAgeMs)) {
  return DEFAULT_SNAPSHOT_TTL_MS;
 }

 return Math.max(0, Math.floor(maxAgeMs));
}

/**
 * Fetches cookie-backed session snapshot with short-lived cache and in-flight dedupe.
 */
export async function getClientSessionSnapshot(options: GetClientSessionSnapshotOptions = {}): Promise<ClientSessionSnapshot> {
 const ttlMs = normalizeTtl(options.maxAgeMs);
 const now = Date.now();

 if (!options.force && sessionCache && sessionCache.expiresAt > now) {
  return sessionCache.snapshot;
 }

 if (!options.force && sessionInFlight) {
  return sessionInFlight;
 }

 sessionInFlight = fetchClientSessionSnapshot();
 try {
  const snapshot = await sessionInFlight;
  sessionCache = {
   snapshot,
   expiresAt: Date.now() + ttlMs,
  };
  return snapshot;
 } finally {
  sessionInFlight = null;
 }
}

/**
 * Clears cached session snapshot so the next call revalidates from server.
 */
export function invalidateClientSessionSnapshot(): void {
 sessionCache = null;
}
