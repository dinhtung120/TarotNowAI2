'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';


export interface AdminUserItem {
 id: string;
 email: string;
 username: string;
 displayName: string;
 status: string;
 role: string;
 level: number;
 exp: number;
 goldBalance: number;
 diamondBalance: number;
 createdAt: string;
}

export interface ListUsersResponse {
 users: AdminUserItem[];
 totalCount: number;
}

export interface PaginatedResult<T> {
 items: T[];
 totalCount: number;
 page: number;
 pageSize: number;
}

export interface UpdateUserParams {
 role: string;
 status: string;
 diamondBalance: number;
 goldBalance: number;
}

export async function listUsers(
 page = 1,
 pageSize = 20,
 searchTerm = ''
): Promise<ActionResult<ListUsersResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (searchTerm) query.append('searchTerm', searchTerm);

  const result = await serverHttpRequest<{
   users?: AdminUserItem[];
   Users?: AdminUserItem[];
   totalCount?: number;
   TotalCount?: number;
  }>(`/admin/users?${query.toString()}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list users',
  });

  if (!result.ok) {
   logger.error('[AdminAction] listUsers', result.error, {
    status: result.status,
    page,
    pageSize,
    searchTerm,
   });
   return actionFail(result.error || 'Failed to list users');
  }

  const data = result.data;
  return actionOk({
   users: data.users || data.Users || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  });
 } catch (error) {
  logger.error('[AdminAction] listUsers', error, { page, pageSize, searchTerm });
  return actionFail('Failed to list users');
 }
}

export async function toggleUserLock(userId: string, isLocked: boolean): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>('/admin/users/lock', {
   method: 'PATCH',
   token: accessToken,
   json: {
    userId,
    UserId: userId,
    isLocked,
    IsLocked: isLocked,
   },
   fallbackErrorMessage: 'Failed to toggle user lock',
  });

  if (!result.ok) {
   logger.error('[AdminAction] toggleUserLock', result.error, {
    status: result.status,
    userId,
    isLocked,
   });
   return actionFail(result.error || 'Failed to toggle user lock');
  }
  return actionOk();
 } catch (error) {
  logger.error('[AdminAction] toggleUserLock', error, { userId, isLocked });
  return actionFail('Failed to toggle user lock');
 }
}

export async function addUserBalance(
 userId: string,
 currency: string,
 amount: number,
 reason?: string
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>('/admin/users/add-balance', {
   method: 'POST',
   token: accessToken,
   json: {
    userId,
    UserId: userId,
    currency,
    Currency: currency,
    amount,
    Amount: amount,
    reason,
    Reason: reason,
   },
   fallbackErrorMessage: 'Failed to add user balance',
  });

  if (!result.ok) {
   logger.error('[AdminAction] addUserBalance', result.error, {
    status: result.status,
    userId,
    currency,
    amount,
   });
   return actionFail(result.error || 'Failed to add user balance');
  }

  return actionOk();
 } catch (error) {
  logger.error('[AdminAction] addUserBalance', error, { userId, currency, amount });
  return actionFail('Failed to add user balance');
 }
}

export async function updateUser(
 userId: string,
 data: UpdateUserParams
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>(`/admin/users/${userId}`, {
   method: 'PUT',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to update user',
  });

  if (!result.ok) {
   logger.error('[AdminAction] updateUser', result.error, { status: result.status, userId });
   return actionFail(result.error || 'Failed to update user');
  }
  return actionOk();
 } catch (error) {
  logger.error('[AdminAction] updateUser', error, { userId });
  return actionFail('Failed to update user');
 }
}
