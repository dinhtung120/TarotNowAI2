'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { IStreakStatusResult } from '../types/checkin.types';

function unauthorized<T>() {
  return actionFail(AUTH_ERROR.UNAUTHORIZED) as ActionResult<T>;
}

export async function getStreakStatus(): Promise<ActionResult<IStreakStatusResult>> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return unauthorized();

  try {
    const result = await serverHttpRequest<IStreakStatusResult>('/checkin/streak', {
      method: 'GET',
      token: accessToken,
      fallbackErrorMessage: 'Failed to retrieve streak status',
    });

    if (!result.ok) {
      if (result.status === 401) {
        logger.warn('[CheckinAction] getStreakStatus: Session expired or invalid', { status: 401 });
      } else {
        logger.error('[CheckinAction] getStreakStatus', result.error, { status: result.status });
      }
      return actionFail(result.error || 'Failed to retrieve streak status');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('[CheckinAction] getStreakStatus', error);
    return actionFail('Failed to retrieve streak status');
  }
}
