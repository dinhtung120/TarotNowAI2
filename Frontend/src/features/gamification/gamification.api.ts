/*
 * ===================================================================
 * FILE: gamification.api.ts
 * BỐI CẢNH (CONTEXT):
 *   Lớp API client-side cho module Gamification.
 *   Sử dụng native fetch + token từ authStore để gọi trực tiếp Backend API.
 *
 * TẠI SAO KHÔNG DÙNG AXIOS?
 *   Dự án TarotNow không bundle axios ở client-side. Toàn bộ server actions
 *   dùng `serverHttpRequest`. Ở client-side (React Query hooks), ta dùng
 *   native fetch kết hợp token từ Zustand authStore — nhẹ hơn và không cần
 *   thêm dependency.
 * ===================================================================
 */

import { useAuthStore } from '@/store/authStore';
import type {
  QuestWithProgress,
  ClaimQuestRewardResult,
  UserAchievementsData,
  UserTitlesData,
  LeaderboardResult,
} from './gamification.types';

/**
 * Hàm tiện ích tạo authenticated fetch request tới Backend API.
 * Tự động gắn Bearer token từ authStore và parse JSON response.
 *
 * Nếu response không ok (4xx, 5xx), ném Error kèm message từ server
 * để React Query có thể bắt và hiển thị cho user.
 */
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

/**
 * Object chứa tất cả các hàm gọi API Gamification.
 * Được inject vào React Query hooks qua `queryFn` / `mutationFn`.
 */
export const gamificationApi = {
  /**
   * Lấy danh sách nhiệm vụ (quests) kèm tiến độ hiện tại của user.
   * @param type - Loại nhiệm vụ: 'daily' | 'weekly' | 'monthly'
   */
  getQuests: async (type: string = 'daily'): Promise<QuestWithProgress[]> => {
    return authFetch<QuestWithProgress[]>(`/gamification/quests?type=${type}`);
  },

  /**
   * Nhận thưởng cho nhiệm vụ đã hoàn thành.
   * Backend sẽ kiểm tra idempotency để tránh claim trùng.
   */
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

  /**
   * Đặt danh hiệu active hiển thị trên profile.
   * Truyền chuỗi rỗng '' để bỏ danh hiệu.
   */
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

  /**
   * Lấy bảng xếp hạng theo track và period.
   * @param track - Loại: 'daily_rank_score' | 'weekly_rank_score' | ...
   * @param periodKey - Khoá chu kỳ: '2026-04-06' cho daily, '2026-W14' cho weekly
   */
  getLeaderboard: async (track: string = 'daily_rank_score', periodKey?: string): Promise<LeaderboardResult> => {
    let url = `/gamification/leaderboard?track=${track}`;
    if (periodKey) url += `&periodKey=${periodKey}`;
    return authFetch<LeaderboardResult>(url);
  },
};
