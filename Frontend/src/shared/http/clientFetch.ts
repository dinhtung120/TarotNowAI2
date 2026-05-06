'use client';

import { parseApiError } from '@/shared/error/parseApiError';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { getRuntimePolicyStoreSnapshot } from '@/shared/config/runtimePolicyStore';

const DEFAULT_CLIENT_TIMEOUT_MS = RUNTIME_POLICY_FALLBACKS.http.clientTimeoutMs;
const MIN_CLIENT_TIMEOUT_MS = RUNTIME_POLICY_FALLBACKS.http.minTimeoutMs;
const REQUEST_TIMEOUT_ERROR_MESSAGE = 'Request timed out.';

function resolveTimeout(timeoutMs: number | undefined): number {
 const runtimePolicy = getRuntimePolicyStoreSnapshot();
 const defaultTimeoutMs = runtimePolicy.http.clientTimeoutMs || DEFAULT_CLIENT_TIMEOUT_MS;
 const minTimeoutMs = runtimePolicy.http.minTimeoutMs || MIN_CLIENT_TIMEOUT_MS;

 if (typeof timeoutMs !== 'number' || !Number.isFinite(timeoutMs)) {
  return defaultTimeoutMs;
 }

 return Math.max(minTimeoutMs, Math.floor(timeoutMs));
}

function isAbortError(error: unknown): boolean {
 if (error instanceof DOMException) {
  return error.name === 'AbortError';
 }

 if (error instanceof Error) {
  return error.name === 'AbortError';
 }

 return false;
}

interface MergedAbortSignal {
 signal: AbortSignal;
 cleanup: () => void;
}

function mergeAbortSignals(
 requestSignal: AbortSignal | null | undefined,
 timeoutSignal: AbortSignal,
): MergedAbortSignal {
 if (!requestSignal) {
  return {
   signal: timeoutSignal,
   cleanup: () => undefined,
  };
 }

 if (typeof AbortSignal.any === 'function') {
  return {
   signal: AbortSignal.any([requestSignal, timeoutSignal]),
   cleanup: () => undefined,
  };
 }

 const mergedController = new AbortController();
 const abortMerged = () => mergedController.abort();

 if (requestSignal.aborted || timeoutSignal.aborted) {
  abortMerged();
  return {
   signal: mergedController.signal,
   cleanup: () => undefined,
  };
 }

 requestSignal.addEventListener('abort', abortMerged, { once: true });
 timeoutSignal.addEventListener('abort', abortMerged, { once: true });
 return {
  signal: mergedController.signal,
  cleanup: () => {
   requestSignal.removeEventListener('abort', abortMerged);
   timeoutSignal.removeEventListener('abort', abortMerged);
  },
 };
}

/**
 * Client fetch with mandatory timeout to prevent pending requests hanging UI flows.
 */
export async function fetchWithTimeout(url: string, init: RequestInit, timeoutMs?: number): Promise<Response> {
 const timeoutController = new AbortController();
 let timedOut = false;
 const timeoutHandle = window.setTimeout(() => {
  timedOut = true;
  timeoutController.abort();
 }, resolveTimeout(timeoutMs));
 const { signal, cleanup } = mergeAbortSignals(init.signal, timeoutController.signal);
 try {
  return await fetch(url, { ...init, signal });
 } catch (error) {
  if (isAbortError(error) && timedOut) {
   throw new Error(REQUEST_TIMEOUT_ERROR_MESSAGE);
  }
  throw error;
 } finally {
  window.clearTimeout(timeoutHandle);
  cleanup();
 }
}

/**
 * Fetch JSON and throw normalized error for React Query hooks.
 */
export async function fetchJsonOrThrow<T>(
 url: string,
 init: RequestInit,
 fallbackErrorMessage: string,
 timeoutMs?: number,
): Promise<T> {
 let response: Response;
 try {
  response = await fetchWithTimeout(url, init, timeoutMs);
 } catch (error) {
  if (error instanceof Error && error.message === REQUEST_TIMEOUT_ERROR_MESSAGE) {
   throw error;
  }

  // Preserve abort for React Query cancellation flow during route changes.
  if (isAbortError(error)) {
   throw error;
  }

  throw new Error(fallbackErrorMessage);
 }

 if (!response.ok) {
  throw new Error(await parseApiError(response, fallbackErrorMessage));
 }

 return (await response.json()) as T;
}
