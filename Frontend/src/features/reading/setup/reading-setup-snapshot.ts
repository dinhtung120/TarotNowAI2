'use server';

import type { CardCatalogItemDto } from '@/features/reading/tarot-catalog/actions/cards-catalog';
import type { WalletBalance } from '@/features/wallet/shared/types';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { AUTH_ERROR } from "@/shared/models/authErrors";

export interface ReadingSetupSnapshotDto {
 wallet: WalletBalance;
 cardsCatalog: CardCatalogItemDto[];
 freeDrawQuotas: {
  spread3: number;
  spread5: number;
  spread10: number;
 };
 pricing: {
  spread3GoldCost: number;
  spread3DiamondCost: number;
  spread5GoldCost: number;
  spread5DiamondCost: number;
  spread10GoldCost: number;
  spread10DiamondCost: number;
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
