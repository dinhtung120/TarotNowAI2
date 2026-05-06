import { beforeEach, describe, expect, it, vi } from 'vitest';
import {
 listReaderRequests,
 processReaderRequest,
} from '@/features/admin/reader-requests/actions/reader-requests';
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

describe('admin reader requests actions', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('rejects processReaderRequest when admin token is missing', async () => {
  mockedGetServerAccessToken.mockResolvedValue(undefined);

  const result = await processReaderRequest('request-1', 'approve');

  expect(result).toEqual({
   success: false,
   error: AUTH_ERROR.UNAUTHORIZED,
  });
  expect(mockedCreateIdempotentInvoker).not.toHaveBeenCalled();
 });

 it('routes processReaderRequest through admin.reader-request.process command', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedCreateIdempotentInvoker.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: undefined,
  });

  const result = await processReaderRequest('request-2', 'reject', 'policy mismatch');

  expect(result.success).toBe(true);
  expect(mockedCreateIdempotentInvoker).toHaveBeenCalledWith('admin.reader-request.process', expect.objectContaining({
   path: '/admin/reader-requests/process',
   method: 'PATCH',
   token: 'admin-token',
   payload: expect.objectContaining({
    requestId: 'request-2',
    action: 'reject',
    adminNote: 'policy mismatch',
   }),
  }));
  expect(mockedServerHttpRequest).not.toHaveBeenCalledWith('/admin/reader-requests/process', expect.anything());
 });

 it('normalizes reader request list payload', async () => {
  mockedGetServerAccessToken.mockResolvedValue('admin-token');
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {
    Requests: [{
     Id: 'request-3',
     UserId: 'user-3',
     Status: 'pending',
     Bio: 'bio',
     Specialties: ['love'],
     YearsOfExperience: 5,
     DiamondPerQuestion: 30,
     ProofDocuments: [],
     CreatedAt: '2026-01-01T00:00:00Z',
    }],
    TotalCount: 1,
   },
  } as never);

  const result = await listReaderRequests(1, 10, 'pending');

  expect(result).toEqual({
   success: true,
   data: {
    requests: expect.arrayContaining([
     expect.objectContaining({
      id: 'request-3',
      userId: 'user-3',
      status: 'pending',
     }),
    ]),
    totalCount: 1,
   },
  });
 });
});
