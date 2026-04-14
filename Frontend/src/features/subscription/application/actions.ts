'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { SubscriptionPlan, EntitlementBalance, SubscribeRequest, SubscribeResponse } from '../types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export async function getSubscriptionPlansAction(): Promise<ActionResult<SubscriptionPlan[]>> {
  try {
    const result = await serverHttpRequest<SubscriptionPlan[]>('/subscriptions/plans', {
      method: 'GET',
    });

    if (!result.ok) {
      return actionFail(result.error || 'Failed to fetch subscription plans');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('SubscriptionAction.getSubscriptionPlansAction', error);
    return actionFail('Network error');
  }
}

export async function getMyEntitlementsAction(): Promise<ActionResult<EntitlementBalance[]>> {
  try {
    const token = await getServerAccessToken();
    if (!token) return actionFail(AUTH_ERROR.UNAUTHORIZED);

    const result = await serverHttpRequest<EntitlementBalance[]>('/subscriptions/me/entitlements', {
      method: 'GET',
      token,
    });

    if (!result.ok) {
      if (result.status === 401) return actionFail(AUTH_ERROR.UNAUTHORIZED);
      return actionFail(result.error || 'Failed to fetch entitlements');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('SubscriptionAction.getMyEntitlementsAction', error);
    return actionFail('Network error');
  }
}

export async function subscribeToPlanAction(request: SubscribeRequest): Promise<ActionResult<SubscribeResponse>> {
  try {
    const token = await getServerAccessToken();
    if (!token) return actionFail(AUTH_ERROR.UNAUTHORIZED);

    const result = await serverHttpRequest<SubscribeResponse>('/subscriptions/subscribe', {
      method: 'POST',
      token,
      json: request,
    });

    if (!result.ok) {
      return actionFail(result.error || 'Failed to subscribe to plan');
    }

    return actionOk(result.data);
  } catch (error) {
    
    logger.error('SubscriptionAction.subscribeToPlanAction', error, { ...request });
    return actionFail('Network error');
  }
}
