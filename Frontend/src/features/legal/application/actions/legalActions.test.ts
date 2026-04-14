import { beforeEach, describe, expect, it, vi } from 'vitest';
import { recordConsent } from '@/features/legal/application/actions/record-consent';
import { checkConsentStatus } from '@/features/legal/application/actions/consent-status';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

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

describe('legal actions', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('returns unauthorized when recordConsent has no access token', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await recordConsent('tos', 'v1');

  expect(result).toEqual({ success: false, error: AUTH_ERROR.UNAUTHORIZED });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('records consent when token exists', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token-123');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: undefined,
  });

  const result = await recordConsent('privacy', '2026-03');

  expect(result).toEqual({ success: true, data: undefined });
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/legal/consent', {
   method: 'POST',
   token: 'token-123',
   json: { documentType: 'privacy', version: '2026-03' },
   fallbackErrorMessage: 'Failed to record consent',
  });
 });

 it('builds query params when checking consent status', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token-abc');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: [{ documentType: 'tos', version: 'v2', acceptedAt: '2026-03-25T00:00:00Z' }],
  });

  const result = await checkConsentStatus('tos', 'v2');

  expect(result.success).toBe(true);
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/legal/consent-status?documentType=tos&version=v2', {
   method: 'GET',
   token: 'token-abc',
   fallbackErrorMessage: 'Failed to check consent status',
  });
 });

 it('returns actionFail and logs when consent status request fails', async () => {
  mockedGetServerAccessToken.mockResolvedValue('token-xyz');
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 500,
   headers: new Headers(),
   error: 'Internal error',
  });

  const result = await checkConsentStatus();

  expect(result).toEqual({ success: false, error: 'Internal error' });
  expect(mockedLoggerError).toHaveBeenCalledWith(
   'LegalAction.checkConsentStatus',
   'Internal error',
   {
    status: 500,
    documentType: undefined,
    version: undefined,
   },
  );
 });
});
