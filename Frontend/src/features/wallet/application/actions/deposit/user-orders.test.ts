import { beforeEach, describe, expect, it, vi } from 'vitest';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import {
 createDepositOrder,
 getMyDepositOrder,
 listDepositPackages,
 listMyDepositOrders,
 reconcileDepositOrder,
} from '@/features/wallet/application/actions/deposit/user-orders';

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

describe('deposit user-orders actions', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('returns package list on successful listDepositPackages', async () => {
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: [{ code: 'pkg-1', amountVnd: 100_000 }],
  });

  const result = await listDepositPackages();

  expect(result.success).toBe(true);
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/deposits/packages', {
   method: 'GET',
   fallbackErrorMessage: 'Failed to list deposit packages',
  });
 });

 it('returns failure and logs when listDepositPackages upstream fails', async () => {
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 502,
   headers: new Headers(),
   error: 'upstream failure',
  });

  const result = await listDepositPackages();

  expect(result).toEqual({ success: false, error: 'upstream failure' });
  expect(mockedLoggerError).toHaveBeenCalledWith('DepositAction.listDepositPackages', 'upstream failure', {
   status: 502,
  });
 });

 it('normalizes invalid paging arguments in listMyDepositOrders', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 1 },
  });

  await listMyDepositOrders(-5, 0, 'pending');

  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/deposits/orders?page=1&pageSize=10&status=pending', {
   method: 'GET',
   token: 'token',
   fallbackErrorMessage: 'Failed to list deposit orders',
  });
 });

 it('returns unauthorized when calling createDepositOrder without access token', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await createDepositOrder('pkg-1', 'idem-1');

  expect(result).toEqual({ success: false, error: AUTH_ERROR.UNAUTHORIZED });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns created deposit order on success path', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: { orderId: 'order-1', checkoutUrl: 'https://checkout.example' },
  });

  const result = await createDepositOrder('pkg-1', 'idem-1');

  expect(result.success).toBe(true);
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/deposits/orders', {
   method: 'POST',
   token: 'token',
   json: { packageCode: 'pkg-1', idempotencyKey: 'idem-1' },
   fallbackErrorMessage: 'Failed to create deposit order',
  });
 });

 it('returns failure and logs when createDepositOrder returns non-ok response', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 409,
   headers: new Headers(),
   error: 'duplicate idempotency key',
  });

  const result = await createDepositOrder('pkg-1', 'idem-1');

  expect(result).toEqual({ success: false, error: 'duplicate idempotency key' });
  expect(mockedLoggerError).toHaveBeenCalledWith('DepositAction.createDepositOrder', 'duplicate idempotency key', {
   status: 409,
   packageCode: 'pkg-1',
  });
 });

 it('returns unauthorized when listing deposit orders without token', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await listMyDepositOrders(1, 10, null);

  expect(result).toEqual({ success: false, error: AUTH_ERROR.UNAUTHORIZED });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns unauthorized when getting deposit order without token', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await getMyDepositOrder('order-1');

  expect(result).toEqual({ success: false, error: AUTH_ERROR.UNAUTHORIZED });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns failure and logs when getMyDepositOrder returns non-ok response', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 404,
   headers: new Headers(),
   error: 'not found',
  });

  const result = await getMyDepositOrder('order-404');

  expect(result).toEqual({ success: false, error: 'not found' });
  expect(mockedLoggerError).toHaveBeenCalledWith('DepositAction.getMyDepositOrder', 'not found', {
   status: 404,
   orderId: 'order-404',
  });
 });

 it('returns order payload when getMyDepositOrder succeeds', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: { orderId: 'order-1', status: 'pending' },
  });

  const result = await getMyDepositOrder('order-1');

  expect(result.success).toBe(true);
 });

 it('logs and returns failure when reconcileDepositOrder request fails', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 500,
   headers: new Headers(),
   error: 'reconcile failed',
  });

  const result = await reconcileDepositOrder('order-1');

  expect(result).toEqual({ success: false, error: 'reconcile failed' });
  expect(mockedLoggerError).toHaveBeenCalledWith('DepositAction.reconcileDepositOrder', 'reconcile failed', {
   status: 500,
   orderId: 'order-1',
  });
 });

 it('returns false when reconcile succeeds without handled flag', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {},
  });

  const result = await reconcileDepositOrder('order-2');

  expect(result).toEqual({ success: true, data: false });
 });

 it('returns failure when reconcileDepositOrder throws unexpectedly', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockRejectedValue(new Error('timeout'));

  const result = await reconcileDepositOrder('order-3');

  expect(result).toEqual({ success: false, error: 'Failed to reconcile deposit order' });
  expect(mockedLoggerError).toHaveBeenCalledWith('DepositAction.reconcileDepositOrder', expect.any(Error), {
   orderId: 'order-3',
  });
 });

 it('returns failure and logs when listMyDepositOrders response is non-ok', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 500,
   headers: new Headers(),
   error: 'cannot list',
  });

  const result = await listMyDepositOrders(1, 10, 'failed');

  expect(result).toEqual({ success: false, error: 'cannot list' });
  expect(mockedLoggerError).toHaveBeenCalledWith('DepositAction.listMyDepositOrders', 'cannot list', {
   status: 500,
   page: 1,
   pageSize: 10,
   filterStatus: 'failed',
  });
 });

 it('returns failure when listMyDepositOrders throws unexpectedly', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token');
  mockedServerHttpRequest.mockRejectedValue(new Error('network panic'));

  const result = await listMyDepositOrders(1, 10, 'success');

  expect(result).toEqual({ success: false, error: 'Failed to list deposit orders' });
  expect(mockedLoggerError).toHaveBeenCalledWith('DepositAction.listMyDepositOrders', expect.any(Error), {
   page: 1,
   pageSize: 10,
   filterStatus: 'success',
  });
 });
});
