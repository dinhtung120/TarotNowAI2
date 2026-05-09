import { beforeEach, describe, expect, it, vi } from 'vitest';
import { fetchWithTimeout } from '@/shared/http/clientFetch';
import { getClientSessionSnapshot, invalidateClientSessionSnapshot } from '@/features/auth/shared/auth/clientSessionSnapshot';

vi.mock('@/shared/http/clientFetch', () => ({
 fetchWithTimeout: vi.fn(),
}));

const mockedFetchWithTimeout = vi.mocked(fetchWithTimeout);

function createSessionResponse(userId: string): Response {
 return {
  ok: true,
  status: 200,
  json: vi.fn().mockResolvedValue({
   authenticated: true,
   user: { id: userId, username: 'Lucifer' },
  }),
 } as unknown as Response;
}

describe('getClientSessionSnapshot', () => {
 beforeEach(() => {
  vi.useRealTimers();
  invalidateClientSessionSnapshot();
  mockedFetchWithTimeout.mockReset();
 });

 it('reuses a fresh full snapshot for lite session checks', async () => {
  mockedFetchWithTimeout.mockResolvedValue(createSessionResponse('user-1'));

  await getClientSessionSnapshot({ mode: 'full', maxAgeMs: 10_000 });
  const liteSnapshot = await getClientSessionSnapshot({ mode: 'lite', maxAgeMs: 10_000 });

  expect(liteSnapshot.authenticated).toBe(true);
  expect(mockedFetchWithTimeout).toHaveBeenCalledTimes(1);
  expect(mockedFetchWithTimeout).toHaveBeenCalledWith(
   '/api/auth/session',
   expect.objectContaining({ method: 'GET', credentials: 'include', cache: 'no-store' }),
   6_000,
  );
 });

 it('reuses an in-flight full request for lite session checks', async () => {
  let resolveResponse: (response: Response) => void = () => undefined;
  mockedFetchWithTimeout.mockReturnValue(new Promise<Response>((resolve) => {
   resolveResponse = resolve;
  }));

  const fullPromise = getClientSessionSnapshot({ mode: 'full', maxAgeMs: 10_000 });
  const litePromise = getClientSessionSnapshot({ mode: 'lite', maxAgeMs: 10_000 });

  resolveResponse(createSessionResponse('user-1'));

  await expect(fullPromise).resolves.toMatchObject({ authenticated: true });
  await expect(litePromise).resolves.toMatchObject({ authenticated: true });
  expect(mockedFetchWithTimeout).toHaveBeenCalledTimes(1);
 });
});
