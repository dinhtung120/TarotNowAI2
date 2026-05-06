'use server';

import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { createIdempotentDomainCommandInvoker } from '@/features/admin/shared/gateways/idempotentDomainCommandInvoker';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { AUTH_ERROR } from '@/shared/models/authErrors';

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
 reviewHistory: AdminReaderRequestReviewHistoryEntry[];
}

export interface AdminReaderRequestReviewHistoryEntry {
 action: string;
 status: string;
 reviewedBy?: string;
 adminNote?: string;
 reviewedAt: string;
}

interface AdminReaderRequestReviewHistoryEntryRaw {
 action?: string;
 Action?: string;
 status?: string;
 Status?: string;
 reviewedBy?: string;
 ReviewedBy?: string;
 adminNote?: string;
 AdminNote?: string;
 reviewedAt?: string;
 ReviewedAt?: string;
}

interface AdminReaderRequestRaw {
 id?: string;
 Id?: string;
 userId?: string;
 UserId?: string;
 status?: string;
 Status?: string;
 bio?: string;
 Bio?: string;
 specialties?: string[];
 Specialties?: string[];
 yearsOfExperience?: number;
 YearsOfExperience?: number;
 facebookUrl?: string | null;
 FacebookUrl?: string | null;
 instagramUrl?: string | null;
 InstagramUrl?: string | null;
 tikTokUrl?: string | null;
 TikTokUrl?: string | null;
 diamondPerQuestion?: number;
 DiamondPerQuestion?: number;
 proofDocuments?: string[];
 ProofDocuments?: string[];
 adminNote?: string;
 AdminNote?: string;
 reviewedBy?: string;
 ReviewedBy?: string;
 reviewedAt?: string;
 ReviewedAt?: string;
 createdAt?: string;
 CreatedAt?: string;
 reviewHistory?: AdminReaderRequestReviewHistoryEntryRaw[];
 ReviewHistory?: AdminReaderRequestReviewHistoryEntryRaw[];
}

interface ListReaderRequestsResponse {
 requests: AdminReaderRequest[];
 totalCount: number;
}

function normalizeReviewHistory(rawEntries: AdminReaderRequestReviewHistoryEntryRaw[] | undefined): AdminReaderRequestReviewHistoryEntry[] {
 if (!rawEntries || rawEntries.length === 0) {
  return [];
 }

 return rawEntries
  .map((entry) => ({
   action: entry.action || entry.Action || '',
   status: entry.status || entry.Status || '',
   reviewedBy: entry.reviewedBy || entry.ReviewedBy || '',
   adminNote: entry.adminNote || entry.AdminNote || '',
   reviewedAt: entry.reviewedAt || entry.ReviewedAt || '',
  }))
  .filter((entry) => entry.reviewedAt.length > 0);
}

function normalizeRequest(raw: AdminReaderRequestRaw): AdminReaderRequest {
 const id = raw.id || raw.Id || '';
 const userId = raw.userId || raw.UserId || '';
 const status = raw.status || raw.Status || '';
 const bio = raw.bio || raw.Bio || '';
 const specialties = raw.specialties || raw.Specialties || [];
 const yearsOfExperience = raw.yearsOfExperience ?? raw.YearsOfExperience ?? 0;
 const facebookUrl = raw.facebookUrl ?? raw.FacebookUrl ?? null;
 const instagramUrl = raw.instagramUrl ?? raw.InstagramUrl ?? null;
 const tikTokUrl = raw.tikTokUrl ?? raw.TikTokUrl ?? null;
 const diamondPerQuestion = raw.diamondPerQuestion ?? raw.DiamondPerQuestion ?? 0;
 const proofDocuments = raw.proofDocuments || raw.ProofDocuments || [];
 const reviewedAt = raw.reviewedAt || raw.ReviewedAt || '';
 const reviewedBy = raw.reviewedBy || raw.ReviewedBy || '';
 const adminNote = raw.adminNote || raw.AdminNote || '';
 const createdAt = raw.createdAt || raw.CreatedAt || '';
 const reviewHistory = normalizeReviewHistory(raw.reviewHistory || raw.ReviewHistory);
 const fallbackHistory = reviewedAt
  ? [{
    action: status === 'approved' ? 'approve' : status === 'rejected' ? 'reject' : status,
    status,
    reviewedBy,
    adminNote,
    reviewedAt,
   }]
  : [];

 return {
  id,
  userId,
  status,
  bio,
  specialties,
  yearsOfExperience,
  facebookUrl,
  instagramUrl,
  tikTokUrl,
  diamondPerQuestion,
  proofDocuments,
  adminNote,
  reviewedBy,
  reviewedAt,
  createdAt,
  reviewHistory: reviewHistory.length > 0 ? reviewHistory : fallbackHistory,
 };
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
   requests?: AdminReaderRequestRaw[];
   Requests?: AdminReaderRequestRaw[];
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
  const requests = (data.requests || data.Requests || []).map(normalizeRequest);
  return actionOk({
   requests,
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
  const result = await createIdempotentDomainCommandInvoker<unknown, Record<string, string>>(
   'admin.reader-request.process',
   {
    path: '/admin/reader-requests/process',
    method: 'PATCH',
    token: accessToken,
    payload: {
     requestId,
     RequestId: requestId,
     action,
     Action: action,
     adminNote: adminNote || '',
     AdminNote: adminNote || '',
    },
    fallbackErrorMessage: 'Failed to process reader request',
   },
  );

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
