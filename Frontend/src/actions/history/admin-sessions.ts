'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { AdminHistoryPaginatedResponse, AdminHistorySessionItem } from './types';

const getAccessToken = getServerAccessToken;

export async function getAllHistorySessionsAdminAction(params: {
 page: number;
 pageSize: number;
 username?: string;
 spreadType?: string;
 startDate?: string;
 endDate?: string;
}) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getAccessToken();
  if (!token) {
   return { error: 'unauthorized' };
  }

  let query = `page=${params.page}&pageSize=${params.pageSize}`;
  if (params.username) query += `&username=${encodeURIComponent(params.username)}`;
  if (params.spreadType) query += `&spreadType=${encodeURIComponent(params.spreadType)}`;
  if (params.startDate) query += `&startDate=${encodeURIComponent(params.startDate)}`;
  if (params.endDate) query += `&endDate=${encodeURIComponent(params.endDate)}`;

  const result = await serverHttpRequest<Record<string, unknown>>(`/History/admin/all-sessions?${query}`, {
   method: 'GET',
   token,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) return { error: 'unauthorized' };
   if (result.status === 403) return { error: tApi('forbidden') };
   logger.error('HistoryAction.getAllHistorySessionsAdminAction', result.error, {
    status: result.status,
    params,
   });
   return { error: result.error || tApi('unknown_error') };
  }

  const data = result.data as Record<string, unknown>;
  const safeData: AdminHistoryPaginatedResponse = {
   ...data,
   items:
    (data.items as AdminHistorySessionItem[] | undefined) ||
    (data.Items as AdminHistorySessionItem[] | undefined) ||
    [],
   totalCount: (data.totalCount as number | undefined) ?? (data.TotalCount as number | undefined) ?? 0,
   totalPages: (data.totalPages as number | undefined) ?? (data.TotalPages as number | undefined) ?? 0,
   page: (data.page as number | undefined) ?? (data.Page as number | undefined) ?? params.page,
   pageSize: (data.pageSize as number | undefined) ?? (data.PageSize as number | undefined) ?? params.pageSize,
  };

  return { success: true, data: safeData };
 } catch (error) {
  logger.error('HistoryAction.getAllHistorySessionsAdminAction', error, { params });
  return { error: tApi('network_error') };
 }
}
