import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { DOMAIN_EVENT_HEADER } from '@/shared/domain/eventContracts';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

vi.mock('@/shared/infrastructure/http/apiUrl', () => ({
 internalApiUrl: vi.fn((path: string) => `http://api.local${path}`),
}));

describe('serverHttpClient', () => {
 const originalFetch = global.fetch;

 beforeEach(() => {
  vi.clearAllMocks();
 });

 afterEach(() => {
  global.fetch = originalFetch;
 });

 it('injects expected domain events header when provided', async () => {
  const fetchMock = vi.fn().mockResolvedValue(new Response(JSON.stringify({ ok: true }), {
   status: 200,
   headers: {
    'content-type': 'application/json',
   },
  }));
  global.fetch = fetchMock as unknown as typeof fetch;

  await serverHttpRequest('/commands/run', {
   method: 'POST',
   expectedDomainEvents: ['MoneyChangedEvent', 'ConversationUpdatedEvent'],
   json: { amount: 1 },
   fallbackErrorMessage: 'failed',
  });

  expect(fetchMock).toHaveBeenCalledTimes(1);
  const [, requestInit] = fetchMock.mock.calls[0] as [string, RequestInit];
  const headers = new Headers(requestInit.headers);
  expect(headers.get(DOMAIN_EVENT_HEADER)).toBe('MoneyChangedEvent,ConversationUpdatedEvent');
 });

 it('returns fallback timeout status when request aborts', async () => {
  const abortError = new DOMException('Aborted', 'AbortError');
  const fetchMock = vi.fn().mockRejectedValue(abortError);
  global.fetch = fetchMock as unknown as typeof fetch;

  const result = await serverHttpRequest('/timeout', {
   method: 'GET',
   timeoutMs: 5,
   fallbackErrorMessage: 'timeout',
  });

  expect(result).toEqual({
   ok: false,
   status: 504,
   headers: new Headers(),
   error: 'timeout',
  });
 });
});
