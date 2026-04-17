'use client';

import { parseApiError } from '@/shared/infrastructure/error/parseApiError';

const DEFAULT_CLIENT_TIMEOUT_MS = 8_000;
const MIN_CLIENT_TIMEOUT_MS = 1_000;

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

/**
 * Client fetch with mandatory timeout to prevent pending requests hanging UI flows.
 */
export async function fetchWithTimeout(url: string, init: RequestInit, timeoutMs?: number): Promise<Response> {
 const controller = new AbortController();
 const timeoutHandle = window.setTimeout(() => controller.abort(), resolveTimeout(timeoutMs));
 try {
  return await fetch(url, { ...init, signal: controller.signal });
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
  if (isAbortError(error)) {
   throw new Error('Request timed out.');
  }

  throw new Error(fallbackErrorMessage);
 }

 if (!response.ok) {
  throw new Error(await parseApiError(response, fallbackErrorMessage));
 }

 return (await response.json()) as T;
}
