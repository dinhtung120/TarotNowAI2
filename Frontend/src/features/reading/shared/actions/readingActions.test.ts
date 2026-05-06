import { beforeEach, describe, expect, it, vi } from 'vitest';
import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { initReadingSession } from '@/features/reading/shared/actions/init-session';
import { revealReadingSession } from '@/features/reading/shared/actions/reveal-session';
import { EVENT_CONTRACTS } from '@/shared/domain/eventContracts';

vi.mock('next-intl/server', () => ({
 getTranslations: vi.fn(),
}));

vi.mock('@/shared/application/gateways/serverAuth', () => ({
 getServerAccessToken: vi.fn(),
}));

vi.mock('@/shared/application/gateways/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

vi.mock('@/shared/application/gateways/logger', () => ({
 logger: {
  error: vi.fn(),
 },
}));

const mockedGetTranslations = vi.mocked(getTranslations);
const mockedGetServerAccessToken = vi.mocked(getServerAccessToken);
const mockedServerHttpRequest = vi.mocked(serverHttpRequest);
const mockedLoggerError = vi.mocked(logger.error);

describe('reading actions', () => {
 beforeEach(() => {
  vi.clearAllMocks();
  mockedGetTranslations.mockResolvedValue(
   ((key: string) => `t:${key}`) as unknown as Awaited<ReturnType<typeof getTranslations>>,
  );
 });

 it('returns unauthorized when initReadingSession has no token', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await initReadingSession({ spreadType: 'single', question: 'Q1' });

  expect(result).toEqual({ success: false, error: 't:unauthorized' });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns success data when initReadingSession succeeds', async () => {
  mockedGetServerAccessToken.mockResolvedValue('access-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {
    sessionId: 'session-123',
    costGold: 10,
    costDiamond: 0,
   },
  });

  const result = await initReadingSession({
   spreadType: 'three-card',
   question: 'What should I do?',
   currency: 'gold',
  });

  expect(result).toEqual({
   success: true,
   data: {
    sessionId: 'session-123',
    costGold: 10,
    costDiamond: 0,
   },
  });
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/reading/init', {
   method: 'POST',
   token: 'access-token',
   expectedDomainEvents: EVENT_CONTRACTS.readingInit,
   json: {
    spreadType: 'three-card',
    question: 'What should I do?',
    currency: 'gold',
   },
   fallbackErrorMessage: 't:unknown_error',
  });
 });

 it('sends expected domain-event contract header metadata for reveal', async () => {
  mockedGetServerAccessToken.mockResolvedValue('access-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: { sessionId: 'session-200', interpretation: 'ok' },
  });

  await revealReadingSession({ sessionId: 'session-200' });

  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/reading/reveal', expect.objectContaining({
   method: 'POST',
   token: 'access-token',
   expectedDomainEvents: EVENT_CONTRACTS.readingReveal,
  }));
 });

 it('maps 401 to unauthorized in revealReadingSession', async () => {
  mockedGetServerAccessToken.mockResolvedValue('access-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 401,
   headers: new Headers(),
   error: 'Token expired',
  });

  const result = await revealReadingSession({ sessionId: 'session-401' });

  expect(result).toEqual({ success: false, error: 't:unauthorized' });
  expect(mockedLoggerError).not.toHaveBeenCalled();
 });

 it('logs and returns network error when revealReadingSession throws', async () => {
  mockedGetServerAccessToken.mockResolvedValue('access-token');
  mockedServerHttpRequest.mockRejectedValue(new Error('network down'));

  const result = await revealReadingSession({ sessionId: 'session-network' });

  expect(result).toEqual({ success: false, error: 't:network_error' });
  expect(mockedLoggerError).toHaveBeenCalledWith(
   'ReadingAction.revealReadingSession',
   expect.any(Error),
  );
 });
});
