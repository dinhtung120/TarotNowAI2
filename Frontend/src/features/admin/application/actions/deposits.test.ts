import { beforeEach, describe, expect, it, vi } from 'vitest';
import { listDeposits, processDeposit } from '@/features/admin/application/actions/deposits';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { createIdempotentDomainCommandInvoker } from '@/shared/application/gateways/idempotentDomainCommandInvoker';

vi.mock('@/shared/application/gateways/serverAuth', () => ({
 getServerAccessToken: vi.fn(),
}));

vi.mock('@/shared/application/gateways/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

vi.mock('@/shared/application/gateways/idempotentDomainCommandInvoker', () => ({
 createIdempotentDomainCommandInvoker: vi.fn(),
}));

vi.mock('@/shared/application/gateways/logger', () => ({
 logger: {
  error: vi.fn(),
 },
}));

const mockedGetServerAccessToken = vi.mocked(getServerAccessToken);
const mockedServerHttpRequest = vi.mocked(serverHttpRequest);
const mockedCreateIdempotentInvoker = vi.mocked(createIdempotentDomainCommandInvoker);

describe('admin deposits actions', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('rejects processDeposit when admin token is missing', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await processDeposit('deposit-1', 'approve');

  expect(result).toEqual({
   success: false,
   error: AUTH_ERROR.UNAUTHORIZED,
  });
  expect(mockedCreateIdempotentInvoker).not.toHaveBeenCalled();
 });

 it('routes processDeposit through wallet.deposit.admin.process domain command', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedCreateIdempotentInvoker.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: undefined,
  });

  const result = await processDeposit('deposit-2', 'reject', 'txn-2');

  expect(result.success).toBe(true);
  expect(mockedCreateIdempotentInvoker).toHaveBeenCalledWith('wallet.deposit.admin.process', expect.objectContaining({
   path: '/admin/deposits/process',
   method: 'PATCH',
   token: 'admin-token',
   payload: expect.objectContaining({
    depositId: 'deposit-2',
    action: 'reject',
    transactionId: 'txn-2',
   }),
  }));
  expect(mockedServerHttpRequest).not.toHaveBeenCalledWith('/admin/deposits/process', expect.anything());
 });

 it('normalizes list response with PascalCase fallback', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {
    Deposits: [{ id: 'deposit-3', userId: 'user-3', amountVnd: 1000, diamondAmount: 10, status: 'pending', createdAt: '2026-01-01T00:00:00Z' }],
    TotalCount: 1,
   },
  } as never);

  const result = await listDeposits(1, 10, 'pending');

  expect(result).toEqual({
   success: true,
   data: {
    deposits: expect.arrayContaining([expect.objectContaining({ id: 'deposit-3' })]),
    totalCount: 1,
   },
  });
 });
});
