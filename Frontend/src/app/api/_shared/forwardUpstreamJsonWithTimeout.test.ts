// @vitest-environment node

import { afterEach, describe, expect, it, vi } from 'vitest';
import { forwardUpstreamJsonWithTimeout } from '@/app/api/_shared/forwardUpstreamJsonWithTimeout';

describe('forwardUpstreamJsonWithTimeout', () => {
 afterEach(() => {
  vi.unstubAllGlobals();
 });

 it('forwards successful upstream JSON response', async () => {
  vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(
   JSON.stringify({ streamToken: 'abc' }),
   { status: 200, headers: { 'content-type': 'application/json' } },
  )));

  const response = await forwardUpstreamJsonWithTimeout('https://example.test', {
   timeoutMs: 3000,
   fallbackErrorDetail: 'fallback',
   request: { method: 'POST' },
  });

  expect(response.status).toBe(200);
  expect(await response.text()).toBe(JSON.stringify({ streamToken: 'abc' }));
 });

 it('maps upstream problem payload to ProblemDetails response', async () => {
  vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(
   JSON.stringify({ detail: 'upstream denied', errorCode: 'DENIED' }),
   { status: 403, headers: { 'content-type': 'application/problem+json' } },
  )));

  const response = await forwardUpstreamJsonWithTimeout('https://example.test', {
   timeoutMs: 3000,
   fallbackErrorDetail: 'fallback',
   request: { method: 'POST' },
  });

  expect(response.status).toBe(403);
  await expect(response.json()).resolves.toEqual(expect.objectContaining({
   detail: 'upstream denied',
   errorCode: 'DENIED',
  }));
 });

 it('returns 504 when upstream request times out', async () => {
  const abortError = new DOMException('Aborted', 'AbortError');
  vi.stubGlobal('fetch', vi.fn().mockRejectedValue(abortError));

  const response = await forwardUpstreamJsonWithTimeout('https://example.test', {
   timeoutMs: 1,
   fallbackErrorDetail: 'fallback',
   request: { method: 'POST' },
  });

  expect(response.status).toBe(504);
 });
});
