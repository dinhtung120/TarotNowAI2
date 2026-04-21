'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from '@/shared/domain/authErrors';

export interface AdminReaderRequest {
 id: string;
 userId: string;
 status: string;
 bio: string;
 specialties: string[];
 yearsOfExperience: number;
 facebookUrl?: string | null;
 instagramUrl?: string | null;
 tikTokUrl?: string | null;
 diamondPerQuestion: number;
 proofDocuments: string[];
 adminNote?: string;
 reviewedBy?: string;
 reviewedAt?: string;
 createdAt: string;
}

interface ListReaderRequestsResponse {
 requests: AdminReaderRequest[];
 totalCount: number;
}

export async function listReaderRequests(
 page = 1,
 pageSize = 20,
 statusFilter = '',
): Promise<ActionResult<ListReaderRequestsResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (statusFilter) query.append('statusFilter', statusFilter);

  const result = await serverHttpRequest<{
   requests?: AdminReaderRequest[];
   Requests?: AdminReaderRequest[];
   totalCount?: number;
   TotalCount?: number;
  }>(`/admin/reader-requests?${query.toString()}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list reader requests',
  });

  if (!result.ok) {
   logger.error('[AdminAction] listReaderRequests', result.error, {
    status: result.status,
    page,
    pageSize,
    statusFilter,
   });
   return actionFail(result.error || 'Failed to list reader requests');
  }

  const data = result.data;
  return actionOk({
   requests: data.requests || data.Requests || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  });
 } catch (error) {
  logger.error('[AdminAction] listReaderRequests', error, { page, pageSize, statusFilter });
  return actionFail('Failed to list reader requests');
 }
}

export async function processReaderRequest(
 requestId: string,
 action: 'approve' | 'reject',
 adminNote?: string,
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<unknown>('/admin/reader-requests/process', {
   method: 'PATCH',
   token: accessToken,
   json: {
    requestId,
    RequestId: requestId,
    action,
    Action: action,
    adminNote: adminNote || '',
    AdminNote: adminNote || '',
   },
   fallbackErrorMessage: 'Failed to process reader request',
  });

  if (!result.ok) {
   logger.error('[AdminAction] processReaderRequest', result.error, {
    status: result.status,
    requestId,
    action,
   });
   return actionFail(result.error || 'Failed to process reader request');
  }

  return actionOk();
 } catch (error) {
  logger.error('[AdminAction] processReaderRequest', error, { requestId, action });
  return actionFail('Failed to process reader request');
 }
}
