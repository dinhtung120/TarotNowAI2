'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import type {
 ClaimQuestRewardResult,
 LeaderboardResult,
 QuestWithProgress,
 UserAchievementsData,
 UserTitlesData,
} from '@/features/gamification/gamification.types';

async function requireToken(): Promise<string> {
 const token = await getServerAccessToken();
 if (!token) {
  throw new Error(AUTH_ERROR.UNAUTHORIZED);
 }
 return token;
}

export async function fetchGamificationQuests(type: string): Promise<QuestWithProgress[]> {
 const token = await requireToken();
 const result = await serverHttpRequest<QuestWithProgress[]>(
  `/gamification/quests?type=${encodeURIComponent(type)}`,
  { method: 'GET', token }
 );
 if (!result.ok) {
  throw new Error(result.error);
 }
 return result.data;
}

export async function fetchGamificationAchievements(): Promise<UserAchievementsData> {
 const token = await requireToken();
 const result = await serverHttpRequest<UserAchievementsData>('/gamification/achievements', {
  method: 'GET',
  token,
 });
 if (!result.ok) {
  throw new Error(result.error);
 }
 return result.data;
}

export async function fetchGamificationTitles(): Promise<UserTitlesData> {
 const token = await requireToken();
 const result = await serverHttpRequest<UserTitlesData>('/gamification/titles', {
  method: 'GET',
  token,
 });
 if (!result.ok) {
  throw new Error(result.error);
 }
 return result.data;
}

export async function fetchGamificationLeaderboard(
 track: string,
 periodKey?: string
): Promise<LeaderboardResult> {
 const token = await requireToken();
 let path = `/gamification/leaderboard?track=${encodeURIComponent(track)}`;
 if (periodKey) {
  path += `&periodKey=${encodeURIComponent(periodKey)}`;
 }
 const result = await serverHttpRequest<LeaderboardResult>(path, { method: 'GET', token });
 if (!result.ok) {
  throw new Error(result.error);
 }
 return result.data;
}

export async function claimGamificationQuestRewardServer(
 questCode: string,
 periodKey: string
): Promise<ClaimQuestRewardResult> {
 const token = await requireToken();
 const result = await serverHttpRequest<ClaimQuestRewardResult>(
  `/gamification/quests/${encodeURIComponent(questCode)}/claim`,
  {
   method: 'POST',
   token,
   json: { periodKey },
  }
 );
 if (!result.ok) {
  throw new Error(result.error);
 }
 return result.data;
}

export async function setGamificationActiveTitleServer(titleCode: string): Promise<void> {
 const token = await requireToken();
 const result = await serverHttpRequest<void>('/gamification/titles/active', {
  method: 'POST',
  token,
  json: { titleCode },
 });
 if (!result.ok) {
  throw new Error(result.error);
 }
}
