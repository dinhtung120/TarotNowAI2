

import { useAuthStore } from '@/store/authStore';
import type {
  QuestWithProgress,
  ClaimQuestRewardResult,
  UserAchievementsData,
  UserTitlesData,
  LeaderboardResult,
} from './gamification.types';

async function authFetch<T>(path: string, options: RequestInit = {}): Promise<T> {
  /* Lấy token mới nhất từ Zustand store — đảm bảo luôn dùng token hiện hành
     kể cả khi token vừa được refresh bởi interceptor */
  const token = useAuthStore.getState().token;

  /* Ghép URL đầy đủ: Backend API gốc chạy tại NEXT_PUBLIC_API_URL */
  const baseUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api';
  const url = `${baseUrl}${path}`;

  const res = await fetch(url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(options.headers || {}),
    },
  });

  if (!res.ok) {
    /* Cố gắng đọc message lỗi từ ProblemDetails response body */
    const errorBody = await res.text().catch(() => '');
    let message = `API error ${res.status}`;
    try {
      const parsed = JSON.parse(errorBody);
      message = parsed.detail || parsed.title || parsed.message || message;
    } catch {
      /* Body không phải JSON — giữ nguyên message mặc định */
    }
    throw new Error(message);
  }

  /* Xử lý trường hợp response rỗng (204 No Content) */
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

  /**
   * Lấy danh sách thành tựu (achievements) + trạng thái đã mở khoá.
   */
  getAchievements: async (): Promise<UserAchievementsData> => {
    return authFetch<UserAchievementsData>('/gamification/achievements');
  },

  /**
   * Lấy danh sách danh hiệu (titles) user đang sở hữu + danh hiệu đang active.
   */
  getTitles: async (): Promise<UserTitlesData> => {
    return authFetch<UserTitlesData>('/gamification/titles');
  },

  
  setActiveTitle: async (titleCode: string): Promise<void> => {
    return authFetch<void>('/gamification/titles/active', {
      method: 'POST',
      body: JSON.stringify({ titleCode }),
    });
  },

  /**
   * DEV SANBDOX: Bấm là nhận tất cả danh hiệu để test giao diện
   */
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
