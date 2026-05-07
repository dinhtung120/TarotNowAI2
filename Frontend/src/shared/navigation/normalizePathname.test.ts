import { describe, expect, it, vi } from 'vitest';

vi.mock('@/i18n/routing', () => ({
  routing: {
    locales: ['vi', 'en', 'zh'],
  },
}));

import { isAuthlessPath, shouldEnableRealtimeForPath } from './normalizePathname';

describe('authless route realtime gating', () => {
  it.each([
    '/login',
    '/vi/login',
    '/en/login',
    '/zh/login',
    '/register',
    '/vi/register',
    '/forgot-password',
    '/vi/forgot-password',
    '/reset-password',
    '/vi/reset-password',
    '/verify-email',
    '/vi/verify-email',
  ])('treats %s as authless', (pathname) => {
    expect(isAuthlessPath(pathname)).toBe(true);
    expect(shouldEnableRealtimeForPath(pathname)).toBe(false);
  });

  it.each([
    '/',
    '/vi',
    '/collection',
    '/vi/collection',
    '/profile',
    '/vi/profile',
    '/chat',
    '/vi/chat',
  ])('allows realtime-capable user routes for %s', (pathname) => {
    expect(shouldEnableRealtimeForPath(pathname)).toBe(true);
  });

  it.each([
    '/admin',
    '/vi/admin',
    '/admin/readings',
    '/vi/admin/readings',
    '/legal/tos',
    '/vi/legal/tos',
  ])('disables realtime for admin/legal route %s', (pathname) => {
    expect(shouldEnableRealtimeForPath(pathname)).toBe(false);
  });
});
