'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { 
  IDailyCheckInResult, 
  IStreakStatusResult, 
  IPurchaseStreakFreezeResult,
  IPurchaseStreakFreezeCommand 
} from '../types/checkin.types';

function unauthorized<T>() {
  return actionFail('Unauthorized') as ActionResult<T>;
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
      logger.error('[CheckinAction] getStreakStatus', result.error, { status: result.status });
      return actionFail(result.error || 'Failed to retrieve streak status');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('[CheckinAction] getStreakStatus', error);
    return actionFail('Failed to retrieve streak status');
  }
}

export async function performDailyCheckIn(): Promise<ActionResult<IDailyCheckInResult>> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return unauthorized();

  try {
    const result = await serverHttpRequest<IDailyCheckInResult>('/checkin', {
      method: 'POST',
      token: accessToken,
      fallbackErrorMessage: 'Failed to perform daily check-in',
    });

    if (!result.ok) {
      logger.error('[CheckinAction] performDailyCheckIn', result.error, { status: result.status });
      return actionFail(result.error || 'Failed to perform daily check-in');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('[CheckinAction] performDailyCheckIn', error);
    return actionFail('Failed to perform daily check-in');
  }
}

export async function purchaseStreakFreeze(
  payload: IPurchaseStreakFreezeCommand
): Promise<ActionResult<IPurchaseStreakFreezeResult>> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return unauthorized();

  try {
    const result = await serverHttpRequest<IPurchaseStreakFreezeResult>('/checkin/freeze', {
      method: 'POST',
      token: accessToken,
      json: payload,
      fallbackErrorMessage: 'Failed to purchase streak freeze',
    });

    if (!result.ok) {
      logger.error('[CheckinAction] purchaseStreakFreeze', result.error, { status: result.status });
      return actionFail(result.error || 'Failed to purchase streak freeze');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('[CheckinAction] purchaseStreakFreeze', error);
    return actionFail('Failed to purchase streak freeze');
  }
}
