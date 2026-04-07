'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

export async function uploadAvatarAction(formData: FormData): Promise<ActionResult<{ avatarUrl: string }>> {
  const tApi = await getTranslations('ApiErrors');

  try {
    const token = await getServerAccessToken();
    if (!token) {
      return actionFail(tApi('unauthorized'));
    }

    const result = await serverHttpRequest<{ avatarUrl: string }>('/profile/avatar', {
      method: 'POST',
      token,
      formData,
      fallbackErrorMessage: tApi('unknown_error'),
    });

    if (!result.ok) {
      if (result.status === 401) {
        return actionFail(tApi('unauthorized'));
      }
      logger.error('ProfileAction.uploadAvatarAction', result.error, { status: result.status });
      return actionFail(result.error || tApi('unknown_error'));
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('ProfileAction.uploadAvatarAction', error);
    return actionFail(tApi('network_error'));
  }
}
