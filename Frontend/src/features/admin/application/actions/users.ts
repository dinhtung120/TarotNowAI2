'use server';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

export interface AdminUserItem { id: string; email: string; username: string; displayName: string; status: string; role: string; level: number; exp: number; goldBalance: number; diamondBalance: number; createdAt: string }
interface ListUsersResponse { users: AdminUserItem[]; totalCount: number }
export interface UpdateUserParams { role: string; status: string; diamondBalance: number; goldBalance: number }
export interface CreateUserParams { email: string; username: string; displayName: string; password: string; role: string }

const UNAUTHORIZED = 'Unauthorized';
const FAIL_LIST_USERS = 'Failed to list users';
const FAIL_UPDATE_USER = 'Failed to update user';
const FAIL_CREATE_USER = 'Failed to create user';

type UsersListApi = { users?: AdminUserItem[]; Users?: AdminUserItem[]; totalCount?: number; TotalCount?: number };

async function withAdminToken<T>(work: (token: string) => Promise<ActionResult<T>>): Promise<ActionResult<T>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(UNAUTHORIZED);
 return work(accessToken);
}

export async function listUsers(page = 1, pageSize = 20, searchTerm = ''): Promise<ActionResult<ListUsersResponse>> {
 return withAdminToken(async (token) => {
  try {
   const query = new URLSearchParams({ page: page.toString(), pageSize: pageSize.toString() });
   if (searchTerm) query.append('searchTerm', searchTerm);
   const result = await serverHttpRequest<UsersListApi>(`/admin/users?${query.toString()}`, { method: 'GET', token, fallbackErrorMessage: FAIL_LIST_USERS });
   if (!result.ok) {
    logger.error('[AdminAction] listUsers', result.error, { status: result.status, page, pageSize, searchTerm });
    return actionFail(result.error || FAIL_LIST_USERS);
   }
   const data = result.data;
   return actionOk({ users: data.users || data.Users || [], totalCount: data.totalCount ?? data.TotalCount ?? 0 });
  } catch (error) {
   logger.error('[AdminAction] listUsers', error, { page, pageSize, searchTerm });
   return actionFail(FAIL_LIST_USERS);
  }
 });
}

export async function updateUser(userId: string, data: UpdateUserParams): Promise<ActionResult<undefined>> {
 return withAdminToken(async (token) => {
  try {
   const result = await serverHttpRequest<unknown>(`/admin/users/${userId}`, { method: 'PUT', token, json: data, fallbackErrorMessage: FAIL_UPDATE_USER });
   if (!result.ok) {
    logger.error('[AdminAction] updateUser', result.error, { status: result.status, userId });
    return actionFail(result.error || FAIL_UPDATE_USER);
   }
   return actionOk();
  } catch (error) {
   logger.error('[AdminAction] updateUser', error, { userId });
   return actionFail(FAIL_UPDATE_USER);
  }
 });
}

export async function createUser(data: CreateUserParams): Promise<ActionResult<{ userId: string }>> {
 return withAdminToken(async (token) => {
  try {
   const result = await serverHttpRequest<{ userId?: string; UserId?: string }>('/admin/users', { method: 'POST', token, json: data, fallbackErrorMessage: FAIL_CREATE_USER });
   if (!result.ok) {
    logger.error('[AdminAction] createUser', result.error, { status: result.status, email: data.email, username: data.username, role: data.role });
    return actionFail(result.error || FAIL_CREATE_USER);
   }
   const userId = result.data.userId ?? result.data.UserId;
   if (!userId) {
    logger.error('[AdminAction] createUser', 'Missing user id in response', { email: data.email, username: data.username, role: data.role });
    return actionFail(FAIL_CREATE_USER);
   }
   return actionOk({ userId });
  } catch (error) {
   logger.error('[AdminAction] createUser', error, { email: data.email, username: data.username, role: data.role });
   return actionFail(FAIL_CREATE_USER);
  }
 });
}
