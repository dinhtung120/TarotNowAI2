'use server';

import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export interface RuntimePoliciesDto {
  reading: {
    spread3GoldCost: number;
    spread3DiamondCost: number;
    spread5GoldCost: number;
    spread5DiamondCost: number;
    spread10GoldCost: number;
    spread10DiamondCost: number;
  };
  followup: {
    maxFollowupsAllowed: number;
    priceTiers: number[];
    freeSlotThresholds: {
      minHighestCardLevel: number;
      freeSlots: number;
    }[];
  };
  wallet: {
    vndPerDiamond: number;
    minWithdrawDiamond: number;
    withdrawFeeRate: number;
  };
  reader: {
    minYearsOfExperience: number;
    minDiamondPerQuestion: number;
    defaultDiamondPerQuestion: number;
  };
  chat: {
    defaultSlaHours: number;
    allowedSlaHours: number[];
    maxActiveConversationsPerUser: number;
  };
  gamification: {
    defaultQuestType: string;
    defaultLeaderboardTrack: string;
  };
}

export async function getRuntimePoliciesAction(): Promise<ActionResult<RuntimePoliciesDto>> {
  const token = await getServerAccessToken();
  if (!token) {
    return actionFail(AUTH_ERROR.UNAUTHORIZED);
  }

  const result = await serverHttpRequest<RuntimePoliciesDto>('/me/runtime-policies', {
    method: 'GET',
    token,
    fallbackErrorMessage: 'Failed to load runtime policies',
  });

  if (!result.ok) {
    return actionFail(result.error || 'Failed to load runtime policies');
  }

  return actionOk(result.data);
}
