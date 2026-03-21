'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export async function updateProfileAction(profileData: {
 displayName: string;
 avatarUrl: string | null;
 dateOfBirth: string;
}) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getAccessToken();
  if (!token) {
   return { error: tApi('unauthorized') };
  }

  const result = await serverHttpRequest<unknown>('/profile', {
   method: 'PATCH',
   token,
   json: profileData,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return { error: tApi('unauthorized') };
   }
   logger.error('ProfileAction.updateProfileAction', result.error, { status: result.status });
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true };
 } catch (error) {
  logger.error('ProfileAction.updateProfileAction', error);
  return { error: tApi('network_error') };
 }
}
