'use client';

import { fetchJsonOrThrow, fetchWithTimeout } from '@/shared/application/gateways/clientFetch';
import type {
 AdminAchievementDefinition,
 AdminQuestDefinition,
 AdminTitleDefinition,
} from '@/features/gamification/admin/application/adminGamification.types';

const ADMIN_GAMIFICATION_API_ROOT = '/api/admin/gamification';

async function adminGamificationDelete(path: string): Promise<void> {
 const response = await fetchWithTimeout(`${ADMIN_GAMIFICATION_API_ROOT}${path}`, {
  method: 'DELETE',
  credentials: 'include',
  cache: 'no-store',
 }, 8_000);

 if (!response.ok) {
  throw new Error('Failed to delete admin gamification entry.');
 }
}

export function listAdminQuests() {
 return fetchJsonOrThrow<AdminQuestDefinition[]>(
  `${ADMIN_GAMIFICATION_API_ROOT}/quests`,
  {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  },
  'Failed to load admin quests.',
  8_000,
 );
}

export function upsertAdminQuest(payload: AdminQuestDefinition) {
 return fetchJsonOrThrow<{ message: string }>(
  `${ADMIN_GAMIFICATION_API_ROOT}/quests`,
  {
   method: 'POST',
   credentials: 'include',
   cache: 'no-store',
   headers: {
    'Content-Type': 'application/json',
   },
   body: JSON.stringify(payload),
  },
  'Failed to save admin quest.',
  8_000,
 );
}

export function deleteAdminQuest(code: string) {
 return adminGamificationDelete(`/quests/${encodeURIComponent(code)}`);
}

export function listAdminAchievements() {
 return fetchJsonOrThrow<AdminAchievementDefinition[]>(
  `${ADMIN_GAMIFICATION_API_ROOT}/achievements`,
  {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  },
  'Failed to load admin achievements.',
  8_000,
 );
}

export function upsertAdminAchievement(payload: AdminAchievementDefinition) {
 return fetchJsonOrThrow<{ message: string }>(
  `${ADMIN_GAMIFICATION_API_ROOT}/achievements`,
  {
   method: 'POST',
   credentials: 'include',
   cache: 'no-store',
   headers: {
    'Content-Type': 'application/json',
   },
   body: JSON.stringify(payload),
  },
  'Failed to save admin achievement.',
  8_000,
 );
}

export function deleteAdminAchievement(code: string) {
 return adminGamificationDelete(`/achievements/${encodeURIComponent(code)}`);
}

export function listAdminTitles() {
 return fetchJsonOrThrow<AdminTitleDefinition[]>(
  `${ADMIN_GAMIFICATION_API_ROOT}/titles`,
  {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  },
  'Failed to load admin titles.',
  8_000,
 );
}

export function upsertAdminTitle(payload: AdminTitleDefinition) {
 return fetchJsonOrThrow<{ message: string }>(
  `${ADMIN_GAMIFICATION_API_ROOT}/titles`,
  {
   method: 'POST',
   credentials: 'include',
   cache: 'no-store',
   headers: {
    'Content-Type': 'application/json',
   },
   body: JSON.stringify(payload),
  },
  'Failed to save admin title.',
  8_000,
 );
}

export function deleteAdminTitle(code: string) {
 return adminGamificationDelete(`/titles/${encodeURIComponent(code)}`);
}
