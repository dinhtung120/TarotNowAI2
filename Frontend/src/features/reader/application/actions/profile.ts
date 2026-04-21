'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from '@/shared/domain/authErrors';

export interface UpdateReaderProfilePayload {
 bioVi?: string;
 bioEn?: string;
 bioZh?: string;
 diamondPerQuestion?: number;
 specialties?: string[];
 yearsOfExperience?: number;
 facebookUrl?: string;
 instagramUrl?: string;
 tikTokUrl?: string;
}

export async function updateReaderProfile(data: UpdateReaderProfilePayload): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) {
  return actionFail(AUTH_ERROR.UNAUTHORIZED);
 }

 try {
  const result = await serverHttpRequest<unknown>('/reader/profile', {
   method: 'PATCH',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to update reader profile',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] updateReaderProfile', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to update reader profile');
  }

  return actionOk();
 } catch (error) {
  logger.error('[ReaderAction] updateReaderProfile', error);
  return actionFail('Failed to update reader profile');
 }
}

export async function updateReaderStatus(status: string): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) {
  return actionFail(AUTH_ERROR.UNAUTHORIZED);
 }

 try {
  const result = await serverHttpRequest<unknown>('/reader/status', {
   method: 'PATCH',
   token: accessToken,
   json: { status },
   fallbackErrorMessage: 'Failed to update reader status',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] updateReaderStatus', result.error, {
    status: result.status,
    readerStatus: status,
   });
   return actionFail(result.error || 'Failed to update reader status');
  }

  return actionOk();
 } catch (error) {
  logger.error('[ReaderAction] updateReaderStatus', error, { readerStatus: status });
  return actionFail('Failed to update reader status');
 }
}
