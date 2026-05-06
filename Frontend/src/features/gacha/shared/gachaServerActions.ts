'use server';

import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import type {
  GachaHistoryPage,
  GachaPool,
  GachaPoolOdds,
  PullGachaPayload,
  PullGachaResult,
} from '@/features/gacha/shared/gachaTypes';
import { GACHA_IDEMPOTENCY_HEADER } from '@/features/gacha/shared/gachaConstants';

async function requireAccessToken(): Promise<string> {
 const token = await getServerAccessToken();
 if (!token) {
  throw new Error(AUTH_ERROR.UNAUTHORIZED);
 }

 return token;
}

export async function fetchGachaPoolsServer(): Promise<GachaPool[]> {
  const token = await requireAccessToken();
  const result = await serverHttpRequest<GachaPool[]>('/gacha/pools', {
    method: 'GET',
    token,
    fallbackErrorMessage: 'Failed to load gacha pools.',
  });

  if (!result.ok) {
    throw new Error(result.error);
  }

  return result.data;
}

export async function fetchGachaPoolOddsServer(poolCode: string): Promise<GachaPoolOdds> {
  const token = await requireAccessToken();
  const result = await serverHttpRequest<GachaPoolOdds>(`/gacha/pools/${encodeURIComponent(poolCode)}/odds`, {
    method: 'GET',
    token,
    fallbackErrorMessage: 'Failed to load gacha pool odds.',
  });

  if (!result.ok) {
    throw new Error(result.error);
  }

  return result.data;
}

export async function fetchGachaHistoryServer(page = 1, pageSize = 20): Promise<GachaHistoryPage> {
  const token = await requireAccessToken();
  const normalizedPage = Math.max(1, page);
  const normalizedPageSize = Math.max(1, Math.min(pageSize, 100));
  const result = await serverHttpRequest<GachaHistoryPage>(
    `/gacha/history?page=${normalizedPage}&pageSize=${normalizedPageSize}`,
    {
      method: 'GET',
      token,
      fallbackErrorMessage: 'Failed to load gacha history.',
    },
  );

  if (!result.ok) {
    throw new Error(result.error);
  }

  return result.data;
}

export async function pullGachaServer(payload: PullGachaPayload): Promise<PullGachaResult> {
  const token = await requireAccessToken();
  const idempotencyKey = payload.idempotencyKey || crypto.randomUUID();
  const result = await serverHttpRequest<PullGachaResult>('/gacha/pull', {
    method: 'POST',
    token,
    json: {
      poolCode: payload.poolCode,
      count: payload.count,
      idempotencyKey,
    },
    headers: {
      [GACHA_IDEMPOTENCY_HEADER]: idempotencyKey,
    },
    fallbackErrorMessage: 'Failed to pull gacha.',
  });

  if (!result.ok) {
    throw new Error(result.error);
  }

  return result.data;
}
