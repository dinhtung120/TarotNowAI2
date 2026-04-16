'use server';

import type { CardCatalogItemDto } from '@/features/reading/application/actions/cards-catalog';
import type { WalletBalance } from '@/features/wallet/domain/types';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export interface ReadingSetupSnapshotDto {
 wallet: WalletBalance;
 cardsCatalog: CardCatalogItemDto[];
 freeDrawQuotas: {
  spread3: number;
  spread5: number;
  spread10: number;
 };
}

export async function getReadingSetupSnapshotAction(): Promise<ActionResult<ReadingSetupSnapshotDto>> {
 const token = await getServerAccessToken();
 if (!token) {
  return actionFail(AUTH_ERROR.UNAUTHORIZED);
 }

 try {
  const result = await serverHttpRequest<ReadingSetupSnapshotDto>('/me/reading-setup-snapshot', {
   method: 'GET',
   token,
   fallbackErrorMessage: 'Failed to load reading setup snapshot',
  });

  if (!result.ok) {
   return actionFail(result.error || 'Failed to load reading setup snapshot');
  }

  const data = result.data;
  if (!data?.wallet || !Array.isArray(data.cardsCatalog) || !data.freeDrawQuotas) {
   return actionFail('Invalid reading setup snapshot');
  }

  return actionOk(data);
 } catch {
  return actionFail('Reading setup snapshot error');
 }
}
