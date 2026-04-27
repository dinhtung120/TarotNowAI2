'use server';

import {
 mapReadersFromHomeSnapshot,
 type HomeSnapshotDto,
} from '@/features/reader/application/homeSnapshotMapper';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';

export async function getHomeSnapshotAction(): Promise<ActionResult<HomeSnapshotDto>> {
 try {
  const result = await serverHttpRequest<unknown>('/home/snapshot', {
   method: 'GET',
   next: { revalidate: 120 },
   fallbackErrorMessage: 'Failed to load home snapshot',
  });

  if (!result.ok) {
   return actionFail(result.error || 'Failed to load home snapshot');
  }

  return actionOk(mapReadersFromHomeSnapshot(result.data));
 } catch {
  return actionFail('Home snapshot error');
 }
}
