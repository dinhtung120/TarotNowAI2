'use server';

import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

export interface ReaderProfile {
 id: string;
 userId: string;
 status: string;
 diamondPerQuestion: number;
 bioVi: string;
 bioEn: string;
 bioZh: string;
 specialties: string[];
 avgRating: number;
 totalReviews: number;
 displayName: string;
 avatarUrl?: string | null;
 createdAt: string;
 updatedAt?: string | null;
}

export interface ListReadersResponse {
 readers: ReaderProfile[];
 totalCount: number;
}

export async function listReaders(
 page = 1,
 pageSize = 12,
 specialty = '',
 status = '',
 searchTerm = ''
): Promise<ActionResult<ListReadersResponse>> {
 try {
  const params = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (specialty) params.append('specialty', specialty);
  if (status) params.append('status', status);
  if (searchTerm) params.append('searchTerm', searchTerm);

  const result = await serverHttpRequest<{
   readers?: ReaderProfile[];
   Readers?: ReaderProfile[];
   totalCount?: number;
   TotalCount?: number;
  }>(`/readers?${params.toString()}`, {
   method: 'GET',
   fallbackErrorMessage: 'Failed to list readers',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] listReaders', result.error, {
    status: result.status,
    page,
    pageSize,
    specialty,
    statusFilter: status,
    searchTerm,
   });
   return actionFail(result.error || 'Failed to list readers');
  }

  const data = result.data;
  return actionOk({
   readers: data.readers || data.Readers || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  });
 } catch (error) {
  logger.error('[ReaderAction] listReaders', error, {
   page,
   pageSize,
   specialty,
   statusFilter: status,
   searchTerm,
  });
  return actionFail('Failed to list readers');
 }
}

export async function listFeaturedReaders(limit = 4): Promise<ActionResult<ReaderProfile[]>> {
 try {
  const result = await serverHttpRequest<{ readers?: ReaderProfile[]; Readers?: ReaderProfile[] }>(
   `/readers?page=1&pageSize=${limit}`,
   {
    method: 'GET',
    next: { revalidate: 120 },
    fallbackErrorMessage: 'Failed to list featured readers',
   }
  );

  if (!result.ok) {
   logger.error('[ReaderAction] listFeaturedReaders', result.error, {
    status: result.status,
    limit,
   });
   return actionFail(result.error || 'Failed to list featured readers');
  }

  const data = result.data;
  return actionOk(data.readers || data.Readers || []);
 } catch (error) {
  logger.error('[ReaderAction] listFeaturedReaders', error, { limit });
  return actionFail('Failed to list featured readers');
 }
}

export async function getReaderProfile(userId: string): Promise<ActionResult<ReaderProfile>> {
 try {
  const result = await serverHttpRequest<ReaderProfile>(`/reader/profile/${userId}`, {
   method: 'GET',
   fallbackErrorMessage: 'Failed to get reader profile',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] getReaderProfile', result.error, {
    status: result.status,
    userId,
   });
   return actionFail(result.error || 'Failed to get reader profile');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ReaderAction] getReaderProfile', error, { userId });
  return actionFail('Failed to get reader profile');
 }
}
