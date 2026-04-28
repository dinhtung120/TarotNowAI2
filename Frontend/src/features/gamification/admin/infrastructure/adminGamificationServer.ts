import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import type {
 AdminAchievementDefinition,
 AdminQuestDefinition,
 AdminTitleDefinition,
} from '@/features/gamification/admin/adminGamification.types';

export async function fetchAdminGamificationQuestsServer(): Promise<AdminQuestDefinition[]> {
 const token = await getServerAccessToken();
 const result = await serverHttpRequest<AdminQuestDefinition[]>('/admin/gamification/quests', {
  method: 'GET',
  token,
  next: { revalidate: 0 },
  fallbackErrorMessage: 'Failed to load admin quests.',
 });
 return result.ok && result.data ? result.data : [];
}

export async function fetchAdminGamificationAchievementsServer(): Promise<AdminAchievementDefinition[]> {
 const token = await getServerAccessToken();
 const result = await serverHttpRequest<AdminAchievementDefinition[]>('/admin/gamification/achievements', {
  method: 'GET',
  token,
  next: { revalidate: 0 },
  fallbackErrorMessage: 'Failed to load admin achievements.',
 });
 return result.ok && result.data ? result.data : [];
}

export async function fetchAdminGamificationTitlesServer(): Promise<AdminTitleDefinition[]> {
 const token = await getServerAccessToken();
 const result = await serverHttpRequest<AdminTitleDefinition[]>('/admin/gamification/titles', {
  method: 'GET',
  token,
  next: { revalidate: 0 },
  fallbackErrorMessage: 'Failed to load admin titles.',
 });
 return result.ok && result.data ? result.data : [];
}
