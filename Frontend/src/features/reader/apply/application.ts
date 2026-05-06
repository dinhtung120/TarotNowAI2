'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { AUTH_ERROR } from '@/shared/models/authErrors';

export interface ReaderSocialLinks {
 facebookUrl?: string | null;
 instagramUrl?: string | null;
 tikTokUrl?: string | null;
}

export interface SubmitReaderApplicationPayload extends ReaderSocialLinks {
 bio: string;
 specialties: string[];
 yearsOfExperience: number;
 diamondPerQuestion: number;
 proofDocuments?: string[];
}

export interface MyReaderRequest extends ReaderSocialLinks {
 hasRequest: boolean;
 status?: string;
 bio?: string;
 specialties?: string[];
 yearsOfExperience?: number;
 diamondPerQuestion?: number;
 adminNote?: string;
 createdAt?: string;
 reviewedAt?: string;
}

export async function submitReaderApplication(
 payload: SubmitReaderApplicationPayload,
): Promise<ActionResult<{ message: string }>> {
 const t = await getTranslations('ReaderApply');
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) {
  return actionFail(tApi('unauthorized'));
 }

 try {
  const result = await serverHttpRequest<{ message?: string }>('/reader/apply', {
   method: 'POST',
   token: accessToken,
   json: {
    bio: payload.bio,
    specialties: payload.specialties,
    yearsOfExperience: payload.yearsOfExperience,
    facebookUrl: payload.facebookUrl,
    instagramUrl: payload.instagramUrl,
    tikTokUrl: payload.tikTokUrl,
    diamondPerQuestion: payload.diamondPerQuestion,
    proofDocuments: payload.proofDocuments ?? [],
   },
   fallbackErrorMessage: t('errors.submit_failed'),
  });

  if (!result.ok) {
   logger.error('[ReaderAction] submitReaderApplication', result.error, { status: result.status });
   return actionFail(result.error || t('errors.submit_failed'));
  }

  return actionOk({ message: result.data.message || t('success.submitted') });
 } catch (error) {
  logger.error('[ReaderAction] submitReaderApplication', error);
  return actionFail(tApi('network_error'));
 }
}

export async function getMyReaderRequest(): Promise<ActionResult<MyReaderRequest>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) {
  return actionFail(AUTH_ERROR.UNAUTHORIZED);
 }

 try {
  const result = await serverHttpRequest<MyReaderRequest>('/reader/my-request', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get reader request',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] getMyReaderRequest', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to get reader request');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ReaderAction] getMyReaderRequest', error);
  return actionFail('Failed to get reader request');
 }
}
