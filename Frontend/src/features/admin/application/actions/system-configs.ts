'use server';

import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { AdminSystemConfigItem, UpdateSystemConfigParams } from '@/features/admin/system-configs/system-config.types';

const FAIL_LIST_SYSTEM_CONFIGS = 'Failed to list system configs';
const FAIL_UPDATE_SYSTEM_CONFIG = 'Failed to update system config';

async function withAdminToken<T>(work: (token: string) => Promise<ActionResult<T>>): Promise<ActionResult<T>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);
 return work(accessToken);
}

export async function listSystemConfigs(): Promise<ActionResult<AdminSystemConfigItem[]>> {
 return withAdminToken(async (token) => {
  try {
   const result = await serverHttpRequest<AdminSystemConfigItem[]>('/admin/system-configs', {
    method: 'GET',
    token,
    fallbackErrorMessage: FAIL_LIST_SYSTEM_CONFIGS,
   });
   if (!result.ok) {
    logger.error('[AdminAction] listSystemConfigs', result.error, { status: result.status });
    return actionFail(result.error || FAIL_LIST_SYSTEM_CONFIGS);
   }

   return actionOk(result.data ?? []);
  } catch (error) {
   logger.error('[AdminAction] listSystemConfigs', error);
   return actionFail(FAIL_LIST_SYSTEM_CONFIGS);
  }
 });
}

export async function updateSystemConfig(
 key: string,
 payload: UpdateSystemConfigParams
): Promise<ActionResult<AdminSystemConfigItem>> {
 return withAdminToken(async (token) => {
  try {
   const result = await serverHttpRequest<AdminSystemConfigItem>(`/admin/system-configs/${encodeURIComponent(key)}`, {
    method: 'PUT',
    token,
    json: payload,
    fallbackErrorMessage: FAIL_UPDATE_SYSTEM_CONFIG,
   });
   if (!result.ok) {
    logger.error('[AdminAction] updateSystemConfig', result.error, {
     status: result.status,
     key,
    });
    return actionFail(result.error || FAIL_UPDATE_SYSTEM_CONFIG);
   }

   return actionOk(result.data);
  } catch (error) {
   logger.error('[AdminAction] updateSystemConfig', error, { key });
   return actionFail(FAIL_UPDATE_SYSTEM_CONFIG);
  }
 });
}

export async function restartServer(): Promise<ActionResult<void>> {
 return withAdminToken(async (token) => {
  try {
   const result = await serverHttpRequest<void>('/admin/system-configs/restart', {
    method: 'POST',
    token,
    fallbackErrorMessage: 'Failed to restart server',
   });
   
   if (!result.ok) {
    logger.error('[AdminAction] restartServer', result.error, { status: result.status });
    return actionFail(result.error || 'Failed to restart server');
   }

   return actionOk(undefined);
  } catch (error) {
   logger.error('[AdminAction] restartServer', error);
   return actionFail('Failed to restart server');
  }
 });
}
