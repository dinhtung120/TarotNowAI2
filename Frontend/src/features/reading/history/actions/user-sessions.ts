'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { AUTH_ERROR } from "@/shared/models/authErrors";

export interface HistorySessionDto {
 id: string;
 spreadType: string;
 isCompleted: boolean;
 createdAt: string;
}

export interface HistorySessionsResponse {
 page: number;
 pageSize: number;
 totalPages: number;
 totalCount: number;
 items: HistorySessionDto[];
}

export interface HistoryAiRequestDto {
 id: string;
 status: string;
 finishReason: string | null;
 chargeDiamond: number;
 createdAt: string;
 requestType: string;
}

export interface HistoryFollowupDto {
 question: string;
 answer: string;
}

export interface HistoryDetailResponse {
 id: string;
 spreadType: string;
 cardsDrawn: string | null;
 isCompleted: boolean;
 createdAt: string;
 completedAt: string | null;
 aiSummary?: string;
 followups?: HistoryFollowupDto[];
 aiInteractions: HistoryAiRequestDto[];
}

export async function getHistorySessionsAction(
 page: number = 1,
 pageSize: number = 10,
 spreadType?: string,
 date?: string
): Promise<ActionResult<HistorySessionsResponse>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();
  if (!token) {
   return actionFail(AUTH_ERROR.UNAUTHORIZED);
  }

  let query = `page=${page}&pageSize=${pageSize}`;
  if (spreadType && spreadType !== 'all') query += `&spreadType=${encodeURIComponent(spreadType)}`;
  if (date) query += `&date=${encodeURIComponent(date)}`;

  const result = await serverHttpRequest<HistorySessionsResponse>(`/history/sessions?${query}`, {
   method: 'GET',
   token,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(AUTH_ERROR.UNAUTHORIZED);
   }
   logger.error('HistoryAction.getHistorySessionsAction', result.error, {
    status: result.status,
    page,
    pageSize,
    spreadType,
    date,
   });
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('HistoryAction.getHistorySessionsAction', error, {
   page,
   pageSize,
   spreadType,
   date,
  });
  return actionFail(tApi('network_error'));
 }
}

export async function getHistoryDetailAction(sessionId: string): Promise<ActionResult<HistoryDetailResponse>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();
  if (!token) {
   return actionFail(AUTH_ERROR.UNAUTHORIZED);
  }

  const result = await serverHttpRequest<HistoryDetailResponse>(`/history/sessions/${sessionId}`, {
   method: 'GET',
   token,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(AUTH_ERROR.UNAUTHORIZED);
   }
   if (result.status === 404) {
    return actionFail(tApi('not_found'));
   }
   logger.error('HistoryAction.getHistoryDetailAction', result.error, {
    status: result.status,
    sessionId,
   });
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('HistoryAction.getHistoryDetailAction', error, { sessionId });
  return actionFail(tApi('network_error'));
 }
}
