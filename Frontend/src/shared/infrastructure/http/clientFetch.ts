'use client';

import { parseApiError } from '@/shared/infrastructure/error/parseApiError';

const DEFAULT_CLIENT_TIMEOUT_MS = 8_000;
const MIN_CLIENT_TIMEOUT_MS = 1_000;
const REQUEST_TIMEOUT_ERROR_MESSAGE = 'Request timed out.';

function resolveTimeout(timeoutMs: number | undefined): number {
 if (typeof timeoutMs !== 'number' || !Number.isFinite(timeoutMs)) {
  return DEFAULT_CLIENT_TIMEOUT_MS;
 }

 return Math.max(MIN_CLIENT_TIMEOUT_MS, Math.floor(timeoutMs));
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

function mergeAbortSignals(
 requestSignal: AbortSignal | null | undefined,
 timeoutSignal: AbortSignal,
): AbortSignal {
 if (!requestSignal) {
  return timeoutSignal;
 }

 if (typeof AbortSignal.any === 'function') {
  return AbortSignal.any([requestSignal, timeoutSignal]);
 }

 const mergedController = new AbortController();
 const abortMerged = () => mergedController.abort();

 if (requestSignal.aborted || timeoutSignal.aborted) {
  abortMerged();
  return mergedController.signal;
 }

 requestSignal.addEventListener('abort', abortMerged, { once: true });
 timeoutSignal.addEventListener('abort', abortMerged, { once: true });
 return mergedController.signal;
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
 const signal = mergeAbortSignals(init.signal, timeoutController.signal);
 try {
  return await fetch(url, { ...init, signal });
 } catch (error) {
  if (isAbortError(error) && timedOut) {
   throw new Error(REQUEST_TIMEOUT_ERROR_MESSAGE);
  }
  throw error;
 } finally {
  window.clearTimeout(timeoutHandle);
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
