import { describe, expect, it } from 'vitest';
import { NextResponse } from 'next/server';
import {
 normalizeAuthTokenCookieValue,
 setRefreshCookieFromHeaders,
} from '@/app/api/auth/_shared';

describe('auth cookie helpers', () => {
 it('normalizes double-encoded refresh token values', () => {
  const raw = '%252BfVCUkB%252FE8HKLkbg1TEcOd8%253D%253D';
  const normalized = normalizeAuthTokenCookieValue(raw);

  expect(normalized).toBe('+fVCUkB/E8HKLkbg1TEcOd8==');
 });

 it('sets refresh cookie without introducing double encoding', () => {
  const response = NextResponse.json({ ok: true });
  const upstreamHeaders = new Headers();
  upstreamHeaders.append(
   'set-cookie',
   'refreshToken=%2BfVCUkB%2FE8HKLkbg1TEcOd8%3D%3D; Path=/; HttpOnly; Secure; SameSite=Strict; Max-Age=86400',
  );

  const applied = setRefreshCookieFromHeaders(response, upstreamHeaders);

  expect(applied).toBe(true);
  const downstreamCookie = response.headers.get('set-cookie') ?? '';
  expect(downstreamCookie).toContain('refreshToken=%2BfVCUkB%2FE8HKLkbg1TEcOd8%3D%3D');
  expect(downstreamCookie).not.toContain('%252B');
 });
});
