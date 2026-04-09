

import { useAuthStore } from '@/store/authStore';
import { API_BASE_URL } from '@/shared/infrastructure/http/apiUrl';
import type {
  QuestWithProgress,
  ClaimQuestRewardResult,
  UserAchievementsData,
  UserTitlesData,
  LeaderboardResult,
} from './gamification.types';

async function authFetch<T>(path: string, options: RequestInit = {}): Promise<T> {
  
  const token = useAuthStore.getState().token;

  
  const url = `${API_BASE_URL}${path}`;

  const res = await fetch(url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(options.headers || {}),
    },
  });

  if (!res.ok) {
    
    const errorBody = await res.text().catch(() => '');
    let message = `API error ${res.status}`;
    try {
      const parsed = JSON.parse(errorBody);
      message = parsed.detail || parsed.title || parsed.message || message;
    } catch {
      
    }
    throw new Error(message);
  }

  
  if (res.status === 204) return undefined as T;
  return (await res.json()) as T;
}

export const gamificationApi = {
  
  getQuests: async (type: string = 'daily'): Promise<QuestWithProgress[]> => {
    return authFetch<QuestWithProgress[]>(`/gamification/quests?type=${type}`);
  },

  
  claimQuestReward: async (questCode: string, periodKey: string): Promise<ClaimQuestRewardResult> => {
    return authFetch<ClaimQuestRewardResult>(`/gamification/quests/${questCode}/claim`, {
      method: 'POST',
      body: JSON.stringify({ periodKey }),
    });
  },

  
  getAchievements: async (): Promise<UserAchievementsData> => {
    return authFetch<UserAchievementsData>('/gamification/achievements');
  },

  
  getTitles: async (): Promise<UserTitlesData> => {
    return authFetch<UserTitlesData>('/gamification/titles');
  },

  
  setActiveTitle: async (titleCode: string): Promise<void> => {
    return authFetch<void>('/gamification/titles/active', {
      method: 'POST',
      body: JSON.stringify({ titleCode }),
    });
  },

  
  grantSandboxTitles: async (): Promise<{ message: string }> => {
    return authFetch<{ message: string }>('/gamification/sandbox-grant-me', {
      method: 'POST',
    });
  },

  
  getLeaderboard: async (track: string = 'daily_rank_score', periodKey?: string): Promise<LeaderboardResult> => {
    let url = `/gamification/leaderboard?track=${track}`;
    if (periodKey) url += `&periodKey=${periodKey}`;
    return authFetch<LeaderboardResult>(url);
  },
};
