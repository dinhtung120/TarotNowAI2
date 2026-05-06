import { beforeEach, describe, expect, it, vi } from 'vitest';
import { EVENT_CONTRACTS } from '@/shared/models/eventContracts';
import { logger } from '@/shared/gateways/logger';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { invokeDomainCommand } from '@/shared/gateways/domainCommandRegistry';

vi.mock('@/shared/gateways/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

vi.mock('@/shared/gateways/logger', () => ({
 logger: {
  error: vi.fn(),
 },
}));

const mockedServerHttpRequest = vi.mocked(serverHttpRequest);
const mockedLoggerError = vi.mocked(logger.error);

describe('domain command registry', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('injects expected event contract for command invocation', async () => {
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: { success: true },
  });

 await invokeDomainCommand('chat.dispute.resolve', {
  path: '/admin/disputes/dispute-1/resolve',
  token: 'access-token',
   json: { action: 'release' },
   fallbackErrorMessage: 'Failed to resolve dispute',
  });

 expect(mockedServerHttpRequest).toHaveBeenCalledWith('/admin/disputes/dispute-1/resolve', {
   method: 'POST',
   token: 'access-token',
   expectedDomainEvents: EVENT_CONTRACTS.chatDisputeResolve,
   json: { action: 'release' },
   fallbackErrorMessage: 'Failed to resolve dispute',
  });
 });

 it('fails fast when invocation does not match command path contract', async () => {
  const result = await invokeDomainCommand('reading.session.init', {
   path: '/reading/init/session',
   token: 'access-token',
   json: { spreadType: 'daily_1' },
   fallbackErrorMessage: 'Failed to initialize reading session',
  });

  expect(result).toEqual({
   ok: false,
   status: 500,
   headers: expect.any(Headers),
   error: 'domain-command.path-mismatch',
  });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
  expect(mockedLoggerError).toHaveBeenCalledWith(
   '[DomainCommand]',
   'domain-command.missing-contract',
   expect.objectContaining({
    key: 'reading.session.init',
   }),
  );
 });

 it('fails fast when invocation method does not match command contract', async () => {
  const result = await invokeDomainCommand('chat.dispute.resolve', {
   path: '/admin/disputes/dispute-2/resolve',
   method: 'PUT' as never,
   token: 'access-token',
   json: { action: 'refund' },
   fallbackErrorMessage: 'Failed to resolve dispute',
  });

  expect(result).toEqual({
   ok: false,
   status: 500,
   headers: expect.any(Headers),
   error: 'domain-command.method-mismatch',
  });
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
  expect(mockedLoggerError).toHaveBeenCalledWith(
   '[DomainCommand]',
   'domain-command.missing-contract',
   expect.objectContaining({
    key: 'chat.dispute.resolve',
    expectedMethod: 'POST',
    receivedMethod: 'PUT',
   }),
  );
 });
});

 it('registers expected domain events for admin.reader-request.process command', async () => {
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: { success: true },
  });

  await invokeDomainCommand('admin.reader-request.process', {
   path: '/admin/reader-requests/process',
   method: 'PATCH',
   token: 'access-token',
   json: { requestId: 'request-1', action: 'approve' },
   fallbackErrorMessage: 'Failed to process reader request',
  });

  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/admin/reader-requests/process', {
   method: 'PATCH',
   token: 'access-token',
   expectedDomainEvents: EVENT_CONTRACTS.adminReaderRequestProcess,
   json: { requestId: 'request-1', action: 'approve' },
   fallbackErrorMessage: 'Failed to process reader request',
  });
 });
