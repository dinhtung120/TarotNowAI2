import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { NextRequest } from 'next/server';
import { GET } from '@/app/[locale]/api/reading/sessions/[sessionId]/stream/route';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { internalApiUrl } from '@/shared/infrastructure/http/apiUrl';

vi.mock('@/shared/infrastructure/auth/serverAuth', () => ({
 getServerAccessToken: vi.fn(),
}));

vi.mock('@/shared/infrastructure/http/apiUrl', () => ({
 internalApiUrl: vi.fn((path: string) => `http://backend.test${path}`),
}));

vi.mock('@/shared/infrastructure/logging/logger', () => ({
 logger: {
  warn: vi.fn(),
  error: vi.fn(),
  debug: vi.fn(),
 },
}));

const mockedGetServerAccessToken = vi.mocked(getServerAccessToken);
const mockedInternalApiUrl = vi.mocked(internalApiUrl);
const mockedFetch = vi.fn<typeof fetch>();

describe('GET /[locale]/api/reading/sessions/[sessionId]/stream', () => {
 beforeEach(() => {
  vi.clearAllMocks();
  mockedGetServerAccessToken.mockResolvedValue('access-token');
  mockedFetch.mockReset();
  vi.stubGlobal('fetch', mockedFetch);
 });

 afterEach(() => {
  vi.useRealTimers();
 });

 it('rejects legacy follow-up query transport before opening the upstream stream', async () => {
  const request = new NextRequest(
   'http://localhost/vi/api/reading/sessions/session-1/stream?followupQuestion=legacy&language=vi',
   {
    headers: {
     origin: 'http://localhost',
    },
   },
  );

  const response = await GET(request, {
   params: Promise.resolve({ sessionId: 'session-1' }),
  });

  expect(response.status).toBe(400);
  await expect(response.json()).resolves.toMatchObject({
   detail: 'Follow-up question must be sent via stream ticket.',
  });
  expect(mockedFetch).not.toHaveBeenCalled();
 });

 it('forwards initial stream requests with the normalized language only', async () => {
  mockedFetch.mockResolvedValue(
   new Response('data: hello\n\n', {
    status: 200,
    headers: {
     'content-type': 'text/event-stream; charset=utf-8',
    },
   }),
  );

  const request = new NextRequest(
   'http://localhost/vi/api/reading/sessions/session-1/stream?language=zh-CN',
   {
    headers: {
     origin: 'http://localhost',
    },
   },
  );

  const response = await GET(request, {
   params: Promise.resolve({ sessionId: 'session-1' }),
  });

  expect(response.status).toBe(200);
  expect(mockedInternalApiUrl).toHaveBeenCalledWith('/sessions/session-1/stream');
  expect(mockedFetch).toHaveBeenCalledTimes(1);
  const [url, init] = mockedFetch.mock.calls[0] ?? [];
  expect(String(url)).toContain('language=zh');
 expect(String(url)).not.toContain('followupQuestion=');
 expect(init?.method).toBe('GET');
  expect(init?.headers).toBeInstanceOf(Headers);
  expect((init?.headers as Headers).get('Authorization')).toBe('Bearer access-token');
  await expect(response.text()).resolves.toContain('data: hello');
 });

 it('forwards follow-up stream requests with streamToken only', async () => {
  mockedFetch.mockResolvedValue(
   new Response('data: followup\n\n', {
    status: 200,
    headers: {
     'content-type': 'text/event-stream; charset=utf-8',
    },
   }),
  );

  const request = new NextRequest(
   'http://localhost/vi/api/reading/sessions/session-1/stream?streamToken=opaque-token&language=vi',
   {
    headers: {
     origin: 'http://localhost',
    },
   },
  );

  const response = await GET(request, {
   params: Promise.resolve({ sessionId: 'session-1' }),
  });

  expect(response.status).toBe(200);
  expect(mockedFetch).toHaveBeenCalledTimes(1);
 const [url] = mockedFetch.mock.calls[0] ?? [];
 expect(String(url)).toContain('streamToken=opaque-token');
 expect(String(url)).not.toContain('language=');
 expect(String(url)).not.toContain('followupQuestion=');
 });

 it('rejects missing access tokens and upstream rejections before proxying the stream body', async () => {
  mockedGetServerAccessToken.mockResolvedValueOnce(null);
  const unauthorizedResponse = await GET(
   new NextRequest('http://localhost/vi/api/reading/sessions/session-1/stream?language=vi'),
   { params: Promise.resolve({ sessionId: 'session-1' }) },
  );
  expect(unauthorizedResponse.status).toBe(401);
  expect(mockedFetch).not.toHaveBeenCalled();

  mockedFetch.mockResolvedValueOnce(new Response(null, { status: 429 }));
  const rejectedResponse = await GET(
   new NextRequest('http://localhost/vi/api/reading/sessions/session-1/stream?language=vi'),
   { params: Promise.resolve({ sessionId: 'session-1' }) },
  );
  expect(rejectedResponse.status).toBe(429);
 });

 it('forwards idempotency headers and aborts stalled upstream opens', async () => {
  mockedFetch.mockResolvedValueOnce(
   new Response('data: followup\n\n', {
    status: 200,
    headers: {
     'content-type': 'text/event-stream; charset=utf-8',
    },
   }),
  );

  const successfulResponse = await GET(
   new NextRequest(
    'http://localhost/vi/api/reading/sessions/session-1/stream?streamToken=opaque-token',
    {
     headers: {
      'x-idempotency-key': 'ticket-1',
     },
    },
   ),
   { params: Promise.resolve({ sessionId: 'session-1' }) },
  );

  expect(successfulResponse.status).toBe(200);
  expect((mockedFetch.mock.calls[0]?.[1]?.headers as Headers).get('x-idempotency-key')).toBe('ticket-1');

  vi.useFakeTimers();
  mockedFetch.mockImplementationOnce((_url, init) => new Promise((_, reject) => {
   const signal = init?.signal as AbortSignal;
   signal.addEventListener('abort', () => reject(new Error('aborted')));
  }));

  const pendingResponse = GET(
   new NextRequest('http://localhost/vi/api/reading/sessions/session-1/stream?language=vi'),
   { params: Promise.resolve({ sessionId: 'session-1' }) },
  );

  await vi.advanceTimersByTimeAsync(12_001);
  const timedOutResponse = await pendingResponse;
  expect(timedOutResponse.status).toBe(502);
 });

 it('rejects untrusted origins before contacting the upstream API', async () => {
  const request = new NextRequest(
   'http://localhost/vi/api/reading/sessions/session-1/stream?language=vi',
   {
    headers: {
     origin: 'https://evil.example',
    },
   },
  );

  const response = await GET(request, {
   params: Promise.resolve({ sessionId: 'session-1' }),
  });

  expect(response.status).toBe(403);
  expect(mockedFetch).not.toHaveBeenCalled();
 });

 it('rejects malformed session identifiers and upstream open failures', async () => {
  const invalidRequest = new NextRequest('http://localhost/vi/api/reading/sessions/bad/session/stream?language=vi');
  const invalidResponse = await GET(invalidRequest, {
   params: Promise.resolve({ sessionId: 'bad/session' }),
  });
  expect(invalidResponse.status).toBe(400);

  mockedFetch.mockRejectedValueOnce(new Error('upstream down'));
  const request = new NextRequest('http://localhost/vi/api/reading/sessions/session-1/stream?language=vi');
  const response = await GET(request, {
   params: Promise.resolve({ sessionId: 'session-1' }),
  });

  expect(response.status).toBe(502);
 });
});
