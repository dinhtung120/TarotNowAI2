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
    paymentOffer: {
      defaultAmount: number;
      maxNoteLength: number;
    };
    history: {
      pageSize: number;
    };
    participants: {
      defaultPageSize: number;
      maxPageSize: number;
    };
  };
  gamification: {
    defaultQuestType: string;
    defaultLeaderboardTrack: string;
  };
  auth: {
    minimumAge: number;
  };
  adminDispute: {
    defaultSplitPercentToReader: number;
    readerFreezePolicy: {
      lookbackDays: number;
      threshold: number;
    };
  };
  realtime: {
    reconnectScheduleMs: number[];
    negotiationTimeoutMs: number;
    presenceNegotiationCooldownMs: number;
    chatUnauthorizedCooldownMs: number;
    serverTimeoutMs: number;
    chat: {
      typingClearMs: number;
      invalidateDebounceMs: number;
      initialLoadGuardMs: number;
      appStartGuardMs: number;
    };
  };
  http: {
    clientTimeoutMs: number;
    serverTimeoutMs: number;
    minTimeoutMs: number;
  };
  runtimePoliciesClient: {
    timeoutMs: number;
    staleMs: number;
  };
  ui: {
    readers: {
      directoryPageSize: number;
      featuredLimit: number;
      directoryStaleMs: number;
    };
    search: {
      debounceMs: number;
    };
    prefetch: {
      chatInboxStaleMs: number;
    };
  };
  media: {
    upload: {
      maxImageBytes: number;
      maxVoiceBytes: number;
      maxVoiceDurationMs: number;
      imageCompressionTargetBytes: number;
      imageCompressionSteps: {
        initialQuality: number;
        maxSizeMb: number;
        maxWidthOrHeight: number;
      }[];
      retryAttempts: number;
      retryDelayMs: number;
    };
  };
}

export interface PublicRuntimePoliciesDto {
  auth: {
    minimumAge: number;
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

export async function getPublicRuntimePoliciesAction(): Promise<ActionResult<PublicRuntimePoliciesDto>> {
  const result = await serverHttpRequest<PublicRuntimePoliciesDto>('/legal/runtime-policies', {
    method: 'GET',
    cache: 'no-store',
    fallbackErrorMessage: 'Failed to load public runtime policies',
  });

  if (!result.ok) {
    return actionFail(result.error || 'Failed to load public runtime policies');
  }

  return actionOk(result.data);
}
