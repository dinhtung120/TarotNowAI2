import { beforeEach, describe, expect, it, vi } from 'vitest';
import { postJsonToApiV1 } from '@/shared/infrastructure/http/clientJsonRequest';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { tryRefreshClientSide } from '@/shared/infrastructure/auth/refreshClient';
import { browserApiPath, getPublicApiBaseUrl } from '@/shared/infrastructure/http/apiUrl';

vi.mock('@/shared/infrastructure/error/parseApiError', () => ({
 parseApiError: vi.fn(),
}));

vi.mock('@/shared/infrastructure/auth/refreshClient', () => ({
 tryRefreshClientSide: vi.fn(),
}));

vi.mock('@/shared/infrastructure/http/apiUrl', () => ({
 browserApiPath: vi.fn((path: string) => `/api${path}`),
 getPublicApiBaseUrl: vi.fn(() => 'https://example.test/api/v1'),
}));

const mockedParseApiError = vi.mocked(parseApiError);
const mockedTryRefreshClientSide = vi.mocked(tryRefreshClientSide);
const mockedBrowserApiPath = vi.mocked(browserApiPath);
const mockedGetPublicApiBaseUrl = vi.mocked(getPublicApiBaseUrl);

describe('clientJsonRequest', () => {
 beforeEach(() => {
  vi.restoreAllMocks();
  vi.unstubAllGlobals();
  mockedParseApiError.mockReset();
  mockedTryRefreshClientSide.mockReset();
  mockedBrowserApiPath.mockClear();
  mockedGetPublicApiBaseUrl.mockReset();
  mockedGetPublicApiBaseUrl.mockReturnValue('https://example.test/api/v1');
 });

 it('returns success payload for happy path request', async () => {
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ ok: true }), { status: 200 }));

  const result = await postJsonToApiV1<{ ok: boolean }, { a: number }>('/wallet/balance', { a: 1 }, {
   fallbackErrorMessage: 'fallback',
  });

  expect(result).toEqual({ ok: true, data: { ok: true } });
  expect(mockedBrowserApiPath).toHaveBeenCalledWith('/wallet/balance');
 });

 it('retries once after 401 when refresh succeeds', async () => {
  const fetchSpy = vi
   .spyOn(globalThis, 'fetch')
   .mockResolvedValueOnce(new Response('unauthorized', { status: 401 }))
   .mockResolvedValueOnce(new Response(JSON.stringify({ saved: true }), { status: 200 }));
  mockedTryRefreshClientSide.mockResolvedValue(true);

  const result = await postJsonToApiV1<{ saved: boolean }, { key: string }>('/resource', { key: 'v' }, {
   fallbackErrorMessage: 'fallback',
   unauthorizedMessage: 'unauthorized',
  });

  expect(fetchSpy).toHaveBeenCalledTimes(2);
  expect(mockedTryRefreshClientSide).toHaveBeenCalledTimes(1);
  expect(result).toEqual({ ok: true, data: { saved: true } });
 });

 it('returns unauthorized message when refresh fails', async () => {
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response('unauthorized', { status: 401 }));
  mockedTryRefreshClientSide.mockResolvedValue(false);

  const result = await postJsonToApiV1<{ saved: boolean }, { key: string }>('/resource', { key: 'v' }, {
   fallbackErrorMessage: 'fallback',
   unauthorizedMessage: 'not-authenticated',
  });

  expect(result).toEqual({ ok: false, error: 'not-authenticated', status: 401 });
 });

 it('falls back to generic message when refresh fails without unauthorized override', async () => {
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response('unauthorized', { status: 401 }));
  mockedTryRefreshClientSide.mockResolvedValue(false);

  const result = await postJsonToApiV1<{ saved: boolean }, { key: string }>('/resource', { key: 'v' }, {
   fallbackErrorMessage: 'fallback',
  });

  expect(result).toEqual({ ok: false, error: 'fallback', status: 401 });
 });

 it('returns 503 when retry request after refresh throws', async () => {
  vi.spyOn(globalThis, 'fetch')
   .mockResolvedValueOnce(new Response('unauthorized', { status: 401 }))
   .mockRejectedValueOnce(new Error('network retry down'));
  mockedTryRefreshClientSide.mockResolvedValue(true);

  const result = await postJsonToApiV1<{ saved: boolean }, { key: string }>('/resource', { key: 'v' }, {
   fallbackErrorMessage: 'fallback',
   unauthorizedMessage: 'not-authenticated',
  });

  expect(result).toEqual({ ok: false, error: 'fallback', status: 503 });
 });

 it('returns normalized error when response is not ok', async () => {
  mockedParseApiError.mockResolvedValue('normalized');
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response('bad request', { status: 422 }));

  const result = await postJsonToApiV1<{ saved: boolean }, { key: string }>('/resource', { key: 'v' }, {
   fallbackErrorMessage: 'fallback',
  });

  expect(result).toEqual({ ok: false, error: 'normalized', status: 422 });
  expect(mockedParseApiError).toHaveBeenCalledTimes(1);
 });

 it('returns 503 fallback when network request throws', async () => {
  vi.spyOn(globalThis, 'fetch').mockRejectedValue(new Error('network down'));

  const result = await postJsonToApiV1<{ saved: boolean }, { key: string }>('/resource', { key: 'v' }, {
   fallbackErrorMessage: 'fallback',
  });

  expect(result).toEqual({ ok: false, error: 'fallback', status: 503 });
 });

 it('resolves client API URL via browser fallback when server base URL resolution fails', async () => {
  vi.stubGlobal('window', undefined);
  mockedGetPublicApiBaseUrl.mockImplementation(() => {
   throw new Error('missing public base');
  });
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ ok: true }), { status: 200 }));

  const result = await postJsonToApiV1<{ ok: boolean }, { a: number }>('/fallback-path', { a: 1 }, {
   fallbackErrorMessage: 'fallback',
  });

  expect(result).toEqual({ ok: true, data: { ok: true } });
  expect(mockedBrowserApiPath).toHaveBeenCalledWith('/fallback-path');
 });
});
