'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export interface AdminReaderRequest {
 id: string;
 userId: string;
 status: string;
 introText: string;
 proofDocuments: string[];
 adminNote?: string;
 reviewedBy?: string;
 reviewedAt?: string;
 createdAt: string;
}

export interface ListReaderRequestsResponse {
 requests: AdminReaderRequest[];
 totalCount: number;
}

export async function listReaderRequests(
 page = 1,
 pageSize = 20,
 statusFilter = ''
): Promise<ListReaderRequestsResponse | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

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
   return null;
  }

  const data = result.data;
  return {
   requests: data.requests || data.Requests || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  };
 } catch (error) {
  logger.error('[AdminAction] listReaderRequests', error, { page, pageSize, statusFilter });
  return null;
 }
}

export async function processReaderRequest(
 requestId: string,
 action: 'approve' | 'reject',
 adminNote?: string
): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

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
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[AdminAction] processReaderRequest', error, { requestId, action });
  return false;
 }
}
