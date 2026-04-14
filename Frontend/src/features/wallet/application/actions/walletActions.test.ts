import { beforeEach, describe, expect, it, vi } from 'vitest';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getWalletBalance } from '@/features/wallet/application/actions/balance';
import { createDepositOrder } from '@/features/wallet/application/actions/deposit/user-orders';

vi.mock('@/shared/infrastructure/auth/serverAuth', () => ({
 getServerAccessToken: vi.fn(),
}));

vi.mock('@/shared/infrastructure/http/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

vi.mock('@/shared/infrastructure/logging/logger', () => ({
 logger: {
  error: vi.fn(),
 },
}));

const mockedGetServerAccessToken = vi.mocked(getServerAccessToken);
const mockedServerHttpRequest = vi.mocked(serverHttpRequest);
const mockedLoggerError = vi.mocked(logger.error);

describe('wallet actions', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('returns unauthorized when getWalletBalance has no token', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await getWalletBalance();

  expect(result).toEqual({ success: false, error: AUTH_ERROR.UNAUTHORIZED });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns wallet balance when request succeeds', async () => {
  mockedGetServerAccessToken.mockResolvedValue('wallet-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {
    goldBalance: 2500,
    diamondBalance: 42,
    frozenDiamondBalance: 5,
   },
  });

  const result = await getWalletBalance();

  expect(result).toEqual({
   success: true,
   data: {
    goldBalance: 2500,
    diamondBalance: 42,
    frozenDiamondBalance: 5,
   },
  });
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/Wallet/balance', {
   method: 'GET',
   token: 'wallet-token',
   fallbackErrorMessage: 'Failed to get wallet balance',
  });
 });

 it('passes payload and returns success for createDepositOrder', async () => {
  mockedGetServerAccessToken.mockResolvedValue('deposit-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 201,
   headers: new Headers(),
   data: {
    orderId: 'order-001',
    paymentUrl: 'https://pay.example/order-001',
    amountVnd: 100000,
    diamondAmount: 100,
   },
  });

  const result = await createDepositOrder(100000);

  expect(result.success).toBe(true);
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/deposits/orders', {
   method: 'POST',
   token: 'deposit-token',
   json: { amountVnd: 100000 },
   fallbackErrorMessage: 'Failed to create deposit order',
  });
 });

 it('logs and returns failure when createDepositOrder request fails', async () => {
  mockedGetServerAccessToken.mockResolvedValue('deposit-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 500,
   headers: new Headers(),
   error: 'Create order failed',
  });

  const result = await createDepositOrder(200000);

  expect(result).toEqual({ success: false, error: 'Create order failed' });
  expect(mockedLoggerError).toHaveBeenCalledWith(
   'DepositAction.createDepositOrder',
   'Create order failed',
   { status: 500, amountVnd: 200000 },
  );
 });
});
