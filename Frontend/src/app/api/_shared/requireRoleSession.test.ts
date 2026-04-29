// @vitest-environment node

import { describe, expect, it, vi, beforeEach } from 'vitest';
import { NextResponse } from 'next/server';
import { getServerAccessToken, getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';
import { requireRoleSession } from '@/app/api/_shared/requireRoleSession';

vi.mock('@/shared/infrastructure/auth/serverAuth', () => ({
 getServerAccessToken: vi.fn(),
 getServerSessionSnapshot: vi.fn(),
}));

const mockedGetServerAccessToken = vi.mocked(getServerAccessToken);
const mockedGetServerSessionSnapshot = vi.mocked(getServerSessionSnapshot);

describe('requireRoleSession', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('returns 401 when token is missing', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await requireRoleSession('admin');

  expect(result).toBeInstanceOf(NextResponse);
  if (result instanceof NextResponse) {
   expect(result.status).toBe(401);
  }
 });

 it('returns 403 when session user role does not match', async () => {
  mockedGetServerAccessToken.mockResolvedValue('access-token');
  mockedGetServerSessionSnapshot.mockResolvedValue({
   authenticated: true,
   user: {
    id: 'user-1',
    email: 'user@example.com',
    username: 'user',
    displayName: 'User',
    avatarUrl: null,
    level: 1,
    exp: 0,
    role: 'user',
    status: 'Active',
   },
  });

  const result = await requireRoleSession('admin');

  expect(result).toBeInstanceOf(NextResponse);
  if (result instanceof NextResponse) {
   expect(result.status).toBe(403);
  }
 });

 it('returns token and user when role matches', async () => {
  mockedGetServerAccessToken.mockResolvedValue('access-token');
  mockedGetServerSessionSnapshot.mockResolvedValue({
   authenticated: true,
   user: {
    id: 'admin-1',
    email: 'admin@example.com',
    username: 'admin',
    displayName: 'Admin',
    avatarUrl: null,
    level: 99,
    exp: 0,
    role: 'Admin',
    status: 'Active',
   },
  });

  const result = await requireRoleSession('admin');

  expect(result).toEqual({
   token: 'access-token',
   user: expect.objectContaining({
    id: 'admin-1',
   }),
  });
 });
});
