'use client';

import { useRef } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import {
 GACHA_API_ROUTES,
 GACHA_IDEMPOTENCY_HEADER,
 gachaQueryKeys,
} from '@/shared/infrastructure/gacha/gachaConstants';
import type { PullGachaPayload, PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';

function createIdempotencyKey(): string {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return crypto.randomUUID();
 }

 return `${Date.now()}_${Math.random().toString(16).slice(2)}`;
}

function buildIntentKey(payload: PullGachaPayload): string {
 return `${payload.poolCode.trim().toLowerCase()}:${payload.count}`;
}

async function sendPullRequest(payload: PullGachaPayload, idempotencyKey: string): Promise<PullGachaResult> {
 const response = await fetch(GACHA_API_ROUTES.pull, {
  method: 'POST',
  credentials: 'include',
  headers: {
   'Content-Type': 'application/json',
   [GACHA_IDEMPOTENCY_HEADER]: idempotencyKey,
  },
  body: JSON.stringify({
   poolCode: payload.poolCode,
   count: payload.count,
   idempotencyKey,
  }),
 });

 if (!response.ok) throw new Error(await parseApiError(response, 'Failed to pull gacha.'));
 return (await response.json()) as PullGachaResult;
}

export function usePullGacha() {
 const queryClient = useQueryClient();
 const pendingIntentMapRef = useRef<Map<string, string>>(new Map());

 return useMutation({
  mutationFn: async (payload: PullGachaPayload) => {
   const intentKey = buildIntentKey(payload);
   const activeIdempotencyKey = payload.idempotencyKey
    || pendingIntentMapRef.current.get(intentKey)
    || createIdempotencyKey();

   pendingIntentMapRef.current.set(intentKey, activeIdempotencyKey);
   return sendPullRequest(payload, activeIdempotencyKey);
  },
  onSuccess: async (_, variables) => {
   pendingIntentMapRef.current.delete(buildIntentKey(variables));
   await Promise.all([
    queryClient.invalidateQueries({ queryKey: gachaQueryKeys.pools() }),
    queryClient.invalidateQueries({ queryKey: [...gachaQueryKeys.all, 'history'] }),
    queryClient.invalidateQueries({ queryKey: ['wallet'] }),
   ]);
   useWalletStore.getState().fetchBalance();
  },
 });
}
