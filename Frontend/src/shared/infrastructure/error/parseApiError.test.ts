import { describe, expect, it } from 'vitest';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';

describe('parseApiError', () => {
 it('preserves backend auth errorCode for 401 responses', async () => {
  const response = new Response(
   JSON.stringify({ errorCode: AUTH_ERROR.TOKEN_REPLAY }),
   {
    status: 401,
    headers: { 'Content-Type': 'application/json' },
   },
  );

  await expect(parseApiError(response)).resolves.toBe(AUTH_ERROR.TOKEN_REPLAY);
 });

 it('falls back to AUTH_UNAUTHORIZED for 401 without body', async () => {
  const response = new Response('', { status: 401 });

  await expect(parseApiError(response)).resolves.toBe(AUTH_ERROR.UNAUTHORIZED);
 });

 it('returns AUTH_RATE_LIMITED for 429 without explicit payload', async () => {
  const response = new Response('', { status: 429 });

  await expect(parseApiError(response)).resolves.toBe(AUTH_ERROR.RATE_LIMITED);
 });
});
