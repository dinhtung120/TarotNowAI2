import { beforeEach, describe, expect, it, vi } from 'vitest';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import { getWalletBalance } from '@/features/wallet/shared/actions/balance';
import { createDepositOrder } from '@/features/wallet/deposit/actions/user-orders';
import { EVENT_CONTRACTS } from '@/shared/models/eventContracts';

vi.mock('@/shared/gateways/serverAuth', () => ({
 getServerAccessToken: vi.fn(),
}));

vi.mock('@/shared/gateways/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

vi.mock('@/shared/gateways/logger', () => ({
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
    status: 'pending',
    amountVnd: 100000,
    baseDiamondAmount: 1000,
    bonusGoldAmount: 50,
    totalDiamondAmount: 1000,
    payOsOrderCode: 9123001,
    checkoutUrl: 'https://pay.example/order-001',
    qrCode: 'PAYOS_QR_001',
    paymentLinkId: 'plink_001',
   },
  });

  const result = await createDepositOrder('topup_100k', 'idem-key-001');

  expect(result.success).toBe(true);
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/deposits/orders', {
   method: 'POST',
   token: 'deposit-token',
   expectedDomainEvents: EVENT_CONTRACTS.walletDeposit,
   json: { packageCode: 'topup_100k', idempotencyKey: 'idem-key-001' },
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

  const result = await createDepositOrder('topup_200k', 'idem-key-002');

  expect(result).toEqual({ success: false, error: 'Create order failed' });
  expect(mockedLoggerError).toHaveBeenCalledWith(
   'DepositAction.createDepositOrder',
   'Create order failed',
   { status: 500, packageCode: 'topup_200k' },
  );
 });
});
