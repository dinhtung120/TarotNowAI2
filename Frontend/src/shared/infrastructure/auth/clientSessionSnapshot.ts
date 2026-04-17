'use client';

import type { UserProfile } from '@/features/auth/domain/types';
import { isTerminalAuthError } from '@/shared/domain/authErrors';
import { fetchWithTimeout } from '@/shared/infrastructure/http/clientFetch';

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
 mode?: ClientSessionSnapshotMode;
}

export type ClientSessionSnapshotMode = 'full' | 'lite';

const DEFAULT_SNAPSHOT_TTL_MS = 10_000;
const SESSION_FETCH_TIMEOUT_MS = 6_000;
const SESSION_MODE_FULL: ClientSessionSnapshotMode = 'full';
const SESSION_MODE_LITE: ClientSessionSnapshotMode = 'lite';

const sessionCache: Record<ClientSessionSnapshotMode, SessionCacheState | null> = {
 full: null,
 lite: null,
};

const sessionInFlight: Record<ClientSessionSnapshotMode, Promise<ClientSessionSnapshot> | null> = {
 full: null,
 lite: null,
};

function resolveTerminalFailure(response: Response, payload: SessionPayload | null): boolean {
 if (response.status === 401 || response.status === 403) {
  return true;
 }

 return isTerminalAuthError(payload?.error);
}

function resolveSessionUrl(mode: ClientSessionSnapshotMode): string {
 if (mode === SESSION_MODE_LITE) {
  return '/api/auth/session?mode=lite';
 }

 return '/api/auth/session';
}

function normalizeMode(mode: ClientSessionSnapshotMode | undefined): ClientSessionSnapshotMode {
 if (mode === SESSION_MODE_LITE) {
  return SESSION_MODE_LITE;
 }

 return SESSION_MODE_FULL;
}

async function fetchClientSessionSnapshot(mode: ClientSessionSnapshotMode): Promise<ClientSessionSnapshot> {
 try {
  const response = await fetchWithTimeout(
   resolveSessionUrl(mode),
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   SESSION_FETCH_TIMEOUT_MS,
  );

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
 const mode = normalizeMode(options.mode);
 const ttlMs = normalizeTtl(options.maxAgeMs);
 const now = Date.now();
 const cacheState = sessionCache[mode];
 const inFlightState = sessionInFlight[mode];

 if (!options.force && cacheState && cacheState.expiresAt > now) {
  return cacheState.snapshot;
 }

 if (!options.force && inFlightState) {
  return inFlightState;
 }

 const activeRequest = fetchClientSessionSnapshot(mode);
 sessionInFlight[mode] = activeRequest;
 try {
  const snapshot = await activeRequest;
  sessionCache[mode] = {
   snapshot,
   expiresAt: Date.now() + ttlMs,
  };
  return snapshot;
 } finally {
  sessionInFlight[mode] = null;
 }
}

/**
 * Clears cached session snapshot so the next call revalidates from server.
 */
export function invalidateClientSessionSnapshot(): void {
 sessionCache[SESSION_MODE_FULL] = null;
 sessionCache[SESSION_MODE_LITE] = null;
}
