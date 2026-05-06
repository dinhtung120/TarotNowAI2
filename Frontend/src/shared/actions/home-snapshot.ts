'use server';

import {
 mapReadersFromHomeSnapshot,
 type HomeSnapshotDto,
} from '@/features/reader/profile/homeSnapshotMapper';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';

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
