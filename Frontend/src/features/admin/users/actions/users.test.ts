import { beforeEach, describe, expect, it, vi } from 'vitest';
import { createUser, listUsers, updateUser } from '@/features/admin/users/actions/users';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { AUTH_HEADER } from '@/shared/application/gateways/authConstants';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { invokeDomainCommand } from '@/shared/application/gateways/domainCommandRegistry';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';

vi.mock('@/shared/application/gateways/serverAuth', () => ({
 getServerAccessToken: vi.fn(),
}));

vi.mock('@/shared/application/gateways/domainCommandRegistry', () => ({
 invokeDomainCommand: vi.fn(),
}));

vi.mock('@/shared/application/gateways/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

vi.mock('@/shared/application/gateways/logger', () => ({
 logger: {
  error: vi.fn(),
 },
}));

const mockedGetServerAccessToken = vi.mocked(getServerAccessToken);
const mockedInvokeDomainCommand = vi.mocked(invokeDomainCommand);
const mockedServerHttpRequest = vi.mocked(serverHttpRequest);

describe('admin users actions', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('fails with unauthorized error when admin token is missing', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await updateUser('user-1', {
   role: 'user',
   status: 'active',
   diamondBalance: 10,
   goldBalance: 20,
  });

  expect(result).toEqual({
   success: false,
   error: AUTH_ERROR.UNAUTHORIZED,
  });
  expect(mockedInvokeDomainCommand).not.toHaveBeenCalled();
 });

 it('always routes user update through admin.user.adjust-balance command contract', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedInvokeDomainCommand.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: undefined,
  });

  const result = await updateUser('user-42', {
   role: 'admin',
   status: 'locked',
   diamondBalance: 0,
   goldBalance: 0,
  });

  expect(result.success).toBe(true);
  expect(mockedInvokeDomainCommand).toHaveBeenCalledTimes(1);
  const [commandKey, options] = mockedInvokeDomainCommand.mock.calls[0];
  expect(commandKey).toBe('admin.user.adjust-balance');
  expect(options).toEqual(expect.objectContaining({
   path: '/admin/users/user-42',
   method: 'PUT',
   token: 'admin-token',
   headers: expect.objectContaining({
    [AUTH_HEADER.IDEMPOTENCY_KEY]: expect.any(String),
   }),
   json: expect.objectContaining({
    role: 'admin',
    status: 'locked',
    diamondBalance: 0,
    goldBalance: 0,
    idempotencyKey: expect.any(String),
   }),
  }));

  expect(options.headers?.[AUTH_HEADER.IDEMPOTENCY_KEY]).toBe(options.json?.idempotencyKey);
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns action failure when command invocation fails', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedInvokeDomainCommand.mockResolvedValue({
   ok: false,
   status: 400,
   headers: new Headers(),
   error: 'Balance update rejected',
  });

  const result = await updateUser('user-2', {
   role: 'user',
   status: 'active',
   diamondBalance: 1,
   goldBalance: 2,
  });

  expect(result).toEqual({
   success: false,
   error: 'Balance update rejected',
  });
 });

 it('lists users from standard response shape', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {
    users: [
     {
      id: 'user-1',
      email: 'user1@example.com',
      username: 'user1',
      displayName: 'User One',
      status: 'active',
      role: 'user',
      level: 3,
      exp: 120,
      goldBalance: 50,
      diamondBalance: 5,
      createdAt: '2026-01-01T00:00:00.000Z',
     },
    ],
    totalCount: 1,
   },
  });

  const result = await listUsers(2, 10, 'user');

  expect(result).toEqual({
   success: true,
   data: {
    users: expect.arrayContaining([
     expect.objectContaining({ id: 'user-1' }),
    ]),
    totalCount: 1,
   },
  });
 });

 it('creates user and supports PascalCase response id', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 201,
   headers: new Headers(),
   data: {
    UserId: 'new-user-id',
   },
  });

  const result = await createUser({
   email: 'new@example.com',
   username: 'new_user',
   displayName: 'New User',
   password: 'Password123!',
   role: 'user',
  });

  expect(result).toEqual({
   success: true,
   data: {
    userId: 'new-user-id',
   },
  });
 });

 it('returns failure when create user response does not contain user id', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 201,
   headers: new Headers(),
   data: {},
  });

  const result = await createUser({
   email: 'missing-id@example.com',
   username: 'missing_id',
   displayName: 'Missing Id',
   password: 'Password123!',
   role: 'admin',
  });

  expect(result).toEqual({
   success: false,
   error: 'Failed to create user',
  });
 });
});
