import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { fetchJsonOrThrow, fetchWithTimeout } from '@/shared/infrastructure/http/clientFetch';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';

vi.mock('@/shared/infrastructure/error/parseApiError', () => ({
 parseApiError: vi.fn(),
}));

vi.mock('@/shared/config/runtimePolicyStore', () => ({
 getRuntimePolicyStoreSnapshot: vi.fn(() => ({
  http: {
   clientTimeoutMs: 20,
   minTimeoutMs: 1,
  },
 })),
}));

const mockedParseApiError = vi.mocked(parseApiError);

describe('clientFetch', () => {
 beforeEach(() => {
  vi.useRealTimers();
  vi.restoreAllMocks();
  mockedParseApiError.mockReset();
 });

 afterEach(() => {
  vi.useRealTimers();
 });

 it('returns fetch response for fetchWithTimeout when request succeeds', async () => {
  const response = new Response(JSON.stringify({ ok: true }), { status: 200 });
  const fetchSpy = vi.spyOn(globalThis, 'fetch').mockResolvedValue(response);

  const result = await fetchWithTimeout('/ok', { method: 'GET' }, 10);

  expect(result).toBe(response);
  expect(fetchSpy).toHaveBeenCalledTimes(1);
 });

 it('throws timeout error when request hangs past timeout budget', async () => {
  vi.useFakeTimers();
  vi.spyOn(globalThis, 'fetch').mockImplementation((_, init) => {
   const signal = init?.signal as AbortSignal | undefined;
   return new Promise<Response>((_resolve, reject) => {
    signal?.addEventListener('abort', () => reject(new DOMException('Aborted', 'AbortError')), { once: true });
   });
  });

  const pending = fetchWithTimeout('/slow', { method: 'GET' }, 1)
   .then(() => null)
   .catch((error) => error as Error);
  await vi.advanceTimersByTimeAsync(5);

  const timeoutError = await pending;
  expect(timeoutError).toBeInstanceOf(Error);
  expect(timeoutError?.message).toBe('Request timed out.');
 });

 it('rethrows abort error when caller signal aborts request', async () => {
  const controller = new AbortController();
  vi.spyOn(globalThis, 'fetch').mockImplementation((_, init) => {
   const signal = init?.signal as AbortSignal | undefined;
   return new Promise<Response>((_resolve, reject) => {
    signal?.addEventListener('abort', () => reject(new DOMException('Aborted', 'AbortError')), { once: true });
   });
  });

  const pending = fetchWithTimeout('/abort', { method: 'GET', signal: controller.signal }, 250)
   .then(() => null)
   .catch((error) => error as Error);
  controller.abort();

  const abortError = await pending;
  expect(abortError).toBeInstanceOf(DOMException);
  expect(abortError?.name).toBe('AbortError');
 });

 it('cleans up merged abort listeners when AbortSignal.any is unavailable', async () => {
  const anyDescriptor = Object.getOwnPropertyDescriptor(AbortSignal, 'any');
  const controller = new AbortController();
  const removeEventListenerSpy = vi.spyOn(controller.signal, 'removeEventListener');
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ ok: true }), { status: 200 }));

  try {
   Object.defineProperty(AbortSignal, 'any', {
    value: undefined,
    configurable: true,
   });
   await fetchWithTimeout('/cleanup', { method: 'GET', signal: controller.signal }, 100);
  } finally {
   if (anyDescriptor) {
    Object.defineProperty(AbortSignal, 'any', anyDescriptor);
   }
  }

  expect(removeEventListenerSpy).toHaveBeenCalledWith('abort', expect.any(Function));
 });

 it('throws fallback error message when network request fails in fetchJsonOrThrow', async () => {
  vi.spyOn(globalThis, 'fetch').mockRejectedValue(new Error('socket error'));

  await expect(fetchJsonOrThrow('/error', { method: 'GET' }, 'fallback', 10)).rejects.toThrow('fallback');
 });

 it('throws parsed API error when response is not ok', async () => {
  mockedParseApiError.mockResolvedValue('normalized error');
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response('bad', { status: 400 }));

  await expect(fetchJsonOrThrow('/bad', { method: 'GET' }, 'fallback', 10)).rejects.toThrow('normalized error');
  expect(mockedParseApiError).toHaveBeenCalledTimes(1);
 });

 it('preserves abort error for fetchJsonOrThrow cancellation flow', async () => {
  const abortError = new DOMException('Aborted', 'AbortError');
  vi.spyOn(globalThis, 'fetch').mockRejectedValue(abortError);

  await expect(fetchJsonOrThrow('/abort-json', { method: 'GET' }, 'fallback', 10)).rejects.toBe(abortError);
 });

 it('returns parsed json when response is ok', async () => {
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(
   new Response(JSON.stringify({ id: 1, name: 'ok' }), {
    status: 200,
    headers: { 'Content-Type': 'application/json' },
   }),
  );

  const data = await fetchJsonOrThrow<{ id: number; name: string }>('/ok-json', { method: 'GET' }, 'fallback', 10);

  expect(data).toEqual({ id: 1, name: 'ok' });
 });
});
