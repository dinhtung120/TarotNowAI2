'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { UserProfile } from '@/features/auth/session/types';

export interface ProfileDto extends UserProfile {
 avatarUrl: string | null;
 dateOfBirth: string;
 zodiac: string;
 numerology: number;
 hasConsented: boolean;
 payoutBankName?: string | null;
 payoutBankBin?: string | null;
 payoutBankAccountNumber?: string | null;
 payoutBankAccountHolder?: string | null;
}

export interface PayoutBankOption {
 bankBin: string;
 bankName: string;
}

export async function getProfileAction(): Promise<ActionResult<ProfileDto>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();
  if (!token) {
   return actionFail(tApi('unauthorized'));
  }

  const result = await serverHttpRequest<ProfileDto>('/profile', {
   method: 'GET',
   token,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(tApi('unauthorized'));
   }
   logger.error('ProfileAction.getProfileAction', result.error, { status: result.status });
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('ProfileAction.getProfileAction', error);
  return actionFail(tApi('network_error'));
 }
}

export async function getPayoutBanksAction(): Promise<ActionResult<PayoutBankOption[]>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();
  if (!token) {
   return actionFail(tApi('unauthorized'));
  }

  const result = await serverHttpRequest<PayoutBankOption[]>('/profile/payout-banks', {
   method: 'GET',
   token,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(tApi('unauthorized'));
   }

   logger.error('ProfileAction.getPayoutBanksAction', result.error, { status: result.status });
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('ProfileAction.getPayoutBanksAction', error);
  return actionFail(tApi('network_error'));
 }
}
