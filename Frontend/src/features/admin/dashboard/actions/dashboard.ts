'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from '@/shared/domain/authErrors';

export interface AdminDashboardSummary {
 users: number;
 deposits: number;
 promotions: number;
 readings: number;
}

interface UsersSummaryResponse {
 totalCount?: number;
 TotalCount?: number;
}

interface DepositsSummaryResponse {
 totalCount?: number;
 TotalCount?: number;
}

interface ReadingsSummaryResponse {
 totalCount?: number;
 TotalCount?: number;
}

const DASHBOARD_SUMMARY_FALLBACK_ERROR = 'Failed to load admin dashboard summary';

function resolveCount(payload: Record<string, unknown>, lowerCaseKey: string, upperCaseKey: string): number {
 const lower = payload[lowerCaseKey];
 if (typeof lower === 'number' && Number.isFinite(lower)) {
  return lower;
 }

 const upper = payload[upperCaseKey];
 if (typeof upper === 'number' && Number.isFinite(upper)) {
  return upper;
 }

 return 0;
}

function resolvePromotionsCount(payload: unknown): number {
 if (Array.isArray(payload)) {
  return payload.length;
 }

 return 0;
}

export async function getAdminDashboardSummary(): Promise<ActionResult<AdminDashboardSummary>> {
 const token = await getServerAccessToken();
 if (!token) {
  return actionFail(AUTH_ERROR.UNAUTHORIZED);
 }

 try {
  const [usersResult, depositsResult, promotionsResult, readingsResult] = await Promise.all([
   serverHttpRequest<UsersSummaryResponse>('/admin/users?page=1&pageSize=1', {
    method: 'GET',
    token,
    fallbackErrorMessage: DASHBOARD_SUMMARY_FALLBACK_ERROR,
   }),
   serverHttpRequest<DepositsSummaryResponse>('/admin/deposits?page=1&pageSize=1', {
    method: 'GET',
    token,
    fallbackErrorMessage: DASHBOARD_SUMMARY_FALLBACK_ERROR,
   }),
   serverHttpRequest<unknown>('/admin/promotions?onlyActive=false', {
    method: 'GET',
    token,
    fallbackErrorMessage: DASHBOARD_SUMMARY_FALLBACK_ERROR,
   }),
   serverHttpRequest<ReadingsSummaryResponse>('/History/admin/all-sessions?page=1&pageSize=1', {
    method: 'GET',
    token,
    fallbackErrorMessage: DASHBOARD_SUMMARY_FALLBACK_ERROR,
   }),
  ]);

  if (!usersResult.ok || !depositsResult.ok || !promotionsResult.ok || !readingsResult.ok) {
   logger.error('[AdminAction] getAdminDashboardSummary', 'upstream_failure', {
    usersStatus: usersResult.status,
    depositsStatus: depositsResult.status,
    promotionsStatus: promotionsResult.status,
    readingsStatus: readingsResult.status,
   });
   return actionFail(DASHBOARD_SUMMARY_FALLBACK_ERROR);
  }

  const users = resolveCount(usersResult.data as Record<string, unknown>, 'totalCount', 'TotalCount');
  const deposits = resolveCount(depositsResult.data as Record<string, unknown>, 'totalCount', 'TotalCount');
  const readings = resolveCount(readingsResult.data as Record<string, unknown>, 'totalCount', 'TotalCount');
  const promotions = resolvePromotionsCount(promotionsResult.data);

  return actionOk({
   users,
   deposits,
   promotions,
   readings,
  });
 } catch (error) {
  logger.error('[AdminAction] getAdminDashboardSummary', error, {});
  return actionFail(DASHBOARD_SUMMARY_FALLBACK_ERROR);
 }
}
