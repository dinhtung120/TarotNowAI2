import { beforeEach, describe, expect, it, vi } from 'vitest';
import { AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { fetchWithTimeout } from '@/shared/infrastructure/http/clientFetch';
import { getOrCreateDeviceId } from '@/shared/infrastructure/auth/deviceId';

vi.mock('@/shared/infrastructure/http/clientFetch', () => ({
 fetchWithTimeout: vi.fn(),
}));

vi.mock('@/shared/infrastructure/auth/deviceId', () => ({
 getOrCreateDeviceId: vi.fn(),
}));

const mockedFetchWithTimeout = vi.mocked(fetchWithTimeout);
const mockedGetOrCreateDeviceId = vi.mocked(getOrCreateDeviceId);

describe('refreshClient', () => {
 beforeEach(() => {
  vi.clearAllMocks();
  mockedGetOrCreateDeviceId.mockReturnValue('device-1');
 });

 it('returns true when refresh endpoint responds ok', async () => {
  mockedFetchWithTimeout.mockResolvedValue(new Response(null, { status: 200 }));
  const { tryRefreshClientSide } = await import('@/shared/infrastructure/auth/refreshClient');

  const result = await tryRefreshClientSide();

  expect(result).toBe(true);
  expect(mockedFetchWithTimeout).toHaveBeenCalledWith('/api/auth/refresh', expect.objectContaining({
   method: 'POST',
   credentials: 'include',
   cache: 'no-store',
   headers: expect.objectContaining({
    [AUTH_HEADER.DEVICE_ID]: 'device-1',
   }),
  }), 8_000);
 });

 it('deduplicates in-flight refresh requests', async () => {
  let resolveFetch: ((value: Response) => void) | null = null;
  mockedFetchWithTimeout.mockImplementation(() => new Promise<Response>((resolve) => {
   resolveFetch = resolve;
  }));

  const { tryRefreshClientSide } = await import('@/shared/infrastructure/auth/refreshClient');
  const pendingA = tryRefreshClientSide();
  const pendingB = tryRefreshClientSide();
  resolveFetch?.(new Response(null, { status: 200 }));
  const [resultA, resultB] = await Promise.all([pendingA, pendingB]);
  expect(resultA).toBe(true);
  expect(resultB).toBe(true);
  expect(mockedFetchWithTimeout).toHaveBeenCalledTimes(1);
 });

 it('returns false when fetch fails', async () => {
  mockedFetchWithTimeout.mockRejectedValue(new Error('network error'));
  const { tryRefreshClientSide } = await import('@/shared/infrastructure/auth/refreshClient');

  const result = await tryRefreshClientSide();

  expect(result).toBe(false);
 });
});
