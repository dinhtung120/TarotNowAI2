

import { useAuthStore } from '@/store/authStore';
import type { 
  GachaBannerDto, 
  GachaBannerOddsDto, 
  GachaHistoryItemDto, 
  SpinGachaRequestDto, 
  SpinGachaResult 
} from './gacha.types'
import { v4 as uuidv4 } from 'uuid'

async function authFetch<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = useAuthStore.getState().token;
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
    const errorBody = await res.text().catch(() => '');
    let message = `API error ${res.status}`;
    try {
      const parsed = JSON.parse(errorBody);
      message = parsed.detail || parsed.title || parsed.message || message;
    } catch {}
    throw new Error(message);
  }

  if (res.status === 204) {
      return {} as T;
  }
  
  return res.json();
}

export const gachaApi = {
  /**
   * Get all active banners
   */
  async getBanners(): Promise<GachaBannerDto[]> {
    return authFetch('/v1/gacha/banners')
  },

  /**
   * Get odds for a specific banner
   */
  async getBannerOdds(bannerCode: string): Promise<GachaBannerOddsDto> {
    return authFetch(`/v1/gacha/banners/${bannerCode}/odds`)
  },

  /**
   * Get user gacha history
   */
  async getHistory(limit: number = 50): Promise<GachaHistoryItemDto[]> {
    return authFetch(`/v1/gacha/history?limit=${limit}`)
  },

  
  async spin(data: SpinGachaRequestDto): Promise<SpinGachaResult> {
    const idempotencyKey = uuidv4()
    return authFetch('/v1/gacha/spin', {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        'X-Idempotency-Key': idempotencyKey
      }
    })
  }
}
