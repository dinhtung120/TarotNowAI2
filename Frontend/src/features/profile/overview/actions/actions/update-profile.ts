'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

export async function updateProfileAction(profileData: {
 displayName: string;
 dateOfBirth: string;
 payoutBankName?: string | null;
 payoutBankBin?: string | null;
 payoutBankAccountNumber?: string | null;
 payoutBankAccountHolder?: string | null;
}): Promise<ActionResult<undefined>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();
  if (!token) {
   return actionFail(tApi('unauthorized'));
  }

  const result = await serverHttpRequest<unknown>('/profile', {
   method: 'PATCH',
   token,
   json: profileData,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(tApi('unauthorized'));
   }
   logger.error('ProfileAction.updateProfileAction', result.error, { status: result.status });
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk();
 } catch (error) {
  logger.error('ProfileAction.updateProfileAction', error);
  return actionFail(tApi('network_error'));
 }
}
